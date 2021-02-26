using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    public int Apples = 0;
    public GameObject WoodPrefab;
    public Transform WoodPosition;
    public StageSettingsData CurrentStageSettings;

    public int CurrentStage { get; private set; }
    public bool IsGameOver { get => _isGameOver; set => _isGameOver = value; }
    public bool IsGameStarted { get => _isGameStarted; set => _isGameStarted = value; }
    public bool IsReadyToBegin { get => _isReadyToBegin; set => _isReadyToBegin = value; }
    public bool IsStageComplited { get; set; }

    private bool _isReadyToBegin;
    private bool _isGameStarted = false;
    private bool _isGameOver = false;

    private ThrowSystem _throwSystem;
    private Wood _wood;
    private GameData _gameData;
    private int _stageIndex;
    private int _playerRecordStage;

    public override void Awake()
    {
        base.Awake();
        _isReadyToBegin = true;
        _stageIndex = 0;
        _throwSystem = FindObjectOfType<ThrowSystem>();
        _wood = FindObjectOfType<Wood>();
        _gameData = GetComponent<GameData>();
        CurrentStageSettings = _gameData.StagesSettings[_stageIndex];
    }

    private void Start()
    {
        _playerRecordStage = Player.Instance.RecordStage;

        UIManager.Instance.UpdateAppleCounter();
        UIManager.Instance.ActiveStagePanel(false);

        StartCoroutine(ExecuteGameLoop());

        UIManager.Instance.MoveMainMenuOn();
    }

    IEnumerator ExecuteGameLoop()
    {
        yield return StartCoroutine(ReadyToBeginRoutine());

        yield return StartCoroutine(StartGameRoutine(0.25f));

        yield return StartCoroutine(PlayGameRoutine());
        
        yield return StartCoroutine(GameOverRoutine());
    }

    IEnumerator ReadyToBeginRoutine()
    {
        while(_isReadyToBegin)
        {
            yield return null;
        }

        UIManager.Instance.MoveMainMenuOff();

        UIManager.Instance.ScreenFadeOn();

        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator StartGameRoutine(float delay)
    {
        UIManager.Instance.ScreenFadeOff();

        _isGameStarted = true;

        if (WoodPrefab != null)
        {
            SetupWood();
        }

        UIManager.Instance.SetupCountKnifes();

        _throwSystem.StartThrowSystem();

        yield return new WaitForSeconds(delay);

        UIManager.Instance.ActiveStagePanel(true);
    }

    IEnumerator PlayGameRoutine()
    {
        UIManager.Instance.UpdateStageNumber(_stageIndex + 1);

        while (_isGameStarted)
        {           
            while(!IsStageComplited && _isGameStarted)
            {
                yield return null;
            }

            VFX_Manager.Instance.PlayCrackWood(WoodPosition.position);

            yield return new WaitForSeconds(1f);

            NextStage();

            if(_stageIndex==_gameData.StagesSettings.Length)
            {
                _isGameStarted = false;
                _isGameOver = true;
                yield return null;
            }

            UIManager.Instance.UpdateStageNumber(_stageIndex + 1);

            if (WoodPrefab != null)
            {
                SetupWood();
            }

            UIManager.Instance.SetupCountKnifes();

            _throwSystem.StartThrowSystem();
        }
    }

    IEnumerator GameOverRoutine()
    {
        IsReadyToBegin = true;

        UIManager.Instance.ScreenFadeOn();
        yield return new WaitForSeconds(0.5f);
        UIManager.Instance.MoveMainMenuOn();
        yield return new WaitForSeconds(0.5f);
        UIManager.Instance.ScreenFadeOff();

        if (_playerRecordStage < _stageIndex + 1)
            Player.Instance.RecordStage = _stageIndex + 1;

        Player.Instance.Apples = Apples;

        UIManager.Instance.UpdateRecordText();

        _stageIndex = 0;

        while (IsGameOver)
        {
            yield return null;
        }

        StartCoroutine(ExecuteGameLoop());
    }


    public void NextStage()
    {
        _stageIndex++;

        if (_stageIndex == _gameData.StagesSettings.Length)
            _stageIndex = 0;

        CurrentStageSettings = _gameData.StagesSettings[_stageIndex];
    }

    public void StartGame()
    {
        _isReadyToBegin = false;
        IsStageComplited = false;
        IsGameOver = false;
    }

    private void SetupWood()
    {
        GameObject Wood = Instantiate(WoodPrefab, WoodPosition);
        _wood = Wood.GetComponent<Wood>();
        _wood.SetWood();
    }
}

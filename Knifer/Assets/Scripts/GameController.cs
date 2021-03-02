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
    private bool _endGame;
    private UIManager _uiManager;

    public override void Awake()
    {
        base.Awake();
        _isReadyToBegin = true;
        _stageIndex = 0;
        _throwSystem = FindObjectOfType<ThrowSystem>();
        _wood = FindObjectOfType<Wood>();
        _gameData = GetComponent<GameData>();
        CurrentStageSettings = _gameData.StagesSettings[_stageIndex];
        _endGame = false;
    }

    private void Start()
    {
        Vibration.Init();
        _uiManager = UIManager.Instance;

        Player.Instance.Load();

        _playerRecordStage = Player.Instance.RecordStage;
        Apples = Player.Instance.Apples;



        _uiManager.ActiveThrowButton(false);
        _uiManager.UpdateAppleCounter();
        _uiManager.ActiveStagePanel(false);
        _uiManager.UpdateRecordText();
        _uiManager.UpdateSkinsPanel();


        StartCoroutine(ExecuteGameLoop());

        _uiManager.MoveMainMenuOn();
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

        _uiManager.MoveMainMenuOff();

        _uiManager.ScreenFadeOn();

        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator StartGameRoutine(float delay)
    {
        _uiManager.ScreenFadeOff();

        _isGameStarted = true;

        if (WoodPrefab != null)
        {
            SetupWood();
        }

        _uiManager.SetupCountKnifes();

        _throwSystem.StartThrowSystem();

        yield return new WaitForSeconds(delay);

        _uiManager.ActiveStagePanel(true);

        _uiManager.ActiveThrowButton(true);
    }

    IEnumerator PlayGameRoutine()
    {
        _uiManager.UpdateStageNumber(_stageIndex + 1);
        _endGame = false;

        while (_isGameStarted)
        {           
            while(!IsStageComplited && _isGameStarted) // Gameplay
            {
                yield return null;
            }

            if (IsStageComplited && _isGameStarted) // Next stage
            {
                VFX_Manager.Instance.PlayCrackWood(WoodPosition.position);

                yield return new WaitForSeconds(1f);

                NextStage();

                if(CurrentStageSettings.IsBoss)
                {
                    _uiManager.ShowBossMessage(true);
                    yield return new WaitForSeconds(1f);
                    _uiManager.ShowBossMessage(false);
                }

                if (_stageIndex == _gameData.StagesSettings.Length)
                {
                    _isGameStarted = false;
                    _isGameOver = true;
                    yield return null;
                }

                _uiManager.UpdateStageNumber(_stageIndex + 1);

                IsStageComplited = false;

                if (WoodPrefab != null)
                {
                    SetupWood();
                }
                _uiManager.ClearCountKnifesPanel();

                _uiManager.SetupCountKnifes();

                _throwSystem.StartThrowSystem();

            }            

            else if(!IsStageComplited && !_isGameStarted) // Lose
            {
                _uiManager.ShowLoseMessage(true);

                yield return new WaitForSeconds(1.5f);

                Destroy(_wood.gameObject);

                _uiManager.ShowLoseMessage(false);

                yield return new WaitForSeconds(0.5f);

                _uiManager.ActiveStagePanel(false);

                if (_playerRecordStage < _stageIndex)
                {
                    _playerRecordStage = _stageIndex;
                    Player.Instance.RecordStage = _stageIndex;  // Record is previous stage
                }

                _uiManager.UpdateRecordText();
            }

            if (_endGame)
            {
                if (_playerRecordStage < _stageIndex + 1)
                {
                    _playerRecordStage = _stageIndex + 1;
                    Player.Instance.RecordStage = _stageIndex + 1; // Record is last stage
                }

                _uiManager.UpdateRecordText();

                _isGameStarted = false;

                _uiManager.ShowWinMessage(true);

                yield return new WaitForSeconds(2.5f);

                _uiManager.ShowWinMessage(false);

                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    IEnumerator GameOverRoutine()
    {
        _uiManager.ActiveThrowButton(false);

        _uiManager.UpdateSkinsPanel();

        Player.Instance.Apples = Apples;
        Player.Instance.RecordStage = _playerRecordStage;
        Player.Instance.Save();

        _uiManager.ClearCountKnifesPanel();

        IsReadyToBegin = true;

        _uiManager.ScreenFadeOn();
        yield return new WaitForSeconds(0.5f);
        _uiManager.MoveMainMenuOn();
        yield return new WaitForSeconds(0.5f);
        _uiManager.ScreenFadeOff();

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
        {
            _stageIndex = 0;
            _endGame = true;
        }

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

    public void QuitApp()
    {
        Application.Quit();
    }
}

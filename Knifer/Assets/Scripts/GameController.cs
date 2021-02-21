using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    public int Apples=0;

    public Transform WoodPosition;
    public StageSettingsData CurrentStageSettings;

    public int CurrentStage { get; private set; }
    public bool IsGameOver { get => _isGameOver; set => _isGameOver = value; }
    public bool IsGameStarted { get => _isGameStarted; set => _isGameStarted = value; }
    
    private bool _isGameStarted = false;
    private bool _isGameOver = false;

    public override void Awake()
    {
        base.Awake();

        CurrentStageSettings = GetComponent<GameData>().StagesSettings[0];
    }


    private void Start()
    {
        _isGameStarted = true;
        UIManager.Instance.UpdateAppleCounter();
    }

    //IEnumerator ExecuteGameLoop()
    //{
    //    yield return StartCoroutine(StartGameRoutine());
    //    yield return StartCoroutine(GameOverRoutine());
    //}

    //IEnumerator StartGameRoutine()
    //{
    //    while(_isGameStarted)
    //    {
            


    //    }
    //}

    //IEnumerator GameOverRoutine()
    //{
    //    while(IsGameOver)
    //    {

    //    }
    //}

    //IEnumerator GameCycleRoutine()
    //{

    //}
}

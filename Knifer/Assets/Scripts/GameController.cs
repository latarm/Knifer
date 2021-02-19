using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    public bool IsGameContinued=true;

    private void Start()
    {
        IsGameContinued = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>
{
    public int Apples { get => _apples; set => _apples = value; }
    public int RecordStage { get => _recordStage; set => _recordStage = value; }

    private int _apples;
    private int _recordStage;



}

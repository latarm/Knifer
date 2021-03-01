using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Player : Singleton<Player>
{
    public int Apples { get => _apples; set => _apples = value; }
    public int RecordStage { get => _recordStage; set => _recordStage = value; }

    private int _apples;
    private int _recordStage;

    PlayerData _playerData;

    public override void Awake()
    {
        _playerData = new PlayerData();
        base.Awake();
        _recordStage = 0;
        _apples = 0;
    }


    public void Save()
    {
        _playerData.ApplesCount = Apples;
        _playerData.RecordCount = RecordStage;
        string saveData =  JsonUtility.ToJson(_playerData);
        File.WriteAllText(Application.dataPath + "/saveFile.txt", saveData);
    }

    public void Load()
    {
        if (File.Exists(Application.dataPath + "/saveFile.txt"))
        {
            string data = File.ReadAllText(Application.dataPath + "/saveFile.txt");
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(data);
            Apples = playerData.ApplesCount;
            RecordStage = playerData.RecordCount;
        }
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    private class PlayerData
    {
        public int ApplesCount;
        public int RecordCount;        
    }
}

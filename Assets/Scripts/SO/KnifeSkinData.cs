using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="KnifeSkinData",menuName ="KnifeData", order =2)]
public class KnifeSkinData : ScriptableObject
{
    public int RequiredStage;
    public int RequiredApplesCount;
    public Sprite Skin;

    public bool CheckAvailability()
    {
        return Player.Instance.RecordStage >= RequiredStage && Player.Instance.Apples >= RequiredApplesCount;
    }

}

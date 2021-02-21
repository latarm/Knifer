using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="GameSettings",menuName ="GameplaySettings",order =1)]
public class StageSettingsData : ScriptableObject
{
    public bool IsBoss;
    public Sprite WoodSkin;
    public float WoodRotationSpeed;
    public float TimeOfRotationSlowing;
    public int KnifesInWoodMinCount;
    public int KnifesInWoodMaxCount;
    public float AppleAppearChance;
    public int CountOfKnifes;
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : Singleton<GameData>
{
    public StageSettingsData[] StagesSettings;
    public SpritesData KnifeSkins;
    public float SpawnKnifeDelay;
}

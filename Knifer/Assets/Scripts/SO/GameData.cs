using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : Singleton<GameData>
{
    public StageSettingsData[] StagesSettings;
    public SpritesData KnifeSkins;

    public Sprite SelectKnifeSkin(int index)
    {
        return KnifeSkins.KnifeSkins[index];
    }
}

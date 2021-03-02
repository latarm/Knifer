using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : Singleton<GameData>
{
    public StageSettingsData[] StagesSettings;
    public KnifeSkinData[] KnifeSkins;

    public int _skinIndex;

    private void Start()
    {
        _skinIndex = 0;
    }

    public void SelectSkin(int index)
    {
        _skinIndex = index;
        Debug.Log(index);
    }

    public Sprite SelectedSkin()
    {
        return KnifeSkins[_skinIndex].Skin;
    }
}

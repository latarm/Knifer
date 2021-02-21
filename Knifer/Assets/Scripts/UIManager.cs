using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public Text AppleCounter;

    public void UpdateAppleCounter()
    {
        if (AppleCounter != null)
            AppleCounter.text = GameController.Instance.Apples.ToString();
    }
}

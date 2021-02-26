using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] Text AppleCounter;
    [SerializeField] RectTransform KnifeCountPanel;
    [SerializeField] GameObject KnifeImagePrefab;
    [SerializeField] GameObject MainMenuPanel;
    [SerializeField] Text RecordText;
    [SerializeField] ScreenFader ScreenFader;
    [SerializeField] RectTransform StagePanel;
    [SerializeField] Text StageNumber;

    int _countOfKnifesLeft;

    public void UpdateAppleCounter()
    {
        if (AppleCounter != null)
            AppleCounter.text = GameController.Instance.Apples.ToString();
    }

    public void SetupCountKnifes()
    {
        if (KnifeCountPanel != null && KnifeImagePrefab != null)
            for (int i = 0; i < GameController.Instance.CurrentStageSettings.CountOfKnifes; i++)
            {
                Instantiate(KnifeImagePrefab, KnifeCountPanel);
            }
    }

    public void ReduceNumberOfKnifes()
    {
        if(KnifeCountPanel.childCount>0)
        {
            Destroy(KnifeCountPanel.GetChild(0).gameObject);
        }
    }

    public void ActiveMainMenu(bool value)
    {
        if (MainMenuPanel != null)
            MainMenuPanel.SetActive(value);
    }

    public void UpdateRecordText()
    {
        if (RecordText != null)
            RecordText.text = Player.Instance.RecordStage.ToString();
    }

    public void UpdateStageNumber(int value)
    {
        if (StageNumber != null)
            StageNumber.text = value.ToString();
    }

    public void ActiveStagePanel(bool value)
    {
        if (StagePanel != null)
            StagePanel.gameObject.SetActive(value);
    }

    public void MoveMainMenuOff()
    {
        if (MainMenuPanel != null)
            MainMenuPanel.GetComponent<RectXMover>().MoveOff();
    }

    public void MoveMainMenuOn()
    {
        if (MainMenuPanel != null)
            MainMenuPanel.GetComponent<RectXMover>().MoveOn();
    }

    public void ScreenFadeOn()
    {
        if (ScreenFader != null)
            ScreenFader.FadeOn();
    }

    public void ScreenFadeOff()
    {
        if (ScreenFader != null)
            ScreenFader.FadeOff();
    }
}

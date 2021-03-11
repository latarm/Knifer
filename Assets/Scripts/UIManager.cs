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
    [SerializeField] Text CompleteText;
    [SerializeField] Button ThrowButton;
    [SerializeField] RectTransform SkinsPanel;
    [SerializeField] GameObject SkinPref;

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

    public void UpdateSkinsPanel()
    {
        for (int i = 0; i < SkinsPanel.childCount; i++)
        {
            Destroy(SkinsPanel.GetChild(i).gameObject);
        }

        for (int i = 0; i < GameData.Instance.KnifeSkins.Length; i++)
        {
            int index = i;
            GameObject skin = Instantiate(SkinPref, SkinsPanel.transform);
            skin.name = "Knife_" + "0" + i.ToString();
            Image skinImage = skin.GetComponent<Image>();

            skinImage.sprite = GameData.Instance.KnifeSkins[i].Skin;

            skin.GetComponent<Button>().onClick.AddListener(() => GameData.Instance.SelectSkin(index));

            if (!GameData.Instance.KnifeSkins[i].CheckAvailability())
            {
                skin.GetComponent<Button>().interactable = false;
                skinImage.color = Color.black;
            }
        }
    }

    public void ActiveThrowButton(bool active)
    {
        ThrowButton.interactable = active;
    }

    public void ClearCountKnifesPanel()
    {
        if (KnifeCountPanel.childCount > 0)
            for (int i = 0; i < KnifeCountPanel.childCount; i++)
            {
                Destroy(KnifeCountPanel.GetChild(i).gameObject);
            }
    }
    
    public void ShowBossMessage(bool show)
    {
        CompleteText.text = "Boss!";

        ShowMessageOfEndStage(show);
    }

    public void ShowWinMessage(bool show)
    {
        CompleteText.text = "Win!";

        ShowMessageOfEndStage(show);
    }

    public void ShowLoseMessage(bool show)
    {
        CompleteText.text = "Lose";

        ShowMessageOfEndStage(show);
    }

    private void ShowMessageOfEndStage(bool show)
    {
        if (show)
            CompleteText.GetComponent<RectXMover>().MoveOn();
        else
            CompleteText.GetComponent<RectXMover>().MoveOff();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowSystem : MonoBehaviour
{
    public GameObject KnifePrefab;
    public Transform ThrowPoint;
    public float ThrowDelay;
    [HideInInspector] public int CountOfKnifes;

    private GameObject _knifeToThrow;
    private Sprite _knifeSkin;

    public void StartThrowSystem()
    {
        CountOfKnifes = GameController.Instance.CurrentStageSettings.CountOfKnifes;

        StartCoroutine(SpawnKnifeRoutine());

        _knifeSkin = GameData.Instance.SelectedSkin();
    }

    IEnumerator ThrowRoutine()
    {
        while(GameController.Instance.IsGameStarted && !GameController.Instance.IsStageComplited)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && ThrowPoint != null && KnifePrefab != null && _knifeToThrow != null)
            {
                _knifeToThrow.GetComponent<KnifeMove>().Speed = 15f;

                CountOfKnifes--;
                UIManager.Instance.ReduceNumberOfKnifes();

                if (CountOfKnifes < 0)
                    CountOfKnifes = 0;

                _knifeToThrow = null;
            }

            yield return null;
        }
    }

    public void ThrowKnife()
    {
        _knifeToThrow.GetComponent<KnifeMove>().Speed = 15f;

        CountOfKnifes--;
        UIManager.Instance.ReduceNumberOfKnifes();

        if (CountOfKnifes < 0)
            CountOfKnifes = 0;

        _knifeToThrow = null;
    }

    IEnumerator SpawnKnifeRoutine()
    {
        while (GameController.Instance.IsGameStarted && !GameController.Instance.IsStageComplited)
        {
            if (_knifeToThrow == null)
            {
                yield return new WaitForSeconds(ThrowDelay);

                if (GameController.Instance.IsGameStarted && CountOfKnifes > 0)
                {
                    _knifeToThrow = Instantiate(KnifePrefab, ThrowPoint.position, Quaternion.identity, ThrowPoint);
                    _knifeToThrow.GetComponent<SpriteRenderer>().sprite = _knifeSkin;
                }
            }
            else
            {
                yield return null;
            }
        }
    }
}

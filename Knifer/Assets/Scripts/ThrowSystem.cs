using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowSystem : MonoBehaviour
{
    public GameObject KnifePrefab;
    public Transform ThrowPoint;
    public float ThrowDelay;
    public int CountOfKnifes;

    private GameObject _knifeToThrow;

    public void StartThrowSystem()
    {
        CountOfKnifes = GameController.Instance.CurrentStageSettings.CountOfKnifes;

        StartCoroutine(SpawnKnifeRoutine());
        StartCoroutine(ThrowRoutine());
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

    IEnumerator SpawnKnifeRoutine()
    {
        while (GameController.Instance.IsGameStarted && !GameController.Instance.IsStageComplited)
        {
            if (_knifeToThrow == null)
            {
                yield return new WaitForSeconds(ThrowDelay);

                if (GameController.Instance.IsGameStarted && CountOfKnifes > 0)
                    _knifeToThrow = Instantiate(KnifePrefab, ThrowPoint.position, Quaternion.identity, ThrowPoint);

            }
            else
            {
                yield return null;
            }
        }
    }
}

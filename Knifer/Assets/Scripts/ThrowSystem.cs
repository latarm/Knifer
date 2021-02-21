using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowSystem : MonoBehaviour
{
    public GameObject KnifePrefab;
    public Transform ThrowPoint;

    private GameObject _knifeToThrow;
    private float _interpolateAmount;    

    void Start()
    {
        StartCoroutine(SpawnKnifeRoutine());
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && ThrowPoint != null && KnifePrefab != null && _knifeToThrow!=null)
        {
            _knifeToThrow.GetComponent<KnifeMove>().Speed=15f;

            _knifeToThrow = null;
        }
    }

    IEnumerator SpawnKnifeRoutine()
    {
        while (GameController.Instance.IsGameStarted)
        {
            if (_knifeToThrow == null )
            {
                yield return new WaitForSeconds(0.25f);

                if (GameController.Instance.IsGameStarted)
                    _knifeToThrow = Instantiate(KnifePrefab, ThrowPoint.position, Quaternion.identity, ThrowPoint);

            }
            else
            {
                yield return null;
            }
        }
    }
}

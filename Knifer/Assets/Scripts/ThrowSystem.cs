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
    MaskableGraphic _graphic;
    

    void Start()
    {
        StartCoroutine(SpawnKnifeRoutine());
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && ThrowPoint != null && KnifePrefab != null && _knifeToThrow!=null)
        {
            _knifeToThrow.GetComponent<KnifeMove>().SetSpeed(15f);

            _knifeToThrow.GetComponent<KnifeMove>().SetForce(15f);

            _knifeToThrow = null;
        }
    }

    IEnumerator SpawnKnifeRoutine()
    {
        while (GameController.Instance.IsGameContinued)
        {
            if (_knifeToThrow == null)
            {
                yield return new WaitForSeconds(0.15f);

                _knifeToThrow = Instantiate(KnifePrefab, ThrowPoint.position-Vector3.up*(-1f), Quaternion.identity, ThrowPoint);

                //_interpolateAmount += Time.deltaTime*5f;

                //_knifeToThrow.transform.position = Vector2.Lerp(transform.position, ThrowPoint.position, _interpolateAmount);

            }
            else
            {
                //_interpolateAmount = 0f;
                yield return null;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    public int AppleCost { get; set; }

    private void Start()
    {
        AppleCost = Random.Range(2, 4);
    }
}

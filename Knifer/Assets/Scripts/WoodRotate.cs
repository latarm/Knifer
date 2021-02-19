using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodRotate : MonoBehaviour
{
    Vector2 _startPosition;
    Animator _animator;
    float _interpolatedTime = 0;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _startPosition = transform.position;
    }

    void Update()
    {
        transform.Rotate(0, 0, 0.5f);
    }

    public void HitReaction()
    {
        _animator.SetTrigger("Hit");
    }

}

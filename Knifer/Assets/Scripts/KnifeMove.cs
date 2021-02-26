using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeMove : MonoBehaviour
{
    public float Speed { get=>_speed; set=>_speed=value; }

    [SerializeField] private bool _knifeInWood = false;
    private float _speed;
    private Rigidbody2D _rigidBody;
    private bool _isStuck;
    private bool _isRicochet;
    private float _rotation;
    private float _ricochetOffset;
    private float _deltaTime;
    private Animator _animator;
    private ThrowSystem _throwSystem;

    private void Start()
    {
        _speed = 0f;
        _rotation = 0f;
        _isStuck = false;
        _isRicochet = false;
        _deltaTime = 0f;
        _rigidBody = GetComponent<Rigidbody2D>();
        _throwSystem = FindObjectOfType<ThrowSystem>();

        if (GetComponent<Animator>())
            GetComponent<Animator>().SetBool("ReadyToThrow", true);

    }

    private void Update()
    {
        if (_isRicochet == false)
            _rigidBody.position += Vector2.up * _speed * Time.deltaTime;

        else if (_isRicochet == true)
        {
            _rigidBody.position += (Vector2.up * (-5f) + Vector2.right * _ricochetOffset) * Time.deltaTime;

            _deltaTime = _deltaTime / 4f + Time.deltaTime;

            _rigidBody.SetRotation(Mathf.Lerp(_rigidBody.rotation, _rotation, _deltaTime));

            Destroy(gameObject, 3f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        ContactPoint2D contactPoint = collision.contacts[0];

        Vector2 vfxHitPosition = contactPoint.point;

        if (collision.transform.CompareTag("Wood") && !_isRicochet)
        {
            _speed = 0f;
            _isStuck = true;
            transform.parent = collision.transform;

            collision.transform.GetComponent<Wood>().HitImpact();

            if (_knifeInWood == false)
                VFX_Manager.Instance.PlayWoodHit(vfxHitPosition);

            _rigidBody.bodyType = RigidbodyType2D.Static;

            if(_throwSystem.CountOfKnifes==0)
            {
                collision.transform.GetComponent<Wood>().DestroyWood();

                _rotation = Random.Range(-540f, -180f);
                _ricochetOffset = Random.Range(0f, 3f);

                _isRicochet = true;
            }
        }

        else if (collision.transform.CompareTag("Knife") && _isStuck == false && _isRicochet == false && _knifeInWood==false)
        {
            VFX_Manager.Instance.PlayClank(vfxHitPosition);

            _rotation = Random.Range(-540f, -180f);
            _ricochetOffset = Random.Range(0f, 3f);

            _isRicochet = true;

            GameController.Instance.IsGameStarted = false;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameController.Instance.Apples += collision.GetComponent<Apple>().AppleCost;
        VFX_Manager.Instance.PlayAppleCut(collision.transform.position);
        Destroy(collision.gameObject);
        UIManager.Instance.UpdateAppleCounter();
    }
}

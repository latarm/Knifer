using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeMove : MonoBehaviour
{
    public GameObject VFX_Hit;

    public float Speed { get=>_speed; set=>_speed=value; }

    private float _speed;
    private Rigidbody2D _rigidBody;
    private bool _isStuck;
    private bool _isRicochet;
    private float _rotation;
    private float _ricochetOffset;
    private float _deltaTime;

    private void Awake()
    {
        _speed = 0f;
        _rotation = 0f;
        _isStuck = false;
        _isRicochet = false;
        _deltaTime = 0f;
    }

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (_isRicochet == false)
            _rigidBody.position += Vector2.up * _speed * Time.deltaTime;

        else if (_isRicochet == true)
        {
            _rigidBody.position += (Vector2.up * (-5f) + Vector2.right * _ricochetOffset) * Time.deltaTime;

            _deltaTime =_deltaTime / 4f + Time.deltaTime;

            _rigidBody.SetRotation(Mathf.Lerp(_rigidBody.rotation, _rotation, _deltaTime));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Wood"))
        {
            _speed = 0f;

            _isStuck = true;

            transform.parent = collision.transform;

            collision.transform.GetComponent<Wood>().HitImpact();

            _rigidBody.bodyType = RigidbodyType2D.Static;
        }

        else if (collision.transform.CompareTag("Knife") && _isStuck == false)
        {
            ContactPoint2D contactPoint = collision.contacts[0];

            Vector2 vfxHitPosition = contactPoint.point;

            if(VFX_Hit!=null)
            {
                GameObject vfxHit = Instantiate(VFX_Hit, vfxHitPosition, Quaternion.identity);

                ParticleSystem ps = vfxHit.GetComponent<ParticleSystem>();
                if (ps == null)
                {
                    ParticleSystem psChild = vfxHit.transform.GetChild(0).GetComponent<ParticleSystem>();
                    Destroy(vfxHit, psChild.main.duration);
                }
                else
                    Destroy(vfxHit, ps.main.duration);
            }

            _rotation = Random.Range(-540f,-180f );
            _ricochetOffset = Random.Range(0f, 3f);

            _isRicochet = true;

            GameController.Instance.IsGameStarted = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameController.Instance.Apples += collision.GetComponent<Apple>().AppleCost;
        Destroy(collision.gameObject);
        UIManager.Instance.UpdateAppleCounter();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeMove : MonoBehaviour
{
    private float _speed = 0f;
    private float _force = 0f;
    Rigidbody2D _rigidBody;


    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //_rigidBody.position += Vector2.up * _speed * Time.deltaTime;
        _rigidBody.AddForce(Vector2.up *_force);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Wood"))
        {
            _speed = 0f;

            transform.parent = collision.transform;

            collision.transform.GetComponent<WoodRotate>().HitReaction();

            _rigidBody.bodyType = RigidbodyType2D.Static;
        }
        else if (collision.transform.CompareTag("Knife") && _speed != 0f)
        {

        }
    }

    Vector2 MoveDirection()
    {
        return Vector2.right * Random.Range(-1f, 1f);
    }
    
    public void SetSpeed(float Speed)
    {
        _speed = Speed;
    }
    public void SetForce(float Force)
    {
        _force = Force;
    }
}

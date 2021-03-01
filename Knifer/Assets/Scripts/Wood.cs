using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour
{
    public SpriteRenderer WoodSkin;
    public GameObject WoodSkinMask;
    public GameObject ApplePrefab;
    public GameObject KnifePrefab;
    public Material WoodCrackMaterial;

    public int KnifeCount;

    Animator _animator;
    StageSettingsData _stageSettings;
    bool _readyToAnimation;
    float _deltaTime=0f;
    float _baseSpeed;
    float _targetSpeed;

    public void SetWood()
    {
        _readyToAnimation = false;
        _animator = GetComponent<Animator>();

        _stageSettings = GameController.Instance.CurrentStageSettings;

        WoodCrackMaterial.mainTexture = _stageSettings.WoodParts;
        _baseSpeed = _stageSettings.WoodRotationSpeed;
        _targetSpeed = 0;

        if (WoodSkin != null)
            WoodSkin.sprite = _stageSettings.WoodSkin;
        if (WoodSkinMask != null)
            WoodSkinMask.GetComponent<SpriteMask>().sprite = _stageSettings.WoodSkin;

        StartCoroutine(ReadyToAnimRoutine());

        SpawnObject(KnifePrefab, _stageSettings.KnifesInWoodMinCount, _stageSettings.KnifesInWoodMaxCount, _stageSettings.AppleAppearChance);
        SpawnObject(ApplePrefab, _stageSettings.AppleAppearChance);

        StartCoroutine(RotateWoodRoutine());
    }

    IEnumerator RotateWoodRoutine()
    {
        while(GameController.Instance.IsGameStarted)
        {
            RotateWood();

            yield return null;
        }
    }

    void RotateWood()
    {
        _deltaTime += Time.deltaTime / _stageSettings.TimeOfRotationSlowing;

        if (_deltaTime >= 1f)
        {
            _deltaTime = 0f;
            ChangeRotationDirection();
        }

        transform.Rotate(0, 0, Mathf.Lerp(_baseSpeed, _targetSpeed, _deltaTime));
    }

    void ChangeRotationDirection()
    {
        float chanceToChangeDirection = Random.Range(0, 100f);
        
        float tmp = _baseSpeed;
        _baseSpeed = _targetSpeed;

        if (chanceToChangeDirection >= 50f)
            _targetSpeed = -tmp;
        else
            _targetSpeed = tmp;
    }

    public void HitImpact()
    {
        if (_readyToAnimation)
            _animator.SetTrigger("Hit");
    }


    void SpawnObject(GameObject Prefab, int minCount = 1, int maxCount = 3, float spawnRate = 100f)
    {
        int count = Random.Range(minCount, maxCount);

        for (int i = 0; i <= count; i++)
        {
            float safeOffset = 0.5f;

            float spawnChance = Random.Range(spawnRate * i / count, spawnRate);

            if (spawnChance >= spawnRate/count)
            {
                float randomAngle = Random.Range((i * Mathf.PI * 2 + safeOffset) / count, Mathf.PI * 2 * (count + i - safeOffset) / count);
                float x = transform.position.x + Mathf.Cos(randomAngle);
                float y = transform.position.y + Mathf.Sin(randomAngle);

                Vector2 spawnPosition = new Vector2(x, y);

                if (!CheckSpawnPosition(spawnPosition) && Prefab != null)
                {
                    GameObject objectSpawned = Instantiate(Prefab, spawnPosition, transform.rotation, transform);

                    objectSpawned.transform.LookAt((Vector2)transform.position);
                    objectSpawned.transform.Rotate(0, -90, -90);
                }
            }
        }
    }
    void SpawnObject(GameObject Prefab, float spawnRate = 25)
    {
        float spawnChance = Random.Range(0f, 100f);

        if (spawnChance <= spawnRate)
        {
            float randomAngle = Random.Range(0, Mathf.PI * 2);
            float x = transform.position.x + Mathf.Cos(randomAngle);
            float y = transform.position.y + Mathf.Sin(randomAngle);

            Vector2 spawnPosition = new Vector2(x, y);

            if (!CheckSpawnPosition(spawnPosition) && Prefab != null)
            {
                GameObject objectSpawned = Instantiate(Prefab, spawnPosition, transform.rotation, transform);

                objectSpawned.transform.LookAt((Vector2)transform.position);
                objectSpawned.transform.Rotate(0, 90, -90);
            }
            else
            {
                SpawnObject(Prefab, spawnRate);
            }
        }
    }

    IEnumerator ReadyToAnimRoutine() //delay to ignore first spawned knifes
    {
        _readyToAnimation = false;
        yield return new WaitForSeconds(0.1f);
        _readyToAnimation = true;
    }

    bool CheckSpawnPosition(Vector2 checkPosition)
    {
        return Physics2D.OverlapCircle(checkPosition, 0.2f);
    }

    public void DestroyWood(float delay)
    {    
        if (transform.GetComponentInChildren<Apple>() != null)
        {
            GameObject apple = FindObjectOfType<Apple>().gameObject;
            apple.transform.parent = null;
            VFX_Manager.Instance.PlayAppleCut(apple.transform.position);
            Destroy(apple.gameObject);
        }
        StartCoroutine(DestroyWoodRoutine(delay));
    }

    IEnumerator DestroyWoodRoutine(float delay)
    {
        Destroy(WoodSkinMask.gameObject);

        foreach (var Child in transform.GetComponentsInChildren<Rigidbody2D>())
        {
            Child.bodyType = RigidbodyType2D.Dynamic;
            Child.gravityScale = 1f;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(0) != null)
            {
                Destroy(transform.GetChild(0).gameObject, delay);
                transform.GetChild(0).parent = null;
            }
        }

        yield return new WaitForSeconds(0.1f);

        GameController.Instance.IsStageComplited = true;

        Destroy(gameObject);
    }
}

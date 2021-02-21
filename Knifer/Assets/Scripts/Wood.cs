using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour
{
    public SpriteRenderer WoodSkin;
    public SpriteMask WoodSkinMask;
    public GameObject ApplePrefab;
    public GameObject KnifePrefab;

    Sprite _woodSkin;
    Animator _animator;
    StageSettingsData _stageSettings;
    bool _readyToAnimation;

    private void Start()
    {
        _readyToAnimation = false;
        _animator = GetComponent<Animator>();

        SetWood();
    }

    void Update()
    {
        transform.Rotate(0, 0, _stageSettings.WoodRotationSpeed);
    }

    public void HitImpact()
    {
        if (_readyToAnimation)
            _animator.SetTrigger("Hit");
    }

    public void SetWood()
    {
        _stageSettings = GameController.Instance.CurrentStageSettings;

        if (WoodSkin != null)
            WoodSkin.sprite = _stageSettings.WoodSkin;
        if (WoodSkinMask != null)
            WoodSkinMask.sprite = _stageSettings.WoodSkin;

        StartCoroutine(ReadyToAnimRoutine());

        SpawnObject(KnifePrefab, _stageSettings.KnifesInWoodMinCount, _stageSettings.KnifesInWoodMaxCount, 100);
        SpawnObject(ApplePrefab, _stageSettings.AppleAppearChance);
    }

    void SpawnObject(GameObject Prefab, int minCount = 0, int maxCount = 3, float spawnRate = 100)
    {
        int count = Random.Range(minCount, maxCount);

        for (int i = 0; i <= count; i++)
        {
            float safeOffset = 0.5f;

            float spawnChance = Random.Range(0f, 100f);

            if (spawnChance <= spawnRate)
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
}

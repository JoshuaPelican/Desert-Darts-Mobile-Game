using UnityEngine;

public class SpineSpawnManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] Vector2 spawnWidthRange = new Vector2(-5, 5);
    [SerializeField] float spawnHeight = 5;
    [Space()]
    [SerializeField] [Min(0.05f)] Vector2 spawnRateRange = new Vector2(1, 10);

    [SerializeField] GameObject basicSpine;

    float t;
    float spawnRate;
    Vector2 randomPosition;

    private void Start()
    {
        ResetSpawnVars();
    }

    private void Update()
    {
        t += Time.deltaTime;

        if(t >= spawnRate)
        {
            SpawnSpineRandomly();
            ResetSpawnVars();
        }
    }

    private void SpawnSpineRandomly()
    {
        float randX = Random.Range(spawnWidthRange.x, spawnWidthRange.y);

        randomPosition = new Vector2(randX, spawnHeight);

        Instantiate(basicSpine, randomPosition, Quaternion.identity);
    }

    private void ResetSpawnVars()
    {
        t = 0;
        spawnRate = Random.Range(spawnRateRange.x, spawnRateRange.y);
    }
}

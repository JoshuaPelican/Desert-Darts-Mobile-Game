using System.Collections;
using UnityEngine;

public class SpineSpawnManager : MonoBehaviour
{
    #region Singleton

    public static SpineSpawnManager instance;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    #endregion

    [Header("Spawn Settings")]
    [SerializeField] float spineGravity = -5f;
    [SerializeField] Vector2 spawnWidthRange = new Vector2(-5, 5);
    [SerializeField] float spawnHeight = 5;

    float intensity = 0;
    [SerializeField] float intensityStrength = 2;

    [Header("Object Assignments")]
    [SerializeField] GameObject basicSpine;

    [Header("Patterns")]
    [SerializeField] SpinePattern startPattern;
    [SerializeField] SpinePattern[] patternDatabase;

    bool spawningPattern;

    private void Start()
    {
        StartCoroutine(SpawnSpinePattern(startPattern));
    }

    private void Update()
    {
        float modGravity = Mathf.Lerp(spineGravity / (intensityStrength * 0.5f), spineGravity * (intensityStrength * 0.5f), intensity);
        Physics2D.gravity = new Vector2(0, modGravity);

        if (!spawningPattern && patternDatabase.Length > 0)
        {
            SpinePattern nextSpinePattern = patternDatabase[Random.Range(0, patternDatabase.Length)];
            StartCoroutine(SpawnSpinePattern(nextSpinePattern));
        }
    }

    private IEnumerator SpawnSpinePattern(SpinePattern pattern)
    {
        spawningPattern = true;

        float modStartDelay = Mathf.Lerp(pattern.startDelay * intensityStrength, pattern.startDelay / (intensityStrength * 0.75f), intensity);
        yield return new WaitForSeconds(modStartDelay);

        for (int i = 0; i < pattern.burstCount; i++)
        {
            float randX = Random.Range(spawnWidthRange.x, spawnWidthRange.y);
            Vector2 randomPosition = new Vector2(randX, spawnHeight);

            Vector2 angledDirection = Vector2.down;
            if (pattern.angled)
            {
                float targetPos = Random.Range(spawnWidthRange.x, spawnWidthRange.y);
                angledDirection = new Vector2(targetPos, -spawnHeight) - randomPosition;
            }

            for (int j = 0; j < pattern.spinesPerBurst; j++)
            {
                SpawnSpine(randomPosition, angledDirection);

                float modSpineDelay = Mathf.Lerp(pattern.spineDelay * (intensityStrength * 1.5f), pattern.spineDelay / (intensityStrength * 1.5f), intensity);
                yield return new WaitForSeconds(modSpineDelay);
            }
        }

        spawningPattern = false;
    }

    private void SpawnSpine(Vector2 position, Vector2 direction)
    {
        GameObject newSpine = Instantiate(basicSpine, position, Quaternion.identity, transform);
        newSpine.transform.up = -direction;
    }

    public void AdjustIntensity(MathUtility.Operation operation, float amount)
    {
        intensity = MathUtility.ApplyOperation(operation, amount, intensity);
        intensity = Mathf.Clamp01(intensity);
    }
}

using System.Collections;
using Unity.VisualScripting;
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
    [SerializeField] float baseSpineGravity = -5f;
    [SerializeField] Vector2 spawnWidthRange = new Vector2(-5, 5);
    [SerializeField] float spawnHeight = 5;
    [SerializeField] int maxIterations = 50;

    float intensity = 0;
    float intensityStrength = 2;
    float intensityGainMultiplier = 1;
    float intensityTimeFactor = 0.125f;

    float gravityIntensityMultiplier = 0.75f;
    float startDelayIntensityMultiplier = 1.5f;
    float spineDelayIntensityMultiplier = 1.5f;

    [Header("Object Assignments")]
    [SerializeField] GameObject basicSpine;

    [Header("Patterns")]
    [SerializeField] SpinePattern startPattern;
    [SerializeField] SpinePattern[] patternDatabase;

    bool spawningPattern;

    public void StartSpawning()
    {
        StopAllCoroutines();
        StartCoroutine(SpawnSpinePattern(startPattern)); //Start the spines spawning when the scene loads
    }

    private void Update() 
    {
        AdjustIntensity(MathUtility.Operation.Add, Time.deltaTime * intensityTimeFactor); //Over time the intensity increases at a fixed rate

        //Debug.Log(intensity);

        float modGravity = CalculateIntensityWeightedValue(baseSpineGravity, MathUtility.Operation.Multiply, gravityIntensityMultiplier);
        Physics2D.gravity = new Vector2(0, modGravity); //Set the global gravity to its modifed value based on intensity

        if (!spawningPattern && patternDatabase.Length > 0) //If we are not spawning a pattern currently, and can find one to spawn, then choose a random weighted one and start it up
        {
            SpinePattern nextSpinePattern = RandomWeightedSpinePattern();
            StartCoroutine(SpawnSpinePattern(nextSpinePattern));
        }
    }

    private SpinePattern RandomWeightedSpinePattern()
    {
        SpinePattern randPattern = patternDatabase[Random.Range(0, patternDatabase.Length)];
        bool inRange = intensity >= randPattern.intensityRange.x && intensity <= randPattern.intensityRange.y;
        int i = 0;

        while (!inRange && i < maxIterations)
        {
            i++;
            randPattern = patternDatabase[Random.Range(0, patternDatabase.Length)];
            inRange = intensity > randPattern.intensityRange.x && intensity < randPattern.intensityRange.y;
        }

        if (i >= maxIterations)
            randPattern = patternDatabase[0];

        return randPattern;
    }

    public void ClearCurrentPattern() //Removes the current pattern and stops it from spawning
    {
        StopAllCoroutines();
        spawningPattern = false;
    }

    private IEnumerator SpawnSpinePattern(SpinePattern pattern) //Spawns spine patterns over time
    {
        spawningPattern = true; //Let the spawner know we are currently spawning something

        float modStartDelay = CalculateIntensityWeightedValue(pattern.startDelay, MathUtility.Operation.Divide, startDelayIntensityMultiplier);
        yield return new WaitForSeconds(modStartDelay); //Wait for the modified start delay in seconds

        for (int i = 0; i < pattern.burstCount; i++) //Repeat per burst of spines
        {
            float randX = Random.Range(spawnWidthRange.x, spawnWidthRange.y); //Get a random x position within the specified range
            Vector2 randomPosition = new Vector2(randX, spawnHeight); //Make this the position by adding the determined spawn height

            Vector2 direction = Vector2.down; //Default the direction to down
            if (pattern.angled) //If the spine is meant to be angled, randomly choose a position on the base of the screen and angle the spine in that direction; This prevents directions that move offscreen
            {
                float targetPos = Random.Range(spawnWidthRange.x, spawnWidthRange.y);
                direction = new Vector2(targetPos, -spawnHeight) - randomPosition;
            }

            for (int j = 0; j < pattern.spinesPerBurst; j++) //Repeat for each spine in a burst
            {
                SpawnSpine(randomPosition, direction); //Spawn the spine using this new position and direction

                float modSpineDelay = CalculateIntensityWeightedValue(pattern.spineDelay, MathUtility.Operation.Divide, spineDelayIntensityMultiplier);
                yield return new WaitForSeconds(modSpineDelay); //Delay the next spine by the pattern's amount in seconds, modified by the intensity
            }
        }

        spawningPattern = false; //Let the spawner know we are done spawning
    }

    private void SpawnSpine(Vector2 position, Vector2 direction) //Instantiate the spine and set its position and direction
    {
        GameObject newSpine = Instantiate(basicSpine, position, Quaternion.identity, transform);
        newSpine.transform.up = -direction;
    }

    public void AdjustIntensity(MathUtility.Operation operation, float amount) //Adjust the intensity by an amount and an operation
    {

        if (operation == MathUtility.Operation.Add)
            intensity = MathUtility.ApplyOperation(operation, intensity, amount * intensityGainMultiplier);
        else
            intensity = MathUtility.ApplyOperation(operation, intensity, amount);


        intensity = Mathf.Clamp01(intensity);
    }

    private float CalculateIntensityWeightedValue(float currentValue, MathUtility.Operation operation, float intensityMultiplier) //Using the current intensity, calculate how a value will change over time based on a multiplier and an operation
    {
        float modifiedValue = MathUtility.ApplyOperation(operation, currentValue, intensityStrength * intensityMultiplier);

        return Mathf.Lerp(currentValue, modifiedValue, intensity);
    }

    public void SetDifficulty(Difficulty difficulty)
    {
        intensityStrength = difficulty.intensityStrength;
        intensityGainMultiplier = difficulty.intensityGainMultiplier;
        intensityTimeFactor = difficulty.intensityTimeFactor;

        gravityIntensityMultiplier = difficulty.gravityIntensityMultiplier;
        startDelayIntensityMultiplier = difficulty.startDelayIntensityMultiplier;
        spineDelayIntensityMultiplier = difficulty.spineDelayIntensityMultiplier;
    }
}

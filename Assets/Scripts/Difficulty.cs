using UnityEngine;

[CreateAssetMenu(fileName = "New Difficulty", menuName = "Difficulty")]
public class Difficulty : ScriptableObject
{
    [Header("Gameplay Settings")]
    public int maxLives = 3;
    [Space()]
    [Range(0.5f, 2)] public float targetScale = 1;

    [Header("Intensity Settings")]
    public float intensityStrength = 1.75f;
    [Range(0, 0.02f)] public float intensityTimeFactor = 0.01f;

    [Space()]

    public float gravityIntensityMultiplier = 0.75f;
    public float startDelayIntensityMultiplier = 1.25f;
    public float spineDelayIntensityMultiplier = 1.5f;
}

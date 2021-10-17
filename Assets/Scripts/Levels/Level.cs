using UnityEngine;

[CreateAssetMenu(fileName = "Level 1", menuName = "Level")]
public class Level : ScriptableObject
{
    public int levelNumber;

    public int intensityStrength;
    public SpinePattern[] spinePatterns;

    public int highScore;
}

using UnityEngine;

[CreateAssetMenu(fileName = "New Spine Pattern", menuName = "Spine Pattern")]
public class SpinePattern : ScriptableObject
{
    [Header("Pattern Settings")]
    [Min(1)] public int burstCount = 10;
    public float startDelay = 1f;
    [Space()]
    public bool angled;

    [Header("Spawn Settings")]
    public int spinesPerBurst = 1;
    public float spineDelay = 0.5f;
}
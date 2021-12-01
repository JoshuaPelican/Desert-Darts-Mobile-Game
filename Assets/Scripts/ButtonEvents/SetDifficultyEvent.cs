using UnityEngine;

public class SetDifficultyEvent : MonoBehaviour
{
    public void SetDifficulty(Difficulty difficulty)
    {
        GameManager.instance.difficulty = difficulty;
    }
}

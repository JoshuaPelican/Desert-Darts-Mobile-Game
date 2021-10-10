using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager instance;

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

        DontDestroyOnLoad(gameObject);
    }

    #endregion

    float totalPoints = 0;
    float pointsMultiplier = 1;



    public void AddPoints(float addedPoints, bool useMultiplier = true)
    {
        if (useMultiplier)
        {
            addedPoints *= pointsMultiplier;
        }

        totalPoints += addedPoints;
    }

    public void LosePoints(float lostPoints, bool useMultiplier = false)
    {
        if (useMultiplier)
        {
            lostPoints *= pointsMultiplier;
        }

        totalPoints -= lostPoints; 
    }
}

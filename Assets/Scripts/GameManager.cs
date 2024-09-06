using UnityEngine;
using UnityEngine.SceneManagement;

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

    [Header("Gameplay Settings")]

    [Header("Lives")]
    [SerializeField] GameObject heartPrefab;
    int currentLives;

    [HideInInspector] public Difficulty difficulty;

    [Header("Points Settings")]
    float totalPoints = 0;
    float pointsMultiplier = 1;
    float multiplierProgress = 1;

    [Header("Floating Text Settings")]
    [SerializeField] GameObject floatingText;
    [Space()]
    [SerializeField] Vector2 positionRange = new Vector2(-0.2f, 0.2f);
    [SerializeField] Vector2 rotationRange = new Vector2(-20, 20);
    [Space()]
    [SerializeField] float pointsScaleMax = 25f;
    [SerializeField] float scaleMax = 2f;

    [Header("Audio Settings")]
    [SerializeField] AudioClip missedSpineClip;
    AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();

        SceneManager.sceneLoaded += StartGame;

        Application.targetFrameRate = 59;
    }

    private void StartGame(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(scene.name == "Main")
        {
            currentLives = difficulty.maxLives;
            totalPoints = 0;
            pointsMultiplier = 1;
            multiplierProgress = 1;

            for (int i = 0; i < currentLives; i++)
                UIManager.instance.AddChildToPanel(UIManager.PanelType.Hearts, heartPrefab);

            SpineSpawnManager.instance.SetDifficulty(difficulty);
            SpineSpawnManager.instance.StartSpawning();

            //Debug.Log("Highscore: " + SaveDataManager.instance.CurrentSaveData.Highscores[0]);
            UIManager.instance.SetValueTextMesh(UIManager.TextType.Highscore, SaveDataManager.instance.CurrentSaveData.Highscore);

            foreach (GameObject targetSection in GameObject.FindGameObjectsWithTag("TargetSection"))
            {
                targetSection.transform.localScale = new Vector3(targetSection.transform.localScale.x * difficulty.targetScale, targetSection.transform.localScale.y, targetSection.transform.localScale.z);
            }
        }
    }

    // Takes in a a point value and a bool for if using the global multiplier
    // Adds the point value to the total points count and uses multiplier if required
    public void ApplyPoints(float value, MathUtility.Operation operation, bool applyMultiplier)
    {
        if (applyMultiplier)
            value *= pointsMultiplier;

        totalPoints = MathUtility.ApplyOperation(operation, totalPoints, value);
        UIManager.instance.SetValueTextMesh(UIManager.TextType.Points, totalPoints);

        if(totalPoints > SaveDataManager.instance.CurrentSaveData.Highscore)
            UIManager.instance.SetValueTextMesh(UIManager.TextType.Highscore, totalPoints);
    }

    // Takes in a a point value and a bool for if using the global multiplier. This version also takes in a position and color used for floating text popup
    // Adds the point value to the total points count and uses multiplier if required. Places floating text at the desired position and gives it the desired color.
    public void ApplyPoints(float value, MathUtility.Operation operation, bool applyMultiplier, Vector3 floatingTextPosition, Color floatingTextColor)
    {
        ApplyPoints(value, operation, applyMultiplier);

        if (applyMultiplier)
            value *= pointsMultiplier;

        float pointsScale = ((value / pointsScaleMax) * scaleMax) + 1;
        string pointsString = "";

        switch (operation)
        {
            case MathUtility.Operation.Add:
                pointsString = "+ ";
                break;
            case MathUtility.Operation.Subtract:
                pointsString = "- ";
                break;
            case MathUtility.Operation.Multiply:
                pointsString = "x ";
                break;
            case MathUtility.Operation.Divide:
                pointsString = "/ ";
                break;
            default:
                break;
        }

        pointsString += value.ToString("F0");

        DisplayFloatingText(pointsString, floatingTextPosition, pointsScale, floatingTextColor);
    }

    //Takes in a position, a base scale miltiplier and a text string
    //Places floating text at the position with the givent text string. Randomly adjusts position, rotation and scales the text based on global random ranges
    public void DisplayFloatingText(string text, Vector2 position, float scale, Color color)
    {
        Vector2 randPos = new Vector2(Random.Range(positionRange.x, positionRange.y), Random.Range(positionRange.x, positionRange.y)) + position;
        Quaternion randRot = Quaternion.Euler(0, 0, Random.Range(rotationRange.x, rotationRange.y));

        GameObject newFloatingTextObject = Instantiate(floatingText, randPos, randRot);
        newFloatingTextObject.transform.localScale *= scale;

        FloatingText newFloatingText = newFloatingTextObject.GetComponent<FloatingText>();
        newFloatingText.SetText(text, color);
    }

    public void MissedSpine()
    {
        SpineSpawnManager.instance.AdjustIntensity(MathUtility.Operation.Multiply, difficulty.missIntensityMultiplier);
        SpineSpawnManager.instance.ClearCurrentPattern();

        ClearAllSpines();

        AudioUtility.RandomizeSourceAndPlay(missedSpineClip, source, 0.65f, 1, 0.05f);

        ChangeMultiplierProgress(-multiplierProgress + 1);
        CalculatePointMultiplier();
        UIManager.instance.SetValueTextMesh(UIManager.TextType.Multiplier, pointsMultiplier, "x");

        AdjustLives(MathUtility.Operation.Subtract, 1);
    }

    public void ClearAllSpines()
    {
        Spine[] allSpines = FindObjectsOfType<Spine>();

        foreach (Spine spine in allSpines)
        {
            spine.Clear();
        }
    }

    public void ChangeMultiplierProgress(float progress)
    {
        multiplierProgress += progress;
        CalculatePointMultiplier();
    }

    private void CalculatePointMultiplier()
    {
        pointsMultiplier = Mathf.RoundToInt(Mathf.Sqrt(multiplierProgress));
        UIManager.instance.SetValueTextMesh(UIManager.TextType.Multiplier, pointsMultiplier, "x");
    }

    private void CheckHighscoreAndSave(float points)
    {
        if (points <= SaveDataManager.instance.CurrentSaveData.Highscore)
            return;

        SaveDataManager.instance.CurrentSaveData.SetHighscore(points);
    }

    private void AdjustLives(MathUtility.Operation operation, int amount)
    {
        int oldLives = currentLives;

        currentLives = MathUtility.ApplyOperation(operation, currentLives, amount);

        int changeInLives = oldLives - currentLives;

        if (currentLives <= 0)
        {
            GameOver();
        }

        if (changeInLives > 0)
        {
            for (int i = 0; i < changeInLives; i++)
            {
                UIManager.instance.RemoveChildFromPanel(UIManager.PanelType.Hearts);
            }
        }
    }

    private void GameOver()
    {
        CheckHighscoreAndSave(totalPoints);

        //Game Over Screen
        UIManager.instance.PauseGame(true, true, UIManager.PanelType.GameEnd);
    }

    public void SetDifficulty(Difficulty difficultyToSet)
    {
        difficulty = difficultyToSet;
    }
}

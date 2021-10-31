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

    [Header("Points Settings")]
    [SerializeField] [Range(0, 1)]float missIntensityMultiplier = 0.667f;
    float totalPoints = 0;
    float pointsMultiplier = 1;
    float multiplierProgress = 1;

    SaveDataManager saveDataManager;

    [Header("Floating Text Settings")]
    [SerializeField] GameObject floatingText;
    [Space()]
    [SerializeField] Vector2 positionRange = new Vector2(-0.2f, 0.2f);
    [SerializeField] Vector2 rotationRange = new Vector2(-20, 20);
    [Space()]
    [SerializeField] float pointsScaleMax = 25f;
    [SerializeField] float scaleMax = 2f;

    UIManager uiManager;

    [Header("Audio Settings")]
    [SerializeField] AudioClip missedSpineClip;
    AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        uiManager = UIManager.instance;
        saveDataManager = SaveDataManager.instance;

        SceneManager.sceneLoaded += StartGame;
    }

    private void StartGame(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(scene.name == "Main")
        {
            SpineSpawnManager.instance.StartSpawning();
        }
    }

    // Takes in a a point value and a bool for if using the global multiplier
    // Adds the point value to the total points count and uses multiplier if required
    public void ApplyPoints(float value, MathUtility.Operation operation, bool applyMultiplier)
    {
        if (applyMultiplier)
            value *= pointsMultiplier;

        totalPoints = MathUtility.ApplyOperation(operation, value, totalPoints);
        uiManager.SetValueTextMesh(UIManager.TextType.Points, totalPoints);
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
        SpineSpawnManager.instance.AdjustIntensity(MathUtility.Operation.Multiply, missIntensityMultiplier);
        SpineSpawnManager.instance.ClearCurrentPattern();

        ClearAllSpines();

        AudioUtility.RandomizeSourceAndPlay(missedSpineClip, source, 0.65f, 1, 0.05f);

        ChangeMultiplierProgress(-multiplierProgress + 1);
        CalculatePointMultiplier();
        uiManager.SetValueTextMesh(UIManager.TextType.Multiplier, pointsMultiplier, "x");

        CheckHighscoreAndSort(totalPoints);
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
        uiManager.SetValueTextMesh(UIManager.TextType.Multiplier, pointsMultiplier, "x");
    }

    private void CheckHighscoreAndSort(float totalPoints)
    {
        float[] highscores = saveDataManager.Highscores;

        int index = 0;

        for (int i = 0; i < highscores.Length; i++)
        {
            if(totalPoints > highscores[i])
            {
                index = i;
                break;
            }
        }

        int j = 0;

        for (int i = 0; i < highscores.Length; i++)
        {
            if(i == index)
            {
                highscores[i] = totalPoints;
            }
            else
            {
                highscores[i] = saveDataManager.Highscores[j];
                j++;
            }
        }

        Debug.Log(highscores[0] + " ," + highscores[1] + " ," + highscores[2] + " ," + highscores[3] + " ," + highscores[4]);
        saveDataManager.Highscores = highscores;
    }
}

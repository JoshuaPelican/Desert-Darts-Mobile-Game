using UnityEngine;
using TMPro;

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

        //DontDestroyOnLoad(gameObject);
    }

    #endregion

    [Header("Timer Settings")]
    [SerializeField] float maxTimerDuration;
    float currentTimer;
    [SerializeField] TextMeshProUGUI timerTextMesh;

    float totalPoints = 0;
    float pointsMultiplier = 1;

    float multiplierProgress = 1;

    [Header("Points Settings")]
    [SerializeField] [Range(0, 1)]float missIntensityMultiplier = 0.667f;
    [SerializeField] TextMeshProUGUI pointsTextMesh;
    [SerializeField] TextMeshProUGUI multiplierTextMesh;

    [Header("Floating Text Settings")]
    [SerializeField] GameObject floatingText;
    [Space()]
    [SerializeField] Vector2 positionRange = new Vector2(-0.2f, 0.2f);
    [SerializeField] Vector2 rotationRange = new Vector2(-20, 20);
    [Space()]
    [SerializeField] float pointsScaleMax = 25f;
    [SerializeField] float scaleMax = 2f;

    [Header("Pause Settings")]
    [SerializeField] GameObject pausedPanel;
    bool paused;

    [Header("Audio Settings")]
    [SerializeField] AudioClip missedSpineClip;
    AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        //currentTimer = maxTimerDuration + Time.deltaTime;
        //UpdateTimer();
    }

    private void Update()
    {
        if(Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Ended && !paused)
            {
                //Pause the game
                paused = true;
                pausedPanel.SetActive(true);
                Time.timeScale = 0;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended && paused)
            {
                //Unpause the game
                Time.timeScale = 1;
                pausedPanel.SetActive(false);
                paused = false;
            }
        }

        //UpdateTimer();
    }

    /*
    private void UpdateTimer()
    {
        currentTimer -= Time.deltaTime;

        if(currentTimer <= 0)
        {

        }

        int minutes = Mathf.FloorToInt(currentTimer / 60);
        int seconds = Mathf.FloorToInt(currentTimer % 60);

        timerTextMesh.SetText(minutes + ":" + seconds);
    }
    */

    // Takes in a a point value and a bool for if using the global multiplier
    // Adds the point value to the total points count and uses multiplier if required
    public void ApplyPoints(float value, MathUtility.Operation operation, bool applyMultiplier)
    {
        if (applyMultiplier)
        {
            value *= pointsMultiplier;
        }

        totalPoints = MathUtility.ApplyOperation(operation, value, totalPoints);

        UpdateValueText(pointsTextMesh, totalPoints);
    }

    // Takes in a a point value and a bool for if using the global multiplier. This version also takes in a position and color used for floating text popup
    // Adds the point value to the total points count and uses multiplier if required. Places floating text at the desired position and gives it the desired color.
    public void ApplyPoints(float value, MathUtility.Operation operation, bool applyMultiplier, Vector3 floatingTextPosition, Color floatingTextColor)
    {
        ApplyPoints(value, operation, applyMultiplier);

        if (applyMultiplier)
        {
            value *= pointsMultiplier;
        }

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

    private void UpdateValueText(TextMeshProUGUI textMesh, float value, string prefix = "", string suffix = "", string valueFormat = "F0")
    {
        textMesh.text = prefix + value.ToString(valueFormat) + suffix;
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
        UpdateValueText(multiplierTextMesh, pointsMultiplier, "x");
    }

    public void ClearAllSpines()
    {
        Spine[] allSpines = FindObjectsOfType<Spine>();

        foreach (Spine spine in allSpines)
        {
            spine.ClearSpine();
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
        UpdateValueText(multiplierTextMesh, pointsMultiplier, "x");
    }
}

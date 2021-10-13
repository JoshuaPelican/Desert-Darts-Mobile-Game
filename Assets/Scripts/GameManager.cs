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
    public enum Operation
    {
        Add,
        Subtract,
        Multiply,
        Divide
    }

    [Header("Floating Text Settings")]
    [SerializeField] GameObject floatingText;
    [Space()]
    [SerializeField] Vector2 positionRange = new Vector2(-0.2f, 0.2f);
    [SerializeField] Vector2 rotationRange = new Vector2(-20, 20);
    [Space()]
    [SerializeField] float pointsScaleMax = 25f;
    [SerializeField] float scaleMax = 2f;

    public void ApplyPoints(float value, Operation operation, bool applyMultiplier)
    {
        if (applyMultiplier)
        {
            value *= pointsMultiplier;
        }

        switch (operation)
        {
            case Operation.Add:
                totalPoints += value;
                break;
            case Operation.Subtract:
                totalPoints -= value;
                break;
            case Operation.Multiply:
                totalPoints *= value;
                break;
            case Operation.Divide:
                totalPoints /= value;
                break;
        }
    }

    public void ApplyPoints(float value, Operation operation, bool applyMultiplier, Vector3 floatingTextPosition, Color floatingTextColor)
    {
        ApplyPoints(value, operation, applyMultiplier);

        float pointsScale = ((value / pointsScaleMax) * scaleMax) + 1;
        string pointsString = "";

        switch (operation)
        {
            case Operation.Add:
                pointsString = "+ ";
                break;
            case Operation.Subtract:
                pointsString = "- ";
                break;
            case Operation.Multiply:
                pointsString = "x ";
                break;
            case Operation.Divide:
                pointsString = "/ ";
                break;
            default:
                break;
        }

        pointsString += value.ToString("F0");

        DisplayFloatingText(pointsString, floatingTextPosition, pointsScale, floatingTextColor);
    }

    /*
    // Takes in a position (used for floating text), a point value and a bool for if using the global multiplier
    // Adds the point value to the total points count and uses multiplier if required. Places floating text at the desired position
    public void AddPoints(Vector2 floatingPos, float addedPoints, bool useMultiplier = true)
    {
        if (useMultiplier)
        {
            addedPoints *= pointsMultiplier;
        }

        totalPoints += addedPoints;

        string addePointsString = "+" + addedPoints.ToString("F1");
        float addedPointsScale = ((addedPoints / pointsScaleMax) * scaleMax) + 1;

        DisplayFloatingText(floatingPos, addedPointsScale, addePointsString);
    }

    // Takes in a position (used for floating text), a point value and a bool for if using the global multiplier
    // Subtracts the point value to the total points count and uses multiplier if required. Places floating text at the desired position
    public void LosePoints(Vector2 floatingPos, float lostPoints, bool useMultiplier = false)
    {
        if (useMultiplier)
        {
            lostPoints *= pointsMultiplier;
        }

        totalPoints -= lostPoints;

        string lostPointsString = "-" + lostPoints.ToString("F1");
        float lostPointsScale = ((lostPoints / -pointsScaleMax) * scaleMax) + 1;

        DisplayFloatingText(floatingPos, lostPointsScale,lostPointsString);
    }
    */

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
}

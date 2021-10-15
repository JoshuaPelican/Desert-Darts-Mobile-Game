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

        DontDestroyOnLoad(gameObject);
    }

    #endregion

    float totalPoints = 0;
    float pointsMultiplier = 1;

    [Header("Points Settings")]
    [SerializeField] TextMeshProUGUI pointsTextMesh;

    [Header("Floating Text Settings")]
    [SerializeField] GameObject floatingText;
    [Space()]
    [SerializeField] Vector2 positionRange = new Vector2(-0.2f, 0.2f);
    [SerializeField] Vector2 rotationRange = new Vector2(-20, 20);
    [Space()]
    [SerializeField] float pointsScaleMax = 25f;
    [SerializeField] float scaleMax = 2f;

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
}

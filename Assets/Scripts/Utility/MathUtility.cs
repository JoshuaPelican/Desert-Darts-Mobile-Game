using UnityEngine;

public class MathUtility : MonoBehaviour
{
    public enum Operation
    {
        Add,
        Subtract,
        Multiply,
        Divide
    }

    public static float ApplyOperation(Operation operation, float mainValue, float secondaryValue)
    {
        switch (operation)
        {
            case Operation.Add:
                mainValue += secondaryValue;
                break;
            case Operation.Subtract:
                mainValue -= secondaryValue;
                break;
            case Operation.Multiply:
                mainValue *= secondaryValue;
                break;
            case Operation.Divide:
                mainValue /= secondaryValue;
                break;
        }
        return mainValue;
    }
}

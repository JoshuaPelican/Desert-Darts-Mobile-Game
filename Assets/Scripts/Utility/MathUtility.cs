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

    public static float ApplyOperation(Operation operation, float value1, float value2)
    {
        switch (operation)
        {
            case Operation.Add:
                value2 += value1;
                break;
            case Operation.Subtract:
                value2 -= value1;
                break;
            case Operation.Multiply:
                value2 *= value1;
                break;
            case Operation.Divide:
                value2 /= value1;
                break;
        }
        return value2;
    }

    public static int ApplyOperation(Operation operation, int value1, int value2)
    {
        switch (operation)
        {
            case Operation.Add:
                value2 += value1;
                break;
            case Operation.Subtract:
                value2 -= value1;
                break;
            case Operation.Multiply:
                value2 *= value1;
                break;
            case Operation.Divide:
                value2 /= value1;
                break;
        }
        return value2;
    }
}

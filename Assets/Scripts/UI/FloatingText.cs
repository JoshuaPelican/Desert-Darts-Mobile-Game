using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    [SerializeField] TextMeshPro floatingTextMesh;

    public void SetText(string text, Color color)
    {
        floatingTextMesh.text = text;
        floatingTextMesh.color = color;
    }
}

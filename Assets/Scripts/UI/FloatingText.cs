using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    [Header("Text Mesh Reference")]
    [SerializeField] TextMeshPro floatingTextMesh;

    public void SetText(string text, Color color) //Sets the text and color of the floating text object
    {
        floatingTextMesh.text = text;
        floatingTextMesh.color = color;
    }
}

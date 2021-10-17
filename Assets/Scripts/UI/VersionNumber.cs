using UnityEngine;
using TMPro;

public class VersionNumber : MonoBehaviour
{
    TextMeshProUGUI versionNumberTextMesh;

    private void Awake()
    {
        versionNumberTextMesh = GetComponent<TextMeshProUGUI>();
        versionNumberTextMesh.text = "Build " + Application.version;
    }
}

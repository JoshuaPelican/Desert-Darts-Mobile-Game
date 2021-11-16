using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region Simple Singleton

    public static UIManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    #endregion

    [Header("Text Mesh References")]
    [SerializeField] TextMeshProUGUI pointsTextMesh;
    [SerializeField] TextMeshProUGUI multiplierTextMesh;
    [SerializeField] TextMeshProUGUI highscoreTextMesh;

    [Space()]

    [Header("Panel References")]
    [SerializeField] GameObject pausedPanel;
    [SerializeField] GameObject gameEndPanel;

    public enum TextType
    {
        Points,
        Multiplier,
        Highscore,
    }

    public enum PanelType
    {
        Pause,
        GameStart,
        GameEnd
    }

    bool paused = false;
    bool canInput = true;

    private void Update()
    {
        if (Input.touchCount > 0)
            if (Input.GetTouch(0).phase == TouchPhase.Began && !paused)
                PauseGame(true, false, PanelType.Pause);
    }

    // Takes in a bool for pause
    // Pauses or Unpauses the game based on this bool
    public void PauseGame(bool pause, bool disableInput, PanelType panelType)
    {
        paused = pause;
        canInput = !disableInput;
        DisplayPanel(panelType, pause);
        Time.timeScale = pause ? 0 : 1;
    }

        public void PauseGame(bool pause)
    {
        paused = pause;
        DisplayPanel(PanelType.Pause, pause);
        Time.timeScale = pause ? 0 : 1;
    }

    public void SetValueTextMesh(TextType textType, float value, string prefix = "", string suffix = "", string valueFormat = "F0")
    {
        TextMeshProUGUI textMesh = null;

        switch (textType)
        {
            case TextType.Points: textMesh = pointsTextMesh;
                break;
            case TextType.Multiplier: textMesh = multiplierTextMesh;
                break;
            case TextType.Highscore: textMesh = highscoreTextMesh;
                break;
        }

        if(textMesh)
            textMesh.text = prefix + value.ToString(valueFormat) + suffix;
    }

    public void DisplayPanel(PanelType panelType, bool setActive)
    {
        GameObject panel = null;

        switch (panelType)
        {
            case PanelType.Pause: panel = pausedPanel;
                break;
            case PanelType.GameStart:
                break;
            case PanelType.GameEnd: panel = gameEndPanel;
                break;
        }

        if (panel)
            panel.SetActive(setActive);
    }
}

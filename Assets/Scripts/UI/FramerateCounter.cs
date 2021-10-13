// https://answers.unity.com/questions/46745/how-do-i-find-the-frames-per-second-of-my-game.html
using UnityEngine;
using TMPro;

public class FramerateCounter : MonoBehaviour
{
    int m_frameCounter = 0;
    float m_timeCounter = 0.0f;
    float m_lastFramerate = 0.0f;
    [SerializeField] float m_refreshTime = 0.5f;

    TextMeshProUGUI framerateCounter;

    private void Start()
    {
        framerateCounter = GetComponent<TextMeshProUGUI>();
        Application.targetFrameRate = 60;
    }


    void Update()
    {
        if (m_timeCounter < m_refreshTime)
        {
            m_timeCounter += Time.deltaTime;
            m_frameCounter++;
        }
        else
        {
            //This code will break if you set your m_refreshTime to 0, which makes no sense.
            m_lastFramerate = (float)m_frameCounter / m_timeCounter;
            m_frameCounter = 0;
            m_timeCounter = 0.0f;

            framerateCounter.text = "FPS: " + m_lastFramerate.ToString("F0");
        }
    }
}

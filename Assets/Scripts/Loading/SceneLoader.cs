using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class SceneLoader : MonoBehaviour
{
    [Header("Transition Settings")]
    [SerializeField] float transitionDuration = 1f;
    [SerializeField] GameObject sandBlast;

    Animator anim;
    [SerializeField] AudioClip transitionClip;
    [SerializeField] AudioMixer masterMixer;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void LoadScene(string sceneName) //Load a scene with a given name
    {
        StartCoroutine(SceneTransition(sceneName));
    }

    public void RestartScene() //Restart the current scene
    {
        StartCoroutine(SceneTransition(SceneManager.GetActiveScene().name));
    }

    private IEnumerator SceneTransition(string sceneName) //Plays a transition before loading the scene
    {
        GenerateTransitionAudio();

        anim.SetTrigger("StartFade");
        anim.speed = 0.75f / transitionDuration;

        yield return new WaitForSeconds(transitionDuration);
        SceneManager.LoadScene(sceneName);
    }

    private void GenerateTransitionAudio()
    {
        GameObject tempAudio = new GameObject("Gust Audio", typeof(AudioSource));
        AudioSource transitionSource = tempAudio.GetComponent<AudioSource>();
        transitionSource.outputAudioMixerGroup = masterMixer.FindMatchingGroups("Effects")[0];

        //Instantiate(tempAudio);
        GameObject tempSandBlast = Instantiate(sandBlast);
        AudioUtility.RandomizeSourceAndPlay(transitionClip, transitionSource, 0.4f, 1, 0.05f);

        DontDestroyOnLoad(tempSandBlast);
        DontDestroyOnLoad(tempAudio);
        Destroy(tempAudio, 4f);
    }
}

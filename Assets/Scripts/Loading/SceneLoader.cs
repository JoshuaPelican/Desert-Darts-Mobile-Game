using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Transition Settings")]
    [SerializeField] float transitionDuration = 1f;

    Animator anim;
    [SerializeField] AudioClip transitionClip;

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
        anim.SetTrigger("StartFade");
        anim.speed = 0.75f / transitionDuration;

        GenerateTransitionAudio();

        yield return new WaitForSeconds(transitionDuration);
        SceneManager.LoadScene(sceneName);
    }

    private void GenerateTransitionAudio()
    {
        GameObject tempAudio = new GameObject("Gust Audio", typeof(AudioSource));
        AudioSource gustSource = tempAudio.GetComponent<AudioSource>();
        gustSource.playOnAwake = false;
        gustSource.loop = false;
        gustSource.volume = 0.5f;
        gustSource.clip = transitionClip;

        Instantiate(tempAudio);
        gustSource.Play();


        DontDestroyOnLoad(tempAudio);
        Destroy(tempAudio, 4f);
    }
}

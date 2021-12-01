using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    #region Simple Singleton

    public static AudioManager instance;
    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    #endregion

    [SerializeField] AudioMixer masterMixer;

    [SerializeField] GameObject musicOnButton;
    [SerializeField] GameObject musicOffButton;
    [SerializeField] GameObject effectsOnButton;
    [SerializeField] GameObject effectsOffButton;

    private void Start()
    {
        MuteMusic(SaveDataManager.instance.CurrentSaveData.MuteMusic);
        MuteEffects(SaveDataManager.instance.CurrentSaveData.MuteEffects);
    }

    public void MuteMusic(bool mute)
    {
        SaveDataManager.instance.CurrentSaveData.MuteMusic = mute;

        musicOnButton.SetActive(!SaveDataManager.instance.CurrentSaveData.MuteMusic);
        musicOffButton.SetActive(SaveDataManager.instance.CurrentSaveData.MuteMusic);

        if (mute)
            masterMixer.FindMatchingGroups("Master/Music")[0].audioMixer.SetFloat("MusicVolume", -80);
        else
            masterMixer.FindMatchingGroups("Master/Music")[0].audioMixer.SetFloat("MusicVolume", 0);
    }

    public void MuteEffects(bool mute)
    {
        SaveDataManager.instance.CurrentSaveData.MuteEffects = mute;

        effectsOnButton.SetActive(!SaveDataManager.instance.CurrentSaveData.MuteEffects);
        effectsOffButton.SetActive(SaveDataManager.instance.CurrentSaveData.MuteEffects);

        if (mute)
            masterMixer.FindMatchingGroups("Master/Effects")[0].audioMixer.SetFloat("EffectsVolume", -80);
        else
            masterMixer.FindMatchingGroups("Master/Effects")[0].audioMixer.SetFloat("EffectsVolume", 0);
    }
}

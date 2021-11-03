using UnityEngine;
using UnityEngine.Audio;

public class AudioEvents : MonoBehaviour
{
    [SerializeField] AudioMixer masterMixer;

    public void MuteMusic(bool mute)
    {
        if(mute)
            masterMixer.FindMatchingGroups("Master/Music")[0].audioMixer.SetFloat("MusicVolume", -80);
        else
            masterMixer.FindMatchingGroups("Master/Music")[0].audioMixer.SetFloat("MusicVolume", 0);
    }

    public void MuteEffects(bool mute)
    {
        if (mute)
            masterMixer.FindMatchingGroups("Master/Effects")[0].audioMixer.SetFloat("EffectsVolume", -80);
        else
            masterMixer.FindMatchingGroups("Master/Effects")[0].audioMixer.SetFloat("EffectsVolume", 0);
    }
}

using UnityEngine;
using UnityEngine.Audio;

public class AudioEvents : MonoBehaviour
{
    [SerializeField] AudioMixer masterMixer;

    public void MuteMusic(bool mute)
    {
        if(mute)
            masterMixer.FindMatchingGroups("Music")[0].audioMixer.SetFloat("Volume", -80);
        else
            masterMixer.FindMatchingGroups("Music")[0].audioMixer.SetFloat("Volume", 0);
    }

    public void MuteEffects(bool mute)
    {
        if (mute)
            masterMixer.FindMatchingGroups("Effects")[0].audioMixer.SetFloat("Volume", -80);
        else
            masterMixer.FindMatchingGroups("Effects")[0].audioMixer.SetFloat("Volume", 0);
    }
}

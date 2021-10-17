using UnityEngine;

public static class AudioUtility
{
    public static void RandomizeSourceAndPlay(AudioClip clip, AudioSource source, float volume, float basePitch = 1f, float randomPitchAmount = 0f)
    {
        source.pitch = Random.Range(-randomPitchAmount, randomPitchAmount) + basePitch;
        source.PlayOneShot(clip, volume);
    }

    public static void RandomizeSourceAndPlay(AudioClip[] clips, AudioSource source, float volume, float basePitch = 1f, float randomPitchAmount = 0f)
    {
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        source.pitch = Random.Range(-randomPitchAmount, randomPitchAmount) + basePitch;
        source.PlayOneShot(clip, volume);
    }

    public static void ValueDrivenSourceAndPlay(AudioClip clip, AudioSource source, float volume, float basePitch, float normalizedValue)
    {
        source.pitch = basePitch * normalizedValue;
        source.PlayOneShot(clip, volume);
    }
}

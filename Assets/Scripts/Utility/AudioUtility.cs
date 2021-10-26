using UnityEngine;

public static class AudioUtility //Holds functionality for simple audio manipulation and playing clips randomly
{
    public static void RandomizeSourceAndPlay(AudioClip clip, AudioSource source, float volume, float basePitch = 1f, float randomPitchAmount = 0f) //Plays a clip with a random pitch and a set volume
    {
        source.pitch = Random.Range(-randomPitchAmount, randomPitchAmount) + basePitch;
        source.PlayOneShot(clip, volume);
    }

    public static void RandomizeSourceAndPlay(AudioClip[] clips, AudioSource source, float volume, float basePitch = 1f, float randomPitchAmount = 0f) //Plays a random clip with random pitch and a set volume
    {
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        source.pitch = Random.Range(-randomPitchAmount, randomPitchAmount) + basePitch;
        source.PlayOneShot(clip, volume);
    }

    public static void ValueDrivenSourceAndPlay(AudioClip clip, AudioSource source, float volume, float basePitch, float normalizedValue) //Plays a clip with a pitch based on the factor of a normalized value
    {
        source.pitch = basePitch * normalizedValue;
        source.PlayOneShot(clip, volume);
    }
}

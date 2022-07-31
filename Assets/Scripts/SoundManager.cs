using UnityEngine;

public static class SoundManager
{
    public static void PlaySound(AudioClip audioClip)
    {
        GameObject soundObject = new GameObject("Sound");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(audioClip);

        Object.Destroy(soundObject, audioClip.length);
    }
}

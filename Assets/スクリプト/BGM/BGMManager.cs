using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip normalBGM;
    public AudioClip bossBGM;

    void Start()
    {
        PlayNormalBGM();
    }

    public void PlayNormalBGM()
    {
        PlayMusic(normalBGM);
    }

    public void PlayBossBGM()
    {
        PlayMusic(bossBGM);
    }

    void PlayMusic(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("BGMÇ™ê›íËÇ≥ÇÍÇƒÇ¢Ç‹ÇπÇÒÅI");
            return;
        }

        if (audioSource.clip == clip && audioSource.isPlaying)
            return;

        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }
}

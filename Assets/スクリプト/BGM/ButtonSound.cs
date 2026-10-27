using UnityEngine;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public AudioSource audioSource;

    public void PlayButtonSound()
    {
        if (audioSource == null || audioSource.clip == null)
        {
            Debug.LogError("AudioSourceÇ‹ÇΩÇÕAudioClipÇ™Ç†ÇËÇ‹ÇπÇÒ");
            return;
        }

        audioSource.enabled = true;
        audioSource.mute = false;
        audioSource.volume = 1f;
        audioSource.spatialBlend = 0f;

        audioSource.PlayOneShot(audioSource.clip, 1f);

        Debug.Log("çƒê∂íÜÅF" + audioSource.isPlaying);
    }
}
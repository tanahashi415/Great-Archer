using UnityEngine;

// 音声を管理するスクリプト
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource audioSource;
    public AudioClip buySE;
    public AudioClip cantBuySE;
    public AudioClip hitSE;
    public AudioClip shotSE;
    public AudioClip coinSE;
    public AudioClip tigerSE;
    public AudioClip damageSE;
    public AudioClip gameOverSE;
    public AudioClip stageClearSE;
    public AudioClip clickSE;

    void Awake()
    {
        instance = this;
    }

    public void PlaySE(AudioClip clip, float volume)
    {
        audioSource.pitch = 1.0f;
        audioSource.PlayOneShot(clip, volume);
    }

    public void PlayHitSE(float pitch)
    {
        audioSource.pitch = pitch;
        audioSource.PlayOneShot(hitSE, 2.0f);
    }
}

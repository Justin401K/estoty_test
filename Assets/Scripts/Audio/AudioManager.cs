using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("SFX")]
    public AudioSource sfxSource;
    public AudioClip stepClip;
    public AudioClip coinClip;
    public AudioClip winClip;

    public void PlayStep()
    {
        if (stepClip != null)
        {
            sfxSource.PlayOneShot(stepClip);
        }
    }

    public void PlayCoin()
    {
        if (coinClip != null)
            sfxSource.PlayOneShot(coinClip);
    }

    public void PlayWin()
    {
        if (winClip != null)
            sfxSource.PlayOneShot(winClip);
    }
}
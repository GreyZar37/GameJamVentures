using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource musicSource;
    
    public void SwitchMusic(AudioClip clip)
    {
         musicSource.clip = clip;
         musicSource.Play();
    }
}

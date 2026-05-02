using UnityEngine;

public class AnimationSoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip[] clipsToPlay;
    public void PlaySpecificSound(AudioClip clip)
    {
        source.pitch = Random.Range(0.9f, 1.1f);
        source.PlayOneShot(clip);
    }
    public void PlayRandomSound()
    {
        AudioClip randomClip = clipsToPlay[Random.Range(0, clipsToPlay.Length)];
        PlaySpecificSound(randomClip);
    }
}
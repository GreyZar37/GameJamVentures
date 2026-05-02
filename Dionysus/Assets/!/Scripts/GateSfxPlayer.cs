using UnityEngine;

public class GateSfxPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip openSoundClip;
    [SerializeField] private AudioClip closeSoundClip;
    
    [SerializeField] private AudioClip slamSoundClip;
    
    private AudioSource _audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _audioSource  = GetComponent<AudioSource>();
    }

    public void PlayOpenSound() => _audioSource.PlayOneShot(openSoundClip);
    public void PlayCloseSound() => _audioSource.PlayOneShot(closeSoundClip);
    public void PlaySlamSound() => _audioSource.PlayOneShot(slamSoundClip);
}

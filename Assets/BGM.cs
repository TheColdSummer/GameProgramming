using UnityEngine;


public class BGM : MonoBehaviour
{
    public AudioClip audioClip;
    public AudioSource audioSource;
    private bool _loop = true;
    
    void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        audioSource.clip = audioClip;
        audioSource.loop = _loop;
        audioSource.Play();
    }
}
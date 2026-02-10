using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private static MusicPlayer instance;
    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // persist across scene reloads
            audioSource = GetComponent<AudioSource>();

            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            Destroy(gameObject); // destroy duplicate music objects
        }
    }
}

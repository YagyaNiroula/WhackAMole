using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour
{
    public AudioClip menuMusic;
    public AudioClip gameMusic;
    private AudioSource audioSource;

    void Awake()
    {
        // Keep only one instance
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");
        if (objs.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Game")
        {
            PlayMusic(gameMusic);
        }
        else
        {
            PlayMusic(menuMusic);
        }
    }

    void PlayMusic(AudioClip clip)
    {
        if (audioSource.clip == clip) return; // Avoid restarting same track
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [System.Serializable]
    public class SceneMusic
    {
        public string sceneName;
        public AudioClip musicClip;
    }

    [Header("Configuración General")]
    public AudioClip defaultMusic;  

    [Header("Música Específica por Escena")]
    public List<SceneMusic> sceneMusicList = new List<SceneMusic>();

    private AudioSource audioSource;
    private Dictionary<string, AudioClip> sceneMusicMap = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();

           
            foreach (var sceneMusic in sceneMusicList)
            {
                sceneMusicMap[sceneMusic.sceneName] = sceneMusic.musicClip;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string currentScene = scene.name;

       
        if (sceneMusicMap.TryGetValue(currentScene, out AudioClip clip))
        {
            PlayMusic(clip);
        }
        else
        {
            PlayMusic(defaultMusic);  
        }
    }

    private void PlayMusic(AudioClip clip)
    {
        if (audioSource.clip == clip && audioSource.isPlaying) return;

        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }
}
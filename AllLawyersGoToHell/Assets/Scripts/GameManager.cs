using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    GameObject optionsScreenUI;
    GameObject mainScreenUI;

	// Trackers
	private bool isOptionsOpen = false;
    [HideInInspector] public int currentPoints;
	[HideInInspector] public int currentPerson = 0;
    private bool _musicPlaying = false;
	public bool musicShouldPlay = true; 

	// Variables
	[HideInInspector] public float speedMultiplier = 1;
    [HideInInspector] public float volumeLevel = 1;
    public List<int> answerOutcomes = new List<int>();

    // Sound related stuff
    private int audioSourceCount = 5;
    private AudioSource[] sources;
    [SerializeField] private AudioClip music;
    private AudioSource _musicSource = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        sources = new AudioSource[audioSourceCount];
        for (int i = 0; i < audioSourceCount; i++)
        {
            GameObject aSource = new GameObject("AudioSource_" + i);  
            aSource.transform.SetParent(transform);                   
            AudioSource source = aSource.AddComponent<AudioSource>(); 
            source.playOnAwake = false;                          
            sources[i] = source;
        }

        if(_musicSource == null)
		    _musicSource = this.AddComponent<AudioSource>();
	}

	private void Update()
	{
        if (_musicSource != null)
        {
            if (SceneManager.GetActiveScene().name == ("JudgeScreen") || SceneManager.GetActiveScene().name == ("EndScene"))
            {
                _musicSource.volume = .0f;
            }
            else
                _musicSource.volume = .3f;
            if (musicShouldPlay)
            {
                if (_musicPlaying == false)
                {
                    _musicSource.PlayOneShot(music, 1);
                    musicShouldPlay = false;
                    _musicPlaying = true;
                }
                if (!_musicSource.isPlaying)
                {
                    musicShouldPlay = true;
                    _musicPlaying = false;
                }
            }
        }
	}

	private void FindReferences()
    {
        if (SceneManager.GetActiveScene().name == "StartScreen")
        {
            optionsScreenUI = GameObject.Find("OptionsMenu");
            mainScreenUI = GameObject.Find("StartScreen");
            if (optionsScreenUI != null)
                optionsScreenUI.SetActive(false);
        }
    }

    public void LoadScene(string sceneName)
    {
        GameObject.Find("LevelLoader").GetComponent<LevelLoaderScript>().LoadLevel(sceneName);
    }

	public void LoadPlayScene(int judgedInduvidual)
	{
		currentPerson = judgedInduvidual;
		GameObject.Find("LevelLoader").GetComponent<LevelLoaderScript>().LoadLevel("PlayScene");
	}

	public void QuitGame()
    {
        Application.Quit();
    }

    public void ToggleOptions()
    {
        if (isOptionsOpen)
        {
            // Close options menu
            isOptionsOpen = false;
            optionsScreenUI.SetActive(false);
            mainScreenUI.SetActive(true);
        }
        else
        {
            // Open options menu
            isOptionsOpen = true;
            optionsScreenUI.SetActive(true);
            mainScreenUI.SetActive(false);
        }
    }

    public void ChangeSpeed(float speed)
    {
        speedMultiplier = speed;
    }

    public void ChangeVolume(float vol)
    {
        volumeLevel = vol;
        //AudioListener.volume = volume;
    }

    public void PlaySound(AudioClip clip, float minPitch = 0.8f, float maxPitch = 1.2f, float volume = 1f)
    {
        AudioSource source = null;
        foreach (var s in sources)
        {
            if (!s.isPlaying)
            {
                source = s;
                break;
            }
        }
        if (source == null) source = sources[0];

        source.pitch = UnityEngine.Random.Range(minPitch, maxPitch);

        source.PlayOneShot(clip, volume * volumeLevel);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isOptionsOpen = false;
        FindReferences();
    }
}

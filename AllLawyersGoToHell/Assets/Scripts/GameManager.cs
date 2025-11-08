using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    GameObject optionsScreen;
    GameObject mainScreen;

	// Trackers
	private bool isOptionsOpen = false;
    public int currentPoints;

	// Variables
	[HideInInspector] public float speedMultiplier = 1;
    [HideInInspector] public float volumeLevel = 1;
    public List<int> answerOutcomes = new List<int>();

    // Sound related stuff
    private int audioSourceCount = 5;
    private AudioSource[] sources;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            optionsScreen = GameObject.Find("OptionsMenu");
            mainScreen = GameObject.Find("StartScreen");
            if(optionsScreen != null)
                optionsScreen.SetActive(false);
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

    }

    public void LoadScene(string sceneName)
    {
        GameObject.Find("LevelLoader").GetComponent<LevelLoaderScript>().LoadLevel(sceneName);
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void toggleOptions()
    {
        if (isOptionsOpen)
        {
            // Close options menu
            isOptionsOpen = false;
            optionsScreen.SetActive(false);
            mainScreen.SetActive(true);
        }
        else
        {
            // Open options menu
            isOptionsOpen = true;
            optionsScreen.SetActive(true);
            mainScreen.SetActive(false);
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
}

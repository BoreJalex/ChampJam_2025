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

        // Adding audioSources
        sources = new AudioSource[audioSourceCount];
        for (int i = 0; i < audioSourceCount; i++)
        {
            GameObject go = new GameObject("AudioSource_" + i);  // temporary child object
            go.transform.SetParent(transform);                   // parent it to GameManager
            AudioSource source = go.AddComponent<AudioSource>(); // add AudioSource component
            source.playOnAwake = false;                          // prevent auto-play
            sources[i] = source;
        }

    }

    public void loadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
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

    public void GoToJudgementScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }

    // Plays sounds
    public void PlaySound(AudioClip clip, float minPitch = 0.8f, float maxPitch = 1.2f, float volume = 1f)
    {
        // Find a free audio source
        AudioSource source = null;
        foreach (var s in sources)
        {
            if (!s.isPlaying)
            {
                source = s;
                break;
            }
        }
        if (source == null) source = sources[0]; // fallback if all busy

        // Randomize pitch
        source.pitch = UnityEngine.Random.Range(minPitch, maxPitch);

        // Play with volume edited as needed
        source.PlayOneShot(clip, volume * volumeLevel);
    }
}

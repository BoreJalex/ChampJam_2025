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
    private bool isOptionsOpen = false;
    public float speedMultiplier;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            optionsScreen = GameObject.Find("OptionsMenu");
            mainScreen = GameObject.Find("StartScreen");
            optionsScreen.SetActive(false);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
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
}

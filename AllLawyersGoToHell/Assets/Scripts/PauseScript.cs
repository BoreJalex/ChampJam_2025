using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    private bool isPaused = false;
    [SerializeField] public GameObject pauseMenuUI;
    
    private void Start()
    {
        Time.timeScale = 1f; // Ensure the game starts unpaused
        isPaused = false;
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
    }

    
    public void TogglePause()
    {

        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            // Show pause menu UI here if you have one
            pauseMenuUI.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            // Hide pause menu UI here
            pauseMenuUI.SetActive(false);
        }
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f; // Reset time scale before returning to menu
        isPaused = false;
        GameObject.Find("LevelLoader").GetComponent<LevelLoaderScript>().LoadLevel("StartScreen");
    }
}

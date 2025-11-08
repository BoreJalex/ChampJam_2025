using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerProxy : MonoBehaviour
{
    public void ToggleOptions()
    {
        GameManager.Instance.ToggleOptions();
    }
    public void LoadScene(string sceneName)
    {
        GameManager.Instance.LoadScene(sceneName);
    }
    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }
    public void ChangeSpeed(float speed)
    {
        GameManager.Instance.ChangeSpeed(speed);
    }

    public void ChangeVolume(float vol)
    {
        GameManager.Instance.ChangeVolume(vol);
    }
}

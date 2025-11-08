using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class LevelLoaderScript : MonoBehaviour
{
    [SerializeField] private bool instantOpen;
    private Animator animator;
    private bool loading;
    
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        animator.SetBool("Instant", instantOpen);
        loading = false;
    }

    // Debug Testing
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.F))
    //    {
    //        LoadLevel("StartScreen");
    //    }
    //}

    public void LoadLevel(string sceneName)
    {
        if (loading) return;
        StartCoroutine(DoLoadLevel(sceneName));
    }

    IEnumerator DoLoadLevel(string sceneName)
    {
        loading = true;
        animator.SetTrigger("CloseTheGates");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(sceneName);
    }
}

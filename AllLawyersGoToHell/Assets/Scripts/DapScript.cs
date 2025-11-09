using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DapScript : MonoBehaviour
{
    private float time = 2.0f;
    [SerializeField] private GameObject background;
    [SerializeField] private Sprite backgroundOpen;
    [SerializeField] private GameObject flashbangSquare;
    [SerializeField] private GameObject textLine1;
    [SerializeField] private GameObject textLine2;
    [SerializeField] private GameObject textLine3;

    //Sound Effects
    [SerializeField] private AudioClip dap;


    private void Start()
    {
        StartCoroutine(waitForText());
    }

    IEnumerator DapRoutine()
    {
        yield return new WaitForSeconds(time);
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
        flashbangSquare.SetActive(true);
        background.GetComponent<SpriteRenderer>().sprite = backgroundOpen;
        textLine2.SetActive(true);
        StartCoroutine(flashbang());
    }

    IEnumerator flashbang()
    {
        GameManager.Instance.PlaySound(dap, 1.0f);
        flashbangSquare.GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 0), 1.0f);
        yield return null;
        StartCoroutine(waitforFinalText());
    }

    IEnumerator waitForText()
    {
        yield return new WaitForSeconds(0.5f);
        textLine1.SetActive(true);
        StartCoroutine(DapRoutine());
    }

    IEnumerator waitforFinalText()
    {
        yield return new WaitForSeconds(2.0f);
        textLine3.SetActive(true);
        yield return new WaitForSeconds(10);
        GameManager.Instance.LoadScene("StartScreen");
    }
}

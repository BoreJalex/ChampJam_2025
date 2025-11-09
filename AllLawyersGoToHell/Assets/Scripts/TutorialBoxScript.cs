using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialBoxScript : MonoBehaviour
{
    private SpriteRenderer _blockRend;
    private TutorialScript _tScript;

    // Values
    public bool stamped = false;
    public bool removed = false;

    public void Initialize(TutorialScript tScript, int block)
    {
        _tScript = tScript;

        _blockRend = GetComponent<SpriteRenderer>();

        switch (block)
        {
            case 0:
                transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Do your best to send you client to heaven!";
                break;

            case 1:
                transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Strike bad deeds to help their case!";
                break;

            case 2:
                transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Stamp good deeds to prove their innocence!";
                break;
        }
    }

    private void Update()
    {
        if (stamped)
        {
            transform.position = transform.position - new Vector3(0, 12, 0) * Time.deltaTime;
        }

        if (transform.position.y <= -6.0f)
            Destroy(gameObject);

        // Visuals
        if (_tScript.currentBox != null)
            if (_tScript.currentBox == gameObject)
                _blockRend.sprite = _tScript.outlineBlock;
            else
                _blockRend.sprite = _tScript.defaultBlock;
    }
}

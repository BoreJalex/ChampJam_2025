using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
	// Caching
	[SerializeField] private BlockSpawnScript _bScript;

	// Trackers
	[HideInInspector] public GameObject currentBox = null;
	[HideInInspector] public int currentIndex = -1;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite pointingSprite;
    [SerializeField] private Sprite approveSprite;
	private Coroutine resetCo = null;

    private void Update()
	{
		if (currentBox == null && _bScript.blockList.Count > 0)
			currentBox = _bScript.blockList[0];
		else if (currentBox != null)
			currentIndex = _bScript.blockList.IndexOf(currentBox);

		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			if ((currentIndex + 1) < _bScript.blockList.Count)
				currentBox = _bScript.blockList[currentIndex + 1];
		}
		if(Input.GetKeyDown(KeyCode.DownArrow))
		{
			if ((currentIndex - 1) >= 0)
				currentBox = _bScript.blockList[currentIndex - 1];
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			currentBox.GetComponent<BoxScript>().stamped = true;
			currentBox.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 3;

			GetComponent<SpriteRenderer>().sprite = approveSprite;
            transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            transform.position = new Vector3(-4.07f, -2.32f, 0);

			if (resetCo != null)
				StopCoroutine(resetCo);
			Coroutine reset = StartCoroutine(ResetSprite());
			resetCo = reset;
		}
        if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			if (currentIndex != -1 && _bScript.blockList.Count > 0 && currentBox != null)
			{
				if (currentBox.GetComponent<BoxScript>().stamped) return;

				currentBox.GetComponent<BoxScript>().removed = true;
                GetComponent<SpriteRenderer>().sprite = pointingSprite;
                transform.localScale = new Vector3(0.38f, 0.38f, 0.38f);
                transform.position = new Vector3(-2.84f, -2.23f, 0);

				if (resetCo != null)
					StopCoroutine(resetCo);
				Coroutine reset = StartCoroutine(ResetSprite());
				resetCo = reset;

				int wasIndex = currentIndex;
				_bScript.blockList.RemoveAt(currentIndex);

				// Random Values for speed and rotation
				float heightForce = UnityEngine.Random.Range(10f, 20f);
				float sidewaysForce = UnityEngine.Random.Range(10f, 20f);
				float rotationForce = UnityEngine.Random.Range(-80f, -120f);

				// Chucking
				Rigidbody2D rb = currentBox.GetComponent<Rigidbody2D>();
				SpriteRenderer rend = currentBox.GetComponent<SpriteRenderer>();
				Vector3 boxDirection = new Vector3(sidewaysForce, heightForce, 0);
				rb.AddForce(boxDirection, ForceMode2D.Impulse);
				rb.gravityScale = 5;
				rb.AddTorque(rotationForce);
				rend.sortingOrder = 2;

				// Setting next highlighted box
				if (_bScript.blockList.Count >= 1)
					if (_bScript.blockList.Count > currentIndex)
						currentBox = _bScript.blockList[wasIndex];
					else if (_bScript.blockList.Count > 1)
					{
						if (_bScript.blockList[wasIndex - 1] != null)
							currentBox = _bScript.blockList[wasIndex - 1];
						else
							currentBox = _bScript.blockList[0];
					}
					else
						currentBox = _bScript.blockList[0];
			}
		}
	}

	IEnumerator ResetSprite()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().sprite = defaultSprite;
		transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
		transform.position = new Vector3(-4.46f, -2.25f, 0);
    }
}

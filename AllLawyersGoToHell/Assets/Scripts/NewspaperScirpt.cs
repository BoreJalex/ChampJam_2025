using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewspaperScirpt : MonoBehaviour
{
	[SerializeField] private GameObject newspaper;

	private void Start()
	{ 
		StartCoroutine(moveToTutorial());
	}

	IEnumerator moveToTutorial()
    {
        yield return new WaitForSeconds(7.5f);
        GameManager.Instance.LoadPlayScene(0);
    }
}

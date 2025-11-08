using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
	// Caching
	[SerializeField] private BlockSpawnScript _bScript;

	// Trackers
	[HideInInspector] public GameObject currentBox = null;

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			int currentIndex = _bScript.blockList.IndexOf(currentBox);
			if ((currentIndex + 1) < _bScript.blockList.Count)
				currentBox = _bScript.blockList[currentIndex + 1];
		}
		if(Input.GetKeyDown(KeyCode.DownArrow))
		{
			int currentIndex = _bScript.blockList.IndexOf(currentBox);
			if ((currentIndex - 1) >= 0)
				currentBox = _bScript.blockList[currentIndex - 1];
		}
	}
}

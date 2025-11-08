using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JudgementSceneScript : MonoBehaviour
{
	[SerializeField] private Transform leftSpawn;
	[SerializeField] private Transform rightSpawn;

	[SerializeField] private GameObject goodSquare;
	[SerializeField] private GameObject evilSquare;

	private int _thingsSpawned = 0;

	private void Start()
	{
		StartCoroutine(spawnDeeds());
	}

	IEnumerator spawnDeeds()
	{
		while (GameManager.Instance.answerOutcomes.Count > _thingsSpawned)
		{
			switch (GameManager.Instance.answerOutcomes[_thingsSpawned])
			{
				case -40:
					Instantiate(evilSquare, rightSpawn.position, Quaternion.identity);
					break;
				case -20:
					Instantiate(evilSquare, rightSpawn.position, Quaternion.identity);
					break;
				case -10:
					Instantiate(evilSquare, rightSpawn.position, Quaternion.identity);
					break;
				case 5:
					Instantiate(goodSquare, leftSpawn.position, Quaternion.identity);
					break;
				case 10:
					Instantiate(goodSquare, leftSpawn.position, Quaternion.identity);
					break;
				case 50:
					Instantiate(goodSquare, rightSpawn.position, Quaternion.identity);
					break;
				case 100:
					Instantiate(goodSquare, rightSpawn.position, Quaternion.identity);
					break;
			}

			yield return new WaitForSeconds(.3f);
		}
	}
}

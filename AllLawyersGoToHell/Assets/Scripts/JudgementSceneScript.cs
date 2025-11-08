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

	[SerializeField] private GameObject _rotatingPart;
	[SerializeField] private GameObject _leftHolder;
	[SerializeField] private GameObject _rightHolder;

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
					GameObject VeryEvil = Instantiate(evilSquare, rightSpawn.position, Quaternion.identity);
					VeryEvil.transform.localScale *= 2;
					break;
				case -20:
					GameObject QuiteEvil = Instantiate(evilSquare, rightSpawn.position, Quaternion.identity);
					QuiteEvil.transform.localScale *= 1.5f;
					break;
				case -10:
					GameObject Evil = Instantiate(evilSquare, rightSpawn.position, Quaternion.identity);
					break;
				case 5:
					GameObject Good = Instantiate(goodSquare, leftSpawn.position, Quaternion.identity);
					break;
				case 10:
					GameObject Great = Instantiate(goodSquare, leftSpawn.position, Quaternion.identity);
					Great.transform.localScale *= 1.5f;
					break;
				case 50:
					GameObject ShouldBeGood = Instantiate(goodSquare, rightSpawn.position, Quaternion.identity);
					break;
				case 100:
					GameObject ShouldBeGreat = Instantiate(goodSquare, rightSpawn.position, Quaternion.identity);
					ShouldBeGreat.transform.localScale *= 1.5f;
					break;
			}

			_thingsSpawned++;
			yield return new WaitForSeconds(.3f);
		}

		yield return new WaitForSeconds(.3f);

		if (GameManager.Instance.currentPoints > 0)
		{

		}
		else
		{

		}


	}
}

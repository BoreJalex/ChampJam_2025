using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

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
		Vector3 randomLeft;
		Vector3 randomRight;

		while (GameManager.Instance.answerOutcomes.Count > _thingsSpawned)
		{
			randomLeft = new Vector3(leftSpawn.position.x + Random.Range(-.4f, .4f), leftSpawn.position.y, 1);
			randomRight = new Vector3(rightSpawn.position.x + Random.Range(-.4f, .4f), leftSpawn.position.y, 1); ;

			switch (GameManager.Instance.answerOutcomes[_thingsSpawned])
			{
				case -40:
					GameObject VeryEvil = Instantiate(evilSquare, randomRight, Quaternion.identity);
					VeryEvil.transform.localScale *= 2;
					break;
				case -20:
					GameObject QuiteEvil = Instantiate(evilSquare, randomRight, Quaternion.identity);
					QuiteEvil.transform.localScale *= 1.5f;
					break;
				case -10:
					GameObject Evil = Instantiate(evilSquare, randomLeft, Quaternion.identity);
					break;
				case 5:
					GameObject Good = Instantiate(goodSquare, randomLeft, Quaternion.identity);
					break;
				case 10:
					GameObject Great = Instantiate(goodSquare, randomLeft, Quaternion.identity);
					Great.transform.localScale *= 1.5f;
					break;
				case 50:
					GameObject ShouldBeGood = Instantiate(goodSquare, randomRight, Quaternion.identity);
					break;
				case 100:
					GameObject ShouldBeGreat = Instantiate(goodSquare, randomRight, Quaternion.identity);
					ShouldBeGreat.transform.localScale *= 1.5f;
					break;
			}

			_thingsSpawned++;
			yield return new WaitForSeconds(.3f);
		}

		yield return new WaitForSeconds(1);

		if (GameManager.Instance.currentPoints > 0)
		{
			Vector3 newRotate = new Vector3(0, 0, 30);
			_rotatingPart.transform.DORotate(newRotate, 1.5f, RotateMode.Fast);
			Vector3 newDownPosition = _leftHolder.transform.position + new Vector3(.35f, -1.65f, 0f);
			_leftHolder.transform.DOMove(newDownPosition, 1.5f);
			Vector3 newUpPosition = _rightHolder.transform.position + new Vector3(-.45f, 1.7f, 0f);
			_rightHolder.transform.DOMove(newUpPosition, 1.5f);
		}
		else
		{
			Vector3 newRotate = new Vector3(0, 0, -30);
			_rotatingPart.transform.DORotate(newRotate, 1.5f, RotateMode.Fast);
			Vector3 newDownPosition = _leftHolder.transform.position + new Vector3(.4f, 1.65f, 0f);
			_leftHolder.transform.DOMove(newDownPosition, 1.5f);
			Vector3 newUpPosition = _rightHolder.transform.position + new Vector3(-.4f, -1.7f, 0f);
			_rightHolder.transform.DOMove(newUpPosition, 1.5f);
		}
	}
}

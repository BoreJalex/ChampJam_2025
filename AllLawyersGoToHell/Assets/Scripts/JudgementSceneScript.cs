using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class JudgementSceneScript : MonoBehaviour
{
	// Spawns
	[SerializeField] private Transform leftSpawn;
	[SerializeField] private Transform rightSpawn;

	// Objects
	[SerializeField] private GameObject goodSquare;
	[SerializeField] private GameObject evilSquare;

	// The Scale
	[SerializeField] private GameObject _rotatingPart;
	[SerializeField] private GameObject _leftHolder;
	[SerializeField] private GameObject _rightHolder;

	// Data
	private bool _decided = false; 

	// Sounds
	[SerializeField] private AudioClip[] judgeVoice;
	[SerializeField] private AudioClip trumpet;
	[SerializeField] private AudioClip sentenceToHeaven;
	[SerializeField] private AudioClip sentenceToHell;

	private int _thingsSpawned = 0;

	private void Start()
	{
		StartCoroutine(judgeSpeaking());
		StartCoroutine(spawnDeeds());
	}

	IEnumerator judgeSpeaking()
	{
		int choice = 0;

		while (!_decided)
		{
			choice = Random.Range(0, 5);
			float randomPitch = UnityEngine.Random.Range(.8f, 1.2f);
			GameManager.Instance.PlaySound(judgeVoice[choice], randomPitch);
			for (int x = 0; x < 3; x++)
			{
				transform.position += new Vector3(0, .05f, 0);
				yield return new WaitForSeconds(0.1f);
				transform.position -= new Vector3(0, .05f, 0);
				yield return new WaitForSeconds(0.1f);
			}
			yield return new WaitForSeconds(.2f);
		}
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
					GameObject Evil = Instantiate(evilSquare, randomRight, Quaternion.identity);
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
			Vector3 newDownPosition = _leftHolder.transform.position + new Vector3(.25f, -1.1f, 0f);
			_leftHolder.transform.DOMove(newDownPosition, 1.5f);
			Vector3 newUpPosition = _rightHolder.transform.position + new Vector3(-.34f, 1.15f, 0f);
			_rightHolder.transform.DOMove(newUpPosition, 1.5f);
		}
		else
		{
			Vector3 newRotate = new Vector3(0, 0, -30);
			_rotatingPart.transform.DORotate(newRotate, 1.5f, RotateMode.Fast);
			Vector3 newDownPosition = _leftHolder.transform.position + new Vector3(.35f, 1.15f, 0f);
			_leftHolder.transform.DOMove(newDownPosition, 1.5f);
			Vector3 newUpPosition = _rightHolder.transform.position + new Vector3(-.265f, -1.1f, 0f);
			_rightHolder.transform.DOMove(newUpPosition, 1.5f);
		}

		_decided = true; 

		yield return new WaitForSeconds(1);

		if (GameManager.Instance.currentPoints > 0)
		{
			GameManager.Instance.PlaySound(sentenceToHeaven);
			yield return new WaitForSeconds(3.25f);
			GameManager.Instance.PlaySound(trumpet);
		}
		else
		{
			GameManager.Instance.PlaySound(sentenceToHell);
		}
	}
}

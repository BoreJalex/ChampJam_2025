using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BlockSpawnScript : MonoBehaviour
{
	// Caching
	[SerializeField] private Transform _spawnPoint;
	[SerializeField] private GameObject _blockPrefab;
	public PlayerScript pScript;
	[SerializeField] private LogOfTextObject _textLog;

	// Block Related Stuff
	public List<GameObject> blockList = new List<GameObject>();
	[SerializeField] private float _blockSpawnRate;
	public float boxFallSpeed;

	// Timer
	private float _timeSinceSpawn;

	// Number tracking
	private int textsUsed = 0;

	private void Start()
	{
		boxFallSpeed = boxFallSpeed * GameManager.Instance.speedMultiplier;
	}

	private void Update()
	{
		_timeSinceSpawn += Time.deltaTime * _blockSpawnRate * GameManager.Instance.speedMultiplier;

		if(_timeSinceSpawn >= 2 && _textLog.texts.Length > textsUsed)
		{
			_timeSinceSpawn = 0;
			SpawnBlock();
		}
		else if(_textLog.texts.Length <= textsUsed)
			GameManager.Instance.GoToJudgementScene();
    }

	void SpawnBlock()
	{
		TestimonyObject blockData = null;
		int choice = 0;
		while (blockData == null)
		{
			choice = Random.Range(0, _textLog.texts.Length);
			if (!_textLog.texts[choice].used)
			{
				blockData = _textLog.texts[choice];
				_textLog.texts[choice].used = true;
				textsUsed++;
			}
		}

		GameObject block = Instantiate(_blockPrefab, _spawnPoint.position, Quaternion.identity);

		if (blockList.Count == 0)
		{
			blockList.Add(block);
			pScript.currentBox = block;
		}
		else
			blockList.Add(block);

		block.AddComponent<BoxScript>();
		block.GetComponent<BoxScript>().Initialize(this, blockData);
	}
}

public class BoxScript : MonoBehaviour
{
	// Cache
	private float _fallSpeed;
	private PlayerScript _pScript;
	private TestimonyObject _blockData;
	private SpriteRenderer _outlineRend;
	private BlockSpawnScript _bScript;

	// Values
	public bool stamped = false;
	public bool removed = false; 

	public void Initialize(BlockSpawnScript bScript, TestimonyObject blockData)
	{
		_bScript = bScript;
		_fallSpeed = bScript.boxFallSpeed;
		_pScript = bScript.pScript;
		_blockData = blockData;
		
		_outlineRend = transform.GetChild(0).GetComponent<SpriteRenderer>();

		transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = blockData.testimonyText;
	}

	private void Update()
	{
		// Falling
		transform.position = transform.position - new Vector3(0, _fallSpeed, 0) * Time.deltaTime;

		if (transform.position.y <= -7)
			Destroy(gameObject);

		// Visuals
		if (_pScript.currentBox != null)
			if(_pScript.currentBox == gameObject)
				_outlineRend.color = Color.red;
			else
				_outlineRend.color = Color.black;
	}

	private void OnDestroy()
	{
		_blockData.used = false;

		if(_blockData.points < 0)
		{
			if (stamped)
			{
				GameManager.Instance.currentPoints += _blockData.points * 2;
				GameManager.Instance.answerOutcomes.Add(_blockData.points * 2);
			}
			else if (!removed)
			{
				GameManager.Instance.currentPoints += _blockData.points;
				GameManager.Instance.answerOutcomes.Add(_blockData.points);
			}
		}
		else
		{
			if (stamped)
			{
				GameManager.Instance.currentPoints += _blockData.points;
				GameManager.Instance.answerOutcomes.Add(_blockData.points);
			}
			else
			{
				GameManager.Instance.currentPoints -= _blockData.points;
				GameManager.Instance.answerOutcomes.Add(_blockData.points * 10);
			}
		}
	}
}

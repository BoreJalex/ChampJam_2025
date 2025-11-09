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
	[SerializeField] private LogOfTextObject[] _textLogs;
	private int _currentInduvidual = 0;
	SpriteRenderer _characterSprite;

	// Block Related Stuff
	[HideInInspector] public List<GameObject> blockList = new List<GameObject>();
	[SerializeField] private float _blockSpawnRate;
	public float boxFallSpeed;

	// Timer
	private float _timeSinceSpawn;

	// Text tracking
	private int _currentChoice = 0;
	private int _textsUsed = 0;
	[HideInInspector] public int currentTexts = 0;
	[HideInInspector] public bool allTextsUsed = false;

	// Sprite Tracking
	[SerializeField] private Sprite[] _characterSprites;
	[SerializeField] public Sprite defaultBlock;
    [SerializeField] public Sprite outlineBlock;
	[SerializeField] public TextMeshProUGUI characterPlaque;
    private Coroutine resetCo = null;

	// Sound Tracking
	[SerializeField] private AudioClip[] _soundLogs;

	private void Start()
	{
		if (GameManager.Instance.speedMultiplier == 0)
			GameManager.Instance.speedMultiplier = 1;

		boxFallSpeed *= GameManager.Instance.speedMultiplier;

		_currentInduvidual = GameManager.Instance.currentPerson;

		_characterSprite = GetComponent<SpriteRenderer>();
		_characterSprite.sprite = _characterSprites[_currentInduvidual * 2];
		string name = "Debug";
		switch (_currentInduvidual)
		{
			case 0:
			name = "Luke Tulern";
			break;
			case 1:
			name = "Sienna Lot";
			break;
			case 2:
			name = "Hank Oveur";
			break;
		}
		characterPlaque.text = name;

		GameManager.Instance.musicShouldPlay = true;
	}

	private void Update()
	{
		_timeSinceSpawn += Time.deltaTime * _blockSpawnRate * GameManager.Instance.speedMultiplier;

		if(_timeSinceSpawn >= 2 && _textLogs[_currentInduvidual].texts.Length > _textsUsed)
		{
			_timeSinceSpawn = 0;
			SpawnBlock();
		}
		else if(_textLogs[_currentInduvidual].texts.Length <= _textsUsed)
			allTextsUsed = true;
	}

	void SpawnBlock() // Spawning text blocks
	{
		// Making the current block and ++'ing all the things we need to
		TestimonyObject blockData = _textLogs[_currentInduvidual].texts[_currentChoice];
		_currentChoice++;
		_textsUsed++;
		currentTexts++;

		// Talking sounds
		GameObject block = Instantiate(_blockPrefab, _spawnPoint.position, Quaternion.identity);
		int choice = Random.Range(GameManager.Instance.currentPerson * 7, (GameManager.Instance.currentPerson + 1) * 7);
		GameManager.Instance.PlaySound(_soundLogs[choice]);

		if (resetCo != null)
			StopCoroutine(resetCo);
		Coroutine reset = StartCoroutine(TalkingCo(blockData));
		resetCo = reset;

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

	IEnumerator TalkingCo(TestimonyObject blockData) // Make a small talking animation
	{
		SpriteRenderer charSprite = GetComponent<SpriteRenderer>();
		Vector3 startTransform = transform.position;

		int yapAmount = blockData.testimonyText.Length / 5;

		for (int x = 0; x < yapAmount; x++)
		{
			charSprite.sprite = _characterSprites[(_currentInduvidual * 2) + 1];
			transform.position += new Vector3(0, .1f, 0);
			yield return new WaitForSeconds(0.1f);
            transform.position = startTransform;
            charSprite.sprite = _characterSprites[_currentInduvidual * 2];
			yield return new WaitForSeconds(0.1f);
		}
	}
}

public class BoxScript : MonoBehaviour // The Script put on the text boxes
{
	// Cache
	private float _fallSpeed;
	private PlayerScript _pScript;
	private TestimonyObject _blockData;
	private SpriteRenderer _blockRend;
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
		
		_blockRend = GetComponent<SpriteRenderer>();

		transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = blockData.testimonyText;
	}

	private void Update()
	{
		// Falling
		transform.position = transform.position - new Vector3(0, _fallSpeed, 0) * Time.deltaTime;

		if (transform.position.y <= -6.0f)
			Destroy(gameObject);

		// Visuals
		if (_pScript.currentBox != null)
			if (_pScript.currentBox == gameObject)
				_blockRend.sprite = _bScript.outlineBlock;
			else
                _blockRend.sprite = _bScript.defaultBlock;
    }

	private void OnDestroy()
	{
		_bScript.currentTexts--;

		if (_bScript.blockList.IndexOf(gameObject) == _bScript.blockList.IndexOf(_pScript.currentBox)) // Removing this box from the list
		{
			_bScript.blockList.Remove(gameObject);
			_pScript.currentBox = null;
		}

		// All the stuff for point calc
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

		if (_bScript.allTextsUsed && _bScript.currentTexts < 1)
			GameManager.Instance.LoadScene("JudgeScreen");
    }
}

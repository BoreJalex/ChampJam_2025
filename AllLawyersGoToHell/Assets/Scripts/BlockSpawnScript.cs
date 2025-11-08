using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawnScript : MonoBehaviour
{
	// Caching
	[SerializeField] private Transform _spawnPoint;
	[SerializeField] private GameObject _blockPrefab;
	[SerializeField] private PlayerScript _pScript;

	// Block Related Stuff
	public List<GameObject> blockList = new List<GameObject>();
	[SerializeField] private float _blockSpawnRate;

	// Timer
	private float _timeSinceSpawn;

	// Block Stats
	[SerializeField] private float _boxFallSpeed;

	private void Update()
	{
		_timeSinceSpawn += Time.deltaTime * _blockSpawnRate;

		if(_timeSinceSpawn >= 2)
		{
			_timeSinceSpawn = 0;
			SpawnBlock();
		}
	}

	void SpawnBlock()
	{
		GameObject block = Instantiate(_blockPrefab, _spawnPoint.position, Quaternion.identity);

		if(blockList.Count == 0)
		{
			blockList.Add(block);
			_pScript.currentBox = block;
		}
		else
			blockList.Add(block);

		block.AddComponent<BoxScript>();
		block.GetComponent<BoxScript>().Initialize(_boxFallSpeed, _pScript);
	}
}

public class BoxScript : MonoBehaviour
{
	// Cache
	private float _fallSpeed;
	private PlayerScript _pScript;
	

	public void Initialize(float fallSpeed, PlayerScript pScript)
	{
		_fallSpeed = fallSpeed;
		_pScript = pScript;
	}

	private void Update()
	{
		// Falling
		transform.position = transform.position - new Vector3(0, _fallSpeed, 0) * Time.deltaTime;

		if (transform.position.y <= -7)
			Destroy(gameObject);

		// Visuals
		if (_pScript.currentBox != null && _pScript.currentBox == gameObject)
			GetComponent<SpriteRenderer>().color = Color.red;
		else
			GetComponent<SpriteRenderer>().color = Color.white;
	}
}

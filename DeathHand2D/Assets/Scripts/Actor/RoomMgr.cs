using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameMgr;

public class RoomMgr : MonoBehaviour
{
	[SerializeField]
	private GameObject background;
	public GameObject Background { get { return background; } }

	[SerializeField]
	private int section;

	private GameObject player;

	private GameObject gates;

	private int enemyGroupCode;

	public bool clear = false;
	//public bool Clear { get { return clear; } }

	private Vector2 mapSizeMin = Vector2.zero;
	public Vector2 MapSizeMin { get { return mapSizeMin; } }

	private Vector2 mapSizeMax = new Vector2(38.4f, 16.2f);
	public Vector2 MapSizeMax { get { return mapSizeMax; } }

	private void Awake()
	{
		player = GameObject.Find("Player");
		gates = transform.Find("Gates").gameObject;
	}

	private void Start()
	{
		ClampMap();
		enemyGroupCode = ChooseEnemyGroup(GM.SecData.ProbabilityTable[section-1]);
		gates.SetActive(false);
	}

	private void Update()
	{
		if(clear && !gates.activeSelf)
			gates.SetActive(true);			
	}

	public void BeginWave()
	{
		Debug.Log("enemyGroupCode : " + enemyGroupCode);
		GM.SecData.InitEnemyGroup(enemyGroupCode, transform);
	}
	
	public void ClampMap()
	{
		mapSizeMin = transform.position;
		mapSizeMin.x += GetSpriteSize(player).x;
		mapSizeMax = new Vector2(transform.position.x, transform.position.y) + GetSpriteSize(background);
		mapSizeMax.x -= GetSpriteSize(player).x;
	}

	public Vector2 GetSpriteSize(GameObject _target)
	{
		Vector2 worldSize = Vector2.zero;
		if(_target.GetComponent<SpriteRenderer>())
		{
			Vector2 spriteSize = _target.GetComponent<SpriteRenderer>().sprite.rect.size;
			Vector2 localSpriteSize = spriteSize / _target.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
			worldSize = localSpriteSize * _target.transform.lossyScale;
		}
		return worldSize;
	}

	int ChooseEnemyGroup(float[] probs)
	{
		float total = 0;

		foreach(float elem in probs)
		{
			total += elem;
		}

		float randomPoint = Random.value * total;

		for(int i = 0; i < probs.Length; i++)
		{
			if(randomPoint < probs[i])
			{
				return i;
			}
			else
			{
				randomPoint -= probs[i];
			}
		}
		return 0;
	}
}
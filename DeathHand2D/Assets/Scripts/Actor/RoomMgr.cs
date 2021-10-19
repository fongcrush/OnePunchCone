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

	public bool Clear = false;

	public bool Reset = false;

	public bool Here = false;

	private GameObject curGroup;

	private Vector2 mapSizeMin = Vector2.zero;
	public Vector2 MapSizeMin { get { return mapSizeMin; } }

	private Vector2 mapSizeMax = new Vector2(38.4f, 10.8f);
	public Vector2 MapSizeMax { get { return mapSizeMax; } }

	private void Awake()
	{
		player = GameObject.Find("Player");
		gates = transform.Find("Gates").gameObject;
	}

	private void Start()
	{
		ClampMap();
		gates.SetActive(false);
	}

	private void Update()
	{
		if(Clear && !gates.activeSelf)
			gates.SetActive(true);
		if(Reset)
			ResetRoom();

		if(Here)
		{
			GM.ChangeRoom(this, GateDirection.Left);
			Here = false;
		}
	}

	public void BeginWave()
	{
		//Debug.Log("enemyGroupCode : " + enemyGroupCode);
		enemyGroupCode = ChooseEnemyGroup(GM.SecData.ProbabilityTable[section - 1]);
		curGroup = GM.SecData.InitEnemyGroup(enemyGroupCode, transform);
	}

	void ResetRoom()
	{
		Reset = false;
		Clear = false;
		if(gates.activeSelf)
			gates.SetActive(false);

		if(curGroup)
			Destroy(curGroup);

		if(GM.CurRoomMgr == this)
			BeginWave();
		
	}

	public void ClampMap()
	{
		mapSizeMin = new Vector2(transform.position.x + 2f, transform.position.y);
		mapSizeMax = mapSizeMin + GetSpriteSize(background) + new Vector2(-4.0f, -0.5f);
	}

	public static Vector2 GetSpriteSize(GameObject _target)
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
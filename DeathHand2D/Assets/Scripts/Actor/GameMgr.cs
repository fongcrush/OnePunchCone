using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using static PlayerAttackData;

public class GameMgr : MonoBehaviour
{
    private static GameMgr instance = null;

    public static GameMgr GM
    {
        get
        {
            if(instance == null)
            {                
                var obj = FindObjectOfType<GameMgr>();
                if(obj == null)
                    instance = new GameObject().AddComponent<GameMgr>();
                else
                    instance = obj;
            }
            return instance;
        }
    }

    [SerializeField]
    private RoomMgr startRoom;

    private GameObject player;
    public GameObject Player { get { return player; } } 

    private CinemachineConfiner2D cineConfiner;

    public Status pcStat = null;

    private SectionData sectionData = null;
    public SectionData SecData { get { return sectionData; } }

    private RoomMgr curRoomMgr = null;
    public RoomMgr CurRoomMgr { get { return curRoomMgr; } }

    private bool isEnemyHit = false;


    private void Awake()
	{
        var objs = FindObjectsOfType<GameMgr>();
        if(objs.Length == 1)
            DontDestroyOnLoad(this);
        else
            Destroy(this);

        sectionData = GetComponent<SectionData>();
        player = GameObject.Find("Player");
        curRoomMgr = startRoom;
        cineConfiner = GameObject.Find("CMV Camera").GetComponent<CinemachineConfiner2D>();
    }

	void Start()
    {
        pcStat = new Status(player.GetComponent<PlayerController>().PlayerMaxHP, 100, AttackTable[100].max);
        curRoomMgr.BeginWave();
    }

	public void ChangeRoom(RoomMgr room, GateDirection gateDir)
	{
        if(curRoomMgr == room)
            return;

        curRoomMgr = room;
        Vector3 dir = Vector2.zero;
        switch(gateDir)
        {
        case GateDirection.Left:
            dir = new Vector3(5f, 2.4f, 0f);
            break;
        case GateDirection.Right:
            dir = new Vector3(33.4f, 2.4f, 0f);
            break;
        case GateDirection.Up:
            dir = new Vector3(19.2f, 4f, 0f);
            break;
        case GateDirection.Down:
            dir = new Vector3(19.2f, 1.2f, 0f);
            break;
        }
        player.transform.position = curRoomMgr.transform.position + dir;
        cineConfiner.m_BoundingShape2D = curRoomMgr.transform.Find("Clamp").GetComponent<PolygonCollider2D>();
        if(!curRoomMgr.Clear)
            curRoomMgr.BeginWave();
	}
    public bool GetEnemyHit()
    {
        return isEnemyHit;
    }
    public void SetEnemyHit(bool state)
    {
        isEnemyHit = state;
    }
}
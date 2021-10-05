using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameMgr;

public class GateMgr : MonoBehaviour
{
	[SerializeField]
	private GateMgr linked_Gate;
	public GateMgr Linked_Gate { get { return linked_Gate; } }

	private RoomMgr room;
	public RoomMgr Room { get { return room; } }

	[SerializeField]
	private GateDirection gateDir = GateDirection.Left;
	public GateDirection GateDir { get { return gateDir; } }

	private void Awake()
	{
		room = transform.parent.parent.GetComponent<RoomMgr>();

		switch(gateDir)
		{
		case GateDirection.Left:
			transform.localPosition = new Vector3(1.5f, 2.7f, 0f);
			break;
		case GateDirection.Right:
			transform.localPosition = new Vector3(36.9f, 2.7f, 0f);
			break;
		case GateDirection.Up:
			transform.localPosition = new Vector3(19.2f, 5.4f, 0f);
			break;
		case GateDirection.Down:
			transform.localPosition = new Vector3(19.2f, 1f, 0f);
			break;
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if(collision.gameObject.tag == "Player")
		{			
			GM.ChangeRoom(Linked_Gate.Room, linked_Gate.GateDir);
			Debug.Log("Player touched Gate");
		}
	}
}

public enum GateDirection
{
	Left,
	Right,
	Up,
	Down
}
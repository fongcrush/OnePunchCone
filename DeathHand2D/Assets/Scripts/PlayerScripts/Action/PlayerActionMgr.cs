using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStatesData;
using static InputManager;
public class PlayerActionMgr : MonoBehaviour
{
	private PlayerController player;

	private IPlayerAction curPlayerAction;

	[SerializeField]
	private PlayerAttackMgr attackMgr;
	public PlayerAttackMgr AttackMgr { get { return attackMgr; } }

	private PlayerDash dash;
	public PlayerDash Dash { get { return dash; } }

	void Awake()
	{
		player = GameObject.Find("Player").GetComponent<PlayerController>();
		dash = GetComponent<PlayerDash>();
	}

	public void Begin()
	{
		playerState = PlayerState.Action;
		player.GetComponent<PlayerMove>().enabled = false;
		switch(curActionKey)
		{
		case ActionKey.LeftShift:
			curPlayerAction = dash;
			break;

		case ActionKey.Z:
		case ActionKey.X:
		case ActionKey.C:
			curPlayerAction = attackMgr;
			break;
		}
		attackMgr.Begin();
	}

	public void UpdateAction()
	{
		if(curPlayerAction !=null)
			curPlayerAction.UpdateAction();
		if(actionState == ActionState.None)
			End();
	}

	public void End()
	{
		playerState = PlayerState.Move;
		player.GetComponent<PlayerMove>().enabled = true;
	}
}

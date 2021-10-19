using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStatesData;
using static InputManager;
using static GameMgr;

public class PlayerActionMgr : MonoBehaviour
{
	private PlayerController player;

	private Animator anim;

	private PlayerAction curAction;

	[SerializeField]
	private PlayerAttackAuto aAttack;
	public PlayerAttackAuto AAttack { get { return aAttack; } }

	[SerializeField]
	private PlayerAttackSkill01 skill_01;
	public PlayerAttackSkill01 Skill_01 { get { return skill_01; } }

	[SerializeField]
	private PlayerAttackSkill02 skill_02;
	public PlayerAttackSkill02 Skill_02 { get { return skill_02; } }

	[SerializeField]
	private PlayerAttackSkill03 skill_03;
	public PlayerAttackSkill03 Skill_03 { get { return skill_03; } }

	private PlayerDash dash;
	public PlayerAction Dash { get { return dash; } }

	private bool skill_03_On = false;
	public bool Skill_03_On { get { return skill_03_On; } set { skill_03_On = value; } }


	void Awake()
	{
		player = GM.Player.GetComponent<PlayerController>();
		anim = player.GetComponent<Animator>();
		PlayerAttackData.ReadAttackData();

		dash = GetComponent<PlayerDash>();
		curAction = AAttack;
	}

	public void Begin()
	{
		switch(curActionKey)
		{
		case ActionKey.Z:
			curAction = AAttack;
			break;
		case ActionKey.X:
			curAction = Skill_01;
			break;
		case ActionKey.C:
			if(!skill_03_On)
				curAction = Skill_02;
			else
				curAction = skill_03;
			break;
		case ActionKey.LeftShift:
			curAction = dash;
			break;
		}

		if(!curAction.Ready())
		{
			curAction = null;
			return;
		}

		playerState = PlayerState.Action;
		player.GetComponent<PlayerMove>().enabled = false;
		moveMode = MoveMode.Idle;
		anim.SetInteger("Move", 0);

		if (curAction != null)
			StartCoroutine(curAction.ActionRoutine());
	}

	public void UpdateAction()
	{
		if(curActionKey == ActionKey.LeftShift && dash.Ready() && curAction!=dash)
			ChangeAction(dash);
		if (actionState == ActionState.None)
			End();
	}

	public void End()
	{
		playerState = PlayerState.Move;
		player.GetComponent<PlayerMove>().enabled = true;
	}

	public void ChangeAction(PlayerAction action)
	{
		if(curAction != null)
			curAction.Quit();

		playerState = PlayerState.Action;
		player.GetComponent<PlayerMove>().enabled = false;

		curAction = action;
		if (curAction != null)
			StartCoroutine(curAction.ActionRoutine());
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStatesData;
using static InputManager;

public class PlayerActionMgr : MonoBehaviour
{
	private PlayerController player;

	private IPlayerAction curAction;

	private PlayerAttackAuto aAttack;
	public IPlayerAction AAttack { get { return aAttack; } }

	private PlayerAttackSkill01 skill_01;
	public PlayerAttackSkill01 Skill_01 { get { return skill_01; } }

	private PlayerAttackSkill02 skill_02;
	public PlayerAttackSkill02 Skill_02 { get { return skill_02; } }

	private PlayerAttackSkill03 skill_03;
	public PlayerAttackSkill03 Skill_03 { get { return skill_03; } }

	private PlayerDash dash;
	public IPlayerAction Dash { get { return dash; } }

	private bool canSkill3;
	public bool CanSkill3 { get { return canSkill3; } set { canSkill3 = value; } }

	public bool skill_03_On = false;

	void Awake()
	{
		player = GameObject.Find("Player").GetComponent<PlayerController>();

		aAttack = GetComponent<PlayerAttackAuto>();
		skill_01 = GetComponent<PlayerAttackSkill01>();
		skill_02 = GetComponent<PlayerAttackSkill02>();
		skill_03 = GetComponent<PlayerAttackSkill03>();
		PlayerAttackData.ReadAttackData();

		dash = GetComponent<PlayerDash>();
		canSkill3 = false;
	}
	public void Update()
	{
		//PlayerAttackData.UpdateCSVData();
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

		if (curAction != null)
			curAction.Begin();
	}

	public void UpdateAction()
	{
		if (curActionKey == ActionKey.LeftShift && dash.Ready())
		{
			if(curAction != skill_02)
				ChangeAction(dash);
		}

		if (curAction != null)
			curAction.UpdateAction();

		if (actionState == ActionState.None)
			End();
	}

	public void End()
	{
		//if(actionState == ActionState.Dash) 
		//{
		//	curAction = dash;
		//	curAction.Begin();
		//}
		playerState = PlayerState.Move;
		player.GetComponent<PlayerMove>().enabled = true;	
	}

	public void ChangeAction(IPlayerAction action)
	{
		if(curAction != null)
			curAction.Quit();

		playerState = PlayerState.Action;
		player.GetComponent<PlayerMove>().enabled = false;

		curAction = action;
		if(curAction != null)
			curAction.Begin();
	}
}

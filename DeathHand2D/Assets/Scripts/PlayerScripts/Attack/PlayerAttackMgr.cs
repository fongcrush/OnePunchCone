using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStatesData;
using static InputManager;

public class PlayerAttackMgr : MonoBehaviour, IPlayerAction
{
	private PlayerController player;

	private PlayerActionMgr actionMgr;

	private IPlayerAttack curAttack, prevAttack;

	private PlayerAttackAuto aAttack;
	public IPlayerAttack AAttack { get { return aAttack; } }

	private PlayerAttackSkill01 skill_01;
	public IPlayerAttack Skill_01 { get { return skill_01; } }

	private PlayerAttackSkill02 skill_02;
	public IPlayerAttack Skill_02 { get { return skill_02; } }

	private PlayerAttackSkill03 skill_03;
	public IPlayerAttack Skill_03 { get { return skill_03; } }

	private void Awake()
	{
		player = GameObject.Find("Player").GetComponent<PlayerController>();
		actionMgr = player.ActionMgr;
		aAttack = GetComponent<PlayerAttackAuto>();
		skill_01 = GetComponent<PlayerAttackSkill01>();
		skill_02 = GetComponent<PlayerAttackSkill02>();
		skill_03 = GetComponent<PlayerAttackSkill03>();
		PlayerAttackData.UpdateCSVData();
	}

	public void Begin()
	{
		actionState = ActionState.Attack;
		if(attackState == AttackState.None)
		{
			switch(curActionKey)
			{
			case ActionKey.Z:
				curAttack = AAttack;
				break;
			case ActionKey.X:
				curAttack = Skill_01;
				break;
			case ActionKey.C:
				curAttack = Skill_02;
				break;
			case ActionKey.LeftShift:
				break;
			}

			if(curAttack != null)
				curAttack.Begin();
		}
	}

	public void UpdateAction()
	{
		if(curAttack != null)
			curAttack.UpdateAttack();
		if(attackState == AttackState.None)
			End();
	}
	public void End()
	{
		actionState = ActionState.None;
	}

	public void ChangeAttack(IPlayerAttack attack)
	{
		if(curAttack != null)
			curAttack.Quit();
		prevAttack = curAttack;
		curAttack = attack;
		if(attack != null)
			curAttack.Begin();
	}
}
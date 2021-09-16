using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StatesManager;

public class PlayerAttackController : MonoBehaviour, IPlayerAction
{
	private IPlayerAttack curPlayerAttack;
	private PlayerActionBehaviour actionBehaviour;
	private AAttack attack;
	private Skill1 skill1;
	private Skill2 skill2;
	private Skill3 skill3;

	public PlayerAttackController(PlayerActionBehaviour ActionBehaviour)
	{
		actionBehaviour = ActionBehaviour;
		attack = new AAttack(this);
		skill1 = new Skill1(this);
		skill2 = new Skill2(this);
		skill3 = new Skill3(this);
	}

	public void Begin()
	{
		actionState = ActionState.Attack;

		if(curActionKey == ActionKey.Z)
			curPlayerAttack = attack;
		else if(curActionKey == ActionKey.X)
			curPlayerAttack = skill1;
		else if(curActionKey == ActionKey.C)
			curPlayerAttack = skill2;

		curPlayerAttack.Run();
	}
	public void End()
	{
		actionState = ActionState.None;
	}

	public void ChangeAttack(string name)
	{
		switch(name)
		{
		case "attack":
			curPlayerAttack = attack;
			break;

		case "skill1":
			curPlayerAttack = skill1;
			break;

		case "skill2":
			curPlayerAttack = skill2;
			break;

		case "skill3":
			curPlayerAttack = skill3;
			break;
		}
	}
}

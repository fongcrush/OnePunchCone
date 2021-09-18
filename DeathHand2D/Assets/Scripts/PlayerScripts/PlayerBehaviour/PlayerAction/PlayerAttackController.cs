using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StatesManager;

[RequireComponent(typeof(AAttack))]
[RequireComponent(typeof(Skill1), typeof(Skill2), typeof(Skill3))]
public class PlayerAttackController : MonoBehaviour, IPlayerAction
{
	private IPlayerAttack curPlayerAttack;
	private PlayerActionBehaviour actionBehaviour;
	private AAttack attack;
	private Skill1 skill1;
	private Skill2 skill2;
	private Skill3 skill3;

    private void Awake()
    {
		actionBehaviour = GetComponent<PlayerActionBehaviour>();
		attack = GetComponent<AAttack>();
		skill1 = GetComponent<Skill1>();
		skill2 = GetComponent<Skill2>();
		skill3 = GetComponent<Skill3>();
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
		curActionKey = ActionKey.None;
		playerState = PlayerState.Idle;
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

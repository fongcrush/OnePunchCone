using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StatesManager;

public class AttackController : MonoBehaviour, IPlayerAction
{
	public IPlayerAttack curPlayerAttack;
	Attack attack = new Attack();
	Skill1 skill1 = new Skill1();
	Skill2 skill2 = new Skill2();
    public void Begin()
	{
	}
	public void Update()
	{
		if(curActionKey == ActionKey.Z)
			curPlayerAttack = attack;
		else if(curActionKey == ActionKey.X)
			curPlayerAttack = skill1;
		else if(curActionKey == ActionKey.C)
			curPlayerAttack = skill2;
	}
	public void End()
	{

	}
}

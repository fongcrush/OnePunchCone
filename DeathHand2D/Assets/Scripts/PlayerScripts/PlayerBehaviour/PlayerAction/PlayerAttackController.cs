using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StatesManager;

public class PlayerAttackController : MonoBehaviour, IPlayerAction
{
	[HideInInspector]
	public IPlayerAttack curPlayerAttack;

	private Attack attack = new Attack();
	private Skill1 skill1 = new Skill1();
	private Skill2 skill2 = new Skill2();
	private Skill3 skill3 = new Skill3();

    public void Begin()
	{
	}
	public void Update()
	{
		if(curActionKey != ActionKey.None)
		{
			if(curActionKey == ActionKey.Z)
				curPlayerAttack = attack;
			else if(curActionKey == ActionKey.X)
				curPlayerAttack = skill1;
			else if(curActionKey == ActionKey.C)
				curPlayerAttack = skill2;

			curPlayerAttack.Run();
		}
	}
	public void End()
	{

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

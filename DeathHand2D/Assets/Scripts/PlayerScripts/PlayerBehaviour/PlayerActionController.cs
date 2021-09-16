using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StatesManager;
using static InputManager;

public class PlayerActionController: MonoBehaviour, IPlayerBehaviour
{
	IPlayerAction curPlayerAction;
	AttackController attackController = new AttackController();
	DashController dashController = new DashController();

	void Awake()
	{
	}

    public void Begin()
	{

	}
	public void Update()
	{
		if(curActionKey != ActionKey.None)
		{
			if(curActionKey == ActionKey.LeftShift)
				curPlayerAction = dashController;
			else
				curPlayerAction = attackController;
		}
	} 
	public void End()
	{

	}
}

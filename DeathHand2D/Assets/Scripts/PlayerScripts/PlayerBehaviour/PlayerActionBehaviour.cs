using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StatesManager;
using static InputManager;

public class PlayerActionBehaviour : MonoBehaviour, IPlayerBehaviour
{
	Player play;

	IPlayerAction curPlayerAction;
	PlayerAttackController attackController = new PlayerAttackController();
	DashController dashController = new DashController();

	public PlayerActionBehaviour(Player playComponent)
	{
		play = playComponent;
	}
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

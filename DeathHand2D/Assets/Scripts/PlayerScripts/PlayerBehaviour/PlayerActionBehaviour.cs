using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StatesManager;
using static InputManager;

[RequireComponent(typeof(PlayerAttackController), typeof(PlayerDashController))]
public class PlayerActionBehaviour : MonoBehaviour, IPlayerBehaviour
{

	private IPlayerAction curPlayerAction;
	private Player player;
	private PlayerAttackController attackController;
	private PlayerDashController dashController;

	void Awake()
	{
		player = GetComponent<Player>();
		attackController = GetComponent<PlayerAttackController>();
		dashController = GetComponent<PlayerDashController>();
	}

    public void Begin()
	{
		playerState = PlayerState.Action;

		if(actionState == ActionState.Ready)
		{
			if (curActionKey != ActionKey.LeftShift)
			{
				curPlayerAction = attackController;
			}
			else
				curPlayerAction = dashController;

			curPlayerAction.Begin();
		}
	}
	public void Update()
	{
		
	}
	public void End()
	{
		playerState = PlayerState.Idle;
		//actionState = ActionState.None;
	}
}

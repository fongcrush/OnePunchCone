using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StatesManager;
using static InputManager;

public class PlayerActionBehaviour : MonoBehaviour, IPlayerBehaviour
{

	private IPlayerAction curPlayerAction;
	private Player player;
	private PlayerAttackController attackController;
	private PlayerDashController dashController;

	public PlayerActionBehaviour(Player playComponent)
	{
		player           = playComponent;
		attackController = new PlayerAttackController(this);
		dashController   = new PlayerDashController(this);
	}
	void Awake()
	{
	}

    public void Begin()
	{

	}
	public void Update()
	{
		if(actionState == ActionState.Ready)
		{
			if(actionState == ActionState.Attack)
				curPlayerAction = attackController;
			else if(actionState == ActionState.Dash)
				curPlayerAction = dashController;

			curPlayerAction.Begin();
		}
	}
	public void End()
	{
		playerState = PlayerState.Idle;
		actionState = ActionState.None;
	}
}

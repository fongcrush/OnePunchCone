using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StatesManager;
using static InputManager;

public class PlayerAction : MonoBehaviour, IPlayerBehaviour
{
	IPlayerAction curPlayerAction;
	AttackController attackController = new AttackController();

	void Awake()
	{
		curPlayerAction = attackController;
	}

    public void Begin()
	{

	}
	public void Update()
	{

	}
	public void End()
	{

	}
}

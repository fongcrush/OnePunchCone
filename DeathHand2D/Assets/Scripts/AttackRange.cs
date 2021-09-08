using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
	public PlayerController player;
	public bool IncludeSelf = true;

	private float playerHalfScale; // (플레이어 크기 / 2)
	private float attackHalfRange; // (공격 범위 크기 / 2)
	private float collPosX;


	private void Awake()
	{
		player = GameObject.Find("Player").GetComponent<PlayerController>();
		playerHalfScale = player.transform.lossyScale.x / 2;
		attackHalfRange = transform.localScale.x / 2;
	}

	void OnEnable()
	{
		if(IncludeSelf)
		{
			if(player.LeftOrRight())
				collPosX = 1 - (playerHalfScale + attackHalfRange);
			else
				collPosX = playerHalfScale + attackHalfRange - 1;
		}
		else
		{
			if(player.LeftOrRight())
				collPosX = -(playerHalfScale + attackHalfRange);
			else
				collPosX = playerHalfScale + attackHalfRange;
		}
		this.transform.localPosition = new Vector3(collPosX, 0f, 0f);
	}
}

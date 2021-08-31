using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
	public PlayerController player;

	private float playerHalfScale;		// (�÷��̾� ũ�� / 2)
	public float maxAttackRange = 2.0f; // �⺻ ���� �ִ��Ÿ�

	private float collPosX;


	private void Awake()
	{
		playerHalfScale = player.transform.lossyScale.x / 2;
		this.transform.localScale = new Vector3(maxAttackRange, 1f, 1f);
	}

	void Update()
	{
		if(player.LeftOrRight())
			collPosX = -(playerHalfScale + maxAttackRange / 2);
		else
			collPosX = playerHalfScale + maxAttackRange / 2;
		this.transform.localPosition = new Vector3(collPosX, 0, 0);
	}

	void OnEnable()
	{
		if(player.LeftOrRight())
			collPosX = -(playerHalfScale + maxAttackRange / 2);
		else
			collPosX = playerHalfScale + maxAttackRange / 2;
		this.transform.localPosition = new Vector3(collPosX, 0, 0);
	}
}

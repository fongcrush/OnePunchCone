using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StatesManager;
using static InputManager;

public class PlayerMoveBehaviour : MonoBehaviour, IPlayerBehaviour
{
    public float walkSpeed = 3f;
    public float runSpeed = 7.5f;

    private Player player;
    private bool canRun;

    Vector3 moveDirection;

    private float curDoubleCheckTime = 0;
    void Awake()
	{
        player = GameObject.Find("Player").GetComponent<Player>();
        canRun = false;
        Debug.Log(player);
    }

    public void Begin()
	{
	}

	public void Update()
    {
        moveDirection = new Vector3(hAxis, vAxis, 0f).normalized;

        if(curArrowKey != ArrowKey.None)
        {
            WalkOrRun();
            Move();
        }
        Turn();
    }

	public void End()
	{

	}

    void WalkOrRun()
    {
		if(canRun)
        {
            if(curArrowKey == ArrowKey.Left)
                if(Input.GetKeyDown(KeyCode.LeftArrow)) moveMode = MoveMode.Run;

            if(curArrowKey == ArrowKey.Right)
                if(Input.GetKeyDown(KeyCode.RightArrow)) moveMode = MoveMode.Run;

            if(curArrowKey == ArrowKey.Up)
                if(Input.GetKeyDown(KeyCode.UpArrow)) moveMode = MoveMode.Run;

            if(curArrowKey == ArrowKey.Down)
                if(Input.GetKeyDown(KeyCode.DownArrow)) moveMode = MoveMode.Run;
        }
        else
		{
            canRun = true;
		}

        if(canRun)
            curDoubleCheckTime += Time.deltaTime;
        else
            curDoubleCheckTime = 0;

        if(curDoubleCheckTime > 0.5f && moveMode == MoveMode.Walk)
        {
            canRun = false;
            curDoubleCheckTime = 0;
        }        

        if(moveDirection == Vector3.zero)
        {
            moveMode = MoveMode.Idle;
            curArrowKey = ArrowKey.None;
            canRun = false;
        }
    }


    void Move()
    {
        Vector3 movePos = Vector3.zero;
        if(moveMode == MoveMode.Run)
        {
            movePos = player.transform.position + moveDirection * runSpeed * Time.deltaTime;
        }
        else
        {
            movePos = player.transform .position + moveDirection * walkSpeed * Time.deltaTime;
        }
        // 맵을 넘어가지 않도록 제한
        movePos.x = Mathf.Clamp(movePos.x, player.mapSizeMin.x, player.mapSizeMax.x);
        movePos.y = Mathf.Clamp(movePos.y, player.mapSizeMin.y, player.mapSizeMax.y);
        player.transform.position = movePos;
    }
    void Turn()
    {
        if(hAxis != 0)
        {
            if(hAxis > 0)
                characterDirection = CharacterDirection.Right;
            else
                characterDirection = CharacterDirection.Left;
        }

        if(LeftOrRight())
            player.transform.rotation = Quaternion.Euler(new Vector3(0, 180f, 0));
        else
            player.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }
}

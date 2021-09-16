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
    private Vector3 moveDirection;

    private float curRunCheckTime;

    private ArrowKey curDoubleCheckKey;

    public PlayerMoveBehaviour(Player playerComponent)
	{
        player = playerComponent;
        canRun = false;
        moveDirection = Vector3.zero;
        curRunCheckTime = 0;
    }

    public void Begin()
	{
	}

	public void Update()
    {
        moveDirection = new Vector3(hAxis, vAxis, 0f).normalized;

        MoveCheck();
        if(moveDirection != Vector3.zero)
            Move();
        Turn();
    }

	public void End()
	{

	}

    private void MoveCheck()
    {
        if(moveDirection == Vector3.zero)
            moveMode = MoveMode.Idle;
        else
            moveMode = MoveMode.Walk;

        if(curRunCheckTime > 0.5f)
        {
            canRun = false;
            curRunCheckTime = 0;
            curDoubleCheckKey = ArrowKey.None;
        }

        if(canRun)
        {
            curRunCheckTime += Time.deltaTime;
        }

        if(Input.GetKeyDown(KeyCode.LeftArrow)) { canRun = true; curDoubleCheckKey = ArrowKey.Left; curRunCheckTime = 0; }

        if(Input.GetKeyDown(KeyCode.RightArrow)) { canRun = true; curDoubleCheckKey = ArrowKey.Right; curRunCheckTime = 0; }

        if(Input.GetKeyDown(KeyCode.UpArrow)) { canRun = true; curDoubleCheckKey = ArrowKey.Up; curRunCheckTime = 0; }

        if(Input.GetKeyDown(KeyCode.DownArrow)) { canRun = true; curDoubleCheckKey = ArrowKey.Down; curRunCheckTime = 0; }

        if(curDoubleCheckKey == curArrowKey && canRun)
            moveMode = MoveMode.Run;

    }


    private void Move()
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
        movePos.x = Mathf.Clamp(movePos.x, Actor.mapSizeMin.x, Actor.mapSizeMax.x);
        movePos.y = Mathf.Clamp(movePos.y, Actor.mapSizeMin.y, Actor.mapSizeMax.y);
        player.transform.position = movePos;
    }
    private void Turn()
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

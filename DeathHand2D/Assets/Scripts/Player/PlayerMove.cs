using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStatesData;
using static InputManager;
using static GameMgr;

public class PlayerMove : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float runSpeed = 7.5f;

    private PlayerController player;
    private Rigidbody2D rigid;

    private ArrowKey curDoubleCheckKey;
    private Vector3 moveDirection;
    private float curRunCheckTime;
    private bool canRun;

    [HideInInspector]
    public bool[] CancelMoveKey;

    private void Awake()
    {
        player = GetComponent<PlayerController>();
        rigid = GetComponent<Rigidbody2D>();
        curDoubleCheckKey = ArrowKey.None;
        moveDirection = Vector3.zero;
        curRunCheckTime = 0;
        canRun = false;
        CancelMoveKey = new bool[4];
        for (int i = 0; i < CancelMoveKey.Length; i++)
            CancelMoveKey[i] = false;
    }

    public void UpdateMove()
    {
        moveDirection = new Vector3(hAxis, vAxis, 0f).normalized;

        MoveCheck();
        if (moveDirection != Vector3.zero)
            Move();
        Turn();

        if (curActionKey != ActionKey.None)
        {
            player.ActionMgr.Begin();
        }
    }

    public void End()
    {
        moveDirection = Vector3.zero;
    }

    private void MoveCheck()
    {
        if (moveDirection == Vector3.zero)
        {
            moveMode = MoveMode.Idle;
        }
        else if (moveMode != MoveMode.Run)
        {
            moveMode = MoveMode.Walk;
        }
        if (!canRun)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) { canRun = true; curDoubleCheckKey = ArrowKey.Left; curRunCheckTime = 0; }

            if (Input.GetKeyDown(KeyCode.RightArrow)) { canRun = true; curDoubleCheckKey = ArrowKey.Right; curRunCheckTime = 0; }

            if (Input.GetKeyDown(KeyCode.UpArrow)) { canRun = true; curDoubleCheckKey = ArrowKey.Up; curRunCheckTime = 0; }

            if (Input.GetKeyDown(KeyCode.DownArrow)) { canRun = true; curDoubleCheckKey = ArrowKey.Down; curRunCheckTime = 0; }
        }
        else
        {
            curRunCheckTime += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                if (curDoubleCheckKey == ArrowKey.Left) { moveMode = MoveMode.Run; } else { canRun = false; }

            if (Input.GetKeyDown(KeyCode.RightArrow))
                if (curDoubleCheckKey == ArrowKey.Right) { moveMode = MoveMode.Run; } else { canRun = false; }

            if (Input.GetKeyDown(KeyCode.UpArrow))
                if (curDoubleCheckKey == ArrowKey.Up) { moveMode = MoveMode.Run; } else { canRun = false; }

            if (Input.GetKeyDown(KeyCode.DownArrow))
                if (curDoubleCheckKey == ArrowKey.Down) { moveMode = MoveMode.Run; } else { canRun = false; }

            if (curRunCheckTime > 0.5f)
                canRun = false;
            if (!canRun)
                curRunCheckTime = 0;
        }
    }

    private void Move()
    {
        //CheckCollider();
        Vector3 movePos = Vector3.zero;
        if (moveMode == MoveMode.Run)
        {
            movePos = player.transform.position + moveDirection * runSpeed * Time.deltaTime;
        }
        else
        {
            movePos = player.transform.position + moveDirection * walkSpeed * Time.deltaTime;
        }
        // 맵을 넘어가지 않도록 제한
        movePos.x = Mathf.Clamp(movePos.x, GM.CurRoomMgr.MapSizeMin.x, GM.CurRoomMgr.MapSizeMax.x);
        movePos.y = Mathf.Clamp(movePos.y, GM.CurRoomMgr.MapSizeMin.y, GM.CurRoomMgr.MapSizeMax.y);
        rigid.MovePosition(movePos);

        //player.transform.position = movePos;
    }

    private void CheckCollider()
    {
        if (CancelMoveKey[0] && curArrowKey == ArrowKey.Left)
        {
            moveDirection.x = 0;
        }
        if (CancelMoveKey[1] && curArrowKey == ArrowKey.Right)
        {
            moveDirection.x = 0;
        }
        if (CancelMoveKey[2] && curArrowKey == ArrowKey.Up)
        {
            moveDirection.y = 0;
        }
        if (CancelMoveKey[3] && curArrowKey == ArrowKey.Down)
        {
            moveDirection.y = 0;
        }
    }

    private void Turn()
    {
        if (hAxis > 0)
        {
            characterDirection = CharacterDirection.Right;
            player.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else if (hAxis < 0)
        {
            characterDirection = CharacterDirection.Left;
            player.transform.rotation = Quaternion.Euler(new Vector3(0, 180f, 0));
        }
    }
}

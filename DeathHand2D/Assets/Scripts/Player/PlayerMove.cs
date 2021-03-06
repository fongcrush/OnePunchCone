using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStatesData;
using static InputManager;
using static GameMgr;
using static PlayerEffectMgr;

public class PlayerMove : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float runSpeed = 7.5f;

    private PlayerController player;
    private Rigidbody2D rigid;
    private Animator anim;
    private BuffMgr buffMgr;

    private ArrowKey curDoubleCheckKey;
    private Vector3 moveDirection;
    private float curRunCheckTime;
    private bool canRun;

    private void Awake()
    {
        player = GetComponent<PlayerController>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        curDoubleCheckKey = ArrowKey.None;
        moveDirection = Vector3.zero;
        curRunCheckTime = 0;
        canRun = false;
    }

    private void Start()
    {
        buffMgr = GameObject.Find("UI").GetComponent<BuffMgr>();
    }

    private void OnEnable()
	{
        playerState = PlayerState.None;
	}

	private void Update()
    {
        moveDirection = new Vector3(hAxis, vAxis, 0f).normalized;
        MoveCheck();
        Turn();

        if(curActionKey != ActionKey.None)
        {
            moveDirection = Vector3.zero;
            player.ActionMgr.Begin();
        }
    }

    private void FixedUpdate()
    {
        if(moveDirection != Vector3.zero)
            Move();
    }

	private void MoveCheck()
    {
        if (moveDirection == Vector3.zero)
        {
            moveMode = MoveMode.Idle;
            anim.SetInteger("Move", 0);
        }
        else if (moveMode != MoveMode.Run)
        {
            moveMode = MoveMode.Walk;
            anim.SetInteger("Move", 1);
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
            if(moveMode != MoveMode.Run)
            {
                if(Input.GetKeyDown(KeyCode.LeftArrow))
                    if(curDoubleCheckKey == ArrowKey.Left) { moveMode = MoveMode.Run; } else { canRun = false; }

                if(Input.GetKeyDown(KeyCode.RightArrow))
                    if(curDoubleCheckKey == ArrowKey.Right) { moveMode = MoveMode.Run; } else { canRun = false; }

                if(Input.GetKeyDown(KeyCode.UpArrow))
                    if(curDoubleCheckKey == ArrowKey.Up) { moveMode = MoveMode.Run; } else { canRun = false; }

                if(Input.GetKeyDown(KeyCode.DownArrow))
                    if(curDoubleCheckKey == ArrowKey.Down) { moveMode = MoveMode.Run; } else { canRun = false; }

                if(moveMode == MoveMode.Run)
                {
                    anim.SetInteger("Move", 2);
                    Destroy(Instantiate(PlayerEffect.Run_Effect[0], player.transform.position, Quaternion.identity),
                        PlayerEffect.Run_Effect[0].GetComponent<ParticleSystem>().main.duration);
                }
            }
            if (curRunCheckTime > 0.5f)
                canRun = false;
            if (!canRun)
                curRunCheckTime = 0;
        }
    }

    private void Move()
    {
        Vector3 movePos = Vector3.zero;

        //if (buffMgr.SlowDebuffCount > 0) 
        //{
        //    moveDirection *= (buffMgr.SlowDebuffCount * 0.1f) + 0.3f;
        //}

        if (moveMode == MoveMode.Run)
            movePos = player.transform.position + moveDirection * runSpeed * Time.deltaTime;
        else
            movePos = player.transform.position + moveDirection * walkSpeed * Time.deltaTime;


        // ???? ???????? ?????? ????
        movePos.x = Mathf.Clamp(movePos.x, GM.CurRoomMgr.MapSizeMin.x, GM.CurRoomMgr.MapSizeMax.x);
        movePos.y = Mathf.Clamp(movePos.y, GM.CurRoomMgr.MapSizeMin.y, GM.CurRoomMgr.MapSizeMax.y);
        rigid.position = movePos;
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

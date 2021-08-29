using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum MoveDirection
    {
        None,
        Left,
        LeftUp,
        LeftDown,
        Right,
        RightUp,
        RightDown,
        Down,
        Up
    }

    public PlayerFSM playerState;

    [SerializeField]
    private MoveDirection moveDirection;

    private float walkSpeed;
    private float[] FirstTime;
    private bool[] canRun;


    private void Awake()
    {
        walkSpeed = 1;
        FirstTime = new float[4];
        canRun = new bool[4];
        moveDirection = MoveDirection.None;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerState = GetComponent<PlayerFSM>();
        playerState.SetState("Idle");
    }

    // Update is called once per frame
    void Update()
    {
        InputKey();
    }

    void InputKey()
    {
        // 방향키 누르다가 반대편 방향키 누르면 양 방향 키입력 무시
        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow) == false)
        {
            // 연속키입력 확인용
            if (Input.GetKeyDown(KeyCode.LeftArrow)) { canRun[0] = CheckRun("LeftArrow"); }
            // 움직이는 함수
            WalkOrRun("LeftArrow", canRun[0]);
        }
        // 달리기 변수 초기화, 대쉬용 방향 처리
        if (Input.GetKeyUp(KeyCode.LeftArrow)) { canRun[0] = false; if (moveDirection == MoveDirection.LeftDown) { moveDirection = MoveDirection.Down; } if (moveDirection == MoveDirection.LeftUp) { moveDirection = MoveDirection.Up; } }
        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow) == false)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow)) { canRun[1] = CheckRun("RightArrow"); }
            WalkOrRun("RightArrow", canRun[1]);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow)) { canRun[1] = false; if (moveDirection == MoveDirection.RightDown) { moveDirection = MoveDirection.Down; } if (moveDirection == MoveDirection.RightUp) { moveDirection = MoveDirection.Up; } }
        if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.UpArrow) == false)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow)) { canRun[2] = CheckRun("DownArrow"); }
            WalkOrRun("DownArrow", canRun[2]);
        }
        if (Input.GetKeyUp(KeyCode.DownArrow)) { canRun[2] = false; }
        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.DownArrow) == false)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) { canRun[3] = CheckRun("UpArrow"); }
            WalkOrRun("UpArrow", canRun[3]);
        }
        if (Input.GetKeyUp(KeyCode.UpArrow)) { canRun[3] = false; }
        // 특정경우 움직임을 멈추는 경우들
        if ((Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow)) && (Input.GetKey(KeyCode.DownArrow) == false && Input.GetKey(KeyCode.UpArrow) == false) || (Input.GetKey(KeyCode.LeftArrow) == false && Input.GetKey(KeyCode.RightArrow) == false) && (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.UpArrow)) || Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.UpArrow))
        {
            playerState.SetState("Idle");
            moveDirection = MoveDirection.None;
        }
        if (Input.GetKey(KeyCode.LeftArrow) == false && Input.GetKey(KeyCode.RightArrow) == false && Input.GetKey(KeyCode.DownArrow) == false && Input.GetKey(KeyCode.UpArrow) == false)
        {
            playerState.SetState("Idle");
            moveDirection = MoveDirection.None;
        }
        // 대쉬
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (playerState.GetState() == "Run" || playerState.GetState() == "Walk")
            {
                DoDesh(moveDirection.ToString());
            }
        }
    }

    bool CheckRun(string keyCode)
    {
        if (keyCode == "LeftArrow")
        {
            // 0.5초 지나서부터 달리기 가능
            if (Time.time < 0.5f) FirstTime[0] = Time.time - 0.5f;
            if (FirstTime[0] > 0 && Time.time - FirstTime[0] <= 0.5f && Time.time - FirstTime[0] >= 0.01f)
            {
                FirstTime[0] = 0.0f;
                return true;
            }
            FirstTime[0] = Time.time;
        }
        if (keyCode == "RightArrow" && Time.time >= 0.5f)
        {
            if (Time.time < 0.5f) FirstTime[1] = Time.time - 0.5f;
            if (FirstTime[1] > 0 && Time.time - FirstTime[1] <= 0.5f && Time.time - FirstTime[1] >= 0.01f)
            {
                FirstTime[1] = 0.0f;
                return true;
            }
            FirstTime[1] = Time.time;
        }
        if (keyCode == "DownArrow" && Time.time >= 0.5f)
        {
            if (Time.time < 0.5f) FirstTime[2] = Time.time - 0.5f;
            if (FirstTime[2] > 0 && Time.time - FirstTime[2] <= 0.5f && Time.time - FirstTime[2] >= 0.01f)
            {
                FirstTime[2] = 0.0f;
                return true;
            }
            FirstTime[2] = Time.time;
        }
        if (keyCode == "UpArrow" && Time.time >= 0.5f)
        {
            if (Time.time < 0.5f) FirstTime[3] = Time.time - 0.5f;
            if (FirstTime[3] > 0 && Time.time - FirstTime[3] <= 0.5f && Time.time - FirstTime[3] >= 0.01f)
            {
                FirstTime[3] = 0.0f;
                return true;
            }
            FirstTime[3] = Time.time;
        }
        return false;
    }

    void WalkOrRun(string keyCode, bool run)
    {
        if (keyCode == "LeftArrow")
        {
            if (run == true)             
                transform.position = new Vector3(transform.position.x - walkSpeed * 5 * Time.deltaTime, transform.position.y, transform.position.z);
            else
                transform.position = new Vector3(transform.position.x - walkSpeed * Time.deltaTime, transform.position.y, transform.position.z);

            if (moveDirection == MoveDirection.Up)
                moveDirection = MoveDirection.LeftUp;
            else if(moveDirection == MoveDirection.Down)
                moveDirection = MoveDirection.LeftDown;
            else
                moveDirection = MoveDirection.Left;
        }

        if (keyCode == "RightArrow" )
        {
            if (run == true)
                transform.position = new Vector3(transform.position.x + walkSpeed * 5 * Time.deltaTime, transform.position.y, transform.position.z);
            else
                transform.position = new Vector3(transform.position.x + walkSpeed * Time.deltaTime, transform.position.y, transform.position.z);

            if (moveDirection == MoveDirection.Up)
                moveDirection = MoveDirection.RightUp;
            else if (moveDirection == MoveDirection.Down)
                moveDirection = MoveDirection.RightDown;
            else
                moveDirection = MoveDirection.Right;
        }

        if (keyCode == "DownArrow")
        {
            if (run == true)
                transform.position = new Vector3(transform.position.x, transform.position.y - walkSpeed * 5 * Time.deltaTime, transform.position.z);
            else
                transform.position = new Vector3(transform.position.x, transform.position.y - walkSpeed * Time.deltaTime, transform.position.z);

            if (moveDirection == MoveDirection.Right)            
                moveDirection = MoveDirection.RightDown;            
            else if (moveDirection == MoveDirection.Left)            
                moveDirection = MoveDirection.LeftDown;            
            else if(moveDirection != MoveDirection.RightDown && moveDirection != MoveDirection.LeftDown)            
                moveDirection = MoveDirection.Down;
            
        }

        if (keyCode == "UpArrow")
        {
            if (run == true)
                transform.position = new Vector3(transform.position.x, transform.position.y + walkSpeed * 5 * Time.deltaTime, transform.position.z);
            else
                transform.position = new Vector3(transform.position.x, transform.position.y + walkSpeed * Time.deltaTime, transform.position.z);

            if (moveDirection == MoveDirection.Right)
                moveDirection = MoveDirection.RightUp;
            else if (moveDirection == MoveDirection.Left)
                moveDirection = MoveDirection.LeftUp;
            else if (moveDirection != MoveDirection.RightUp && moveDirection != MoveDirection.LeftUp)
                moveDirection = MoveDirection.Up;
        }

        if (run == true) playerState.SetState("Run");
        else playerState.SetState("Walk");
    }

    void DoDesh(string direction) 
    {
        switch (direction) 
        {
            case "Left":
                break;
            case "LeftUp":
                break;
            case "LeftDown":
                break;
            case "Right":
                break;
            case "RightUp":
                break;
            case "RightDown":
                break;
            case "Down":
                break;
            case "Up":
                break;
        }
    }
}
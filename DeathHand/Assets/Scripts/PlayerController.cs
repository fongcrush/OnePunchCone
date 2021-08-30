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

    PlayerFSM playerState;

    [SerializeField]
    private MoveDirection moveDirection;

    private const float walkSpeed = 1;
    private float runSpeed;
    private const float deshSpeed = 10;

    private float[] FirstTime;
    private bool[] canRun;

    private void Awake()
    {
        runSpeed = walkSpeed * 5;
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
        if (Input.GetKeyDown(KeyCode.LeftShift))
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
                transform.position = new Vector3(transform.position.x - runSpeed * Time.deltaTime, transform.position.y, transform.position.z);
            else
                transform.position = new Vector3(transform.position.x - walkSpeed * Time.deltaTime, transform.position.y, transform.position.z);

            if (moveDirection == MoveDirection.Up)
                moveDirection = MoveDirection.LeftUp;
            else if (moveDirection == MoveDirection.Down)
                moveDirection = MoveDirection.LeftDown;
            else
                moveDirection = MoveDirection.Left;
        }

        if (keyCode == "RightArrow")
        {
            if (run == true)
                transform.position = new Vector3(transform.position.x + runSpeed * Time.deltaTime, transform.position.y, transform.position.z);
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
                transform.position = new Vector3(transform.position.x, transform.position.y - runSpeed * Time.deltaTime, transform.position.z);
            else
                transform.position = new Vector3(transform.position.x, transform.position.y - walkSpeed * Time.deltaTime, transform.position.z);

            if (moveDirection == MoveDirection.Right)
                moveDirection = MoveDirection.RightDown;
            else if (moveDirection == MoveDirection.Left)
                moveDirection = MoveDirection.LeftDown;
            else if (moveDirection != MoveDirection.RightDown && moveDirection != MoveDirection.LeftDown)
                moveDirection = MoveDirection.Down;

        }

        if (keyCode == "UpArrow")
        {
            if (run == true)
                transform.position = new Vector3(transform.position.x, transform.position.y + runSpeed * Time.deltaTime, transform.position.z);
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

    void DrawRayPoint(string direction)
    {
        switch (direction)
        {
            case "Left":
                Debug.DrawRay(transform.position, new Vector3(-deshSpeed, 0, 0), new Color(0, 1, 0), 1.0f);
                break;
            case "LeftUp":
                Debug.DrawRay(transform.position, new Vector3(-deshSpeed, 0, deshSpeed), new Color(0, 1, 0), 1.0f);
                break;
            case "LeftDown":
                Debug.DrawRay(transform.position, new Vector3(-deshSpeed, 0, -deshSpeed), new Color(0, 1, 0), 1.0f);
                break;
            case "Right":
                Debug.DrawRay(transform.position, new Vector3(deshSpeed, 0, 0), new Color(0, 1, 0), 1.0f);
                break;
            case "RightUp":
                Debug.DrawRay(transform.position, new Vector3(deshSpeed, 0, deshSpeed), new Color(0, 1, 0), 1.0f);
                break;
            case "RightDown":
                Debug.DrawRay(transform.position, new Vector3(deshSpeed, 0, -deshSpeed), new Color(0, 1, 0), 1.0f);
                break;
            case "Down":
                Debug.DrawRay(transform.position, new Vector3(0, 0, -deshSpeed), new Color(0, 1, 0), 1.0f);
                break;
            case "Up":
                Debug.DrawRay(transform.position, new Vector3(0, 0, deshSpeed), new Color(0, 1, 0), 1.0f);
                break;
        }
    }

    bool RayCast2D(string direction) 
    {
        RaycastHit[] raycastHit;
        switch (direction)
        {
            case "Left":
                Debug.Log("left");
                raycastHit = Physics.RaycastAll(transform.position, new Vector3(-1, 0, 0), 10.0f);                
                break;
            case "LeftUp":
                raycastHit = Physics.RaycastAll(transform.position, new Vector3(-1, 0, 1), 10.0f);
                break;
            case "LeftDown":
                raycastHit = Physics.RaycastAll(transform.position, new Vector3(-1, 0, -1), 10.0f);
                break;
            case "Right":
                Debug.Log("right");
                raycastHit = Physics.RaycastAll(transform.position, new Vector3(1, 0, 0), 10.0f);
                break;
            case "RightUp":
                raycastHit = Physics.RaycastAll(transform.position, new Vector3(1, 0, 1), 10.0f);
                break;
            case "RightDown":
                raycastHit = Physics.RaycastAll(transform.position, new Vector3(1, 0, -1), 10.0f);
                break;
            case "Down":
                raycastHit = Physics.RaycastAll(transform.position, new Vector3(0, 0, -1), 10.0f);
                break;
            case "Up":
                raycastHit = Physics.RaycastAll(transform.position, new Vector3(0, 0, 1), 10.0f);
                break;
            default:
                Debug.LogWarning("RayCast No Direction");
                raycastHit = Physics.RaycastAll(transform.position, new Vector3(1, 0, 0), 0.0f);
                break;
        }
        for (int i = 0; i < raycastHit.Length; i++)
        {
            if (raycastHit[i].transform.tag == "Wall")
            {
                Debug.LogWarning("RayToWall You Can't Dash");
                return false;
            }            
        }
        return true;
    }

    void DoDesh(string direction)
    {
        switch (direction)
        {
            case "Left":
                DrawRayPoint("Left");
                if(RayCast2D("Left"))
                    transform.position = new Vector3(transform.position.x - deshSpeed, transform.position.y, transform.position.z);
                break;
            case "LeftUp":
                DrawRayPoint("LeftUp");
                if (RayCast2D("LeftUp"))
                    transform.position = new Vector3(transform.position.x - deshSpeed, transform.position.y + deshSpeed, transform.position.z);
                break;
            case "LeftDown":
                DrawRayPoint("LeftDown");
                if (RayCast2D("LeftDown"))
                    transform.position = new Vector3(transform.position.x - deshSpeed, transform.position.y - deshSpeed, transform.position.z);
                break;
            case "Right":
                DrawRayPoint("Right");
                if (RayCast2D("Right"))
                    transform.position = new Vector3(transform.position.x + deshSpeed, transform.position.y, transform.position.z);
                break;
            case "RightUp":
                DrawRayPoint("RightUp");
                if (RayCast2D("RightUp"))
                    transform.position = new Vector3(transform.position.x + deshSpeed, transform.position.y + deshSpeed, transform.position.z);
                break;
            case "RightDown":
                DrawRayPoint("RightDown");
                if (RayCast2D("RightDown"))
                    transform.position = new Vector3(transform.position.x + deshSpeed, transform.position.y - deshSpeed, transform.position.z);
                break;
            case "Down":
                DrawRayPoint("Down");
                if (RayCast2D("Down"))
                    transform.position = new Vector3(transform.position.x, transform.position.y - deshSpeed, transform.position.z);
                break;
            case "Up":
                DrawRayPoint("Up");
                if (RayCast2D("Up"))
                    transform.position = new Vector3(transform.position.x, transform.position.y + deshSpeed, transform.position.z);
                break;
        }
    }
}

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

    enum CharacterDirection
	{
        Right,
        Left
	}

    Player player;
    PlayerFSM playerState;

    [SerializeField]
    private MoveDirection moveDirection;
    private CharacterDirection characterDirection;

    [SerializeField]
    private int dashCount;

    private bool useDash;

    private const float WalkSpeed = 1;
    private float runSpeed;
    private const float DeshSpeed = 10;

    private const float MinWallDistance = 0.5f;
    private float[] FirstTime;
    private bool[] canRun;

    private Transform AttackCollObject;

    private void Awake()
    {
        runSpeed = WalkSpeed * 5;
        FirstTime = new float[4];
        canRun = new bool[4];
        moveDirection = MoveDirection.None;
        characterDirection = CharacterDirection.Right;
        dashCount = 2;
        useDash = false;
        AttackCollObject = GameObject.Find("Player").transform.Find("AttackColl");
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        playerState = GetComponent<PlayerFSM>();
        playerState.State = "Idle";
    }

    // Update is called once per frame
    void Update()
    {
        if(playerState.State != "Attack" && playerState.State != "Skill1" && playerState.State != "Skill2")
        {
            MoveInput();
            ActionInput();
        }
    }

    public int DashCount
    {
        get { return dashCount; }
        set { dashCount = value; }
    }
    public bool UseDash
    {
        get { return useDash; }
        set { useDash = value; }
    }

    /// <summary>
    /// ĳ���Ͱ� ���� ������ ���� return false
    /// ������ ������ ���� return true
    /// </summary>
    /// <returns></returns>
    public bool LeftOrRight()
	{
        if(characterDirection == CharacterDirection.Left)
            return true;
        else
            return false;
	}

    void MoveInput()
    {
        // ����Ű �����ٰ� �ݴ��� ����Ű ������ �� ���� Ű�Է� ����
        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow) == false)
        {
            characterDirection = CharacterDirection.Left;
            // ����Ű�Է� Ȯ�ο�
            if (Input.GetKeyDown(KeyCode.LeftArrow)) { canRun[0] = CheckRun("LeftArrow"); }
            // �����̴� �Լ�
            WalkOrRun("LeftArrow", canRun[0]);
        }
        // �޸��� ���� �ʱ�ȭ, �뽬�� ���� ó��
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            canRun[0] = false; if (moveDirection == MoveDirection.LeftDown) moveDirection = MoveDirection.Down;
            if (moveDirection == MoveDirection.LeftUp) moveDirection = MoveDirection.Up;
        }
        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow) == false)
        {
            characterDirection = CharacterDirection.Right;
            if (Input.GetKeyDown(KeyCode.RightArrow)) { canRun[1] = CheckRun("RightArrow"); }
            WalkOrRun("RightArrow", canRun[1]);
        }

        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            canRun[1] = false; if (moveDirection == MoveDirection.RightDown) { moveDirection = MoveDirection.Down;} 
            if (moveDirection == MoveDirection.RightUp) moveDirection = MoveDirection.Up;
        }
        if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.UpArrow) == false)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow)) canRun[2] = CheckRun("DownArrow");
            WalkOrRun("DownArrow", canRun[2]);
        }

        if (Input.GetKeyUp(KeyCode.DownArrow)) { canRun[2] = false; }
        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.DownArrow) == false)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) { canRun[3] = CheckRun("UpArrow"); }
            WalkOrRun("UpArrow", canRun[3]);
        }
        if (Input.GetKeyUp(KeyCode.UpArrow)) { canRun[3] = false; }
        // Ư����� �������� ���ߴ� ����
        if ((Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow)) && (Input.GetKey(KeyCode.DownArrow) == false && Input.GetKey(KeyCode.UpArrow) == false) || (Input.GetKey(KeyCode.LeftArrow) == false && Input.GetKey(KeyCode.RightArrow) == false) && (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.UpArrow)) || Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.UpArrow))
        {
            playerState.State = "Idle";
            moveDirection = MoveDirection.None;
        }
        if (Input.GetKey(KeyCode.LeftArrow) == false && Input.GetKey(KeyCode.RightArrow) == false && Input.GetKey(KeyCode.DownArrow) == false && Input.GetKey(KeyCode.UpArrow) == false)
        {
            playerState.State = "Idle";
            moveDirection = MoveDirection.None;
        }
        // �뽬
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCount > 0 && useDash == false)
        {
            if (playerState.State == "Run" || playerState.State == "Walk")
            {
                DoDesh(moveDirection.ToString());
            }
            if (playerState.State == "Idle")
            {
                DoDesh(moveDirection.ToString());
            }
        }
    }

    void ActionInput()
    {
        // ����
        if(Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(AttackRountine(0.5f, 1f));
        }
        // ��ų 1
        if(Input.GetKeyDown(KeyCode.X))
        {
        }
        //��ų 2
        if(Input.GetKeyDown(KeyCode.C))
        {
        }
    }

    IEnumerator AttackRountine(float delay, float attackSpeed)
    {
        playerState.State = "Attack";
        player.stat.Power = player.AttackDamage;
        AttackCollObject.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        AttackCollObject.gameObject.GetComponent<BoxCollider>().enabled = true;
        Debug.Log("����");
        yield return new WaitForSeconds(0.1f);
        Debug.Log("���� �Ϸ�");
        AttackCollObject.gameObject.SetActive(false);
        AttackCollObject.gameObject.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(attackSpeed - delay);
        playerState.State = "Idle";
    }

    bool CheckRun(string keyCode)
    {
        if (keyCode == "LeftArrow")
        {
            // 0.5�� ���������� �޸��� ����
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
            if (RayCast("Left", MinWallDistance))
            {
                if (run == true)
                    transform.position = new Vector3(transform.position.x - runSpeed * Time.deltaTime, transform.position.y, transform.position.z);
                else
                    transform.position = new Vector3(transform.position.x - WalkSpeed * Time.deltaTime, transform.position.y, transform.position.z);

                if (moveDirection == MoveDirection.Up)
                    moveDirection = MoveDirection.LeftUp;
                else if (moveDirection == MoveDirection.Down)
                    moveDirection = MoveDirection.LeftDown;
                else
                    moveDirection = MoveDirection.Left;
            }
        }

        if (keyCode == "RightArrow")
        {
            if (RayCast("Right", MinWallDistance))
            {
                if (run == true)
                    transform.position = new Vector3(transform.position.x + runSpeed * Time.deltaTime, transform.position.y, transform.position.z);
                else
                    transform.position = new Vector3(transform.position.x + WalkSpeed * Time.deltaTime, transform.position.y, transform.position.z);

                if (moveDirection == MoveDirection.Up)
                    moveDirection = MoveDirection.RightUp;
                else if (moveDirection == MoveDirection.Down)
                    moveDirection = MoveDirection.RightDown;
                else
                    moveDirection = MoveDirection.Right;
            }
        }

        if (keyCode == "DownArrow")
        {
            if (RayCast("Down", MinWallDistance))
            {
                if (run == true)
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - runSpeed * Time.deltaTime);
                else
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - WalkSpeed * Time.deltaTime);

                if (moveDirection == MoveDirection.Right)
                    moveDirection = MoveDirection.RightDown;
                else if (moveDirection == MoveDirection.Left)
                    moveDirection = MoveDirection.LeftDown;
                else if (moveDirection != MoveDirection.RightDown && moveDirection != MoveDirection.LeftDown)
                    moveDirection = MoveDirection.Down;
            }
        }

        if (keyCode == "UpArrow")
        {
            if (RayCast("Up", MinWallDistance))
            {
                if (run == true)
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + runSpeed * Time.deltaTime);
                else
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + WalkSpeed * Time.deltaTime);

                if (moveDirection == MoveDirection.Right)
                    moveDirection = MoveDirection.RightUp;
                else if (moveDirection == MoveDirection.Left)
                    moveDirection = MoveDirection.LeftUp;
                else if (moveDirection != MoveDirection.RightUp && moveDirection != MoveDirection.LeftUp)
                    moveDirection = MoveDirection.Up;
            }
        }

        if (run == true) playerState.State = "Run";
        else playerState.State = "Walk";
    }

    void DrawRayPoint(string direction)
    {
        switch (direction)
        {
            case "Left":
                Debug.DrawRay(transform.position, new Vector3(-DeshSpeed, 0, 0), new Color(0, 1, 0), 1.0f);
                break;
            case "LeftUp":
                Debug.DrawRay(transform.position, new Vector3(-DeshSpeed, 0, DeshSpeed), new Color(0, 1, 0), 1.0f);
                break;
            case "LeftDown":
                Debug.DrawRay(transform.position, new Vector3(-DeshSpeed, 0, -DeshSpeed), new Color(0, 1, 0), 1.0f);
                break;
            case "Right":
                Debug.DrawRay(transform.position, new Vector3(DeshSpeed, 0, 0), new Color(0, 1, 0), 1.0f);
                break;
            case "RightUp":
                Debug.DrawRay(transform.position, new Vector3(DeshSpeed, 0, DeshSpeed), new Color(0, 1, 0), 1.0f);
                break;
            case "RightDown":
                Debug.DrawRay(transform.position, new Vector3(DeshSpeed, 0, -DeshSpeed), new Color(0, 1, 0), 1.0f);
                break;
            case "Down":
                Debug.DrawRay(transform.position, new Vector3(0, 0, -DeshSpeed), new Color(0, 1, 0), 1.0f);
                break;
            case "Up":
                Debug.DrawRay(transform.position, new Vector3(0, 0, DeshSpeed), new Color(0, 1, 0), 1.0f);
                break;
        }
    }

    bool RayCast(string direction, float maxDistance)
    {
        RaycastHit[] raycastHit;

        switch(direction)
        {
        case "Left":
            raycastHit = Physics.RaycastAll(transform.position, new Vector3(-1, 0, 0), maxDistance);
            break;
        case "LeftUp":
            raycastHit = Physics.RaycastAll(transform.position, new Vector3(-1, 0, 1), maxDistance);
            break;
        case "LeftDown":
            raycastHit = Physics.RaycastAll(transform.position, new Vector3(-1, 0, -1), maxDistance);
            break;
        case "Right":
            raycastHit = Physics.RaycastAll(transform.position, new Vector3(1, 0, 0), maxDistance);
            break;
        case "RightUp":
            raycastHit = Physics.RaycastAll(transform.position, new Vector3(1, 0, 1), maxDistance);
            break;
        case "RightDown":
            raycastHit = Physics.RaycastAll(transform.position, new Vector3(1, 0, -1), maxDistance);
            break;
        case "Down":
            raycastHit = Physics.RaycastAll(transform.position, new Vector3(0, 0, -1), maxDistance);
            break;
        case "Up":
            raycastHit = Physics.RaycastAll(transform.position, new Vector3(0, 0, 1), maxDistance);
            break;
        default:
            Debug.LogWarning("RayCast No Direction");
            raycastHit = Physics.RaycastAll(transform.position, new Vector3(1, 0, 0), 0.0f);
            break;
        }
        for(int i = 0; i < raycastHit.Length; i++)
        {
            if(raycastHit[i].transform.tag == "Wall")
            {
                switch(direction)
                {
                case "Left":
                    transform.position = new Vector3(raycastHit[i].point.x + 0.5f, transform.position.y, transform.position.z);
                    break;
                case "LeftUp":
                    transform.position = new Vector3(raycastHit[i].point.x + 0.5f, transform.position.y, raycastHit[i].point.z - 0.5f);
                    break;
                case "LeftDown":
                    transform.position = new Vector3(raycastHit[i].point.x + 0.5f, transform.position.y, raycastHit[i].point.z + 0.5f);
                    break;
                case "Right":
                    transform.position = new Vector3(raycastHit[i].point.x - 0.5f, transform.position.y, transform.position.z);
                    break;
                case "RightUp":
                    transform.position = new Vector3(raycastHit[i].point.x - 0.5f, transform.position.y, raycastHit[i].point.z - 0.5f);
                    break;
                case "RightDown":
                    transform.position = new Vector3(raycastHit[i].point.x - 0.5f, transform.position.y, raycastHit[i].point.z + 0.5f);
                    break;
                case "Down":
                    transform.position = new Vector3(transform.position.x, transform.position.y, raycastHit[i].point.z + 0.5f);
                    break;
                case "Up":
                    transform.position = new Vector3(transform.position.x, transform.position.y, raycastHit[i].point.z - 0.5f);
                    break;
                }
                Debug.LogWarning("RayToWall You Can't Dash of Move");
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
                if (RayCast("Left", 10.0f))
                {
                    dashCount -= 1;
                    useDash = true;
                    transform.position = new Vector3(transform.position.x - DeshSpeed, transform.position.y, transform.position.z);
                }
                break;
            case "LeftUp":
                DrawRayPoint("LeftUp");
                if (RayCast("LeftUp", 10.0f))
                {
                    dashCount -= 1;
                    useDash = true;
                    transform.position = new Vector3(transform.position.x - DeshSpeed, transform.position.y, transform.position.z + DeshSpeed);
                }
                break;
            case "LeftDown":
                DrawRayPoint("LeftDown");
                if (RayCast("LeftDown", 10.0f))
                {
                    dashCount -= 1;
                    useDash = true;
                    transform.position = new Vector3(transform.position.x - DeshSpeed, transform.position.y, transform.position.z - DeshSpeed);
                }
                break;
            case "Right":
                DrawRayPoint("Right");
                if (RayCast("Right", 10.0f))
                {
                    dashCount -= 1;
                    useDash = true;
                    transform.position = new Vector3(transform.position.x + DeshSpeed, transform.position.y, transform.position.z);
                }
                break;
            case "RightUp":
                DrawRayPoint("RightUp");
                if (RayCast("RightUp", 10.0f))
                {
                    dashCount -= 1;
                    useDash = true;
                    transform.position = new Vector3(transform.position.x + DeshSpeed, transform.position.y, transform.position.z + DeshSpeed);
                }
                break;
            case "RightDown":
                DrawRayPoint("RightDown");
                if (RayCast("RightDown", 10.0f))
                {
                    dashCount -= 1;
                    useDash = true;
                    transform.position = new Vector3(transform.position.x + DeshSpeed, transform.position.y, transform.position.z - DeshSpeed);
                }
                break;
            case "Down":
                DrawRayPoint("Down");
                if (RayCast("Down", 10.0f))
                {
                    dashCount -= 1;
                    useDash = true;
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - DeshSpeed);
                }
                break;
            case "Up":
                DrawRayPoint("Up");
                if (RayCast("Up", 10.0f))
                {
                    dashCount -= 1;
                    useDash = true;
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + DeshSpeed);
                }
                break;
            case "None":
                DrawRayPoint("Right");
                if (RayCast("Right", 10.0f))
                {
                    dashCount -= 1;
                    useDash = true;
                    transform.position = new Vector3(transform.position.x + DeshSpeed, transform.position.y, transform.position.z);
                }
                break;
        }
    }
}
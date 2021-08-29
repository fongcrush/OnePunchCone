using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerFSM playerState;

    private float walkSpeed;
    private float[] FirstTime;
    private bool[] canRun;

    private void Awake()
    {
        walkSpeed = 1;
        FirstTime = new float[4];
        canRun = new bool[4];
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
            // 달리기 변수 초기화
            if (Input.GetKeyUp(KeyCode.LeftArrow)) { canRun[0] = false; }
        }
        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow) == false)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow)) { canRun[1] = CheckRun("RightArrow"); }
            WalkOrRun("RightArrow", canRun[1]);
            if (Input.GetKeyUp(KeyCode.RightArrow)) { canRun[1] = false; }
        }
        if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.UpArrow) == false)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow)) { canRun[2] = CheckRun("DownArrow"); }
            WalkOrRun("DownArrow", canRun[2]);
            if (Input.GetKeyUp(KeyCode.DownArrow)) { canRun[2] = false; }
        }
        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.DownArrow) == false)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) { canRun[3] = CheckRun("UpArrow"); }
            WalkOrRun("UpArrow", canRun[3]);
            if (Input.GetKeyUp(KeyCode.UpArrow)) { canRun[3] = false; }
        }
        if(Input.GetKey(KeyCode.LeftArrow)==false && Input.GetKey(KeyCode.RightArrow) == false && Input.GetKey(KeyCode.DownArrow) == false && Input.GetKey(KeyCode.UpArrow) == false) 
        {
            playerState.SetState("Idle");
        }
    }

    bool CheckRun(string keyCode)
    {
        if (keyCode == "LeftArrow")
        {
            if (Time.time - FirstTime[0] <= 0.5f && Time.time - FirstTime[0] >= 0.01f)
            {
                FirstTime[0] = 0.0f;
                return true;
            }
            FirstTime[0] = Time.time;
        }
        if (keyCode == "RightArrow")
        {
            if (Time.time - FirstTime[1] <= 0.5f && Time.time - FirstTime[1] >= 0.01f)
            {
                FirstTime[1] = 0.0f;
                return true;
            }
            FirstTime[1] = Time.time;
        }
        if (keyCode == "DownArrow")
        {
            if (Time.time - FirstTime[2] <= 0.5f && Time.time - FirstTime[2] >= 0.01f)
            {
                FirstTime[2] = 0.0f;
                return true;
            }
            FirstTime[2] = Time.time;
        }
        if (keyCode == "UpArrow")
        {
            if (Time.time - FirstTime[3] <= 0.5f && Time.time - FirstTime[3] >= 0.01f)
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
        if (keyCode == "LeftArrow" && run == true) transform.position = new Vector3(transform.position.x - walkSpeed * 5 * Time.deltaTime, transform.position.y, transform.position.z);
        if (keyCode == "LeftArrow" && run == false) transform.position = new Vector3(transform.position.x - walkSpeed * Time.deltaTime, transform.position.y, transform.position.z);
        if (keyCode == "RightArrow" && run == true) transform.position = new Vector3(transform.position.x + walkSpeed * 5 * Time.deltaTime, transform.position.y, transform.position.z);
        if (keyCode == "RightArrow" && run == false) transform.position = new Vector3(transform.position.x + walkSpeed * Time.deltaTime, transform.position.y, transform.position.z);
        if (keyCode == "DownArrow" && run == true) transform.position = new Vector3(transform.position.x, transform.position.y - walkSpeed * 5 * Time.deltaTime, transform.position.z);
        if (keyCode == "DownArrow" && run == false) transform.position = new Vector3(transform.position.x, transform.position.y - walkSpeed * Time.deltaTime, transform.position.z);
        if (keyCode == "UpArrow" && run == true) transform.position = new Vector3(transform.position.x, transform.position.y + walkSpeed * 5 * Time.deltaTime, transform.position.z);
        if (keyCode == "UpArrow" && run == false) transform.position = new Vector3(transform.position.x, transform.position.y + walkSpeed * Time.deltaTime, transform.position.z);
        if (run == true) playerState.SetState("Run");
        else playerState.SetState("Walk");
    }
}

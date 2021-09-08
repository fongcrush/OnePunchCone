using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum CharacterDirection
    {
        Right,
        Left
    }

    Player player;
    PlayerFSM playerState;

    private CharacterDirection characterDirection;

    private Vector3 moveDirection;
    private Vector3 wallDirection;
    private Vector2 boxColliderSize;

    [SerializeField]
    private int dashCount;
    private bool useDash;

    private const float WalkSpeed = 1;
    private float runSpeed;
    private const float DashSpeed = 10;

    private const float MinWallDistance = 0.5f;
    private float[] FirstTime;
    private bool[] canRun;

    private Transform AttackCollObject;
    private Transform Skill1CollObject;
    private Transform Skill2CollObject;
    private Transform ChargeCollObject;

    private IEnumerator _AttackRountine = null;
    private IEnumerator _Skill1Rountine = null;

    bool isTiming;

    private void Awake()
    {
        runSpeed = WalkSpeed * 5;
        FirstTime = new float[4];
        canRun = new bool[4];
        characterDirection = CharacterDirection.Right;
        dashCount = 2;
        useDash = false;
        AttackCollObject = GameObject.Find("Player").transform.Find("AttackColl");
        Skill1CollObject = GameObject.Find("Player").transform.Find("Skill1Coll");
        Skill2CollObject = GameObject.Find("Player").transform.Find("Skill2Coll");
        ChargeCollObject = GameObject.Find("Player").transform.Find("ChargeColl");
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        playerState = GetComponent<PlayerFSM>();
        playerState.State = "Idle";
        boxColliderSize = GetComponent<BoxCollider2D>().size;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerState.State != "Attack" && playerState.State != "Skill1" && playerState.State != "Skill2")
        {
            MoveInput();
            ActionInput();
        }
        else if (playerState.State == "Attack" || playerState.State == "Skill1")
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && dashCount > 0)
            {
                AttackCancel();
                DoDash();
                playerState.State = "Idle";
            }
        }
        //Debug.Log(player.skills["Judgement"].coolTime + ", " + player.skills["Judgement"].curTime);

        if(player.skills["Judgement"].curTime == 0) 
        {
            player.canskill1 = true;
        }
        if (player.skills["Charge"].curTime == 0)
        {
            player.canskill2 = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Arrow")
        {
            Destroy(other.gameObject);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "PerfectTiming")
        {
            if (!isTiming)
            {
                Debug.Log("Timing!");
                isTiming = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "PerfectTiming")
        {
            isTiming = false;
        }
    }
    /* LeftShift(DoDash)
    
        if(isTiming)
        {
            isTiming = false;
            // code
        }
    */

    private void CheckPerfectTiming() 
    {
        if(isTiming == true) 
        {
            //player.skills["Judgement"].curTime = player.skills["Judgement"].coolTime;
            //player.skills["Charge"].curTime = player.skills["Charge"].coolTime;
            StopAllCoroutines();
            player.skills["Judgement"].curTime = 0;
            player.skills["Charge"].curTime = 0;
            isTiming = false;
        }
        else 
        {
            Debug.Log("dpdpdppdp");
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
    /// 캐릭터가 왼쪽 방향을 보면 return true
    /// 오른쪽 방향을 보면 return false
    /// </summary>
    /// <returns></returns>
    public bool LeftOrRight()
    {
        if (characterDirection == CharacterDirection.Left)
            return true;
        else
            return false;
    }

    void MoveInput()
    {
        moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0.0f).normalized;

        if ((wallDirection.x > 0 && moveDirection.x < 0) || (wallDirection.x < 0 && moveDirection.x > 0))
        {
            moveDirection.x = 0;
        }
        if ((wallDirection.y > 0 && moveDirection.y < 0) || (wallDirection.y < 0 && moveDirection.y > 0))
        {
            moveDirection.y = 0;
        }

        Debug.Log(moveDirection);

        if (moveDirection.x > 0)
        {
            DrawRayPoint(boxColliderSize.x / 2);
            RayCast(boxColliderSize.x / 2);
            characterDirection = CharacterDirection.Right;
            if (Input.GetKeyDown(KeyCode.RightArrow)) { canRun[1] = CheckRun("RightArrow"); }
            WalkOrRun(canRun[1]);
        }
        else if (moveDirection.x == 0)
        {
            canRun[0] = false;
            canRun[1] = false;
        }
        else if (moveDirection.x < 0)
        {
            DrawRayPoint(boxColliderSize.x / 2);
            RayCast(boxColliderSize.x / 2);
            characterDirection = CharacterDirection.Left;
            if (Input.GetKeyDown(KeyCode.LeftArrow)) { canRun[0] = CheckRun("LeftArrow"); }
            WalkOrRun(canRun[0]);
        }

        if (moveDirection.y > 0)
        {
            DrawRayPoint(boxColliderSize.y / 2);
            RayCast(boxColliderSize.y / 2);
            if (Input.GetKeyDown(KeyCode.UpArrow)) { canRun[3] = CheckRun("UpArrow"); }
            WalkOrRun(canRun[3]);
        }
        else if (moveDirection.y == 0)
        {
            canRun[2] = false;
            canRun[3] = false;
        }
        else if (moveDirection.y < 0)
        {
            DrawRayPoint(boxColliderSize.y / 2);
            RayCast(boxColliderSize.y / 2);
            if (Input.GetKeyDown(KeyCode.DownArrow)) { canRun[2] = CheckRun("DownArrow"); }
            WalkOrRun(canRun[2]);
        }

        if (moveDirection.x == 0 && moveDirection.y == 0) playerState.State = "Idle";
        // 대쉬
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCount > 0 && useDash == false)
        {
            if (playerState.State == "Run" || playerState.State == "Walk")
            {
                DoDash();
            }
            if (playerState.State == "Idle")
            {
                DoDash();
            }
        }
    }
    void AttackCancel()
    {
        if (playerState.State == "Attack")
        {
            StopCoroutine(_AttackRountine);
            AttackCollObject.gameObject.SetActive(false);
            AttackCollObject.gameObject.GetComponent<MeshCollider>().enabled = false;
        }
        else if (playerState.State == "Skill1")
        {
            StopCoroutine(_Skill1Rountine);
            Skill1CollObject.gameObject.SetActive(false);
            Skill1CollObject.gameObject.GetComponent<MeshCollider>().enabled = false;
        }
    }
    void ActionInput()
    {
        SkillInfo skill;
        // 공격
        if (Input.GetKeyDown(KeyCode.Z))
        {
            _AttackRountine = AttackRountine(0.5f, 0.8f);
            StartCoroutine(_AttackRountine);
        }
        // 스킬 1
        if(Input.GetKeyDown(KeyCode.X) && player.skills["Judgement"].curTime == 0f)
        {
            _Skill1Rountine = Skill1Rountine(0.5f, 1f);
            StartCoroutine(_Skill1Rountine);
            player.canskill1 = false;
        }
        //스킬 2
        if (Input.GetKeyDown(KeyCode.C) && player.skills["Charge"].curTime == 0f)
        {
            StartCoroutine(Skill2Rountine(0.5f, 1f));
            player.canskill2 = false;
        }
    }

    IEnumerator AttackRountine(float delay, float time)
    {
        playerState.State = "Attack";
        player.stat.Power = player.AttackDamage;
        AttackCollObject.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        AttackCollObject.gameObject.GetComponent<MeshCollider>().enabled = true;
        Debug.Log("공격");
        yield return new WaitForSeconds(0.1f);
        AttackCollObject.gameObject.SetActive(false);
        AttackCollObject.gameObject.GetComponent<MeshCollider>().enabled = false;
        yield return new WaitForSeconds(time - delay);
        playerState.State = "Idle";
    }

    IEnumerator Skill1Rountine(float delay, float time)
    {
        StartCoroutine(SkillTimer("Judgement"));

        playerState.State = "Skill1";
        player.stat.Power = player.Skill1Damage;
        Skill1CollObject.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);

        Skill1CollObject.gameObject.GetComponent<MeshCollider>().enabled = true;
        Debug.Log("스킬1 발동");
        yield return new WaitForSeconds(0.1f);
        Skill1CollObject.gameObject.SetActive(false);
        Skill1CollObject.gameObject.GetComponent<MeshCollider>().enabled = false;
        yield return new WaitForSeconds(time - delay);
        playerState.State = "Idle";
    }

    IEnumerator Skill2Rountine(float delay, float time)
    {
        StartCoroutine(SkillTimer("Charge"));

        float curTime = 0f;
        Vector3 startPos = transform.position;
        Vector3 dir;
        float dirX;
        if(LeftOrRight())
        {
            dirX = startPos.x - ChargeCollObject.localScale.x + 1.5f;
            if(dirX < -19)
                dirX = -19;
            dir = new Vector3(dirX, startPos.y, startPos.z);
        }
        else
        {
            dirX = startPos.x + ChargeCollObject.localScale.x - 1.5f;
            if(dirX > 19)
                dirX = 19;
            dir = new Vector3(dirX, startPos.y, startPos.z);
        }

        playerState.State = "Skill2";
        player.stat.Power = player.Skill2Damage;
        Skill2CollObject.gameObject.SetActive(true);
        ChargeCollObject.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);

        Skill2CollObject.gameObject.GetComponent<BoxCollider>().enabled = true;
        Debug.Log("스킬2 발동");
        yield return null;

        Vector3 fixedChargePos = ChargeCollObject.position;
        while (curTime < time - delay)
        {
            curTime += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, dir, Time.deltaTime * 20);
            ChargeCollObject.position = fixedChargePos;
            yield return null;
        }
        Skill2CollObject.gameObject.SetActive(false);
        ChargeCollObject.gameObject.SetActive(false);
        Skill2CollObject.gameObject.GetComponent<BoxCollider>().enabled = false;
        playerState.State = "Idle";
    }

    IEnumerator SkillTimer(string name)
	{
        while(player.skills[name].curTime < player.skills[name].coolTime)
		{
            player.skills[name].curTime += Time.deltaTime;
            yield return null;
		}
        player.skills[name].curTime = 0f;
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

    void WalkOrRun(bool run)
    {
        if (run == true)
            transform.position = transform.position + moveDirection * runSpeed * Time.deltaTime;
        else
            transform.position = transform.position + moveDirection * WalkSpeed * Time.deltaTime;

        if (run == true) playerState.State = "Run";
        else playerState.State = "Walk";
    }

    void DrawRayPoint(float maxDistance)
    {
        if (moveDirection.x == 0 && moveDirection.y == 0)
        {
            if (LeftOrRight())
                Debug.DrawRay(transform.position, Vector3.left * maxDistance, new Color(0, 1, 0), 1.0f);
            else
                Debug.DrawRay(transform.position, Vector3.right * maxDistance, new Color(0, 1, 0), 1.0f);
        }
        else
        {
            Debug.DrawRay(transform.position, moveDirection * maxDistance, new Color(0, 1, 0), 1.0f);
        }
    }

    bool RayCast(float maxDistance)
    {
        RaycastHit2D[] raycastHit;

        if (moveDirection.x == 0 && moveDirection.y == 0)
        {
            if (LeftOrRight())
                raycastHit = Physics2D.RaycastAll(transform.position, Vector3.left, maxDistance);
            else
                raycastHit = Physics2D.RaycastAll(transform.position, Vector3.right, maxDistance);
        }
        else
            raycastHit = Physics2D.RaycastAll(transform.position, moveDirection, maxDistance);
        wallDirection = Vector3.zero;
        for (int i = 0; i < raycastHit.Length; i++)
        {
            if (raycastHit[i].transform.tag == "Wall")
            {
                Vector2 playerPos = new Vector2(transform.position.x, transform.position.y);
                wallDirection = (playerPos - raycastHit[i].point).normalized;

                if (wallDirection.x > 0)
                {
                    transform.position = new Vector2(raycastHit[i].point.x + boxColliderSize.x / 2, raycastHit[i].point.y);
                }
                else if (wallDirection.x < 0)
                {
                    transform.position = new Vector2(raycastHit[i].point.x - boxColliderSize.x / 2, raycastHit[i].point.y);
                }
                if (wallDirection.y > 0)
                {
                    transform.position = new Vector2(raycastHit[i].point.x, raycastHit[i].point.y + boxColliderSize.y / 2);
                }
                else if (wallDirection.y < 0)
                {
                    transform.position = new Vector2(raycastHit[i].point.x, raycastHit[i].point.y - boxColliderSize.y / 2);
                }
                return false;
            }
        }
        return true;
    }

    void DoDash()
    {
        DrawRayPoint(DashSpeed);
        if (RayCast(DashSpeed))
        {
            dashCount -= 1;
            useDash = true;
            if (playerState.State != "Idle")
            {
                transform.position = transform.position + moveDirection * DashSpeed;
                return;
            }
            if (LeftOrRight())
                transform.position = transform.position + Vector3.left * DashSpeed;
            else
                transform.position = transform.position + Vector3.right * DashSpeed;
        }
    }
}
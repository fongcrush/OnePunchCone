using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum CharacterDirection
    {
        Left,
        Right
	}

	enum CurrentArrowKey
	{
        Left,
        Right,
        Up,
        Down
    }

    public Vector2 mapSizeMin, mapSizeMax;

    Player player;
    PlayerFSM playerState;

    private CharacterDirection characterDirection;
    private CurrentArrowKey currentArrowKey;

    private Vector3 moveDirection;
    private Vector3 wallDirection;
    private Vector2 boxColliderSize;

    [SerializeField]
    private int dashCount;
    private bool useDash;

    public float walkSpeed = 300f;
    public float runSpeed = 750f;
    private const float DashSpeed = 10;

    private const float MinWallDistance = 0.5f;
    //private float[] FirstTime;
    private bool canRun;
    private float curDoubleCheckTime = 0;

    private Transform attackCollObject;
    private Transform skill1CollObject;
    private Transform skill2CollObject;
    private Transform chargeCollObject;

    private SpriteRenderer playerSprite;

    private IEnumerator _AttackRountine = null;
    private IEnumerator _Skill1Rountine = null;

    float hAxis = 0;
    float vAxis = 0;

    bool isRun;
    bool isTiming;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Arrow")
        {
            Destroy(other.gameObject);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "PerfectTiming")
        {
            if(!isTiming)
            {
                Debug.Log("Timing!");
                isTiming = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "PerfectTiming")
        {
            isTiming = false;
        }
    }

    private void Awake()
    {
        //FirstTime = new float[4];
        canRun = false;
        isRun = false;
        characterDirection = CharacterDirection.Right;
        dashCount = 2;
        useDash = false;
        attackCollObject = GameObject.Find("Player").transform.Find("AttackColl");
        skill1CollObject = GameObject.Find("Player").transform.Find("Skill1Coll");
        skill2CollObject = GameObject.Find("Player").transform.Find("Skill2Coll");
        chargeCollObject = GameObject.Find("Player").transform.Find("ChargeColl");
        playerSprite = GetComponent<SpriteRenderer>();
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
            PlayerMoveInput();
            PlayerActionInput();
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

        Move();
        Turn();
    }

    void PlayerMoveInput()
    {
        if(curDoubleCheckTime > 0.5f)
        {
            canRun = false;
            curDoubleCheckTime = 0;
        }

        if(Input.GetKeyDown(KeyCode.LeftArrow))
            if(canRun && currentArrowKey == CurrentArrowKey.Left) { isRun = true; playerState.State = "Run"; }
            else { canRun = true; currentArrowKey = CurrentArrowKey.Left; }

        if(Input.GetKeyDown(KeyCode.RightArrow))
            if(canRun && currentArrowKey == CurrentArrowKey.Right) { isRun = true; playerState.State = "Run"; }
            else { canRun = true; currentArrowKey = CurrentArrowKey.Right; }

        if(Input.GetKeyDown(KeyCode.UpArrow))
            if(canRun && currentArrowKey == CurrentArrowKey.Up) { isRun = true; playerState.State = "Run"; }
            else { canRun = true; currentArrowKey = CurrentArrowKey.Up; }

        if(Input.GetKeyDown(KeyCode.DownArrow))
            if(canRun && currentArrowKey == CurrentArrowKey.Down) { isRun = true; playerState.State = "Run"; }
            else { canRun = true; currentArrowKey = CurrentArrowKey.Down; }

        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector3(hAxis, vAxis, 0).normalized;

        if(hAxis != 0)
        {
            if(hAxis > 0)
                characterDirection = CharacterDirection.Left;
            else
                characterDirection = CharacterDirection.Right;
        }

        if(canRun)
            curDoubleCheckTime += Time.deltaTime;

        if(moveDirection == Vector3.zero && !canRun)
		{
            isRun = false;
            playerState.State = "Idle";
		}
    }

    void PlayerActionInput()
    {
        // 공격
        if(Input.GetKeyDown(KeyCode.Z))
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
        if(Input.GetKeyDown(KeyCode.C) && player.skills["Charge"].curTime == 0f)
        {
            StartCoroutine(Skill2Rountine(0.5f, 1f));
            player.canskill2 = false;
        }
        // 대쉬
        if(Input.GetKeyDown(KeyCode.LeftShift) && dashCount > 0 && useDash == false)
        {
            if(playerState.State == "Run" || playerState.State == "Walk" || playerState.State == "Idle")
            {
                DoDash();
            }
        }
    }

    void Move()
    {
        Vector3 movePos = Vector3.zero;
		if(isRun == true)
		{
			movePos = transform.position + moveDirection * runSpeed * Time.deltaTime;
		}
		else
		{
			movePos = transform.position + moveDirection * walkSpeed * Time.deltaTime;
		}
        movePos.x = Mathf.Clamp(movePos.x, mapSizeMin.x, mapSizeMax.x);
        movePos.y = Mathf.Clamp(movePos.y, mapSizeMin.y, mapSizeMax.y);
        transform.position = movePos;
    }

    void Turn()
	{
        if(characterDirection == CharacterDirection.Left)
            playerSprite.flipX = false;
        else
            playerSprite.flipX = true;
    }


	private void CheckPerfectTiming() 
    {
        if(isTiming == true) 
        {
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

	void AttackCancel()
    {
        if (playerState.State == "Attack")
        {
            StopCoroutine(_AttackRountine);
            attackCollObject.gameObject.SetActive(false);
            attackCollObject.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        else if (playerState.State == "Skill1")
        {
            StopCoroutine(_Skill1Rountine);
            skill1CollObject.gameObject.SetActive(false);
            skill1CollObject.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    IEnumerator AttackRountine(float delay, float time)
    {
        playerState.State = "Attack";
        player.stat.Power = player.AttackDamage;
        attackCollObject.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        attackCollObject.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        Debug.Log("공격");
        yield return new WaitForSeconds(0.1f);
        attackCollObject.gameObject.SetActive(false);
        attackCollObject.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(time - delay);
        playerState.State = "Idle";
    }

    IEnumerator Skill1Rountine(float delay, float time)
    {
        StartCoroutine(SkillTimer("Judgement"));

        playerState.State = "Skill1";
        player.stat.Power = player.Skill1Damage;
        skill1CollObject.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);

        skill1CollObject.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        Debug.Log("스킬1 발동");
        yield return new WaitForSeconds(0.1f);
        skill1CollObject.gameObject.SetActive(false);
        skill1CollObject.gameObject.GetComponent<BoxCollider2D>().enabled = false;
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
            dirX = startPos.x - chargeCollObject.localScale.x + 1.5f;
            if(dirX < -19)
                dirX = -19;
            dir = new Vector3(dirX, startPos.y, startPos.z);
        }
        else
        {
            dirX = startPos.x + chargeCollObject.localScale.x - 1.5f;
            if(dirX > 19)
                dirX = 19;
            dir = new Vector3(dirX, startPos.y, startPos.z);
        }

        playerState.State = "Skill2";
        player.stat.Power = player.Skill2Damage;
        skill2CollObject.gameObject.SetActive(true);
        chargeCollObject.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);

        skill2CollObject.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        Debug.Log("스킬2 발동");
        yield return null;

        Vector3 fixedChargePos = chargeCollObject.position;
        while (curTime < time - delay)
        {
            curTime += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, dir, Time.deltaTime * 20);
            chargeCollObject.position = fixedChargePos;
            yield return null;
        }
        skill2CollObject.gameObject.SetActive(false);
        chargeCollObject.gameObject.SetActive(false);
        skill2CollObject.gameObject.GetComponent<BoxCollider2D>().enabled = false;
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
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
    private Vector2 boxColliderSize;

    [SerializeField]
    private int dashCount;
    private bool useDash;

    public float walkSpeed = 3f;
    public float runSpeed = 7.5f;
    private const float DashSpeed = 10;

    //private float[] FirstTime;
    private bool canRun;
    private float curDoubleCheckTime = 0;

    private Transform attackCollObject;
    private Transform skill1CollObject;
    private Transform skill2CollObject;
    private Transform chargeRange;

    private SpriteRenderer playerSprite;

    private IEnumerator _AttackRountine = null;
    private IEnumerator _Skill1Rountine = null;

    float hAxis = 0;
    float vAxis = 0;

    bool isRun;
    bool isTiming;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Arrow")
        {
            Destroy(collision.gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PerfectTiming")
        {
            if (!isTiming)
            {
                Debug.Log("Timing!");
                isTiming = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PerfectTiming")
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
        attackCollObject = GameObject.Find("Player Coll").transform.Find("AttackColl");
        skill1CollObject = GameObject.Find("Player Coll").transform.Find("Skill1Coll");
        skill2CollObject = GameObject.Find("Include Self").transform.Find("Skill2Coll");
        chargeRange = GameObject.Find("Include Self").transform.Find("ChargeRange");
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

            Move();
            Turn();
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

        if(player.skills["Judgement"].curTime == 0) 
        {
            player.canskill1 = true;
        }
        if (player.skills["Charge"].curTime == 0)
        {
            player.canskill2 = true;
        }
    }

    void PlayerMoveInput()
    {

        if(Input.GetKeyDown(KeyCode.LeftArrow))
            if(canRun && currentArrowKey == CurrentArrowKey.Left) { isRun = true; playerState.State = "Run"; }
            else { canRun = true; currentArrowKey = CurrentArrowKey.Left; playerState.State = "Walk"; }

        if(Input.GetKeyDown(KeyCode.RightArrow))
            if(canRun && currentArrowKey == CurrentArrowKey.Right) { isRun = true; playerState.State = "Run"; }
            else { canRun = true; currentArrowKey = CurrentArrowKey.Right; playerState.State = "Walk"; }

        if(Input.GetKeyDown(KeyCode.UpArrow))
            if(canRun && currentArrowKey == CurrentArrowKey.Up) { isRun = true; playerState.State = "Run"; }
            else { canRun = true; currentArrowKey = CurrentArrowKey.Up; playerState.State = "Walk"; }

        if(Input.GetKeyDown(KeyCode.DownArrow))
            if(canRun && currentArrowKey == CurrentArrowKey.Down) { isRun = true; playerState.State = "Run"; }
            else { canRun = true; currentArrowKey = CurrentArrowKey.Down; playerState.State = "Walk"; }

        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector3(hAxis, vAxis, 0).normalized;

        if(hAxis != 0)
        {
            if(hAxis > 0)
                characterDirection = CharacterDirection.Right;
            else
                characterDirection = CharacterDirection.Left;
        }

        if(curDoubleCheckTime > 0.5f)
        {
            canRun = false;
            curDoubleCheckTime = 0;
        }
        if(canRun)
            curDoubleCheckTime += Time.deltaTime;

        if (moveDirection == Vector3.zero)
        {
            playerState.State = "Idle";
            isRun = false;
		}
    }

    void PlayerActionInput()
    {
        // 공격
        if(Input.GetKeyDown(KeyCode.Z))
        {
            _AttackRountine = AttackRoutine(0.5f, 0.8f);
            StartCoroutine(_AttackRountine);
        }
        // 스킬 1
        if(Input.GetKeyDown(KeyCode.X) && player.skills["Judgement"].curTime == 0f)
        {
            _Skill1Rountine = Skill1Routine(0.5f, 1f);
            StartCoroutine(_Skill1Rountine);
            player.canskill1 = false;
        }
        //스킬 2
        if(Input.GetKeyDown(KeyCode.C) && player.skills["Charge"].curTime == 0f)
        {
            StartCoroutine(Skill2Routine(0.5f, 1f));
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
        // 맵을 넘어가지 않도록 제한
        movePos.x = Mathf.Clamp(movePos.x, mapSizeMin.x, mapSizeMax.x);
        movePos.y = Mathf.Clamp(movePos.y, mapSizeMin.y, mapSizeMax.y);
        transform.position = movePos;
    }

    void Turn()
	{
        if(LeftOrRight())
            transform.rotation = Quaternion.Euler(new Vector3(0, 180f, 0));
        else
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }


	private void CheckPerfectTiming() 
    {
        if(isTiming == true) 
        {
            StopAllCoroutines();
            player.skills["Judgement"].curTime = 0;
            player.skills["Charge"].curTime = 0;
            isTiming = false;
            Debug.Log(1);
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

    IEnumerator AttackRoutine(float delay, float time)
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

    IEnumerator Skill1Routine(float delay, float time)
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

    IEnumerator Skill2Routine(float delay, float time)
    {
        StartCoroutine(SkillTimer("Charge"));
        playerState.State = "Skill2";

        float curTime = 0;

        // 돌진 목표 지점 계산
        // 300(플레이어의 월드 크기) * chargeRange.localScale.x - 150(플레이어의 pivot 이 Bottom center)
        Vector3 dir;
		if(LeftOrRight())
			dir = new Vector3(chargeRange.position.x - 3f * (chargeRange.localScale.x - 0.5f), transform.position.y, 0);
		else
			dir = new Vector3(chargeRange.position.x + 3f * (chargeRange.localScale.x - 0.5f), transform.position.y, 0);
		dir.x = Mathf.Clamp(dir.x, mapSizeMin.x, mapSizeMax.x);
        dir.y = Mathf.Clamp(dir.y, mapSizeMin.y, mapSizeMax.y);

        player.stat.Power = player.Skill2Damage;
        skill2CollObject.gameObject.SetActive(true);
        chargeRange.gameObject.SetActive(true); 
        yield return new WaitForSeconds(delay);

        skill2CollObject.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        Debug.Log("스킬2 발동");
        yield return null;

        Vector3 fixedChargePos = chargeRange.position;
        while (curTime < time - delay)
        {
            curTime += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, dir, Time.deltaTime * 20);
            chargeRange.position = fixedChargePos;
            yield return null;
        }
        skill2CollObject.gameObject.SetActive(false);
        skill2CollObject.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        chargeRange.gameObject.SetActive(false);
        chargeRange.localPosition = Vector3.zero;
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


    void DoDash()
    {
        dashCount -= 1;
        useDash = true;
        CheckPerfectTiming();
        if (playerState.State != "Idle")
        {
            transform.position = transform.position + moveDirection * DashSpeed;
            return;
        }
        if (!LeftOrRight())
            transform.position = transform.position + Vector3.left * DashSpeed;
        else
            transform.position = transform.position + Vector3.right * DashSpeed;
    }
}
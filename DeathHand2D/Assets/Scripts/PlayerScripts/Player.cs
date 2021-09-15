using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;
using static StatesManager;

public class Player : Actor
{
    SkillManager skillManager;

    private Vector2 boxColliderSize;

    public IPlayerBehaviour curPlayerBehaviour;

    [SerializeField]
    private int dashCount;
    private bool useDash;

    private const float DashSpeed = 10;


    private Transform attackCollObject;
    private Transform skill1CollObject;
    private Transform skill2CollObject;
    private Transform skill3CollObject;
    private Transform chargeRange;

    private SpriteRenderer playerSprite;

    private IEnumerator _AttackRountine = null;
    private IEnumerator _Skill1Rountine = null;
    private IEnumerator _Skill3Rountine = null;

    bool isTiming;

    public Actor gameManager;

    [SerializeField]
    bool dashGodMode;
    float dashGodModeTime;
    const float MaxDashGodModeTimer = 0.3f;
    public StatusManager stat;

    public Dictionary<string, SkillInfo> skills;

    private const int AttackDamage = 50;

    private const int Skill1Damage = 300;
    private const float Skill1CoolTime = 10;
    private const int Skill2Damage = 150;
    private const float Skill2CoolTime = 15;
    private const int Skill3Damage = 70;

    public bool canskill1;
    public bool canskill2;
    public bool canskill3;

    float dashTime;
    const float MaxDashTimer = 10.0f;

    public int GetAttackDamage { get { return AttackDamage; } }
    public int GetSkill1Damage { get { return Skill1Damage; } }
    public int GetSkill2Damage { get { return Skill2Damage; } }
    public int GetSkill3Damage { get { return Skill3Damage; } }


    private void Awake()
    {
        gameManager = GameObject.Find("@GM").GetComponent<Actor>();
        backgroundSize = gameManager.backgroundSize;

        stat = new StatusManager(100, 100, 50);
        dashGodMode = false;
        dashGodModeTime = 0.0f;
        dashTime = 0.0f;
        canskill1 = true;
        canskill2 = true;
        canskill3 = false;

        characterDirection = CharacterDirection.Right;
        dashCount = 2;
        useDash = false;
        attackCollObject = GameObject.Find("Player Coll").transform.Find("AttackColl");
        skill1CollObject = GameObject.Find("Player Coll").transform.Find("Skill1Coll");
        skill2CollObject = GameObject.Find("Include Self").transform.Find("Skill2Coll");
        skill3CollObject = GameObject.Find("Player Coll").transform.Find("Skill3Coll");
        chargeRange = GameObject.Find("Include Self").transform.Find("ChargeRange");
        playerSprite = GetComponent<SpriteRenderer>();


        //skills = new Dictionary<string, SkillInfo>();
        //skills.Add("CrossSword", new SkillInfo(GetSkill1Damage, Skill1CoolTime, 0f));
        //skills.Add("Rush", new SkillInfo(GetSkill2Damage, Skill2CoolTime, 0f));
        //skills.Add("QuadSlash", new SkillInfo(GetSkill3Damage, Skill1CoolTime, 0f));
    }

    public PlayerState CurrentState()
	{
        return playerState;
	}

    // Start is called before the first frame update
    void Start()
    {
        playerState = PlayerState.Normal;
        boxColliderSize = GetComponent<BoxCollider2D>().size;

        skillManager = GameObject.Find("SkillManager").GetComponent<SkillManager>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDashCount();


        if(playerState.State == "Attack" || playerState.State == "Skill1" || playerState.State == "Skill3")
        {
            if(Input.GetKeyDown(KeyCode.LeftShift) && dashCount > 0)
            {
                AttackCancel();
                DoDash();
                playerState.State = "Idle";
            }
        }

        if(skills["CrossSword"].curTime == 0)
        {
            canskill1 = true;
        }
        if(skills["Rush"].curTime == 0)
        {
            canskill2 = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Arrow")
        {
            Destroy(collision.gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "PerfectTiming")
        {
            if(!isTiming)
            {
                isTiming = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "PerfectTiming")
        {
            isTiming = false;
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
        if(Input.GetKeyDown(KeyCode.X) && skills["CrossSword"].curTime == 0f)
        {
            _Skill1Rountine = Skill1Routine(0.5f, 1f);
            StartCoroutine(_Skill1Rountine);
            canskill1 = false;
        }

        //스킬 2
        if(Input.GetKeyDown(KeyCode.C) && skills["Rush"].curTime == 0f)
        {
            StartCoroutine(Skill2Routine(0.5f, 1f));
            canskill2 = false;
        }

        //스킬 2-1
        else if(Input.GetKeyDown(KeyCode.C) && canskill3 == true)
        {
            _Skill3Rountine = Skill3Routine(0.5f, 1f);
            StartCoroutine(_Skill3Rountine);
            canskill3 = false;
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

    private void CheckPerfectTiming()
    {
        if(isTiming == true)
        {
            StopAllCoroutines();
            skills["CrossSword"].curTime = 0;
            skills["Rush"].curTime = 0;
            skills["QuadSlash"].curTime = 0;
            isTiming = false;
        }
    }    

    void AttackCancel()
    {
        if(playerState.State == "Attack")
        {
            StopCoroutine(_AttackRountine);
            attackCollObject.gameObject.SetActive(false);
            attackCollObject.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        else if(playerState.State == "Skill1")
        {
            StopCoroutine(_Skill1Rountine);
            skill1CollObject.gameObject.SetActive(false);
            skill1CollObject.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        else if(playerState.State == "Skill3")
        {
            StopCoroutine(_Skill3Rountine);
            skill3CollObject.gameObject.SetActive(false);
            skill3CollObject.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    IEnumerator AttackRoutine(float delay, float time)
    {
        playerState.State = "Attack";

        stat.Power = GetAttackDamage;
        attackCollObject.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        attackCollObject.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        yield return new WaitForSeconds(0.1f);
        attackCollObject.gameObject.SetActive(false);
        attackCollObject.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(time - delay);
        playerState.State = "Idle";
    }

    IEnumerator Skill1Routine(float delay, float time)
    {
        StartCoroutine(SkillTimer("CrossSword"));
        playerState.State = "Skill1";

        stat.Power = player.GetSkill1Damage;
        skill1CollObject.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);

        skill1CollObject.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        yield return new WaitForSeconds(0.1f);
        skill1CollObject.gameObject.SetActive(false);
        skill1CollObject.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(time - delay);
        playerState.State = "Idle";
    }

    IEnumerator Skill2Routine(float delay, float time)
    {
        StartCoroutine(SkillTimer("Rush"));
        StartCoroutine(Skill3ComboTimer());
        playerState.State = "Skill2";

        float curTime = 0;

        // 돌진 목표 지점 계산
        // 300(플레이어의 월드 크기) * chargeRange.localScale.x - 150(플레이어의 pivot 이 Bottom center)
        Vector2 dir;
        if(LeftOrRight())
            dir = new Vector2(chargeRange.position.x - 3f * (chargeRange.localScale.x - 0.5f), transform.position.y);
        else
            dir = new Vector2(chargeRange.position.x + 3f * (chargeRange.localScale.x - 0.5f), transform.position.y);
        dir.x = Mathf.Clamp(dir.x, mapSizeMin.x, mapSizeMax.x);
        dir.y = Mathf.Clamp(dir.y, mapSizeMin.y, mapSizeMax.y);

        stat.Power = GetSkill2Damage;
        skill2CollObject.gameObject.SetActive(true);
        chargeRange.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);

        skill2CollObject.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        yield return null;

        Vector2 fixedChargePos = chargeRange.position;
        while(curTime < time - delay)
        {
            curTime += Time.deltaTime;
            transform.localPosition = Vector2.Lerp(transform.position, dir, Time.deltaTime * 20);
            chargeRange.position = fixedChargePos;
            yield return null;
        }
        skill2CollObject.gameObject.SetActive(false);
        skill2CollObject.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        chargeRange.gameObject.SetActive(false);
        chargeRange.localPosition = Vector2.zero;
        playerState.State = "Idle";
    }

    IEnumerator Skill3Routine(float delay, float time)
    {
        playerState.State = "Skill3";
        stat.Power = GetSkill3Damage;

        for(int i = 0; i < 4; i++)
        {
            skill3CollObject.gameObject.SetActive(true);
            yield return new WaitForSeconds(delay);
            skill3CollObject.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            yield return new WaitForSeconds(0.3f);
            skill3CollObject.gameObject.SetActive(false);
            skill3CollObject.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        yield return new WaitForSeconds(time - delay);
        playerState.State = "Idle";
    }

    IEnumerator SkillTimer(string name)
    {
        while(skills[name].curTime < skills[name].coolTime)
        {
            skills[name].curTime += Time.deltaTime;
            yield return null;
        }
        skills[name].curTime = 0f;
    }
    IEnumerator Skill3ComboTimer()
    {
        canskill3 = true;
        yield return new WaitForSeconds(5.0f);
        canskill3 = false;
        yield return null;
    }

    void DoDash()
    {
        dashCount -= 1;
        useDash = true;
        CheckPerfectTiming();
        if(playerState.State != "Idle" && playerState.State != "Attack" && playerState.State != "Skill1" && playerState.State != "Skill3")
        {
            transform.position = transform.position + moveDirection * DashSpeed;
            return;
        }
        if(LeftOrRight())
            transform.position = transform.position + Vector3.left * DashSpeed;
        else
            transform.position = transform.position + Vector3.right * DashSpeed;
    }

    void UpdateDashCount() 
    {
        if (DashCount < 2) 
        {
            dashTime += Time.deltaTime;

            if(UseDash == true) 
            {
                UpdateDashGodMode();
            }

            if(dashTime >= MaxDashTimer) 
            {
                DashCount += 1;
                dashTime = 0.0f;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Enemy") 
        {
            if (dashGodMode == false) 
            {
                //Hp -= 0.5f;
            }
        }
    }

    void UpdateDashGodMode() 
    {
        dashGodMode = true;
        dashGodModeTime += Time.deltaTime;
        if (dashGodModeTime >= MaxDashGodModeTimer) 
        {
            UseDash = false;
            dashGodMode = false;
            dashGodModeTime = 0.0f;
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
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;

public class Player : Actor
{
    public Actor gameManager;

    PlayerController playerController;

    [SerializeField]
    bool dashGodMode;
    float dashGodModeTime;
    const float MaxDashGodModeTimer = 0.3f;
    public StatusManager stat;

    public Dictionary<string, SkillInfo> skills;

    public int attackDamage = 50;

    public int skill1Damage = 250;
    public float skill1CoolTime;
    public int skill2Damage = 400;
    public float Skill2CoolTime;


    //테스트용 나중에 삭제
    public bool canskill1;
    public bool canskill2;

    float dashTime;
    const float MaxDashTimer = 3.0f;


    public int AttackDamage { get { return attackDamage; } }
    public int Skill1Damage { get { return skill1Damage; } }
    public int Skill2Damage { get { return skill2Damage; } }

    

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();

        gameManager = GameObject.Find("@GM").GetComponent<Actor>();
        backgroundSize = gameManager.backgroundSize;

        stat = new StatusManager(100, 100, 50);
        dashGodMode = false;
        dashGodModeTime = 0.0f;
        dashTime = 0.0f;

        skills = new Dictionary<string, SkillInfo>();
        skills.Add("Judgement", new SkillInfo(300, skill1CoolTime, 0f));
        skills.Add("Charge", new SkillInfo(150, Skill2CoolTime, 0f));
    }

    // Start is called before the first frame update
    void Start()
    { 
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDashCount();
    }

    void UpdateDashCount() 
    {
        if (playerController.DashCount < 2) 
        {
            dashTime += Time.deltaTime;

            if(playerController.UseDash == true) 
            {
                UpdateDashGodMode();
            }

            if(dashTime >= MaxDashTimer) 
            {
                playerController.DashCount += 1;
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
            playerController.UseDash = false;
            dashGodMode = false;
            dashGodModeTime = 0.0f;
        }
    }
}

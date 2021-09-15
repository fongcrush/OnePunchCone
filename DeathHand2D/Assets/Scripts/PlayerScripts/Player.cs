using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;

public class Player : Actor
{
    public Actor gameManager;
    [HideInInspector]
    public PlayerController playerController;

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

    public PlayerController GetPlayerController { get { return playerController; } }


    private void Awake()
    {
        playerController = GetComponent<PlayerController>();

        gameManager = GameObject.Find("@GM").GetComponent<Actor>();
        backgroundSize = gameManager.backgroundSize;

        stat = new StatusManager(100, 100, 50);
        dashGodMode = false;
        dashGodModeTime = 0.0f;
        dashTime = 0.0f;
        canskill1 = true;
        canskill2 = true;
        canskill3 = false;

        skills = new Dictionary<string, SkillInfo>();
        skills.Add("CrossSword", new SkillInfo(GetSkill1Damage, Skill1CoolTime, 0f));
        skills.Add("Rush", new SkillInfo(GetSkill2Damage, Skill2CoolTime, 0f));
        skills.Add("QuadSlash", new SkillInfo(GetSkill3Damage, Skill1CoolTime, 0f));
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;

public class Player : Actor
{
    PlayerController playerController;

    [SerializeField]
    bool dashGodMode;
    float dashGodModeTime;
    const float MaxDashGodModeTimer = 0.3f;
    public StatusManager stat;
    private int attackDamage = 50;
    private int skill1Damage = 250;
    private int skill2Damage = 400;

    const float Skill1MaxCollDown = 20.0f;
    const float Skill2MaxCollDown = 30.0f;

    float skill1CollDown;
    float skill2CollDown;

    float dashTime;
    const float MaxDashTimer = 3.0f;


    public int AttackDamage { get { return attackDamage; } }
    public int Skill1Damage { get { return skill1Damage; } }
    public int Skill2Damage { get { return skill2Damage; } }

    

    private void Awake()
    {
        stat = new StatusManager(100, 100, 50);
        dashGodMode = false;
        dashGodModeTime = 0.0f;
        dashTime = 0.0f;
        skill1CollDown = 0.0f;
        skill2CollDown = 0.0f;
    }

    // Start is called before the first frame update
    void Start()
    {        
        playerController = GetComponent<PlayerController>();        
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
                Hp -= 0.5f;
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
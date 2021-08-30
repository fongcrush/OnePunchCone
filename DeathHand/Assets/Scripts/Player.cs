using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;

public class Player : Actor
{
    PlayerController playerController;

    [SerializeField]
    bool deshGodMode;
    float deshGodModeTime;
    const float MaxDeshGodModeTimer = 0.3f;

    const float Skill1MaxCollDown = 20.0f;
    const float Skill2MaxCollDown = 30.0f;

    float skill1CollDown;
    float skill2CollDown;

    float dashTime;
    const float MaxDashTimer = 3.0f;

    private void Awake()
    {        
        Hp = 10.0f;
        deshGodMode = false;
        deshGodModeTime = 0.0f;
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
            if (deshGodMode == false) 
            {
                Hp -= 0.5f;
            }
        }
    }

    void UpdateDashGodMode() 
    {
        deshGodMode = true;
        deshGodModeTime += Time.deltaTime;
        if (deshGodModeTime >= MaxDeshGodModeTimer) 
        {
            playerController.UseDash = false;
            deshGodMode = false;
            deshGodModeTime = 0.0f;
        }
    }
}

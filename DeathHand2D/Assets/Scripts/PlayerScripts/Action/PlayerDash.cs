using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStatesData;
using static InputManager;

public class PlayerDash : IPlayerAction
{
    private const float MaxDashGodModeTimer = 0.3f;
    private const float MaxDashTimer = 10.0f;
    private const float DashSpeed = 10.0f;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        actionMgr = player.ActionMgr;

        if(player)
            StartCoroutine(UpdateDashCount());
    }
    public override void Begin()
    {
        actionState = ActionState.Dash;
        Dash();
    }

    public override void UpdateAction()
    {

    }

    public override void End()
    {
        actionState = ActionState.None;

        actionMgr.End();
    }

    public override void Quit()
    {

    }

    void Dash()
    {
        if(player.DashCount > 0)
        {
            player.DashCount -= 1;
            if(hAxis != 0 || vAxis != 0)
                player.transform.position = player.transform.position + new Vector3(hAxis, vAxis, 0f).normalized * DashSpeed;
            else
            {
                if(player.LeftOrRight())
                    player.transform.position = player.transform.position + Vector3.left * DashSpeed;
                else
                    player.transform.position = player.transform.position + Vector3.right * DashSpeed;
            }
            CheckPerfectTiming();
            if(player)
                StartCoroutine(UpdateGodMode());
        }
        End();
    }

    IEnumerator UpdateDashCount()
    {
        while(true)
        {
            while(player.DashCount < 2)
            {
                yield return new WaitForSeconds(MaxDashTimer - MaxDashGodModeTimer);
                player.DashCount++;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator UpdateGodMode()
    {
        player.DashGodMode = true;
        yield return new WaitForSeconds(MaxDashGodModeTimer);
        player.DashGodMode = false;
    }

    private void CheckPerfectTiming()
    {

    }
}
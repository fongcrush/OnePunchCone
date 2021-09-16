using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StatesManager;
using static InputManager;

public class DashController : MonoBehaviour, IPlayerAction
{
    private Player player;

    const float MaxDashGodModeTimer = 0.3f;
    const float MaxDashTimer = 10.0f;
    const float DashSpeed = 10.0f;

    private void Start()
    {
        StartCoroutine(UpdateDashCount());
    }
    public void Begin()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        DoDash();
    }
    public void Update()
    {
    }
    public void End()
    {
        actionState = ActionState.None;
    }

    void DoDash()
    {
        player.DashCount -= 1;
        if(moveMode != MoveMode.Idle)
        {
            transform.position = transform.position + new Vector3(hAxis, vAxis, 0f).normalized * DashSpeed;
            return;
        }
        if(LeftOrRight())
            transform.position = transform.position + Vector3.left * DashSpeed;
        else
            transform.position = transform.position + Vector3.right * DashSpeed;
        CheckPerfectTiming();
        StartCoroutine(UpdateGodMode());
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
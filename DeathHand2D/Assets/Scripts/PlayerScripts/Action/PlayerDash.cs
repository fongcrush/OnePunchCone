using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStatesData;
using static InputManager;

public class PlayerDash : MonoBehaviour, IPlayerAction
{
    private PlayerActionMgr actionBehaviour;
    private PlayerController player;
    private const float MaxDashGodModeTimer = 0.3f;
    private const float MaxDashTimer = 10.0f;
    private const float DashSpeed = 10.0f;

    public PlayerDash(PlayerActionMgr ActionBehaviour)
	{
        actionBehaviour = ActionBehaviour;
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void Start()
    {
        if(player)
        StartCoroutine(UpdateDashCount());
    }
    public void Begin()
    {
        actionState = ActionState.Dash;
        Dash();
    }

	public void UpdateAction()
	{
		
	}

	public void End()
    {
        actionBehaviour.End();
    }

    void Dash()
    {
        player.DashCount -= 1;
        player.transform.position = player.transform.position + new Vector3(hAxis, vAxis, 0f).normalized * DashSpeed;

        if(player.LeftOrRight())
            player.transform.position = player.transform.position + Vector3.left * DashSpeed;
        else
            player.transform.position = player.transform.position + Vector3.right * DashSpeed;
        CheckPerfectTiming();
        if(player)
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
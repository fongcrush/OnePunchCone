using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StatesManager;
using static InputManager;

public class PlayerDashController : MonoBehaviour, IPlayerAction
{
    private PlayerActionBehaviour actionBehaviour;
    private Player player;
    private const float MaxDashGodModeTimer = 0.3f;
    private const float MaxDashTimer = 10.0f;
    private const float DashSpeed = 10.0f;

    public PlayerDashController(PlayerActionBehaviour ActionBehaviour)
	{
        actionBehaviour = ActionBehaviour;
        player = GameObject.Find("Player").GetComponent<Player>();
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
    public void End()
    {
        actionBehaviour.End();
    }

    void Dash()
    {
        player.DashCount -= 1;
        player.transform.position = player.transform.position + new Vector3(hAxis, vAxis, 0f).normalized * DashSpeed;

        if(LeftOrRight())
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
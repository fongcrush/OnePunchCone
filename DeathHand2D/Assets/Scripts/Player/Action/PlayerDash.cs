using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStatesData;
using static InputManager;
using static PlayerAttackData;
using static GameMgr;

public class PlayerDash : PlayerAction
{
    private const float MaxDashGodModeTimer = 0.3f;
    public float MaxDashTimer = 10.0f;
    private const float DashSpeed = 5.0f;
    private Animator anim;

    Coroutine UpdateDashCountCoroutine = null;

    private void Awake()
    {
        player = GM.Player.GetComponent<PlayerController>();
        anim = player.GetComponent<Animator>();
    }

    public override void Quit()
    {
        actionState = ActionState.None;
    }

    public override bool Ready()
    {
        //player = GameObject.Find("Player").GetComponent<PlayerController>();
        return player.DashCount > 0;
    }

    public IEnumerator MovePos(float time)
    {
        float curTime = 0;

        Vector3 movePos = Vector3.zero;

        //가야하는 지점 계산
        if (hAxis != 0 || vAxis != 0)
            movePos = player.transform.position + new Vector3(hAxis, vAxis, 0f).normalized * DashSpeed;
        else
        {
            if (player.LeftOrRight())
                movePos = player.transform.position + Vector3.left * DashSpeed;
            else
                movePos = player.transform.position + Vector3.right * DashSpeed;
        }
        movePos.x = Mathf.Clamp(movePos.x, GM.CurRoomMgr.MapSizeMin.x, GM.CurRoomMgr.MapSizeMax.x);
        movePos.y = Mathf.Clamp(movePos.y, GM.CurRoomMgr.MapSizeMin.y, GM.CurRoomMgr.MapSizeMax.y);

        while (true)
        {
            curTime += Time.deltaTime;

            Debug.Log(Vector3.Lerp(player.transform.position, movePos, Time.deltaTime));
            player.transform.position = Vector3.Lerp(player.transform.position, movePos, Time.deltaTime);
            if(curTime>=time)
            break;
            yield return null;
        }
        yield return null;
    }

    public override IEnumerator ActionRoutine()
    {
        actionMgr = player.ActionMgr;
        actionState = ActionState.Dash;

        player.DashCount -= 1;
        if (player) 
        {
            if(UpdateDashCountCoroutine == null)
                UpdateDashCountCoroutine = StartCoroutine(UpdateDashCount());
            StartCoroutine(UpdateGodMode());
        }
        CheckPerfectTiming();

        anim.SetTrigger("Dodge");

        StartCoroutine(MovePos(1.5f));

        yield return new WaitForSeconds(1.5f);

        Quit();
        yield return null;
    }

    IEnumerator UpdateDashCount()
    {
        while (player.DashCount < 2)
        {
            yield return new WaitForSeconds(MaxDashTimer - MaxDashGodModeTimer);
            player.DashCount++;
        }
        UpdateDashCountCoroutine = null;
    }

    IEnumerator UpdateGodMode()
    {
        player.DashGodMode = true;
        yield return new WaitForSeconds(MaxDashGodModeTimer);
        player.DashGodMode = false;
    }

    private void CheckPerfectTiming()
    {
        if (player.isTiming == true)
        {
            foreach (var attackInfo in AttackTable)
            {
                attackInfo.Value.curTime = 0;
            }
        }
    }
}
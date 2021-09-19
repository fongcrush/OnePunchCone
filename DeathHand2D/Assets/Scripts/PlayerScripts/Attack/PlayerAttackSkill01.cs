using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStatesData;
using static PlayerAttackData;

public class PlayerAttackSkill01 : IPlayerAttack
{
    private PlayerController player;

    [SerializeField]
    private Transform coll;

    private PlayerAttackMgr attackMgr;

    private AttackInfo attackInfo;

    private float curTime;

    private bool isDone;

	public void Awake()
	{
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        attackMgr = player.AttackMgr;

        curTime = 0;
        isDone = false;
    }

    private void Start()
    {
        attackInfo = PlayerAttackData.AttackTable[100];
    }

    public override void Begin()
    {
        attackState = AttackState.Skill1;

        coll.gameObject.SetActive(true);
        player.stat.Power = Random.Range(attackInfo.min, attackInfo.max);
        curTime = 0;
        isDone = false;

        StartCoroutine(PlayerAttackData.SkillTimer(attackInfo.code));
        StartCoroutine(CheckDash());
    }

	public override void UpdateAttack()
    {
        if(curTime < attackInfo.delay) { } // ¼± µô·¹ÀÌ
        else if(curTime < attackInfo.delay + 0.1f)
        {
            if(coll.GetComponent<BoxCollider2D>().enabled == false)
                coll.GetComponent<BoxCollider2D>().enabled = true;
        }
        else if(curTime < attackInfo.cTime) { } // ÈÄ µô·¹ÀÌ
        else 
        {
            if(coll.gameObject.activeSelf)
            {
                coll.GetComponent<BoxCollider2D>().enabled = false;
                coll.gameObject.SetActive(false);
                End();
            }
        }
        curTime += Time.deltaTime;
    }

    public override void End()
    {
        attackState = AttackState.None;

        coll.gameObject.SetActive(false);
        isDone = true;
    }

	public override void Quit()
    {
        isDone = true;
        coll.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        coll.gameObject.SetActive(false);
        attackMgr.End();
    }

    IEnumerator CheckDash()
    {
        while(!isDone)
        {
            if(actionState == ActionState.Dash)
                Quit();
            yield return null;
        }
    }
}
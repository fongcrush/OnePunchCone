using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using static PlayerStatesData;
using static GameMgr;

public class PlayerAttackSkill02 : IPlayerAction
{
    [SerializeField]
    private Transform coll;

    [SerializeField]
    private Transform chargeRange;

	private Rigidbody2D rigid;

    private SkeletonAnimation skelAnim;

    private Animator anim;

    private AttackInfo attackInfo;
    public AttackInfo Info { get { return attackInfo; } }

    private Vector2 fixedChargePos;

    private Vector3 dir;

    private ActionStep attackStep;

    public void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        skelAnim = player.GetComponent<SkeletonAnimation>();
        anim = player.GetComponent<Animator>();
        rigid = player.GetComponent<Rigidbody2D>();
        dir = Vector3.zero;
        curTime = 0;
        attackStep = ActionStep.None;
    }

    private void Start()
    {
        actionMgr = player.ActionMgr;
        attackInfo = PlayerAttackData.AttackTable[102];
    }

	public override void Begin()
    {
        //Debug.Log("Attack Skill 02!");
        actionState = ActionState.Skill2;

        coll.gameObject.SetActive(true);
        chargeRange.gameObject.SetActive(true);
        GM.pcStat.Power = Random.Range(attackInfo.min, attackInfo.max);
        attackInfo = PlayerAttackData.AttackTable[102];
        curTime = 0;
        attackStep = ActionStep.First_Delay;

        // 돌진 목표 지점 계산
        if(player.LeftOrRight())
            dir = new Vector3(player.transform.position.x - 4f, player.transform.position.y, 0);
        else
            dir = new Vector3(player.transform.position.x + 4f, player.transform.position.y, 0);

        dir.x = Mathf.Clamp(dir.x, GM.CurRoomMgr.MapSizeMin.x, GM.CurRoomMgr.MapSizeMax.x);
        dir.y = Mathf.Clamp(dir.y, GM.CurRoomMgr.MapSizeMin.y, GM.CurRoomMgr.MapSizeMax.y);
        fixedChargePos = player.transform.position;

        anim.SetTrigger("Skill1");
        StartCoroutine(SkillTimer(attackInfo.code));
    }

    public override void UpdateAction()
    {
        switch(attackStep)
        {
        case ActionStep.First_Delay:
            if(curTime < attackInfo.fDelay) { }
            else
            {
                curTime = 0;
                attackStep = ActionStep.Action;
            }
            break;
        case ActionStep.Action:
            rigid.MovePosition(Vector3.Lerp(transform.position, dir, Time.deltaTime * 20));
            chargeRange.position = fixedChargePos;
            if(curTime > 1f)
            {
                curTime = 0;
                attackStep = ActionStep.Second_Delay;
            }
            break;
        case ActionStep.Second_Delay:
            if(curTime < attackInfo.sDelay) { }
            else
            {
                coll.gameObject.SetActive(false);
                coll.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                chargeRange.localPosition = Vector2.zero;
                chargeRange.gameObject.SetActive(false);
                End();
                StartCoroutine(CheckCombo(5f));
            }
            break;
        }
        curTime += Time.deltaTime;
    }

    public override void End()
    {
        actionState = ActionState.None;

        coll.gameObject.SetActive(false);
        chargeRange.gameObject.SetActive(false);
    }

	public override void Quit()
    {
        coll.gameObject.SetActive(false);
        coll.gameObject.GetComponent<BoxCollider2D>().enabled = false;

        chargeRange.localPosition = Vector2.zero;
        chargeRange.gameObject.SetActive(false);
        StartCoroutine(CheckCombo(5f));
    }

    IEnumerator CheckCombo(float checkTime)
	{
        actionMgr.CanSkill3 = true;
        float curTime = 0f;
        while(checkTime > curTime)
		{
            yield return null;
            curTime += Time.deltaTime;

            if(Input.GetKeyDown(KeyCode.C))
			{
                actionMgr.skill_03_On = true;
                actionMgr.Begin();
                break;
            }
        }
        actionMgr.CanSkill3 = false;
    }

    public override bool Ready()
    {
        return PlayerAttackData.AttackTable[102].curTime == 0;
    }
}

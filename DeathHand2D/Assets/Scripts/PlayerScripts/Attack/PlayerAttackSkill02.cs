using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStatesData;

public class PlayerAttackSkill02 : IPlayerAction
{
    [SerializeField]
    private Transform coll;

    [SerializeField]
    private Transform chargeRange;

    private AttackInfo attackInfo;
    public AttackInfo Info { get { return attackInfo; } }

    private Vector2 fixedChargePos;

    private Vector3 dir;

    private ActionStep attackStep;

    public void Awake()
	{
        player = GameObject.Find("Player").GetComponent<PlayerController>();
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
        Debug.Log("Attack Skill 02!");
        actionState = ActionState.Skill2;

        coll.gameObject.SetActive(true);
        chargeRange.gameObject.SetActive(true);
        player.stat.Power = Random.Range(attackInfo.min, attackInfo.max);
        attackInfo = PlayerAttackData.AttackTable[102];
        curTime = 0;
        attackStep = ActionStep.First_Delay;

        // 돌진 목표 지점 계산 : 300(플레이어의 월드 크기) * chargeRange.localScale.x - 150(플레이어의 pivot 이 Bottom center)
        if(player.LeftOrRight())
            dir = new Vector3(chargeRange.position.x - 3f * (chargeRange.localScale.x - 0.5f), player.transform.position.y, 0);
        else
            dir = new Vector3(chargeRange.position.x + 3f * (chargeRange.localScale.x - 0.5f), player.transform.position.y, 0);

        dir.x = Mathf.Clamp(dir.x, Actor.mapSizeMin.x, Actor.mapSizeMax.x);
        dir.y = Mathf.Clamp(dir.y, Actor.mapSizeMin.y, Actor.mapSizeMax.y);
        fixedChargePos = chargeRange.position;

        StartCoroutine(PlayerAttackData.SkillTimer(attackInfo.code));
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
            player.transform.position = Vector3.Lerp(transform.position, dir, Time.deltaTime * 20);
            chargeRange.position = fixedChargePos;
            if(player.transform.position == dir)
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
        Debug.Log("Attack Skill 02 end!");
        actionState = ActionState.None;

        Debug.Log("Skill 2 End");
        coll.gameObject.SetActive(false);
        chargeRange.gameObject.SetActive(false);
        //StartCoroutine(CheckCombo(5f));
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

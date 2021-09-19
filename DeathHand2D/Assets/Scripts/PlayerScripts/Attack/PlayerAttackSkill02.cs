using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStatesData;

public class PlayerAttackSkill02 : IPlayerAttack
{
    private PlayerController player;

    [SerializeField]
    private Transform coll;

    [SerializeField]
    private Transform chargeRange;

    private PlayerAttackMgr attackMgr;

    private AttackInfo attackInfo;

    Vector2 fixedChargePos;

    private Vector2 dir;

    private float curTime;

    private bool ComboOn;

	public void Awake()
	{
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        coll = GameObject.Find("AttackManager").transform.Find("Skill1Coll");
        attackMgr = player.AttackMgr;

        dir = Vector2.zero;
        curTime = 0;
        ComboOn = false;
    }

    private void Start()
    {
        attackInfo = PlayerAttackData.AttackTable[100];
    }

    public override void Begin()
    {
        attackState = AttackState.Skill2;

        coll.gameObject.SetActive(true);
        chargeRange.gameObject.SetActive(true);
        player.stat.Power = Random.Range(attackInfo.min, attackInfo.max);
        curTime = 0;

        // 돌진 목표 지점 계산 : 300(플레이어의 월드 크기) * chargeRange.localScale.x - 150(플레이어의 pivot 이 Bottom center)
        if(player.LeftOrRight())
            dir = new Vector2(chargeRange.position.x - 3f * (chargeRange.localScale.x - 0.5f), player.transform.position.y);
        else
            dir = new Vector2(chargeRange.position.x + 3f * (chargeRange.localScale.x - 0.5f), player.transform.position.y);

        dir.x = Mathf.Clamp(dir.x, Actor.mapSizeMin.x, Actor.mapSizeMax.x);
        dir.y = Mathf.Clamp(dir.y, Actor.mapSizeMin.y, Actor.mapSizeMax.y);
        fixedChargePos = chargeRange.position;

        StartCoroutine(PlayerAttackData.SkillTimer(attackInfo.code));
    }

	public override void UpdateAttack()
    {
        if(curTime < attackInfo.delay) { }
        else if(curTime < attackInfo.cTime)
        {
            player.transform.position = Vector2.Lerp(transform.position, dir, Time.deltaTime * 20);
            chargeRange.position = fixedChargePos;
        }
        else
        {
            coll.gameObject.SetActive(false);
            coll.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            chargeRange.localPosition = Vector2.zero;
            chargeRange.gameObject.SetActive(false);
            End();
        }
        curTime += Time.deltaTime;
    }

    public override void End()
    {
        attackState = AttackState.None;

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
    }

    IEnumerator CheckCombo(float checkTime)
	{
        float curTime = 0f;
        while(checkTime > curTime)
		{
            yield return null;
            curTime += Time.deltaTime;

            if(Input.GetKeyDown(KeyCode.C))
			{
                attackMgr.ChangeAttack(attackMgr.Skill_03);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StatesManager;

public class Skill2 : MonoBehaviour, IPlayerAttack
{
    private Player player;
    private PlayerAttackController attackController;
    private AttackInfo skillInfo;
    private Transform coll;
    private Transform chargeRange;
    private bool isDone;
    private bool ComboOn;

    public void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        attackController = GameObject.Find("AttackManager").GetComponent<PlayerAttackController>();
        skillInfo = PlayerAttackManager.attackTable[101];
        coll = GameObject.Find("AttackManager").transform.Find("Skill2Coll");
        chargeRange = GameObject.Find("Include Self").transform.Find("ChargeRange");
    }
    public void Run()
    {
        actionState = ActionState.Attack;
        player.stat.Power = Random.Range(skillInfo.min, skillInfo.max);
        isDone = false;
        StartCoroutine(AttackRoutine());
    }

    public void Quit()
    {
        isDone = true;
        actionState = ActionState.None;
    }

    IEnumerator AttackRoutine()
    {
        StartCoroutine(PlayerAttackManager.SkillTimer(skillInfo.code));

        float curTime = 0;

        // 돌진 목표 지점 계산 : 300(플레이어의 월드 크기) * chargeRange.localScale.x - 150(플레이어의 pivot 이 Bottom center)
        Vector2 dir;
        if(LeftOrRight())
            dir = new Vector2(chargeRange.position.x - 3f * (chargeRange.localScale.x - 0.5f), transform.position.y);
        else
            dir = new Vector2(chargeRange.position.x + 3f * (chargeRange.localScale.x - 0.5f), transform.position.y);

        dir.x = Mathf.Clamp(dir.x, Actor.mapSizeMin.x, Actor.mapSizeMax.x);
        dir.y = Mathf.Clamp(dir.y, Actor.mapSizeMin.y, Actor.mapSizeMax.y);

        coll.gameObject.SetActive(true);
        chargeRange.gameObject.SetActive(true);

        yield return new WaitForSeconds(skillInfo.delay);

        coll.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        yield return null;

        Vector2 fixedChargePos = chargeRange.position;
        while(curTime < skillInfo.cTime - skillInfo.delay)
        {
            curTime += Time.deltaTime;
            transform.localPosition = Vector2.Lerp(transform.position, dir, Time.deltaTime * 20);
            chargeRange.position = fixedChargePos;
            yield return null;
        }
        coll.gameObject.SetActive(false);
        coll.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        chargeRange.gameObject.SetActive(false);
        chargeRange.localPosition = Vector2.zero;

        Quit();
    }

    IEnumerator ComboCheck(float checkTime)
	{
        float curTime = 0f;
        while(checkTime > curTime)
		{
            yield return null;
            curTime += Time.deltaTime;

            if(Input.GetKeyDown(KeyCode.C))
			{
                attackController.ChangeAttack("skill3");
            }
        }
    }
}

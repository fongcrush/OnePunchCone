using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StatesManager;

public class Skill2 : MonoBehaviour, IPlayerAttack
{
    private Player player;
    private SkillInfo skillInfo;
    private Transform CollObject;
    private bool isDone;
    private bool ComboOn;

    public void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        skillInfo = AttackManager.skillTable[101];
        CollObject = GameObject.Find("AttackManager").transform.Find("Skill1Coll");
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
        StartCoroutine(AttackManager.SkillTimer(skillInfo.code));

        CollObject.gameObject.SetActive(true);
        yield return new WaitForSeconds(skillInfo.delay);
        CollObject.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        yield return new WaitForSeconds(0.1f);
        CollObject.gameObject.SetActive(false);
        CollObject.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(skillInfo.cTime - skillInfo.delay - 0.1f);
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

			}
        }
    }

    IEnumerator Skill2Routine(float delay, float time)
    {
        StartCoroutine(SkillTimer("Rush"));
        StartCoroutine(Skill3ComboTimer());
        playerState.State = "Skill2";

        float curTime = 0;

        // 돌진 목표 지점 계산
        // 300(플레이어의 월드 크기) * chargeRange.localScale.x - 150(플레이어의 pivot 이 Bottom center)
        Vector2 dir;
        if(LeftOrRight())
            dir = new Vector2(chargeRange.position.x - 3f * (chargeRange.localScale.x - 0.5f), transform.position.y);
        else
            dir = new Vector2(chargeRange.position.x + 3f * (chargeRange.localScale.x - 0.5f), transform.position.y);
        dir.x = Mathf.Clamp(dir.x, mapSizeMin.x, mapSizeMax.x);
        dir.y = Mathf.Clamp(dir.y, mapSizeMin.y, mapSizeMax.y);

        stat.Power = GetSkill2Damage;
        skill2CollObject.gameObject.SetActive(true);
        chargeRange.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);

        skill2CollObject.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        yield return null;

        Vector2 fixedChargePos = chargeRange.position;
        while(curTime < time - delay)
        {
            curTime += Time.deltaTime;
            transform.localPosition = Vector2.Lerp(transform.position, dir, Time.deltaTime * 20);
            chargeRange.position = fixedChargePos;
            yield return null;
        }
        skill2CollObject.gameObject.SetActive(false);
        skill2CollObject.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        chargeRange.gameObject.SetActive(false);
        chargeRange.localPosition = Vector2.zero;
        playerState.State = "Idle";
    }

}

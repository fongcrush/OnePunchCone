using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StatesManager;

public class Skill1 : MonoBehaviour, IPlayerAttack
{
    private Player player;
    private SkillInfo skillInfo;
    private Transform coll;
    private bool isDone;

    public void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        skillInfo = PlayerAttackManager.skillTable[101];
        coll = GameObject.Find("AttackManager").transform.Find("Skill1Coll");
        isDone = false;
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
        StartCoroutine(CheckDash());

        coll.gameObject.SetActive(true);
        yield return new WaitForSeconds(skillInfo.delay);
        coll.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        yield return new WaitForSeconds(0.1f);
        coll.gameObject.SetActive(false);
        coll.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(skillInfo.cTime - skillInfo.delay - 0.1f);
        Quit();
    }

    IEnumerator CheckDash()
    {
        while(isDone)
        {
            yield return null;
        }
    }
}

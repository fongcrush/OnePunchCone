using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StatesManager;

public class Skill3 : MonoBehaviour, IPlayerAttack
{
    private Player player;
    private SkillInfo skillInfo;
    private Transform coll;
    private bool isDone;

    public void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        skillInfo = PlayerAttackManager.skillTable[112];
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

        yield return new WaitForSeconds(skillInfo.delay);
        for(int i = 0; i < 4; i++)
        {
            coll.gameObject.SetActive(true);
            coll.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            yield return new WaitForSeconds(0.3f);
            coll.gameObject.SetActive(false);
            coll.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        yield return new WaitForSeconds(0.5f);
        Quit();
    }


    IEnumerator CheckDash()
    {
        while(!isDone)
        {
            if(actionState == ActionState.Dash)
            {
                StopCoroutine(AttackRoutine());
            }
            yield return null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using static PlayerStatesData;
using static GameMgr;

public class PlayerAttackSkill02 : PlayerAction
{
    private Rigidbody2D rigid;

    private SkeletonAnimation skelAnim;

    private Animator anim;

    private BoxCollider2D boxColl;

    private AttackInfo attackInfo;
    public AttackInfo Info { get { return attackInfo; } }

    private Vector3 dir;

    private bool canCombo = false;
    public bool CanCombo { get { return canCombo; } set { canCombo = value; } }

    private bool comboOn = false;
    public bool ComboOn { get { return comboOn; } }

    private bool on = false;

	public void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        skelAnim = player.GetComponent<SkeletonAnimation>();
        anim = player.GetComponent<Animator>();
        rigid = player.GetComponent<Rigidbody2D>();
        boxColl = GetComponent<BoxCollider2D>();
        dir = Vector3.zero;
    }

    private void Start()
    {
        actionMgr = player.ActionMgr;
        attackInfo = PlayerAttackData.AttackTable[102];
    }

    public override IEnumerator ActionRoutine()
    {
        StartCoroutine(SkillTimer(attackInfo.code));
        anim.SetTrigger("Skill1");
        actionState = ActionState.Skill2;
        isDone = true;

        // 돌진 목표 지점 계산
        if(player.LeftOrRight())
            dir = new Vector3(player.transform.position.x - 5f, player.transform.position.y, 0);
        else
            dir = new Vector3(player.transform.position.x + 5f, player.transform.position.y, 0);
        dir.x = Mathf.Clamp(dir.x, GM.CurRoomMgr.MapSizeMin.x, GM.CurRoomMgr.MapSizeMax.x);

        yield return new WaitForSeconds(attackInfo.fDelay);
        
        Vector2 castDir;
        if(player.LeftOrRight())
            castDir = Vector2.left;
        else
            castDir = Vector2.right;

        on = true;
        curTime = 0;
        RaycastHit2D[] hitResults = new RaycastHit2D[100];
        while(curTime  < 1f)
        {
            for(int i =0; i< boxColl.Cast(transform.position, hitResults, 0); i++)
            {
                if(hitResults[i].collider.gameObject.tag == "Enemy")
                {
                    var obj = hitResults[i].collider.transform.parent;                    
                    obj.transform.position = new Vector3(player.transform.position.x + castDir.x, obj.transform.position.y, 0);
                }
            }
            rigid.MovePosition(Vector3.Lerp(rigid.position, dir, Time.deltaTime * 20));
            curTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        on = false;
        actionState = ActionState.None;
        isDone = true;

        yield return CheckCombo(5f);
    }

    private IEnumerator CheckCombo(float checkTime)
	{
        canCombo = true;
        float curTime = 0f;
        while(checkTime > curTime)
		{
            yield return null;
            curTime += Time.deltaTime;

            if(Input.GetKeyDown(KeyCode.C))
			{
                actionMgr.Skill_03_On = true;
                break;
            }
        }
        canCombo = false;
    }

    public override void Quit()
    {
        StopCoroutine(ActionRoutine());
        StartCoroutine(CheckCombo(5f));
        isDone = true;
    }

    public override bool Ready()
    {
        return PlayerAttackData.AttackTable[102].curTime == 0;
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if(collision.gameObject.tag == "Enemy" && on)
        {
            int power = Random.Range(attackInfo.min, attackInfo.max);
            collision.transform.parent.GetComponent<EnemyController>().Hit(power);
        }
    }
}

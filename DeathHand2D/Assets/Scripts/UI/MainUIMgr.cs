using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static PlayerAttackData;

public class MainUIMgr : MonoBehaviour
{
    [SerializeField]
    private GameObject HPBar;

    [SerializeField]
    private GameObject MPBar;

    [SerializeField]
    private GameObject DashCount1;

    [SerializeField]
    private GameObject DashCount2;

    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private Image[] skillCoolImage;

    [SerializeField]
    private Image[] TwinkleIcon;

    [SerializeField]
    private Image[] skill;

    private PlayerAttackSkill02 skill_02;

    private BuffMgr buffManager;

	private void Awake()
	{
        buffManager = GetComponent<BuffMgr>();
        skill_02 = GameObject.Find("Skill2 Coll").GetComponent<PlayerAttackSkill02>();
    }

	void Update()
    {
        CheckPlayerDashCount();
        CheckPlayerUseSkill();
        CheckPlayerDebuff();
    }

    void CheckPlayerDashCount()
    {
        switch(playerController.DashCount)
        {
        case 0:
            DashCount1.SetActive(false);
            break;
        case 1:
            DashCount1.SetActive(true);
            DashCount2.SetActive(false);
            break;
        case 2:
            DashCount2.SetActive(true);
            break;
        }
    }

    void CheckPlayerUseSkill()
    {
        skillCoolImage[0].fillAmount = AttackTable[101].curTime / AttackTable[101].cTime;

        skillCoolImage[1].fillAmount = AttackTable[102].curTime / AttackTable[102].cTime;

        if(skill_02.CanCombo)
            skill[2].gameObject.SetActive(true);
        else
            skill[2].gameObject.SetActive(false);
    }

    void CheckPlayerDebuff()
    {
        if(playerController.GetComponent<PlayerEffectController>().DarkDebuff == true)
        {
            buffManager.DarkDebuff.SetActive(true);
            buffManager.DarkDebuff.transform.Find("Count").GetComponent<Text>().text = buffManager.DarkDebuffCount.ToString();
        }
        else
        {
            buffManager.DarkDebuff.SetActive(false);
        }
    }
    public IEnumerator SkillReady(short code)
    {
        float curTime = 0;
        int skillIndex = code - 101;

        TwinkleIcon[skillIndex].gameObject.SetActive(true);
        while(curTime < 0.25f)
        {
            float value = 1 - curTime * 4;
            TwinkleIcon[skillIndex].color = new Color(value, value, value);

            curTime += Time.deltaTime;
            yield return null;
        }
        while(curTime > 0f)
        {
            float value = 1 - curTime * 4;
            TwinkleIcon[skillIndex].color = new Color(value, value, value);

            curTime -= Time.deltaTime;
            yield return null;
        }
        TwinkleIcon[skillIndex].gameObject.SetActive(false);
    }
}

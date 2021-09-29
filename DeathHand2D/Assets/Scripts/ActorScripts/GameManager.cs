using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static PlayerAttackData;

public class GameManager : MonoBehaviour
{
    public GameObject background;

    private static GameManager instance = null;

    public static GameManager GM
    {
        get
        {
            if(instance == null)
            {                
                var obj = FindObjectOfType<GameManager>();
                if(obj == null)
                    instance = new GameObject().AddComponent<GameManager>();
                else
                    instance = obj;
            }
            return instance;
        }
    }

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
    private Image[] skillTwinkeImage;

    [SerializeField]
    private Image[] skill;

    [SerializeField]
    private PlayerActionMgr playerActionMgr;


    private BuffManager buffManager;

    public StatusManager pcStat;


    private Vector2 backgroundSize;
    public Vector2 BackgroundSize { get { return backgroundSize; } }

    private Vector2 mapSizeMin = new Vector2(2.0f, 1.0f);
    public Vector2 MapSizeMin { get { return mapSizeMin; } }

    private Vector2 mapSizeMax = new Vector2(3840.4f, 1620.2f);
    public Vector2 MapSizeMax { get { return mapSizeMax; } }

    private void Awake()
	{
        var objs = FindObjectsOfType<GameManager>();
        if(objs.Length == 1)
            DontDestroyOnLoad(this);
        else
            Destroy(this);
    }

	void Start()
    {
        pcStat = new StatusManager(100, 100, AttackTable[100].max);
        buffManager = GetComponent<BuffManager>();

        Vector2 spriteSize = background.GetComponent<SpriteRenderer>().sprite.rect.size;
        Vector2 localSpriteSize = spriteSize / background.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
        backgroundSize = localSpriteSize * background.transform.lossyScale;
    }

    public Vector2 GetSpriteSize(GameObject _target)
    {
        Vector2 worldSize = Vector2.zero;
        if(_target.GetComponent<SpriteRenderer>())
        {
            Vector2 spriteSize = _target.GetComponent<SpriteRenderer>().sprite.rect.size;
            Vector2 localSpriteSize = spriteSize / _target.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
            worldSize = localSpriteSize * _target.transform.lossyScale;
        }

        return worldSize;
    }

    void Update()
    {
        CheckPlayerDashCount();
        CheckPlayerUseSkill();
        CheckPlayerDebuff();
    }

    void CheckPlayerDashCount() 
    {
        switch (playerController.DashCount) 
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

        if(playerActionMgr.CanSkill3)
            skill[2].gameObject.SetActive(true);
		else
            skill[2].gameObject.SetActive(false);
	}

	void CheckPlayerDebuff() 
    {
        if (playerController.GetComponent<PlayerEffectController>().DarkDebuff == true) 
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

        skillTwinkeImage[skillIndex].gameObject.SetActive(true);
        while(curTime < 0.25f)
        {
            float value = 1 - curTime * 4;
            skillTwinkeImage[skillIndex].color = new Color(value, value, value);

            curTime += Time.deltaTime;
            yield return null;
        }
        while(curTime > 0f)
        {
            float value = 1 - curTime * 4;
            skillTwinkeImage[skillIndex].color = new Color(value, value, value);

            curTime -= Time.deltaTime;
            yield return null;
        }
        skillTwinkeImage[skillIndex].gameObject.SetActive(false);
    }

    public IEnumerator SkillTimer(short code)
    {
        AttackTable[code].curTime = AttackTable[code].cTime;
        yield return null;

        while(AttackTable[code].curTime > 0f)
        {
            AttackTable[code].curTime -= Time.deltaTime;
            yield return null;
		}
		AttackTable[code].curTime = 0f;
		switch(code)
		{
        case 101:
        case 102:
            yield return SkillReady(code);
            break;
        }
	}
}

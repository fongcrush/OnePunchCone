using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static CSVUtil;

public class PlayerAttackManager : MonoBehaviour
{
	public static List<Dictionary<string, object>> CSVData;
	public static Dictionary<short, AttackInfo> attackTable = new Dictionary<short, AttackInfo>();

	private Transform skill1CollObject;
	private Transform skill2CollObject;
	private Transform skill3CollObject;
	private Transform chargeRange;

	private void Awake()
	{
		CSVData = ReadCSV("SkillTable");

		foreach(var value in CSVData)
		{
			AttackInfo skill = new AttackInfo();
			skill.code = shortParse(value["ATTACK_CODE"]);
			skill.name = stringParse(value["NAME"]);
			skill.type = byteParse(value["ATTACK_TYPE"]);
			skill.clear = byteParse(value["ATTACK_Clear"]);
			skill.delay = floatParse(value["ATTACK_DELAY"]) / 100f;
			skill.distance = new Vector2(intParse(value["ATTACK_DISTANCE_X"]), intParse(value["ATTACK_DISTANCE_Y"]));
			skill.hit = byteParse(value["ATTACK_HIT"]);
			skill.min = shortParse(value["ATTACK_MIN_DAMAGE"]);
			skill.max = shortParse(value["ATTACK_MAX_DAMAGE"]);
			skill.cTime = floatParse(value["ATTACK_C_TIME"]);
			skill.icon = stringParse(value["ATTACK_ICON"]);
			skill.effect = stringParse(value["ATTACK_ATTACK_EFFECT"]);
			skill.anim1 = stringParse(value["ATTACK_Ani_01"]);
			skill.anim2 = stringParse(value["ATTACK_Ani_02"]);
			skill.anim3 = stringParse(value["ATTACK_Ani_03"]);
			skill.curTime = 0f;
			skill.On = false;
			skill.Can = true;


			attackTable.Add(skill.code, skill);
		}
	}

	private void Update()
	{

	}

	void UpdateCSVData()
	{
		CSVData = null;
		attackTable = null;

		CSVData = ReadCSV("SkillTable");

		foreach(var value in CSVData)
		{
			AttackInfo skill = new AttackInfo();
			skill.code = shortParse(value["ATTACK_CODE"]);
			skill.name = stringParse(value["NAME"]);
			skill.type = byteParse(value["ATTACK_TYPE"]);
			skill.clear = byteParse(value["ATTACK_Clear"]);
			skill.delay = floatParse(value["ATTACK_DELAY"]) / 100f;
			skill.distance = new Vector2(intParse(value["ATTACKL_DISTANCE_X"]), intParse(value["SKILL_DISTANCE_Y"]));
			skill.hit = byteParse(value["ATTACK_HIT"]);
			skill.min = shortParse(value["ATTACK_MIN_DAMAGE"]);
			skill.max = shortParse(value["ATTACK_MAX_DAMAGE"]);
			skill.cTime = floatParse(value["ATTACK_C_TIME"]);
			skill.icon = stringParse(value["ATTACK_ICON"]);
			skill.effect = stringParse(value["ATTACK_ATTACK_EFFECT"]);
			skill.anim1 = stringParse(value["ATTACK_Ani_01"]);
			skill.anim2 = stringParse(value["ATTACK_Ani_02"]);
			skill.anim3 = stringParse(value["ATTACK_Ani_03"]);
			skill.curTime = 0f;
			skill.On = false;
			skill.Can = true;

			attackTable.Add(skill.code, skill);
		}
	}

	public static IEnumerator SkillTimer(short code)
	{
		while(attackTable[code].curTime < attackTable[code].curTime)
		{
			attackTable[code].curTime += Time.deltaTime;
			yield return null;
		}
		attackTable[code].curTime = 0f;
	}
}

public class AttackInfo
{
	public short code;
	public string name;
	public byte type;
	public short clear;
	public float delay;
	public Vector2 distance;
	public byte hit;
	public short min;
	public short max;
	public float cTime;
	public string icon;
	public string effect;
	public string anim1, anim2, anim3;
	public float curTime;
	public bool On;		// 스위치 트리거
	public bool Can;	// curTime == 0 --> true
}
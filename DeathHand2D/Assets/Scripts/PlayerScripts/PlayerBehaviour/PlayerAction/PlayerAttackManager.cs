using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static CSVUtil;

public class PlayerAttackManager : MonoBehaviour
{
	public static List<Dictionary<string, object>> CSVData;
	public static Dictionary<short, SkillInfo> skillTable = new Dictionary<short, SkillInfo>();

	private Transform skill1CollObject;
	private Transform skill2CollObject;
	private Transform skill3CollObject;
	private Transform chargeRange;

	private void Awake()
	{
		CSVData = ReadCSV("SkillTable");

		foreach(var value in CSVData)
		{
			SkillInfo skill = new SkillInfo();
			skill.code = shortParse(value["SKILL_CODE"]);
			skill.name = stringParse(value["NAME"]);
			skill.type = byteParse(value["SKILL_TYPE"]);
			skill.clear = byteParse(value["SKILL_Clear"]);
			skill.delay = floatParse(value["SKILL_DELAY"]) / 100f;
			skill.distance = new Vector2(intParse(value["SKILL_DISTANCE_X"]), intParse(value["SKILL_DISTANCE_Y"]));
			skill.hit = byteParse(value["SKILL_HIT"]);
			skill.min = shortParse(value["SKILL_MIN_DAMAGE"]);
			skill.max = shortParse(value["SKILL_MAX_DAMAGE"]);
			skill.cTime = floatParse(value["SKILL_C_TIME"]);
			skill.icon = stringParse(value["SKILL_ICON"]);
			skill.effect = stringParse(value["SKILL_ATTACK_EFFECT"]);
			skill.anim1 = stringParse(value["SKILL_Ani_01"]);
			skill.anim2 = stringParse(value["SKILL_Ani_02"]);
			skill.anim3 = stringParse(value["SKILL_Ani_03"]);
			skill.curTime = 0f;
			skill.On = false;
			skill.Can = true;


			skillTable.Add(skill.code, skill);
		}
	}

	private void Update()
	{

	}

	void UpdateCSVData()
	{
		CSVData = null;
		skillTable = null;

		CSVData = ReadCSV("SkillTable");

		foreach(var value in CSVData)
		{
			SkillInfo skill = new SkillInfo();
			skill.code = shortParse(value["SKILL_CODE"]);
			skill.name = stringParse(value["NAME"]);
			skill.type = byteParse(value["SKILL_TYPE"]);
			skill.clear = byteParse(value["SKILL_Clear"]);
			skill.delay = floatParse(value["SKILL_DELAY"]) / 100f;
			skill.distance = new Vector2(intParse(value["SKILL_DISTANCE_X"]), intParse(value["SKILL_DISTANCE_Y"]));
			skill.hit = byteParse(value["SKILL_HIT"]);
			skill.min = shortParse(value["SKILL_MIN_DAMAGE"]);
			skill.max = shortParse(value["SKILL_MAX_DAMAGE"]);
			skill.cTime = floatParse(value["SKILL_C_TIME"]);
			skill.icon = stringParse(value["SKILL_ICON"]);
			skill.effect = stringParse(value["SKILL_ATTACK_EFFECT"]);
			skill.anim1 = stringParse(value["SKILL_Ani_01"]);
			skill.anim2 = stringParse(value["SKILL_Ani_02"]);
			skill.anim3 = stringParse(value["SKILL_Ani_03"]);
			skill.curTime = 0f;
			skill.On = false;
			skill.Can = true;

			skillTable.Add(skill.code, skill);
		}
	}

	public static IEnumerator SkillTimer(short code)
	{
		while(skillTable[code].curTime < skillTable[code].curTime)
		{
			skillTable[code].curTime += Time.deltaTime;
			yield return null;
		}
		skillTable[code].curTime = 0f;
	}
}

public class SkillInfo
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
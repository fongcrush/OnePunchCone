using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static CSVUtil;

public class PlayerAttackData
{
	private static List<Dictionary<string, object>> CSVData;

	private static Dictionary<short, AttackInfo> attackTable;
	public static Dictionary<short, AttackInfo> AttackTable { get { return attackTable; } }

	public static void UpdateCSVData()
	{
		CSVData = null;
		attackTable = null;
		attackTable = new Dictionary<short, AttackInfo>();

		CSVData = ReadCSV("SkillTable");

		foreach(var value in CSVData)
		{
			AttackInfo skill = new AttackInfo();
			skill.code = shortParse(value["CODE"]);
			skill.type = byteParse(value["TYPE"]);
			skill.clear = byteParse(value["CLEAR"]);
			skill.fDelay = floatParse(value["FIRST_DELAY"]) / 100f;
			skill.sDelay = floatParse(value["FIRST_DELAY"]) / 100f;
			skill.distance = new Vector2(intParse(value["DISTANCE_X"]), intParse(value["DISTANCE_Y"]));
			skill.hit = byteParse(value["HIT"]);
			skill.min = shortParse(value["MIN_DAMAGE"]);
			skill.max = shortParse(value["MAX_DAMAGE"]);
			skill.cTime = floatParse(value["C_TIME"]);
			skill.icon = stringParse(value["ICON"]);
			skill.effect = stringParse(value["EFFECT"]);
			skill.anim1 = stringParse(value["ANI_01"]);
			skill.anim2 = stringParse(value["ANI_02"]);
			skill.anim3 = stringParse(value["ANI_03"]);
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
	public byte type;
	public short clear;
	public float fDelay;
	public float sDelay;
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
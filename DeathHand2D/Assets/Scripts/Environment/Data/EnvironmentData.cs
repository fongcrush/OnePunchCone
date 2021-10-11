using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CSVUtil;

public class EnvironmentData
{
	private static List<Dictionary<string, object>> CSVData;

	private static Dictionary<string, EnvironmentInfo> environmentTable;
	public static Dictionary<string, EnvironmentInfo> EnvironmentTable { get { return environmentTable; } set { environmentTable = value; } }

	public static void UpdateCSVData()
	{
		CSVData = null;
		environmentTable = null;
		environmentTable = new Dictionary<string, EnvironmentInfo>();

		CSVData = ReadCSV("EnvironmentTable");

		foreach (var value in CSVData)
		{
			EnvironmentInfo environment = new EnvironmentInfo();
			environment.name = stringParse(value["NAME"]);
			environment.type = byteParse(value["TYPE"]);
			environment.durability = intParse(value["DURABILITY"]);
			environment.scale_x = floatParse(value["SCALE_X"]);
			environment.scale_y = floatParse(value["SCALE_Y"]);
			environment.time = shortParse(value["TIME"]);
			environment.respawn = shortParse(value["RESPAWN"]);

			environmentTable.Add(environment.name, environment);
		}
	}
}

public class EnvironmentInfo
{
	public string name;
	public byte type;
	public int durability;
	public float scale_x;
	public float scale_y;
	public short time;
	public short respawn;
}
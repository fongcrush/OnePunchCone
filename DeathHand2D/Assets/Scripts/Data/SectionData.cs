using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CSVUtil;

public class SectionData : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemyGroup;
    public GameObject[] EnemyGroup { get { return enemyGroup; } }

    private List<Dictionary<string, object>> CSVData;

    private Dictionary<int, float[]> probabilityTable;
    public Dictionary<int, float[]> ProbabilityTable { get { return probabilityTable; } }

	public void Awake()
	{
        ReadStageData();
    }

	public void Start()
    {
    }

    public void ReadStageData()
	{
        CSVData = null;
        CSVData = ReadCSV("SectionTable");
        probabilityTable = new Dictionary<int, float[]>();
		foreach(var value in CSVData)
		{
            float[] probability = new float[6];
            probability[0] = floatParse(value["100"]);
            probability[1] = floatParse(value["101"]);
            probability[2] = floatParse(value["102"]);
            probability[3] = floatParse(value["103"]);
            probability[4] = floatParse(value["104"]);
            probability[5] = floatParse(value["105"]);
            probabilityTable.Add(intParse(value["CODE"]), probability);
        }
	}
    public GameObject InitEnemyGroup(int code, Transform parent)
    {
        //Debug.Log(enemyGroup[code]);        
        return Instantiate(enemyGroup[code], parent);
    }
}
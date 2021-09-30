using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameMgr;

public class EnemySection : MonoBehaviour
{
	[SerializeField]
	private GameObject[] wave;

	[SerializeField]
	private int waveCount = 0;

	private void Awake()
	{
		++waveCount;
	}

	private void Update()
	{
		if(GameObject.FindGameObjectWithTag("Enemy") == null)
		{
			if(waveCount <= wave.Length)
			{
				Instantiate(wave[waveCount-1], transform);
				++waveCount;
			}
			else if(waveCount > wave.Length)
			{
				GM.CurRoomMgr.clear = true;
				Destroy(gameObject);
			}
		}

	}
}
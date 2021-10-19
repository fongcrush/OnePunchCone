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

	private GameObject curWave;

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
				curWave = Instantiate(wave[waveCount-1], transform);
				StartCoroutine(Show());
				++waveCount;
			}
			else if(waveCount > wave.Length)
			{
				GM.CurRoomMgr.Clear = true;
			}
		}
		if(GM.CurRoomMgr.Clear)
		{
			Destroy(gameObject);
		}
	}

	private IEnumerator Show()
	{
		yield return null;
		//enemys.GetComponentsInChildren<>();
		float curTIme = 0;
		float duration = 2f;
		float invDuration = 1 / duration;

		Transform[] allChildren = curWave.GetComponentsInChildren<Transform>();
		List<Renderer> rendererList = new List<Renderer>();
		foreach(var child in allChildren)
		{
			Transform spine = child.Find("SpineAnimation");
			if(spine == null) continue;

			if(spine.gameObject.activeSelf)
				rendererList.Add(spine.GetComponent<Renderer>());
		}

		Color[] color = new Color[rendererList.Count];
		for(int i = 0; i < rendererList.Count; i++)
			color[i] = rendererList[i].materials[0].GetColor("_Color");

		while(curTIme < duration)
		{
			curTIme += Time.deltaTime;

			for(int i = 0; i < rendererList.Count; i++)
			{
				color[i].a = curTIme * invDuration;
				rendererList[i].materials[0].SetColor("_Color", color[i]);
			}
			yield return null;
		}
	}
}
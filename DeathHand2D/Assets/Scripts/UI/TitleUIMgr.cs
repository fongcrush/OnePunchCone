using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TitleUIMgr : MonoBehaviour
{
    private bool isStart = false;
    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
	{
        isStart = true;
        SceneManager.LoadScene("Stage 1");
	}
}

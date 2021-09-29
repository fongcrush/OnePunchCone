using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private bool isStart = false;
    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
	{
        isStart = true;
        SceneManager.LoadScene("MainScene");
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffMgr : MonoBehaviour
{
    public GameObject DarkDebuff;
    private int darkDebuffCount;
    public int DarkDebuffCount { get { return darkDebuffCount; } set { darkDebuffCount = value; } }

    public GameObject SlowDebuff;
    private int slowDebuffCount;
    public int SlowDebuffCount { get { return slowDebuffCount; } set { slowDebuffCount = value; } }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public GameObject DarkDebuff;
    private int darkDebuffCount;
    public int DarkDebuffCount { get { return darkDebuffCount; } set { darkDebuffCount = value; } }
}

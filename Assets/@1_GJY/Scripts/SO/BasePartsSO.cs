using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePartsSO : ScriptableObject
{
    [Header("# Info")]
    public int dev_ID;
    public string dev_Name;

    public string display_Name;
    public string display_Description;

    [Header("# Common Stats")]
    public float armor;
    public float weight;
}

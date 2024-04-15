using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TutorialData : IEntity
{
    [SerializeField] private int dev_ID;
    [SerializeField] private string script1;
    [SerializeField] private string script2;
    [SerializeField] private string script3;
    [SerializeField] private string script4;
    [SerializeField] private string script5;
    [SerializeField] private string script6;
    [SerializeField] private string script7;
    [SerializeField] private string script8;
    [SerializeField] private string script9;
    [SerializeField] private string script10;
    [SerializeField] private string missionPath;

    private List<string> scripts;

    public int Dev_ID => dev_ID;
    public string MissionPath => missionPath;

    public List<string> Scripts
    {
        get
        {
            if (scripts == null)
            {
                scripts = new List<string>();
                AddScript(script1);
                AddScript(script2);
                AddScript(script3);
                AddScript(script4);
                AddScript(script5);
                AddScript(script6);
                AddScript(script7);
                AddScript(script8);
                AddScript(script9);
                AddScript(script10);
            }

            return scripts;
        }
    }

    private void AddScript(string script)
    {
        if (!string.IsNullOrEmpty(script))
            scripts.Add(script);
    }
}

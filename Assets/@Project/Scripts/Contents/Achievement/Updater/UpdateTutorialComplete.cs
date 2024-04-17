using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateTutorialComplete : AchievementUpdater
{
    private void Start()
    {
        Managers.Tutorial.OnTutorialClear += UpdateReport;
    }

    public void UpdateReport()
    {
        Report();
    }
}

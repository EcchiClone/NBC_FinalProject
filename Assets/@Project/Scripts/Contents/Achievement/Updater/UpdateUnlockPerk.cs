using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateUnlockPerk : AchievementUpdater
{
    public void UnlockPerk()
    {
        Report();
    }
    public void ResetPerk()
    {
        Report(0);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Test_PhaseLoop : MonoBehaviour
{
    EnemyPhaseStarter ep;
    int phaseMax;

    void Start()
    {
        ep = GetComponent<EnemyPhaseStarter>();
        phaseMax = ep.Phases.Count();
        StartCoroutine(Co_PhaseLooper());
    }

    private IEnumerator Co_PhaseLooper()
    {
        while(true)
        {
            for(int i=0; i<phaseMax; i++)
            {
                ep.StartPhase(i, 0, true);
                yield return Util.GetWaitSeconds(3f);
            }
        }
    }
}

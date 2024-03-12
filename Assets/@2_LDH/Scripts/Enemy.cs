using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public PhaseSO currentPhase;

    private void Start()
    {
        foreach (var patternHierarchy in currentPhase.hierarchicalPatterns)
        {
            DanmakuGenerator.instance.StartPatternHierarchy(patternHierarchy, currentPhase.cycleTime, gameObject);
        }
    }
}
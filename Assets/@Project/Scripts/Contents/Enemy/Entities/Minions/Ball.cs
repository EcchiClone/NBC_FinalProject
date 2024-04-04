using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : Entity
{
    protected override void Initialize()
    {
        Target = FindObjectOfType<TargetCenter>().transform;
        CurrentHelth = Data.maxHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

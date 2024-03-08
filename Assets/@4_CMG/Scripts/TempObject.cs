using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.testEvent, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

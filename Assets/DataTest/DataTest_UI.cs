using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTest_UI : MonoBehaviour
{
    public GameObject prefab;

    private void Start()
    {
        PartData data = Managers.Data.GetPartData(10001001);        
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBrain_Stupid : MonoBehaviour
{
    public Transform target;

    private float _shootingDistance = 10;

    public float moveSpeed = 5f;

    private NavMeshAgent _nma;

    private void Awake()
    {
        _nma = gameObject.GetOrAddComponent<NavMeshAgent>();
    }


    void Start()
    {
        
    }


    void Update()
    {
        _nma.SetDestination(target.position);


    }
}

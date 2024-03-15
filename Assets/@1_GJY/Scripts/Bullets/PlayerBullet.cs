using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed;    

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnEnable()
    {
        Destroy(gameObject, 5f);
    }
}

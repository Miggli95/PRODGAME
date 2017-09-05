using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraShootsScript : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            col.gameObject.GetComponent<ShootScript>().AddShoots(1);
            Destroy(transform.gameObject);
        }
    }
}

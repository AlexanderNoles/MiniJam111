using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillCircle : MonoBehaviour
{
    public float radius = 100.0f;

    private void Update()
    {
        if (PlayerManagment.levelLost) return;
        if ((PlayerMovement._instance.transform.position - transform.position).sqrMagnitude > radius * radius)
        {
            PlayerManagment.TakeDamage(1000000);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,radius);
    }
}

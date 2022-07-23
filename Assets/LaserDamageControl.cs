using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDamageControl : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            PlayerManagment.TakeDamage(10);
        }
        else if (collision.CompareTag("Enemy"))
        {
            collision.SendMessage("Attacked", SendMessageOptions.DontRequireReceiver);
        }
    }
}

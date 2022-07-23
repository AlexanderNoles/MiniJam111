using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDamageControl : MonoBehaviour
{
    public static List<LaserDamageControl> scriptList = new List<LaserDamageControl>();
    public bool canDamagePlayer = false;

    private void Awake()
    {
        scriptList = new List<LaserDamageControl>();
    }

    private void Start()
    {
        scriptList.Add(this);
    }

    private void Update()
    {
        if (PlayerManagment.levelLost) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canDamagePlayer = true;
        }
        else if (collision.CompareTag("Enemy"))
        {
            collision.SendMessage("Attacked", SendMessageOptions.DontRequireReceiver);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canDamagePlayer = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float timeTillDestroy = 1.0f;

    private void Update()
    {
        if(timeTillDestroy < 0)
        {
            Destroy(gameObject);
        }
        else
        {
            timeTillDestroy -= Time.deltaTime;
        }
    }
}

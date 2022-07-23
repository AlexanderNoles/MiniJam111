using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineAmmoEffectControl : MonoBehaviour
{
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1.0f, 0.25f), Time.deltaTime * 10.0f);
        if(transform.localScale.x > 0.9f)
        {
            Destroy(gameObject);
        }
    }
}

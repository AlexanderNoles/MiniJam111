using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GremlinControl : MonoBehaviour
{
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private float leapForce = 5f;
    private float timeBetweenLeaps = 0.5f;
    private float timeLeftTillNextLeap;
    public LayerMask ground = 8;
    private float damageRadius = 0.2f;

    [Header("Sprites")]
    public Sprite normalSprite;
    public Sprite InAirSprie;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        GunControl.onGunChange.AddListener(OnColorChange);
        OnColorChange();
        timeLeftTillNextLeap = timeBetweenLeaps;
    }

    private void Update()
    {
        if(transform.position.y < -20)
        {
            Attacked();
        }

        //Get Data
        //List<Vector3> otherGremlinPositions = (List<Vector3>)EnemyManagment.getGremlinPositions().Where(x => x != transform.position);
        Vector3 playerPostionThisFrame = EnemyManagment.playerPosition;

        if (grounded())
        {
            //Run damage check
            if((playerPostionThisFrame - transform.position).sqrMagnitude < damageRadius * damageRadius)
            {
                PlayerManagment.TakeDamage(1,(playerPostionThisFrame - transform.position).normalized);
            }

            sr.sprite = normalSprite;
            if(timeLeftTillNextLeap > 0)
            {
                timeLeftTillNextLeap -= Time.deltaTime;
                return;
            }

            //Peform Leap
            rb.AddForce((leapForce * (playerPostionThisFrame - transform.position).normalized) + (Vector3.up * 2.0f), ForceMode2D.Impulse);

            timeLeftTillNextLeap = timeBetweenLeaps;
        }
        else
        {
            sr.sprite = InAirSprie;
        }
    }

    private bool grounded()
    {
        return Physics2D.RaycastAll(transform.position, Vector2.down, 0.5f, ground).Length > 0;
    }

    private void OnColorChange()
    {
        sr.color = GunControl.GetAccentColor();
    }

    public void Attacked()
    {
        EnemyManagment.EnemyKilled();
        Destroy(gameObject);
    }
}

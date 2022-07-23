using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BatControl : MonoBehaviour
{
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Vector3 movementVector;
    private float flySpeed = 0.7f;
    private float killCheckRadius = 0.2f;
    private float upwardsAmount = 0.0f;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        GunControl.onGunChange.AddListener(OnColorChange);
        OnColorChange();
    }

    private void Update()
    {
        movementVector = EnemyManagment.playerPosition - transform.position;

        if(movementVector.sqrMagnitude < killCheckRadius * killCheckRadius)
        {
            PlayerManagment.TakeDamage(1, movementVector);
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce((movementVector.normalized * flySpeed) + (Vector3.up * upwardsAmount));
    }

    private void OnColorChange()
    {
        sr.color = GunControl.GetAccentColor();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 8)
        {
            upwardsAmount = 0.5f;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            upwardsAmount = 0.0f;
        }
    }

    private void Attacked()
    {
        EnemyManagment.EnemyKilled();
        Destroy(gameObject);
    }
}

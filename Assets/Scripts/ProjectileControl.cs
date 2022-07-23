using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileControl : MonoBehaviour
{
    private BorderTimer bordersToIgnore = new BorderTimer();

    public Vector3 velocity;
    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private float timeTillDestroy;


    [Header("Extras")]
    [SerializeField]
    private bool canBecomeHostile = false;
    private bool canDamagePlayer = false;
    [SerializeField]
    private float timeTillHostile;

    private void Update()
    {
        transform.position += velocity.normalized * speed * Time.deltaTime;

        if (canBecomeHostile && !canDamagePlayer)
        {
            if(timeTillHostile <= 0)
            {
                canDamagePlayer = true;
            }
            else
            {
                timeTillHostile -= Time.deltaTime;
            }
        }

        if(timeTillDestroy > 0)
        {
            timeTillDestroy -= Time.deltaTime;
        }
        else
        {
            DestroyProjectile();
        }

        bordersToIgnore.Run();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 10)
        {
            if (!bordersToIgnore.Contains(collision.gameObject))
            {
                BorderControl currentBorder = collision.GetComponent<BorderControl>();
                Vector3 displacement = currentBorder.GetDisplacement();
                transform.position += displacement;
                if(displacement.x > 0)
                {
                    velocity = new Vector3(velocity.x * displacement.normalized.x, velocity.y);
                }    
                bordersToIgnore.Add(currentBorder.partnerBorder.gameObject);
            }
        }

        if (collision.CompareTag("Player") && canDamagePlayer)
        {
            PlayerManagment.TakeDamage(1, velocity);
        }
        else if(collision.CompareTag("Enemy"))
        {
            collision.SendMessage("Attacked", SendMessageOptions.DontRequireReceiver);
            DestroyProjectile();
        }
        else if (collision.gameObject.layer == 8)
        {
            if (transform.position.y > -4.795f)
            {
                Instantiate(Resources.Load("BulletDeathFloor"),transform.position,Quaternion.identity);
            }
            DestroyProjectile(false);
        }
    }

    private void DestroyProjectile(bool playAnimation = true)
    {
        if (playAnimation)
        {
            Instantiate(Resources.Load("BulletDeath"), transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}

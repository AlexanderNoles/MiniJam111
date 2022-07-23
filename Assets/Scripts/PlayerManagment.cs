using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagment : MonoBehaviour
{
    public SpriteRenderer playerSprite;
    private static PlayerMovement pmInstance;
    private Color baseColor;

    private static float maxHealth = 4;
    private static float currentHealth;

    public static float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }

    private static float timeBetweenDamage = 0.3f;
    public static float timeLeftTillDamage;
    public static bool invincibilityFromDash = false;

    public static bool levelLost = false;

    private void Start()
    {
        levelLost = false;
        baseColor = playerSprite.color;
        currentHealth = maxHealth;
        pmInstance = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TakeDamage(1, Vector2.left);
        }
#endif

        if(currentHealth <= 0)
        {
            //Die
            Instantiate(Resources.Load("PlayerDeath"),Vector3.zero,Quaternion.identity);
            levelLost = true;
            Destroy(gameObject);
        }

        if (timeLeftTillDamage > 0)
        {
            timeLeftTillDamage -= Time.deltaTime;
            if (invincibilityFromDash)
            {
                playerSprite.color = GunControl.GetCurrentColor() * (Mathf.Sin(Time.time * 100.0f) + 1.0f) / 2.0f;
            }
            else
            {
                playerSprite.color = GunControl.GetAccentColor() * (Mathf.Sin(Time.time * 100.0f) + 1.0f) / 2.0f;
            }
            
        }
        else
        {
            playerSprite.color = baseColor;
            invincibilityFromDash = false;
        }
    }

    public static void TakeDamage(float damageAmount = 1, Vector2 knockBackDirection = (default))
    {
        if(timeLeftTillDamage > 0)
        {
            return;
        }

        currentHealth -= damageAmount;
        timeLeftTillDamage = timeBetweenDamage;
        pmInstance.GetRB().AddForce(knockBackDirection.normalized * 50.0f);

        HealthUIManagment.OnChangeHealth();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagment : MonoBehaviour
{
    public SpriteRenderer playerSprite;
    private static PlayerMovement pmInstance;
    private Color baseColor;

    public int maxHealth = 3;
    private static int currentHealth;

    private static float timeBetweenDamage = 0.3f;
    private static float timeLeftTillDamage;

    private void Start()
    {
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
            //Temp Die
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        if (timeLeftTillDamage > 0)
        {
            timeLeftTillDamage -= Time.deltaTime;
            playerSprite.color = new Color(1.0f,1.0f,1.0f, (Mathf.Sin(Time.time * 100.0f) + 1.0f)/2.0f);
        }
        else
        {
            playerSprite.color = baseColor;
        }
    }

    public static void TakeDamage(int damageAmount = 1, Vector2 knockBackDirection = (default))
    {
        if(timeLeftTillDamage > 0)
        {
            return;
        }

        currentHealth -= damageAmount;
        timeLeftTillDamage = timeBetweenDamage;
        pmInstance.GetRB().AddForce(knockBackDirection.normalized * 50.0f);
    }
}

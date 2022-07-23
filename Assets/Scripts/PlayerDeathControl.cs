using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerDeathControl : MonoBehaviour
{
    private float slowDownSpeed = 2.5f;
    private Image fadeOut;
    public RectTransform skullImage;
    public TextMeshProUGUI text;
    private float timeToSkull = 1.2f;
    private float timeToRestart = 1.5f;

    private void Start()
    {
        fadeOut = GameObject.Find("FadeOut").GetComponent<Image>();
    }

    private void Update()
    {
        Time.timeScale = Mathf.Lerp(Time.timeScale, 0.001f, Time.deltaTime * slowDownSpeed);
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        if(timeToSkull > 0)
        {
            timeToSkull -= Time.unscaledDeltaTime;
        }
        else
        {
            if (!skullImage.gameObject.activeSelf)
            {
                skullImage.gameObject.SetActive(true);
                text.text = $"{EnemyManagment.GetNumberOfEnemyKilled()} kills";
                GameObject.Find("Music").SetActive(false);
            }
            
            fadeOut.color = Color.black;
            if(timeToRestart > 0)
            {
                timeToRestart -= Time.unscaledDeltaTime;
            }
            else
            {
                Time.timeScale = 1.0f;
                Time.fixedDeltaTime = 0.02f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}

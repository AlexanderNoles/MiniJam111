using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIManagment : MonoBehaviour
{
    private static Image _image;

    private void Start()
    {
        _image = GetComponent<Image>();
    }

    public static void OnChangeHealth()
    {
        _image.fillAmount = PlayerManagment.GetHealthPercentage();
    }
}

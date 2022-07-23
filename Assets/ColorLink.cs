using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorLink : MonoBehaviour
{
    public float alpha = 1.0f;
    private Image _image;
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if(sr == null)
        {
            _image = GetComponent<Image>();
        }
        GunControl.onGunChange.AddListener(OnGunChange);
    }

    private void OnGunChange()
    {
        Color gunColor = GunControl.GetCurrentColor();
        Color finalColor = new Color(gunColor.r,gunColor.g,gunColor.b,alpha);
        if(sr == null)
        {
            _image.color = finalColor;
            return;
        }
        sr.color = finalColor;
    }
}

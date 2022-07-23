using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInControl : MonoBehaviour
{
    private Image _image;
    private float timeToReveal = 0.7f;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _image.color = Color.black;
    }

    private void Update()
    {
        if(timeToReveal > 0)
        {
            timeToReveal -= Time.deltaTime;
        }
        else
        {
            _image.color = Color.clear;
            Destroy(this);
        }
    }
}

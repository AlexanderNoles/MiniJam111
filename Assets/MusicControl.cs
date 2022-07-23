using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControl : MonoBehaviour
{
    private AudioSource _as;

    private void Start()
    {
        _as = GetComponent<AudioSource>();
    }

    private void Update()
    {
        _as.pitch = Time.timeScale;
        _as.volume = Time.timeScale;
    }
}

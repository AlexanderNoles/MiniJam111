using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessColorLink : MonoBehaviour
{
    private Vignette vignette;

    void Start()
    {
        GetComponent<PostProcessVolume>().profile.TryGetSettings(out vignette);
        GunControl.onGunChange.AddListener(OnColorChange);
    }

    private void OnColorChange()
    {
        vignette.color.value = GunControl.GetAccentColor();
    }

}

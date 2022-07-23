using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUIManagment : MonoBehaviour
{
    public static AmmoUIManagment _instance;

    private static Image frame;
    private static Image backing;

    private const float machineGunEffectYLevel = 456f;
    private const float effectOffsetPer = 37.2f;

    [System.Serializable]
    public struct AmmoUI
    {
        public Sprite frameSprite;
        public Vector3 anchoredBackingPos;
        public Vector2 backingSizeDelta;
        public Color backingColor;
    }

    public List<AmmoUI> ammoUis = new List<AmmoUI>();

    private void Start()
    {
        _instance = this;
        frame = transform.GetChild(1).GetComponent<Image>();
        backing = transform.GetChild(0).GetComponent<Image>();
    }

    public static void OnGunUpdate()
    {
        AmmoUI currentAmmoUI = _instance.ammoUis[(int)GunControl.getCurrentGun()];
        //Setup backing
        (backing.transform as RectTransform).anchoredPosition = currentAmmoUI.anchoredBackingPos;
        (backing.transform as RectTransform).sizeDelta = currentAmmoUI.backingSizeDelta;
        backing.color = currentAmmoUI.backingColor;
        //Reset backing
        backing.fillAmount = 1.0f;

        //Change frame
        frame.sprite = currentAmmoUI.frameSprite;
        frame.SetNativeSize();
    }

    public static void OnFire()
    {
        backing.fillAmount = GunControl.getCurrentAmmoPercentage();
        if(GunControl.getCurrentGun() == GunControl.Guns.Backwards)
        {
            (((GameObject)Instantiate(Resources.Load("machineAmmoEffect"), _instance.transform)).transform as RectTransform).anchoredPosition =
                new Vector3
                (-375f,
                machineGunEffectYLevel - (effectOffsetPer * GunControl.getCurrentAmmoDifference()));
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

public class GunControl : MonoBehaviour
{
    private static GunControl _instance;
    public static UnityEvent onGunChange = new UnityEvent();

    public Transform actualGun;
    private Transform fireEmpty;
    private SpriteRenderer gunSR;

    public enum Guns
    {
        Laser,
        BlowBack,
        Backwards
    }
    [Header("Gun Info")]
    public Guns currentGun;
    public static Guns getCurrentGun()
    {
        return _instance.currentGun;
    }
    [Serializable]
    public struct StandardGunInfo
    {
        //Basic Info
        public Sprite gunSprite;
        public Sprite reticleSprite;
        public float ammo;
        public float ammoPerShot;
        public float timeBetweenShots;
        public Vector3 fireEmptyPos;
        public Color gunColor;
        public Color accentColor;
    }
    public List<StandardGunInfo> guns = new List<StandardGunInfo>();
    private static StandardGunInfo currentGunInfo = new StandardGunInfo();
    public static Color GetCurrentColor()
    {
        return currentGunInfo.gunColor;
    }
    public static Color GetAccentColor()
    {
        return currentGunInfo.accentColor;
    }
    private float currentAmmo;
    public static float getCurrentAmmoPercentage()
    {
        return _instance.currentAmmo / currentGunInfo.ammo;
    }
    public static float getCurrentAmmoDifference()
    {
        return currentGunInfo.ammo - _instance.currentAmmo;
    }
    private float currentTimeBetweenShots;

    private static float timeBetweenGunSwitch = 0.6f;
    private static float timeLeftTillGunSwitch;

    [Header("Laser")]
    [Header("Specific Gun Info")]
    public GameObject actualLaser;
    public GameObject laserEffects;
    private SpriteRenderer laserEffectsSR;
    public float chargeUpTime = 0.5f;
    private static float chargeUpTimeLeft;

    public int maxNumberOfBounces = 5;
    public LayerMask thingsThatCanBeHit;
    public Transform laserHolder;

    private float timeTillCanDamagePlayer = 0.05f;
    private float timeLeftTillDamagePlayer = 0.05f;


    [Header("BlowBack")]
    public float knockbackForce = 500.0f;

    private void Start()
    {
        _instance = this;
        gunSR = actualGun.GetComponent<SpriteRenderer>();
        fireEmpty = actualGun.GetChild(0);
        laserEffectsSR = laserEffects.GetComponent<SpriteRenderer>();
        for(int i = 0; i < maxNumberOfBounces; i++)
        {
            ((GameObject)Instantiate(Resources.Load("LaserSegment"),laserHolder)).SetActive(false);
        }
    }

    private Vector3 GetToMouseVector()
    {
        return MouseManagment.GetMousePositionInWorld() - actualGun.position;
    }

    private void OnDestroy()
    {
        onGunChange.RemoveAllListeners();
    }

    public static void ChangeToRandomGun()
    {
        Array enumArray = Enum.GetValues(typeof(Guns));
        Guns tempGuns = _instance.currentGun;
        System.Random random = new System.Random();
        while(tempGuns == _instance.currentGun)
        {
            tempGuns = (Guns)enumArray.GetValue(random.Next(enumArray.Length));
        }
        ChangeGun(tempGuns);
    }

    public static void ChangeGun(Guns newGun)
    {
        timeLeftTillGunSwitch = timeBetweenGunSwitch;

        _instance.currentGun = newGun;

        currentGunInfo = _instance.guns[(int)newGun];

        //Apply new gun info
        _instance.currentAmmo = currentGunInfo.ammo;
        _instance.currentTimeBetweenShots = currentGunInfo.timeBetweenShots;

        _instance.gunSR.sprite = currentGunInfo.gunSprite;
        _instance.fireEmpty.localPosition = currentGunInfo.fireEmptyPos;

        MouseManagment.SetReticle(currentGunInfo.reticleSprite);

        //Reset
        chargeUpTimeLeft = _instance.chargeUpTime;
        _instance.actualLaser.SetActive(false);
        _instance.laserEffects.SetActive(false);
        _instance.HideAllLasers();


        onGunChange.Invoke();

        AmmoUIManagment.OnGunUpdate();
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeToRandomGun();
        }
#endif
        if(currentGun == Guns.Laser)
        {
            actualGun.right = (fireEmpty.position + GetToMouseVector().normalized) - fireEmpty.position;

            LaserDamageCheck();
        }
        else
        {
            actualGun.right = GetToMouseVector();
        }
        if (actualGun.rotation.eulerAngles.z > 90 && actualGun.rotation.eulerAngles.z < 270)
        {
            gunSR.flipY = true;
            _instance.fireEmpty.localPosition = new Vector3(currentGunInfo.fireEmptyPos.x, currentGunInfo.fireEmptyPos.y * -1);
        }
        else if (gunSR.flipY)
        {
            gunSR.flipY = false;
            _instance.fireEmpty.localPosition = currentGunInfo.fireEmptyPos;
        }

        if (currentAmmo <= 0)
        {
            if(timeLeftTillGunSwitch > 0)
            {
                timeLeftTillGunSwitch -= Time.deltaTime;
                laserEffects.SetActive(false);
                actualLaser.SetActive(false);
                HideAllLasers();
            }
            else
            {
                ChangeToRandomGun();
            }
            return;
        }

        if (InputManager.FireKeyPressed() && currentTimeBetweenShots <= 0)
        {
            Fire();
            currentTimeBetweenShots = currentGunInfo.timeBetweenShots;
        }
        else
        {
            currentTimeBetweenShots -= Time.deltaTime;
            laserEffects.SetActive(false);
            actualLaser.SetActive(false);
            chargeUpTimeLeft = chargeUpTime;
            HideAllLasers();
        }   
    }

    private void HideAllLasers()
    {
        foreach (Transform child in laserHolder)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void LaserDamageCheck()
    {
        foreach (LaserDamageControl ldc in LaserDamageControl.scriptList)
        {
            if (ldc.canDamagePlayer)
            {
                if (timeLeftTillDamagePlayer > 0)
                {
                    timeLeftTillDamagePlayer -= Time.deltaTime;
                }
                else
                {
                    PlayerManagment.TakeDamage(1.0f, Vector3.zero);
                    timeLeftTillDamagePlayer = timeTillCanDamagePlayer;
                }
                return;
            }
        }

        timeLeftTillDamagePlayer = timeTillCanDamagePlayer;
    }

    private void Fire()
    {
        if(currentGun == Guns.Laser)
        {
            if (chargeUpTimeLeft > 0)
            {
                chargeUpTimeLeft -= Time.deltaTime;
                laserEffects.SetActive(true);
                laserEffectsSR.color = new Color(1,0,0,(Mathf.Sin(Time.time * 500)+1)/2);
            }
            else
            {
                actualLaser.SetActive(true);
                laserEffects.SetActive(false);
                currentAmmo -= Time.deltaTime * currentGunInfo.ammoPerShot;
                CameraManager.Shake(0.1f);

                HideAllLasers();

                Vector3 startPos = fireEmpty.position;
                Vector3 direction = actualGun.right;
                for(int i = 0; i < maxNumberOfBounces; i++)
                {
                    //Actually setup new segment
                    Transform laserSegment = laserHolder.GetChild(i);
                    laserSegment.gameObject.SetActive(true);
                    laserSegment.position = startPos;
                    laserSegment.right = (startPos + direction) - startPos;

                    if(i == 0) 
                    {
                        laserSegment.position += laserSegment.right.normalized * 501.5f;
                    }

                    Ray ray = new Ray(startPos,direction);
                    RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction, Mathf.Infinity, thingsThatCanBeHit);
                    //Debug.DrawRay(ray.origin,ray.direction * 10000,Color.yellow,0.3f);
                    if (hits.Length == 0 || hits[0].collider.gameObject.layer == 8) //Is hitting ground first
                    {
                        break;
                    }
                    else
                    {
                        //Setup for setup
                        RaycastHit2D hit = hits[0];
                        //Debug.DrawRay(hit.point, hit.normal, Color.yellow, 0.3f);   
                        if (hit.normal.x != 0)
                        {
                            direction = new Vector3(-direction.x, direction.y);
                        }
                        else
                        {
                            direction = new Vector3(direction.x, -direction.y);
                        }
                        startPos = (Vector3)hit.point + direction.normalized;
                    }
                }
            }   
        }
        else if(currentGun == Guns.BlowBack)
        {
            currentAmmo -= currentGunInfo.ammoPerShot;
            Instantiate(Resources.Load("ShotgunParticles"), fireEmpty.position, Quaternion.identity);
            ((GameObject)Instantiate(Resources.Load("ShotgunCone"), transform.position, Quaternion.identity)).transform.right = GetToMouseVector();
            PlayerMovement._instance.GetRB().AddForce(fireEmpty.right.normalized * -knockbackForce);
            CameraManager.Shake(0.3f);
        }
        else if(currentGun == Guns.Backwards)
        {
            GameObject newBullet = (GameObject)Instantiate(Resources.Load("Bullet"),fireEmpty.position, Quaternion.identity);
            newBullet.transform.right = GetToMouseVector();
            newBullet.GetComponent<ProjectileControl>().velocity = GetToMouseVector();
            ((GameObject)Instantiate(Resources.Load("MuzzleFlash"), fireEmpty.position, Quaternion.identity)).transform.parent = fireEmpty;
            ((GameObject)Instantiate(Resources.Load("BulletCasing"), transform.position, Quaternion.identity)).GetComponent<Rigidbody2D>().velocity = new Vector2(fireEmpty.right.x,2.0f);
            currentAmmo -= currentGunInfo.ammoPerShot;
        }

        AmmoUIManagment.OnFire();
    }
}

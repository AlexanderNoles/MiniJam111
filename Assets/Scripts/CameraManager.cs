using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager _instance;
    public static Camera cam;

    private Vector3 startPos = new Vector3(0, 0, -10);

    //Camera Shake
    private float currentShakeIntensity = 0f;
    private const float maxShakeInensity = 0.1f;
    private const float shakeDrag = 0.5f;
    private bool shaking;

    private void Awake()
    {
        _instance = this;
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        if (!shaking)
        {
            currentShakeIntensity = Mathf.Clamp(currentShakeIntensity - (Time.deltaTime * shakeDrag), 0, Mathf.Infinity);
        }
        transform.position = startPos + (Vector3.up * (Mathf.Cos(Time.time * 50f) * currentShakeIntensity));
    }

    public static void Shake(float time)
    {
        _instance.CameraShakeIntermediary(time);
    }

    private void CameraShakeIntermediary(float time)
    {
        StartCoroutine("cameraShake", time);
    }

    IEnumerator cameraShake(float time)
    {
        shaking = true;
        currentShakeIntensity = maxShakeInensity;
        yield return new WaitForSeconds(time);
        shaking = false;
    }

}

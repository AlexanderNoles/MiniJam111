using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseManagment : MonoBehaviour
{
    public static MouseManagment _instance;
    private static Camera _camera;
    public Image _image;

    private void Start()
    {
        _instance = this;
        _camera = CameraManager.cam;
    }

    void OnApplicationFocus(bool hasFocus)
    {
        Cursor.visible = !hasFocus;
    }

    private void Update()
    {
        _image.transform.position = (Input.mousePosition);
    }

    public static void SetReticle(Sprite reticle)
    {
        _instance._image.sprite = reticle;
    }

    public static Vector3 GetMousePositionInWorld()
    {
        return (Vector2)_camera.ScreenToWorldPoint(Input.mousePosition);
    }
}

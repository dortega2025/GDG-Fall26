using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Arm : MonoBehaviour
{
    private Camera camera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        RotateArm();
    }

    void RotateArm()
    {
        float mousePosX = Mouse.current.position.x.ReadValue();
        float mousePosY = Mouse.current.position.y.ReadValue();
        Vector3 mousePos = new Vector3(mousePosX, mousePosY, 0f);
        mousePos = camera.ScreenToWorldPoint(mousePos);
        Vector3 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        angle = Mathf.Repeat(angle, 360);
        angle = angle - transform.rotation.z;
        transform.eulerAngles = new Vector3(0, 0, angle);
    }
}

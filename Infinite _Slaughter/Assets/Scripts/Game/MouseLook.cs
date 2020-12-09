using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform playerBody;
    public float mouseSensitivity = 1000.0f;
    private float XRotation = 0;
    public float maxAngleXRotation = 90.0f;
    public float minAngleXRotation = -90.0f;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        XRotation -= mouseY;
        XRotation = Mathf.Clamp(XRotation, minAngleXRotation, maxAngleXRotation);
        transform.localRotation = Quaternion.Euler(XRotation, 0.0f, 0.0f);
        playerBody.Rotate(Vector3.up * mouseX);


    }
}

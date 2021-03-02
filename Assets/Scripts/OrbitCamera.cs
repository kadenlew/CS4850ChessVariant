using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    public float verticalMin;
    public float verticalMax;
    public float panMax;
    public float sensitivity;
    public float zoomMin;
    public float zoomMax;
    public float zoomDefault;
    public float zoomSpeed;
    public float panSpeed;

    private float verticalRot = 0;
    private float horizontalRot = 0;
    private float zoomCurrent = 0;

    private Transform cameraChild;
    private bool rotate = false;
    private bool pan = false;

    // Start is called before the first frame update
    void Start()
    {
        horizontalRot = transform.eulerAngles.y;
        cameraChild = transform.GetChild(0);
        zoomCurrent = zoomDefault;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !pan)
        {
            Cursor.lockState = CursorLockMode.Locked;
            rotate = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            Cursor.lockState = CursorLockMode.None;
            rotate = false;
        }
        if (Input.GetMouseButtonDown(2) && !rotate)
        {
            Cursor.lockState = CursorLockMode.Locked;
            pan = true;
        }

        if (Input.GetMouseButtonUp(2))
        {
            Cursor.lockState = CursorLockMode.None;
            pan = false;
        }

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            if (rotate)
            {
                horizontalRot += Input.GetAxis("Mouse X") * sensitivity;
                verticalRot -= Input.GetAxis("Mouse Y") * sensitivity;
            }
            else
            {
                //Vectors intensify
                transform.Translate(((Quaternion.Euler(0, horizontalRot, 0) * (Input.GetAxis("Mouse Y") * -1f * Vector3.forward)) + (Quaternion.Euler(0, horizontalRot, 0) * (Input.GetAxis("Mouse X") * -1f * Vector3.right))) * panSpeed, Space.World);
                transform.position = new Vector3(Mathf.Clamp(transform.position.x, -panMax, panMax), transform.position.y, Mathf.Clamp(transform.position.z, -panMax, panMax));
            }
            
        }

        zoomCurrent += Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        verticalRot = Mathf.Clamp(verticalRot, -verticalMax, -verticalMin);
        zoomCurrent = Mathf.Clamp(zoomCurrent, zoomMax, zoomMin);
    }

    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(new Vector3(verticalRot, horizontalRot, 0));
        cameraChild.localPosition = new Vector3(0, 1.5f, zoomCurrent);
    }
}

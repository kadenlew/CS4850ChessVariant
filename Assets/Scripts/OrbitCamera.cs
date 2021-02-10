using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    public float verticalMin;
    public float verticalMax;
    public float sensitivity;

    private float verticalRot = 0;
    private float horizontalRot = 0;

    // Start is called before the first frame update
    void Start()
    {
        horizontalRot = transform.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
            Cursor.lockState = CursorLockMode.Locked;

        if (Input.GetMouseButtonUp(1))
            Cursor.lockState = CursorLockMode.None;

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            horizontalRot += Input.GetAxis("Mouse X") * sensitivity;
            verticalRot -= Input.GetAxis("Mouse Y") * sensitivity;
        }

        verticalRot = Mathf.Clamp(verticalRot, -verticalMax, -verticalMin);
    }

    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(new Vector3(verticalRot, horizontalRot, 0));
    }
}

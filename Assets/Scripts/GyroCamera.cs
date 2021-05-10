using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GyroCamera : MonoBehaviour
{

    Gyroscope gyro;
    bool gyroSupported;

    void Start()
    {
        gyroSupported = SystemInfo.supportsGyroscope;

        if (gyroSupported)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            SetParent();

        }
    }

    void SetParent()
    {

        GameObject camParent = new GameObject("camParent");
        camParent.transform.position = transform.position;
        transform.parent = camParent.transform;
        //Set Pivot position
        camParent.transform.rotation = Quaternion.Euler(0, 0, 0f);

    }

    void SetCameraRotation()
    {
        Vector3 previousEulerAngles = transform.eulerAngles;
        Vector3 gyroInput = -gyro.rotationRateUnbiased;

        Vector3 targetEulerAngles = previousEulerAngles + gyroInput * Time.deltaTime * Mathf.Rad2Deg;

        targetEulerAngles.z = 0.0f;

        transform.eulerAngles = targetEulerAngles;
    }





    void Update()
    {
        if (gyroSupported)
        {
            SetCameraRotation();
        }
    }


}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeCamera : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(0f, CameraController.Instance.CameraTrm.eulerAngles.z, 0f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : SingleTon<CameraController>
{
    public Transform CameraTrm => transform;
    CinemachineVirtualCamera _virtualCamera;
    CinemachineTransposer _transposer;

    Vector3 ratio = new Vector3(0f, 1f, -0.3f);
    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _transposer = _virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    }
    public void SetFOV(float value)
    {
        _transposer.m_FollowOffset = ratio * value;
    }
}

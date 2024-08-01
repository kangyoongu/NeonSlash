using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class CameraController : SingleTon<CameraController>
{
    public Transform CameraTrm => transform;
    CinemachineVirtualCamera _virtualCamera;
    CinemachineTransposer _transposer;

    Vector3 ratio = new Vector3(0f, 1f, 0f);

    private Tween dampingTween;
    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _transposer = _virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    }
    private void Start()
    {
        SetDamping(new Vector3(0.2f, 0.2f, 0.2f), new Vector3(0.2f, 0.2f, 0.2f), 0f);
    }
    public void SetFOV(float value)
    {
        _transposer.m_FollowOffset = ratio * value;

        Vector3 direction = ratio * -value;
        direction.z = 3f;
        // 타겟 방향으로의 회전
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        // 회전을 즉시 적용
        transform.rotation = targetRotation;

    }
    public void SetDamping(Vector3 startValue, Vector3 endValue, float duration)
    {
        if (_transposer != null)
        {
            dampingTween?.Kill();
            // 초기 댐핑 값을 설정
            _transposer.m_XDamping = startValue.x;
            _transposer.m_YDamping = startValue.y;
            _transposer.m_ZDamping = startValue.z;

            // DOTween을 사용하여 댐핑 값을 점진적으로 변경
            dampingTween = DOTween.To(() => new Vector3(_transposer.m_XDamping, _transposer.m_YDamping, _transposer.m_ZDamping),
               value => {
                   _transposer.m_XDamping = value.x;
                   _transposer.m_YDamping = value.y;
                   _transposer.m_ZDamping = value.z;
               },
               endValue, duration);
        }
    }
}

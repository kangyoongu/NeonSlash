using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float smoothing = 0.1f;
    public float rotationSpeed = 5f;
    public LayerMask groundLayer;

    private Vector2 _currentVelocity;
    private Rigidbody _rigid;
    private Player _playerCompo;
    public float Speed => _playerCompo.playerStat.speed;

    void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        _playerCompo = GetComponentInParent<Player>();
    }
    private void OnEnable()
    {
        InputManager.Instance.OnMove += Move;
        InputManager.Instance.OnMousePos += LookAtScreenPosition;
    }
    private void OnDisable()
    {
        InputManager.Instance.OnMove -= Move;
        InputManager.Instance.OnMousePos -= LookAtScreenPosition;
    }
    void Move(Vector2 input)
    {
        if (GameManager.Instance.isGamePlaying)
        {
            _currentVelocity = Vector2.Lerp(_currentVelocity, input, smoothing * Time.deltaTime);
            _rigid.velocity = new Vector3(_currentVelocity.x * Speed, _rigid.velocity.y, _currentVelocity.y * Speed);
        }
    }
    void LookAtScreenPosition(Vector2 screenPosition)
    {
        if (GameManager.Instance.isGamePlaying)
        {
            Ray ray = Camera.main.ScreenPointToRay(screenPosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                Vector3 targetPoint = hit.point;
                targetPoint.y = transform.position.y;
                Vector3 direction = (targetPoint - transform.position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                // Smoothly rotate towards the target point
                _playerCompo.visual.rotation = Quaternion.RotateTowards(_playerCompo.visual.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FloorMove : MonoBehaviour
{
    Vector2 _currentPos = Vector3.zero;
    Material floorMaterial;
    [SerializeField] private float duration;
    private void Awake()
    {
        float trash;
        floorMaterial = GetComponent<MeshRenderer>().material;
    }
    private void Start()
    {
        Color initialColor = floorMaterial.color;
        Color.RGBToHSV(initialColor, out float h, out float s, out float v);
        // 무한 반복 애니메이션 설정
        DOTween.To(() => h, x => h = x, 1f, duration)
            .SetLoops(-1, LoopType.Restart)
            .OnUpdate(() => {
                // HSV 값에서 새로운 색상을 계산하고 Material의 색상 업데이트
                Color newColor = Color.HSVToRGB(h % 1f, s, v); // x % 1f로 Hue를 0~1 사이로 유지
                floorMaterial.color = newColor;
            });
    }

    void Update()
    {
        if (_currentPos.x - Player.player.position.x > 1000f)
        {
            _currentPos.x -= 1000f;
            transform.position -= new Vector3(1000f, 0f, 0f);
        }
        else if (_currentPos.x - Player.player.position.x < -1000f)
        {
            _currentPos.x += 1000f;
            transform.position -= new Vector3(-1000f, 0f, 0f);
        }

        if (_currentPos.y - Player.player.position.z > 1000f)
        {
            _currentPos.y -= 1000f;
            transform.position -= new Vector3(0f, 0f, 1000f);
        }
        else if (_currentPos.y - Player.player.position.z < -1000f)
        {
            _currentPos.y += 1000f;
            transform.position -= new Vector3(0f, 0f, -1000f);
        }
    }
}

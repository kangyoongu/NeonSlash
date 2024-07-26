using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorMove : MonoBehaviour
{
    Vector2 _currentPos = Vector3.zero;

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

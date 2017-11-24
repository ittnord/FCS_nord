using UnityEngine;
using System.Collections;

public class MoveEffect : MonoBehaviour
{
    private float _maxMoveDistance;
    private float _moveDistance;
    private float _moveSpeed;
    private Vector3 _direction;

    public void Init(Vector3 direction, float moveSpeed, float maxMoveDistance)
    {
        _direction = direction;
        _moveSpeed = moveSpeed;
        _maxMoveDistance = maxMoveDistance;
    }

    private void Update()
    {
        _moveDistance += _moveSpeed * Time.deltaTime;
        Vector3 v = transform.rotation * _direction * _moveSpeed * Time.deltaTime;
        transform.position += v;

        if (_moveDistance >= _maxMoveDistance)
        {
            Destroy(this);
        }
        
    }
}

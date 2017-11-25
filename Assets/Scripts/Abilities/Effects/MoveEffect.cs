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

    private void FixedUpdate()
    {
        _moveDistance += _moveSpeed * Time.fixedDeltaTime;
        Vector3 v = _direction * _moveSpeed * Time.fixedDeltaTime;
        transform.Translate(v);

        if (_moveDistance >= _maxMoveDistance)
        {
            Destroy(this);
        }
        
    }
}

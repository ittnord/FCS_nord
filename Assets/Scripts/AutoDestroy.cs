using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour
{
    public float Lifetime;
    private float _expireTime;

    private void Start()
    {
        _expireTime = Time.time + Lifetime;
    }

    private void Update()
    {
        if (Time.time >= _expireTime)
        {
            Destroy(gameObject);
        }
    }
}

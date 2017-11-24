using UnityEngine;
using System.Collections;

namespace FCS
{
    public class DefaultAttackAbility : Ability
    {
        private const int _damage = 10;

        private void Awake()
        {
            MaxDuration = 3f;
            ExpireTime = Time.time + MaxDuration;
            ProjectileSpeed = 10f;
        }

        private void Update()
        {
            if (Time.time >= ExpireTime)
            {
                Destroy(gameObject);
            }

            transform.Translate(Vector3.forward * ProjectileSpeed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            other.GetComponent<Character>()?.Change(StatType.Hp, _damage);
            Destroy(gameObject);
        }
    }
}

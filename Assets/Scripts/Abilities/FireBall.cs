using UnityEngine;
using System.Collections;

namespace FCS
{
    public class FireBall : Ability 
    {
        private const int _damage = -15;
        private const float _explosionPower = 4f;
        private const float _explosionSpeed = 8f;

        private const float _innerRadius = 5f;
        private const float _maxRadius = 10f;

        public override void OnMaxDistance()
        {
            Explode();
        }

        public override void OnCollideWithEnvironment(Environment env)
        {
            Explode();
        }

        public override void OnCollideWithCharacter(Character character)
        {
            Explode();
        }

        private void Explode()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, _maxRadius);
            foreach (Collider collider in hitColliders)
            {
                var character = collider.GetComponent<Character>();
                if (character != null)
                {
                    var distance = Vector3.Distance(transform.position, collider.transform.position);
                    var direction = (collider.transform.position - transform.position).normalized;
                    var move = collider.gameObject.AddComponent<MoveEffect>();

                    if (distance <= _innerRadius)
                    {
                        character.Change(StatType.Hp, _damage);
                        move.Init(direction, _explosionSpeed, _explosionPower);

                    }
                    else
                    {
                        character.Change(StatType.Hp, (int)(1 - distance / _maxRadius) * _damage);
                        move.Init(direction, _explosionSpeed, (int)(1 - distance / _maxRadius) * _explosionPower);
                    }
                }
            }

            Destroy(gameObject);
        }

    }
}

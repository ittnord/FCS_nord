using UnityEngine;
using System.Collections;

namespace FCS
{
    public class FireBall : Ability 
    {
        private const int _damage = -15;
        private const float _explosionDistance = 4f;
        private const float _explosionSpeed = 20f;

        private const float _innerRadius = 5f;
        private const float _maxRadius = 10f;

        public override void OnMaxDistance()
        {
            Explode(true, true);
        }

        public override void OnCollideWithEnvironment(Environment env)
        {
            Explode(true, true);
        }

        public override void OnCollideWithCharacter(CharacterBehaviour character)
        {
            Explode(true, true);
        }

        protected void Explode(bool damageSelf, bool pushSelf)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, _maxRadius);
            foreach (Collider collider in hitColliders)
            {
                var character = collider.GetComponent<CharacterBehaviour>();
                if (character != null)
                {
                    var distance = Vector3.Distance(transform.position, collider.transform.position);
                    var direction = (collider.transform.position - transform.position).normalized;
                    var move = collider.gameObject.AddComponent<MoveEffect>();

                    if (distance <= _innerRadius)
                    {
                        character.Change(StatType.Hp, _damage);
                        move.Init(direction, _explosionSpeed, _explosionDistance);

                    }
                    else
                    {
                        character.Change(StatType.Hp, (int)(1 - distance / _maxRadius) * _damage);
                        move.Init(direction, _explosionSpeed, (int)(1 - distance / _maxRadius) * _explosionDistance);
                    }
                }
            }

            PlayEffect(_innerRadius);
            PlayEffect(_maxRadius);

            Destroy(gameObject);
        }

        private void PlayEffect(float explosionRadius)
        {
            var effect = Instantiate(ImpactEffect, transform.position, Quaternion.identity);
            Vector3 explosionSize = new Vector3(explosionRadius, explosionRadius, explosionRadius);
            effect.transform.localScale = explosionSize;
        }

    }
}

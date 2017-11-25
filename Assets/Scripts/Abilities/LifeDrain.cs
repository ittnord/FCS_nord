using UnityEngine;
using System.Collections;

namespace FCS
{
    public class LifeDrain : Ability
    {
        private const int _maxDamage = 50;

        public override void OnCollideWithEnvironment(Environment env)
        {
            var effectDirection = transform.rotation * Vector3.forward;
            var reflect = Vector3.Reflect(effectDirection, env.transform.position.normalized);
            transform.rotation = Quaternion.FromToRotation(effectDirection, reflect);
        }

        public override void OnCollideWithCharacter(CharacterBehaviour character)
        {
            int damage = (int)(_maxDamage * (Distance / MaxDistance));
            character.Change(StatType.Hp, -damage);
            Caster.Change(StatType.Hp, damage);
            Destroy(gameObject);
        }
    }
}

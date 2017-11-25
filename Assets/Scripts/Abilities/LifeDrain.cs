using UnityEngine;
using System.Collections;

namespace FCS
{
    public class LifeDrain : Ability
    {
        private const int _maxDamage = 50;

        public override void OnCollideWithEnvironment(Environment env)
        {
            Destroy(gameObject);
        }

        public override void OnCollideWithCharacter(CharacterBehaviour character)
        {
            int damage = (int)(_maxDamage * (Distance / MaxDistance));
            character.Change(StatType.Hp, -damage);
            Caster.Change(StatType.Hp, damage);
        }
    }
}

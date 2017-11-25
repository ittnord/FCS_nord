using UnityEngine;
using System.Collections;

namespace FCS
{
    public class DefaultAttackAbility : Ability
    {
        private const int _damage = -10;

        public override void OnCollideWithCharacter(CharacterBehaviour character)
        {
            character.Change(StatType.Hp, _damage);
            Destroy(gameObject);
        }
    }
}

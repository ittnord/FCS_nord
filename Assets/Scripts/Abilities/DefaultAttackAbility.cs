using UnityEngine;
using System.Collections;

namespace FCS
{
    public class DefaultAttackAbility : Ability
    {
        private const int _damage = -10;
        private const float _moveDistance = 4f;
        private const float _moveSpeed = 8f;

        public override void OnCollideWithCharacter(CharacterBehaviour character)
        {
            character.Change(StatType.Hp, _damage);
            var direction = (character.transform.position - transform.position).normalized;
            var move = character.gameObject.AddComponent<MoveEffect>();
            move.Init(direction, _moveSpeed, _moveDistance);
            Destroy(gameObject);
        }
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace FCS
{
    public class DefaultAttackAbility : Ability
    {
        private const int _damage = -10;
        private const float _moveDistance = 4f;
        private const float _moveSpeed = 8f;

        [ServerCallback]
        public override void OnCollideWithCharacter(CharacterBehaviour character)
        {
            if (character == Caster && Distance < 1)
            {
                return;
            }

            character.Change(StatType.Hp, _damage);
            var direction = (character.transform.position - transform.position).normalized;
            var move = character.gameObject.AddComponent<MoveEffect>();
            move.Init(direction, _moveSpeed, _moveDistance);
            Destroy(gameObject);
        }
    }
}

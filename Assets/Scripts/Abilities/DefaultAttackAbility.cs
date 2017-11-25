using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace FCS
{
    public class DefaultAttackAbility : Ability
    {
        public  int Damage = -10;
        public float MoveDistance = 4f;
        public float MoveSpeed = 8f;

        public override void OnCollideWithCharacter(CharacterBehaviour character)
        {
            if (character == Caster && Distance < 1)
            {
                return;
            }

            character.Change(StatType.Hp, Damage);
            var direction = (character.transform.position - transform.position).normalized;
            var move = character.gameObject.AddComponent<MoveEffect>();
            move.Init(direction, MoveSpeed, MoveDistance);
            Destroy(gameObject);
        }
    }
}

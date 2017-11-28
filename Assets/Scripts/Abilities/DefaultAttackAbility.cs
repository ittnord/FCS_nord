using FCS.Abilities.Effects;
using UnityEngine;
using UnityEngine.Networking;

namespace FCS.Abilities
{
    public class DefaultAttackAbility : Ability
    {
        public  int Damage = -10;
        public float MoveDistance = 4f;
        public float MoveSpeed = 8f;

        [ServerCallback]
        public override void OnCollideWithEnvironment(Environment env)
        {
            var effectDirection = transform.rotation * Vector3.forward;
            var reflect = Vector3.Reflect(effectDirection, env.transform.position.normalized);
            transform.rotation = Quaternion.FromToRotation(effectDirection, reflect);
        }

        [ServerCallback]
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

using UnityEngine;
using UnityEngine.Networking;

namespace FCS.Abilities
{
    public class LifeDrain : Ability
    {
        public int MaxDamage = 50;

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

            int damage = (int)(MaxDamage * (Distance / MaxDistance));
            character.Change(StatType.Hp, -damage);
            Caster.Change(StatType.Hp, damage);
            Destroy(gameObject);
        }
    }
}

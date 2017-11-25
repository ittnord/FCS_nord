using UnityEngine;
using System.Collections;

namespace FCS
{
    public class TeleportAbility : Ability
    {
        public override void OnCollideWithCharacter(CharacterBehaviour character)
        {
            Teleport();
        }

        public override void OnCollideWithEnvironment(Environment env)
        {
            Teleport();
        }

        public override void OnMaxDistance()
        {
            Teleport();
        }

        private void Teleport()
        {
            Caster.transform.position = transform.position;
            Destroy(gameObject);
        }
    }
}
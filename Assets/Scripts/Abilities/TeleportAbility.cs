using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace FCS
{
    public class TeleportAbility : Ability
    {
        [ServerCallback]
        public override void OnCollideWithCharacter(CharacterBehaviour character)
        {
            Teleport();
        }

        [ServerCallback]
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
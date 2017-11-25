using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace FCS
{
    public class InvisibilityAbility : Ability
    {
        private const float _invisibilityDuration = 3f;

        [ServerCallback]
        public override void OnInstantiate()
        {
            var invisEffect = Caster.gameObject.AddComponent<InvisibilityEffect>();
            invisEffect.Init(_invisibilityDuration);
            base.OnInstantiate();
            Destroy(gameObject);
        }
    }
}

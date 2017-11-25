using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace FCS
{
    public class InvisibilityAbility : Ability
    {
        private float InvisibilityDuration = 3f;

        [ServerCallback]
        public override void OnInstantiate()
        {
            var invisEffect = Caster.gameObject.AddComponent<InvisibilityEffect>();
            invisEffect.Init(InvisibilityDuration);
            base.OnInstantiate();
            Destroy(gameObject);
        }
    }
}

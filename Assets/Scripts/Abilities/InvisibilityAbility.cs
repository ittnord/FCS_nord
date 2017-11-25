using UnityEngine;
using System.Collections;

namespace FCS
{
    public class InvisibilityAbility : Ability
    {
        private const float _invisibilityDuration = 3f;

        public override void OnInstantiate()
        {
            var invisEffect = Caster.gameObject.AddComponent<InvisibilityEffect>();
            invisEffect.Init(_invisibilityDuration);
            base.OnInstantiate();
            Destroy(gameObject);
        }
    }
}

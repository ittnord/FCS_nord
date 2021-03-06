﻿namespace FCS.Abilities
{
    public class InvisibilityAbility : Ability
    {
        public float InvisibilityDuration = 3f;

        public override void OnInstantiate()
        {
            var invisEffect = Caster.gameObject.AddComponent<InvisibilityEffect>();
            invisEffect.Init(InvisibilityDuration);
            base.OnInstantiate();
            Destroy(gameObject);
        }
    }
}

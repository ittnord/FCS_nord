using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


namespace FCS
{
    public class HookAbility : Ability
    {
        private LineRenderer _lineRenderer;
        private const float _hookSpeed = 10f;
        private const float _hookDuration = 5f;

        public Abilities AbilityType = Abilities.Hook;

        public override void OnInstantiate()
        {
            base.OnInstantiate();
            _lineRenderer = gameObject.AddComponent<LineRenderer>();

            _lineRenderer.SetPosition(0, new Vector3(Caster.transform.position.x, 1, Caster.transform.position.z));
            _lineRenderer.SetPosition(1, transform.position);
            _lineRenderer.SetWidth(0.1f, 0.1f);
        }

        protected override void Update()
        {
            base.Update();
            if (_lineRenderer == null) return;
            _lineRenderer.SetPosition(0, new Vector3(Caster.transform.position.x, 1, Caster.transform.position.z));
            _lineRenderer.SetPosition(1, transform.position);
        }

        [ServerCallback]
        public override void OnCollideWithCharacter(CharacterBehaviour character)
        {
            var hookEffect = character.gameObject.AddComponent<HookEffect>();
            hookEffect.Init(Caster.transform, character.transform, _hookSpeed, _hookDuration);
            Destroy(gameObject);
        }

        [ServerCallback]
        public override void OnCollideWithEnvironment(Environment env)
        {
            var hookEffect = Caster.gameObject.AddComponent<HookEffect>();
            hookEffect.Init(env.transform, Caster.transform, _hookSpeed, _hookDuration);
            Destroy(gameObject);
        }
    }
}

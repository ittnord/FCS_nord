using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


namespace FCS
{
    public class HookAbility : Ability
    {
        public LineRenderer _lineRenderer;
        public float HookSpeed = 10f;
        public float HookDuration = 5f;

        public Material HookMaterial;

        public override void OnInstantiate()
        {
            base.OnInstantiate();
            _lineRenderer = gameObject.AddComponent<LineRenderer>();

            _lineRenderer.SetPosition(0, new Vector3(Caster.transform.position.x, 1, Caster.transform.position.z));
            _lineRenderer.SetPosition(1, transform.position);
            _lineRenderer.SetWidth(0.1f, 0.1f);
            _lineRenderer.textureMode = LineTextureMode.Tile;
            _lineRenderer.material = HookMaterial;
        }

        protected override void Update()
        {
            base.Update();
            if (_lineRenderer == null) return;
            _lineRenderer.SetPosition(0, new Vector3(Caster.transform.position.x, 1, Caster.transform.position.z));
            _lineRenderer.SetPosition(1, transform.position);
        }

        public override void OnCollideWithCharacter(CharacterBehaviour character)
        {
            if (character == Caster && Distance < 1)
            {
                return;
            }
            var hookEffect = character.gameObject.AddComponent<HookEffect>();
            hookEffect.Init(Caster.transform, character.transform, HookSpeed, HookDuration, HookMaterial);
            Destroy(gameObject);
        }

        public override void OnCollideWithEnvironment(Environment env)
        {
            var hookEffect = Caster.gameObject.AddComponent<HookEffect>();
            hookEffect.Init(env.transform, Caster.transform, HookSpeed, HookDuration, HookMaterial);
            Destroy(gameObject);
        }
    }
}

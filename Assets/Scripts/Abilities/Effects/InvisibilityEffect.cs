using UnityEngine;
using System.Collections;
using FCS.Abilities.Effects;

namespace FCS
{
    public class InvisibilityEffect : TimeBasedEffect
    {
        private SkinnedMeshRenderer _renderer;

        public override void Init(float duration)
        {
            base.Init(duration);

            _renderer = gameObject.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
            if (_renderer != null)
            {
                _renderer.enabled = false;
            }
        }

        public override void OnExpire()
        {
            _renderer.enabled = true;
            base.OnExpire();
        }
    }
}
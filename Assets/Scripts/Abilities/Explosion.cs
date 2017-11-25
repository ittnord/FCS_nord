using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace FCS
{
    public class Explosion : FireBall
    {
        private const int _damage = -15;
        private const float _explosionDistance = 4f;
        private const float _explosionSpeed = 20f;

        private const float _innerRadius = 5f;
        private const float _maxRadius = 10f;

        public Abilities AbilityType = Abilities.Explosion;

        [ServerCallback]
        public override void OnInstantiate()
        {
            Explode(true);
            Destroy(gameObject);
        }
    }
}
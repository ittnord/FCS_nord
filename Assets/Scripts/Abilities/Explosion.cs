using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace FCS
{
    public class Explosion : FireBall
    {
        public int Damage = -15;
        public float ExplosionDistance = 4f;
        public float ExplosionSpeed = 20f;

        public float InnerRadius = 5f;
        public float MaxRadius = 10f;

        [ServerCallback]
        public override void OnInstantiate()
        {
            Explode(true);
            Destroy(gameObject);
        }
    }
}
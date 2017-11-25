using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace FCS
{
    public class Explosion : FireBall
    {
        [ServerCallback]
        public override void OnInstantiate()
        {
            Explode(true);
            Destroy(gameObject);
        }
    }
}
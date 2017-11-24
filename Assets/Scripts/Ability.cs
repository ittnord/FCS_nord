using UnityEngine;

namespace FCS
{
    public class Ability : MonoBehaviour
    {
        private Character _owner;
        private IAbilityPerformer _performer;

        protected float MaxDuration;
        protected float ProjectileSpeed;
        protected float ExpireTime;
    }
}
using UnityEngine;
using UnityEngine.Networking;

namespace FCS
{
    public class ForcePush : Ability
    {
        private const int _moveDistance = 10;
        private const int _moveSpeed = 10;

        [ServerCallback]
        public override void OnInstantiate()
        {
            var moveEffect = Caster.gameObject.AddComponent<MoveEffect>();
            moveEffect.Init(Vector3.forward, _moveSpeed, _moveDistance);
            base.OnInstantiate();
            Destroy(gameObject);
        }
    }
}

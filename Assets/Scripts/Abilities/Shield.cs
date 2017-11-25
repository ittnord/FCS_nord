using UnityEngine.Networking;

namespace FCS
{
    public class Shield : Ability
    {
        [ServerCallback]
        public override void OnInstantiate()
        {
            Caster.gameObject.AddComponent<ShieldEffect>();
            Destroy(gameObject);
        }
    }
}
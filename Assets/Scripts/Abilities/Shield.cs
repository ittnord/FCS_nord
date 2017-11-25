using UnityEngine.Networking;

namespace FCS
{
    public class Shield : Ability
    {
        public override void OnInstantiate()
        {
            Caster.gameObject.AddComponent<ShieldEffect>();
            Destroy(gameObject);
        }
    }
}
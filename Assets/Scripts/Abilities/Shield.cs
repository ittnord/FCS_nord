using UnityEngine.Networking;

namespace FCS
{
    public class Shield : Ability
    {
        public Abilities AbilityType = Abilities.Shield;

        [ServerCallback]
        public override void OnInstantiate()
        {
            Caster.gameObject.AddComponent<ShieldEffect>();
            Destroy(gameObject);
        }
    }
}
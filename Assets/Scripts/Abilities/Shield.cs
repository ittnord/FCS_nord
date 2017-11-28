using FCS.Abilities.Effects;

namespace FCS.Abilities
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
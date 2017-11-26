namespace FCS
{
    public class TeleportAbility : Ability
    {
        public override void OnCollideWithCharacter(CharacterBehaviour character)
        {
            if (character == Caster && Distance < 1)
            {
                return;
            }

            Teleport();
        }

        public override void OnCollideWithEnvironment(Environment env)
        {
            Teleport();
        }

        public override void OnMaxDistance()
        {
            Teleport();
        }

        private void Teleport()
        {
            Caster.SetPosition(transform.position);
            Destroy(gameObject);
        }
    }
}
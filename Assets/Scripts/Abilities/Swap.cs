namespace FCS
{
    public class Swap : Ability
    {
        public Abilities AbilityType = Abilities.Swap;

        public override void OnCollideWithCharacter(CharacterBehaviour character)
        {
            var characterPosition = character.transform.position;
            character.transform.position = Caster.transform.position;
            Caster.transform.position = characterPosition;
        }
    }
}
namespace FCS.Abilities
{
    public class Swap : Ability
    {
        public override void OnCollideWithCharacter(CharacterBehaviour character)
        {
            if (character == Caster && Distance < 1)
            {
                return;
            }

            var characterPosition = character.transform.position;
            Caster.SetPosition(characterPosition);
            character.GetComponent<CharacterBehaviour>().SetPosition(Caster.transform.position);
        }
    }
}
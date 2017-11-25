﻿namespace FCS
{
    public class Swap : Ability
    {
        public override void OnCollideWithCharacter(CharacterBehaviour character)
        {
            var characterPosition = character.transform.position;
            character.transform.position = Caster.transform.position;
            Caster.transform.position = characterPosition;
        }
    }
}
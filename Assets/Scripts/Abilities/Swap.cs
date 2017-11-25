using UnityEngine.Networking;

namespace FCS
{
    public class Swap : Ability
    {
        [ServerCallback]
        public override void OnCollideWithCharacter(CharacterBehaviour character)
        {
            if (character == Caster && Distance < 1)
            {
                return;
            }

            var characterPosition = character.transform.position;
            character.transform.position = Caster.transform.position;
            Caster.transform.position = characterPosition;
        }
    }
}
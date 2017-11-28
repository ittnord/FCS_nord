namespace FCS.Abilities
{
    public interface IAbilityPerformer
    {
        void Perform(CharacterBehaviour character);
        void Perform(Environment env);
    }
}
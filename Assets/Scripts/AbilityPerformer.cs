namespace FCS
{
    public interface IAbilityPerformer
    {
        void Perform(CharacterBehaviour character);
        void Perform(Environment env);
    }
}
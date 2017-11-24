namespace FCS
{
    public interface IAbilityPerformer
    {
        void Perform(Character character);
        void Perform(Environment env);
    }
}
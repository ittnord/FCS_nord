namespace FCS
{
    public interface IAbility
    {
        void Perform(ICharacter character);
        void Perform(IEnvironment env);
    }
}
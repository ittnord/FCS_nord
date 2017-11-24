using System;
using System.Collections.Generic;
using System.Linq;

namespace FCS
{
    public interface ICharacter
    {
        event Action<IStat> OnStatChanged;
        event Action<IStat> OnStatDie;

        HashSet<IStat> GetStats();
        List<IAbility> GetAbilities();
    }

    public class Character : ICharacter
    {
        private readonly HashSet<IStat> _stats = new HashSet<IStat>();
        private readonly List<IAbility> _abilities = new List<IAbility>();


        public event Action<IStat> OnStatChanged;
        public event Action<IStat> OnStatDie;

        public HashSet<IStat> GetStats()
        {
            return _stats;
        }

        public List<IAbility> GetAbilities()
        {
            return _abilities;
        }

        public void Use(IAbility ability)
        {
            // TODO: implement me
        }

        public void Change(StatType type, int value)
        {
            var stat = _stats.First(element => element.Type == type);
            stat.Current += value;
            if (stat.Current <= 0)
            {
                if (OnStatDie != null)
                {
                    OnStatDie(stat);
                }
            }
            else
            {
                if (OnStatChanged != null)
                {
                    OnStatChanged(stat);
                }
            }
        }
    }
}
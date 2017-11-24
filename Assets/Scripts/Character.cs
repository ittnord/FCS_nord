using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FCS
{
    public class Character : MonoBehaviour
    {
        private readonly HashSet<IStat> _stats = new HashSet<IStat>();

        public event Action<IStat> OnStatChanged;
        public event Action<IStat> OnStatDie;

        public HashSet<IStat> GetStats()
        {
            return _stats;
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
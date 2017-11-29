using System;
using FCS.Abilities;
using UnityEngine;

namespace FCS
{
    public class InputController : Singleton<InputController>
    {
        public Vector3 InputDirection { get; set; }

        public event Action<AbilityType> OnAbilityUsed;

        public void HandleAbility(AbilityType abilityType)
        {
            if (OnAbilityUsed != null)
            {
                OnAbilityUsed(abilityType);
            }
        }

        public bool InverseDirection { set; get; }
    }
}

using System;
using UnityEngine;

namespace FCS
{
    public class InputController : Singleton<InputController>
    {
        public Vector3 InputDirection { get; set; }

        public event Action<Ability> OnAbilityUsed;

        public void HandleAbility(Ability ability)
        {
            OnAbilityUsed?.Invoke(ability);
        }

        public bool InverseDirection { set; get; }
    }
}
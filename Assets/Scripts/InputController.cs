using System;
using UnityEngine;

namespace FCS
{
    public class InputController : Singleton<InputController>
    {
        public Vector3 InputDirection { get; set; }

        public event Action<Abilities> OnAbilityUsed;

        public void HandleAbility(Abilities ability)
        {
            OnAbilityUsed?.Invoke(ability);
        }

        public bool InverseDirection { set; get; }
    }
}
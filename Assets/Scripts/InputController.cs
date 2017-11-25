using System;
using UnityEngine;

namespace FCS
{
    public class InputController : Singleton<InputController>
    {
        public Vector3 InputDirection { get; set; }

        public event Action OnAbilityUsed;

        public void HandleAbility()
        {
            OnAbilityUsed?.Invoke();
        }

        public bool InverseDirection { set; get; }
    }
}
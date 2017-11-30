using System;
using System.Collections.Generic;

namespace FCS.Abilities
{
    public class AbilitiesStorage : Singleton<AbilitiesStorage>
    {
        private readonly HashSet<AbilityType> _selectedAbilities = new HashSet<AbilityType>();
        private readonly Dictionary<AbilityType, float> _abilitiesCd = new Dictionary<AbilityType, float>();

        public event Action<bool> AvailableChanged;
        public event Action<AbilityType, float> OnCdChanged;
        public event Action<AbilityType> OnCdBegin;
        public event Action<AbilityType> OnCdEnd;

        public bool CanStart
        {
            get { return _selectedAbilities.Count == 3; }
        }

        public List<AbilityType> SelectedAbilities
        {
            get
            {
                var result = new List<AbilityType>();
                result.Add(AbilityType.DefaultAbility);
                result.AddRange(_selectedAbilities);
                return result;
            }
        }

        public void Clear()
        {
            _selectedAbilities.Clear();
            _abilitiesCd.Clear();
        }

        public void ChangeState(AbilityType abilityType)
        {
            if (_selectedAbilities.Contains(abilityType))
            {
                _selectedAbilities.Remove(abilityType);
            }
            else
            {
                if (_selectedAbilities.Count >= 3)
                {
                    return;
                }

                _selectedAbilities.Add(abilityType);
            }

            if (AvailableChanged != null)
            {
                AvailableChanged(CanStart);
            }
        }

        public bool IsUnderCd(AbilityType abilityTypeType)
        {
            return _abilitiesCd.ContainsKey(abilityTypeType) && _abilitiesCd[abilityTypeType] > 0.0f;
        }

        private float GetCd(AbilityType abilityType)
        {
            return abilityType == AbilityType.DefaultAbility ? 1.0f : 2.0f;
        }

        public void CallOnAbilityCdBegin(AbilityType abilityTypeType)
        {
            _abilitiesCd[abilityTypeType] = GetCd(abilityTypeType);
            if (OnCdBegin != null)
            {
                OnCdBegin(abilityTypeType);
            }
        }

        public void CallOnAbilityCdEnd(AbilityType abilityTypeType)
        {
            _abilitiesCd[abilityTypeType] = 0.0f;
            if (OnCdEnd != null)
            {
                OnCdEnd(abilityTypeType);
            }
        }


        public void CallOnCdChanged(AbilityType abilityTypeType, float value)
        {
            if (!_abilitiesCd.ContainsKey(abilityTypeType))
            {
                _abilitiesCd[abilityTypeType] = 0.0f;
            }
            var oldValue = _abilitiesCd[abilityTypeType];
            _abilitiesCd[abilityTypeType] = Math.Max(oldValue + value, 0.0f);
            if (OnCdChanged != null)
            {
                OnCdChanged(abilityTypeType, 1.0f - _abilitiesCd[abilityTypeType] / GetCd(abilityTypeType));
            }
            if (oldValue > 0.0f && _abilitiesCd[abilityTypeType] == 0.0f)
            {
                CallOnAbilityCdEnd(abilityTypeType);
            }
        }
    }
}

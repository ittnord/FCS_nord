using System;
using System.Collections;
using System.Collections.Generic;
using FCS;
using UnityEngine;

public class AbilitiesStorage : Singleton<AbilitiesStorage>
{
    private readonly HashSet<Abilities> selectedAbilities = new HashSet<Abilities>();
    private readonly Dictionary<Abilities, float> abilitiesCd = new Dictionary<Abilities, float>();

    public event Action<bool> AvailableChanged;
    public event Action<Abilities, float> OnCdChanged;
    public event Action<Abilities> OnCdBegin;
    public event Action<Abilities> OnCdEnd; 

    public bool CanStart => selectedAbilities.Count == 3;

    public List<Abilities> SelectedAbilities
    {
        get
        {
            var result = new List<Abilities>();
            result.Add(Abilities.DefaultAbility);
            result.AddRange(selectedAbilities);
            return result;
        }
    }

    public void ChangeState(Abilities abilities)
    {
        if (selectedAbilities.Contains(abilities))
        {
            selectedAbilities.Remove(abilities);
        }
        else
        {
            if (selectedAbilities.Count >= 3)
            {
                return;
            }

            selectedAbilities.Add(abilities);
        }

        if (AvailableChanged != null)
        {
            AvailableChanged(CanStart);
        }
    }

    public bool IsUnderCd(Abilities abilityType)
    {
        return abilitiesCd.ContainsKey(abilityType) && abilitiesCd[abilityType] > 0.0f;
    }

    private float GetCd(Abilities ability)
    {
        return ability == Abilities.DefaultAbility ? 1.0f : 2.0f;
    }

    public void CallOnAbilityCdBegin(Abilities abilityType)
    {
        abilitiesCd[abilityType] = GetCd(abilityType);
        if (OnCdBegin != null)
        {
            OnCdBegin(abilityType);
        }
    }

    public void CallOnAbilityCdEnd(Abilities abilityType)
    {
        abilitiesCd[abilityType] = 0.0f;
        if (OnCdEnd != null)
        {
            OnCdEnd(abilityType);
        }
    }


    public void CallOnCdChanged(Abilities abilityType, float value)
    {
        if (!abilitiesCd.ContainsKey(abilityType))
        {
            abilitiesCd[abilityType] = 0.0f;
        }
        var oldValue = abilitiesCd[abilityType];
        abilitiesCd[abilityType] = Math.Max(oldValue + value, 0.0f);
        if (OnCdChanged != null)
        {
            OnCdChanged(abilityType, 1.0f - abilitiesCd[abilityType] / GetCd(abilityType));
        }
        if (oldValue > 0.0f && abilitiesCd[abilityType] == 0.0f)
        {
            CallOnAbilityCdEnd(abilityType);
        }
    }
}

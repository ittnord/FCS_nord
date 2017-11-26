using System;
using System.Collections;
using System.Collections.Generic;
using FCS;
using UnityEngine;

public class AbilitiesStorage : Singleton<AbilitiesStorage>
{
    private readonly HashSet<Abilities> selectedAbilities = new HashSet<Abilities>();

    public event Action<bool> AvailableChanged;

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
}

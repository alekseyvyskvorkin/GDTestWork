using System;
using UnityEngine;

[Serializable]
public class Attack
{
    public Action<bool> OnDistanceCheck;

    public AttackSettings AttackSettings => _attackSettings;    
    public bool IsReadyToUse { get; set; } = true;

    [SerializeField] private AttackSettings _attackSettings;

    public bool IsEnoughtDistance(float distance)
    {
        return AttackSettings.AttackDistance >= distance;
    }
}

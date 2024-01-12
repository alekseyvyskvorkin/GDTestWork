using UnityEngine;

[System.Serializable]
public class AttackSettings
{
    public string Name => _name;
    public int Damage => _damage;
    public float HitTime => _hitTime;
    public float AttackDistance => _attackDistance;
    public float Cooldown => _cooldown;

    [SerializeField] private string _name;
    [SerializeField] private int _damage;
    [SerializeField] private float _hitTime;
    [SerializeField] private float _attackDistance;
    [SerializeField] private float _cooldown;
}


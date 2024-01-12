using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;

namespace TestWork.Units
{
    [RequireComponent(typeof(Animator))]
    public abstract class BaseUnit : MonoBehaviour
    {
        private const string Die = "Die";

        public abstract BaseUnit Target { get; }

        public Action OnDie;
        public Action OnSpawn;

        public UnitSettings UnitSettings => _unitSettings;
        public List<Attack> Attacks => _attacks;
        public Animator Animator { get; private set; }
        public bool IsAbilityAnimationCompleted { get; set; } = true;
        public int HealthPoints { get; protected set; }
        public float DistanceToTarget { get; protected set; }

        [SerializeField] private UnitSettings _unitSettings;
        [SerializeField] private List<Attack> _attacks = new List<Attack>();

        public virtual void Init()
        {
            Animator = GetComponent<Animator>();
            OnSpawn += ActivateUnit;
            OnDie += DeactivateUnit;
        }

        public virtual void ActivateUnit()
        {
            HealthPoints = _unitSettings.HealthPoints;
        }

        public virtual void DeactivateUnit()
        {
            foreach (var ability in Attacks)
            {
                ability.IsReadyToUse = true;
            }
            Animator.SetTrigger(Die);
        }

        public bool IsDead()
        {
            return HealthPoints <= 0;
        }

        public void Attack(Attack ability)
        {
            if (ability.IsReadyToUse == false || IsAbilityAnimationCompleted == false) return;
            Animator.SetTrigger(ability.AttackSettings.Name);
            StartCoroutine(CCooldown(ability));
            if (!Target || !ability.IsEnoughtDistance((Target.transform.position - transform.position).magnitude)) return;
            transform.DOLookAt(Target.transform.position, 0.1f);
            StartCoroutine(HitTarget(ability));
        }

        private IEnumerator CCooldown(Attack ability)
        {
            ability.IsReadyToUse = false;
            yield return new WaitForSeconds(ability.AttackSettings.Cooldown);
            ability.IsReadyToUse = true;
        }

        private IEnumerator HitTarget(Attack ability)
        {
            yield return ability.AttackSettings.HitTime;
            Target.TakeDamage(ability.AttackSettings.Damage);
        }

        private void TakeDamage(int damage)
        {
            if (IsDead()) return;

            HealthPoints -= damage;

            if (HealthPoints <= 0)
            {
                OnDie?.Invoke();
            }
        }
    }
}


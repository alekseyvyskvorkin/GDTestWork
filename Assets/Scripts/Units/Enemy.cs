using UnityEngine;
using UnityEngine.AI;
using Zenject;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace TestWork.Units
{
    public class Enemy : BaseUnit
    {
        public NavMeshAgent Agent { get; private set; }

        public override BaseUnit Target => _player;
        public UnitType UnitType => _unitType;

        [SerializeField] private UnitType _unitType;

        private Player _player;

        private Spawner _spawner;

        private CapsuleCollider _collider;

        [Inject]
        private void Construct(Player player, Spawner spawner)
        {
            _player = player;
            _spawner = spawner;
        }

        public override void Init()
        {
            base.Init();
            Agent = GetComponent<NavMeshAgent>();
            _collider = GetComponent<CapsuleCollider>();
            Agent.speed = UnitSettings.MoveSpeed;
            OnDie += Die;
            OnDie += ReturnToPool;
            OnDie += _player.HealPlayer;
        }

        private void Update()
        {
            if (IsDead())
            {
                return;
            }

            DistanceToTarget = (transform.position - Target.transform.position).magnitude;

            if (Attacks[0].IsEnoughtDistance(DistanceToTarget) && !Target.IsDead())
            {
                StopAgent();
                Attack(Attacks[0]);
            }
            else if (Target.IsDead())
            {
                StopAgent();
            }

            if (IsAbilityAnimationCompleted && !Attacks[0].IsEnoughtDistance(DistanceToTarget))
            {
                Move(Target.transform.position);
            }
        }

        private void StopAgent()
        {
            Animator.SetFloat("Speed", 0);
            Agent.isStopped = true;
        }

        private void Move(Vector3 destination)
        {
            Animator.SetFloat("Speed", 1);
            Agent.isStopped = false;
            Agent.SetDestination(destination);
        }

        private void ReturnToPool()
        {
            _spawner.ReturnToPool(this);
        }

        private async void Die()
        {
            await UniTask.Delay(1000);
            transform.DOMove(transform.position - Vector3.up, 1f)
                .OnComplete(() => gameObject.SetActive(false));
        }

        public override void ActivateUnit()
        {
            base.ActivateUnit();
            IsAbilityAnimationCompleted = true;
            gameObject.SetActive(true);
            Agent.enabled = true;
            _collider.enabled = true;
        }

        public override void DeactivateUnit()
        {
            base.DeactivateUnit();
            Agent.enabled = false;
            _collider.enabled = false;
        }
    }
}
using UnityEngine;

namespace TestWork.Units
{
    public class Player : BaseUnit
    {
        public override BaseUnit Target => NearestTarget();
        public Spawner Spawner { get; set; }

        [SerializeField] private int _playerHealPerEnemy = 2;

        private CharacterController _characterController;

        private int _nearestTargetIndex;

        private void Awake()
        {
            Init();
        }

        public override void Init()
        {
            base.Init();
            _characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            _nearestTargetIndex = -1;
            DistanceToTarget = float.MaxValue;
            for (int i = 0; i < Spawner.ActiveEnemies.Count; i++)
            {
                float distance = (transform.position - Spawner.ActiveEnemies[i].transform.position).magnitude;
                if (distance < DistanceToTarget)
                {
                    DistanceToTarget = distance;
                    _nearestTargetIndex = i;
                }
            }

            foreach (var ability in Attacks)
            {
                ability.OnDistanceCheck?.Invoke(ability.IsEnoughtDistance(DistanceToTarget));
            }
        }

        public void ResetPlayer()
        {
            Animator.Rebind();
            IsAbilityAnimationCompleted = true;
            transform.position = Vector3.zero;
        }

        public void StopPlayer() => Animator.SetFloat("Speed", 0);

        public void HealPlayer()
        {
            if (HealthPoints > 0) HealthPoints += _playerHealPerEnemy;
            HealthPoints = Mathf.Clamp(HealthPoints, 0, UnitSettings.HealthPoints);
        }

        public void Move(Vector3 direction)
        {
            if (direction.magnitude > 0.1f)
                Animator.SetFloat("Speed", 1);
            else
                Animator.SetFloat("Speed", 0);

            if (IsAbilityAnimationCompleted && direction.magnitude > 0.1f)
            {
                Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * UnitSettings.RotateSpeed);
                _characterController.Move(direction * Time.deltaTime * UnitSettings.MoveSpeed);
            }
        }

        private Enemy NearestTarget()
        {
            if (_nearestTargetIndex != -1)
                return Spawner.ActiveEnemies[_nearestTargetIndex];
            return null;
        }
    }
}
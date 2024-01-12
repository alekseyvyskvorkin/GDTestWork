using System;
using UnityEngine;

namespace TestWork.Units
{
    [Serializable]
    public class UnitSettings
    {
        public int HealthPoints => _healthPoints;
        public float MoveSpeed => _moveSpeed;
        public float RotateSpeed => _rotateSpeed;

        [SerializeField] private int _healthPoints = 2;
        [SerializeField] private float _moveSpeed = 3.5f;
        [SerializeField] private float _rotateSpeed = 5f;
    }
}
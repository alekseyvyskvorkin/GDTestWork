using UnityEngine;
using System.Collections.Generic;
using TestWork.Units;

namespace TestWork.Infrastructure
{
    [CreateAssetMenu(fileName = "Config")]
    public class Config : ScriptableObject
    {
        public Wave[] Waves => _waves;

        [SerializeField] private Wave[] _waves;
        [SerializeField] private List<Enemy> _enemies;

        public BaseUnit GetEnemy(UnitType unitType)
        {
            foreach (var unit in _enemies)
            {
                if (unit.UnitType == unitType)
                {
                    return unit;
                }
            }
            Debug.Log("unit not founded");
            return null;
        }
    }
}
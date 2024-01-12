using UnityEngine;
using Zenject;

namespace TestWork.Infrastructure
{
    public class Factory
    {
        public DiContainer DiContainer;

        public Factory(DiContainer diContainer)
        {
            DiContainer = diContainer;
        }

        public T Create<T>(MonoBehaviour behaviour) where T : MonoBehaviour
        {
            var spawnObject = DiContainer.InstantiatePrefabForComponent<T>(behaviour);
            DiContainer.Rebind<T>().FromInstance(spawnObject);
            return spawnObject;
        }
    }
}
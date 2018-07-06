using UnityEngine;
using Unity.Entities;

namespace Example.Components
{
    [System.Serializable]
    public struct IsDead : IComponentData
    {
        public float TimeOfDeath;
    }

    [System.Serializable]
    [RequireComponent(typeof(GameObjectEntity))]
    public class IsDeadComponent : ComponentDataWrapper<IsDead> { }
}

using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using AnimationBaker.Components;
using AnimationBaker.Interfaces;

namespace AnimationBaker.Baked
{
    [System.Serializable]
    public struct Cerberus : IUnitState
    {
        public float Runtime
        {
            get;
            set;
        }
        public float StateTimer
        {
            get;
            set;
        }
        public int CurrentState
        {
            get;
            set;
        }
        public int PreviousState
        {
            get;
            set;
        }

        public int IsHit; // 0
        public int Attack; // 1
        public float MoveSpeed; // 2
        public int IsDead; // 3

        public float this [int index]
        {
            get
            {
                switch (index)
                {

                    case 0:
                        return IsHit;
                    case 1:
                        return Attack;
                    case 2:
                        return MoveSpeed;
                    case 3:
                        return IsDead;
                }
                return 0;
            }
            set
            {
                switch (index)
                {

                    case 0:
                        IsHit = (int) value;
                        break;
                    case 1:
                        Attack = (int) value;
                        break;
                    case 2:
                        MoveSpeed = value;
                        break;
                    case 3:
                        IsDead = (int) value;
                        break;
                }
            }
        }
    }

    [RequireComponent(typeof(StateMachineUnitComponent), typeof(PositionComponent), typeof(RotationComponent))]
    public class CerberusComponent : ComponentDataWrapper<Cerberus> { }
}

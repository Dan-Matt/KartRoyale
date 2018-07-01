using Assets.Scripts.Components;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class EnemyCar
    {
        public GameObject GameObject;
        public AiMovementComponent AiMovementComponent;
        public LapTrackingComponent LapTrackingComponent;
        public HealthComponent HealthComponent;
        public MoveComponent MoveComponent;
    }
}

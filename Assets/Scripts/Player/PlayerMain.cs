using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMain : MonoBehaviour
    {
        [Header("Speed and Force")]
        [field: SerializeField] public float MovementSpeed { get; private set; }
        [field: SerializeField] public float JumpForce { get; private set; }
        [field: SerializeField] public float SlowMovementSpeed { get; private set; }
    
        [Header("Level Inputs")]
        [field: SerializeField] public bool SlowPlaying { get; private set; }
        
        public Rigidbody2D Rb { get; private set; }
        public Animator Anim { get; private set; }
    
        void Awake()
        {
            if (SlowPlaying)
            {
                MovementSpeed = SlowMovementSpeed;
            }
            Rb = GetComponent<Rigidbody2D>();
            Anim = GetComponent<Animator>();
        }
        
    }
}

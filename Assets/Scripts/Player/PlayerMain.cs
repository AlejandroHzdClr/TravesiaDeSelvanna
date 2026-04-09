using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMain : MonoBehaviour
    {
        [field: SerializeField] public float MovementSpeed { get; private set; }
        [field: SerializeField] public float JumpForce { get; private set; }
    
        public Rigidbody2D Rb { get; private set; }
        public Animator Anim { get; private set; }
    
        void Awake()
        {
            Rb = GetComponent<Rigidbody2D>();
            Anim = GetComponent<Animator>();
        }
        
    }
}

using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "InputSystemSO", menuName = "Scriptable Objects/InputSystemSO")]
    public class InputSystemSO : ScriptableObject, PlayerInput.IBosqueActions, PlayerInput.ITerrorActions
    {
        [SerializeField] private bool terror;

        public event Action<Vector2> OnMove, OnAim;
        public event Action OnShootStarted, OnShootEnded;
        public event Action OnLightStarted, OnLightEnded;
        public event Action OnJumpPressed;
        public event Action OnInteractPressed;

        private PlayerInput inputs;

        private void OnEnable()
        {
            inputs = new PlayerInput();

            if (!terror)
            {
                inputs.Bosque.SetCallbacks(this);
                inputs.Bosque.Enable();
                inputs.Terror.Disable();
            }
            else
            {
                inputs.Terror.SetCallbacks(this);
                inputs.Terror.Enable();
                inputs.Bosque.Disable();
            }

            inputs.Enable();
        }

        private void OnDisable()
        {
            inputs.Disable();
        }

        void PlayerInput.IBosqueActions.OnShoot(InputAction.CallbackContext context)
        {
            if (context.started) OnShootStarted?.Invoke();
            else if (context.canceled) OnShootEnded?.Invoke();
        }

        void PlayerInput.ITerrorActions.OnShoot(InputAction.CallbackContext context)
        {
            if (context.started) OnShootStarted?.Invoke();
            else if (context.canceled) OnShootEnded?.Invoke();
        }

        void PlayerInput.ITerrorActions.OnLight(InputAction.CallbackContext context)
        {
            if (context.started) OnLightStarted?.Invoke();
            else if (context.canceled) OnLightEnded?.Invoke();
        }

        void PlayerInput.IBosqueActions.OnAim(InputAction.CallbackContext context)
        {
            OnAim?.Invoke(context.ReadValue<Vector2>());
        }

        void PlayerInput.ITerrorActions.OnAim(InputAction.CallbackContext context)
        {
            OnAim?.Invoke(context.ReadValue<Vector2>());
        }

        void PlayerInput.IBosqueActions.OnAD(InputAction.CallbackContext context)
        {
            float x = context.ReadValue<float>();
            OnMove?.Invoke(new Vector2(x, 0));
        }

        void PlayerInput.ITerrorActions.OnAD(InputAction.CallbackContext context)
        {
            float x = context.ReadValue<float>();
            OnMove?.Invoke(new Vector2(x, 0));
        }

        void PlayerInput.ITerrorActions.OnWS(InputAction.CallbackContext context)
        {
            float y = context.ReadValue<float>();
            OnMove?.Invoke(new Vector2(0, y));
        }

        void PlayerInput.IBosqueActions.OnJump(InputAction.CallbackContext context)
        {
            if (context.started) OnJumpPressed?.Invoke();
        }

        void PlayerInput.IBosqueActions.OnInteract(InputAction.CallbackContext context)
        {
            if (context.started) OnInteractPressed?.Invoke();
        }

        void PlayerInput.ITerrorActions.OnInteract(InputAction.CallbackContext context)
        {
            if (context.started) OnInteractPressed?.Invoke();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tanks
{
    [RequireComponent(typeof(MoveComponent), typeof(FireComponent))]
    public class InputComponent : MonoBehaviour
    {
        private DirectionType _lastInput = DirectionType.Top;
        private MoveComponent _moveComp;
        private FireComponent _fireComp;

        [SerializeField]
        private InputAction _move;
        [SerializeField]
        private InputAction _fire;

        private void Start()
        {
            _moveComp = GetComponent<MoveComponent>();
            _fireComp = GetComponent<FireComponent>();

            _move.Enable();
            _fire.Enable();
        }

        private void Update()
        {
            var button = _fire.ReadValue<float>();
            if (button == 1f)
            {
                _fireComp.OnFire();
            }

            var direction = _move.ReadValue<Vector2>();
            DirectionType input;
            if (direction.x != 0f && direction.y != 0)
            {
                input = _lastInput;
            }
            else if (direction.x == 0 && direction.y == 0) return;
            else
            {
                input = _lastInput = direction.GetDirectionFromVector();
            }

            _moveComp.OnMove(input);
        }

        private void OnDestroy()
        {
            _move.Disable();
            _fire.Disable();
        }
    }
}

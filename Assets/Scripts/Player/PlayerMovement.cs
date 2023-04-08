using System;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public event Action<bool> OnPlayerMove;
        
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotationSpeed;
        
        private PlayerInputAsset _playerInput;
        private Vector2 _movingDirection;
        
        private void Awake()
        {
            _playerInput = new PlayerInputAsset();
             _playerInput.Player.Enable();
        }

        private void Update()
        {
            SetMovingDirection();
        }

        private void FixedUpdate()
        {
            MovePlayer();
            RotatePlayer();
        }

        private void SetMovingDirection()
        {
            _movingDirection = _playerInput.Player.Move.ReadValue<Vector2>();
        }

        private void MovePlayer()
        {
            if (_movingDirection == Vector2.zero)
            {
                OnPlayerMove?.Invoke(false);
                return;
                
            }
            
            Vector3 moveDirection = new Vector3(_movingDirection.x, 0f, _movingDirection.y);
            transform.position += moveDirection * (_moveSpeed * Time.deltaTime);
            OnPlayerMove?.Invoke(true);
        }
        
        private void RotatePlayer()
        {
            Vector3 moveDirection = new Vector3(_movingDirection.x, 0f, _movingDirection.y);
            transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * _rotationSpeed);
        }
    }
}

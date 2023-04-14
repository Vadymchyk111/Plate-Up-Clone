using System;
using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public event Action<bool> OnPlayerMove;

        [SerializeField] private float _speed;
        [SerializeField] private bool _isMoving = true;
        [SerializeField] private bool _isRotating = true;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _intervalRotation = 0.1f;

        private PlayerInputAsset _actionsAsset;
        private Vector3 _axisRotation = new(0, 1, 0);
        private Vector3 _direction;
        private Coroutine _rotationCoroutine;
        private Coroutine _moveCoroutine;
        private Vector3 _refVelocity = Vector3.zero;
        private readonly WaitForFixedUpdate _waitForFixedUpdate = new();
        private readonly float _smoothVal = .2f; // Higher = 'Smoother'

        public bool IsMove => _isMoving;
        public bool IsRotation => _isRotating;

        public float Speed
        {
            set => _speed = value;
        }

        private void Awake()
        {
            _actionsAsset = new PlayerInputAsset();
        }

        private void OnEnable()
        {
            _actionsAsset.Player.Move.Enable();
        }

        private void OnDisable()
        {
            _actionsAsset.Player.Move.Disable();
        }

        protected virtual void SetActive(bool isActive)
        {
            if (IsMove)
            {
                SetActiveMove(isActive);
            }

            if (IsRotation)
            {
                SetActiveRotation(isActive);
            }
        }

        protected virtual void SetActiveMove(bool isActive)
        {
            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
            }

            if (isActive && _direction != Vector3.zero)
            {
                _moveCoroutine = StartCoroutine(MoveCoroutine());
                OnPlayerMove?.Invoke(true);
            }
            else
            {
                _rigidbody.velocity = Vector3.zero;
                OnPlayerMove?.Invoke(false);
            }
        }

        protected virtual IEnumerator MoveCoroutine()
        {
            while (true)
            {
                yield return _waitForFixedUpdate;
                _direction = new Vector3(_direction.x, 0, _direction.z);
                var velocity = _rigidbody.velocity;
                var speedLimit = new Vector3(velocity.x, 0, velocity.z);

                if (speedLimit.magnitude < _speed)
                {
                    _rigidbody.velocity = Vector3.SmoothDamp(
                        _rigidbody.velocity,
                        _direction * _speed, ref _refVelocity, _smoothVal);
                }
            }
        }

        protected virtual void SetActiveRotation(bool isActive)
        {
            if (_rotationCoroutine != null)
            {
                StopCoroutine(_rotationCoroutine);
            }

            if (isActive)
            {
                _rotationCoroutine = StartCoroutine(RotationCoroutine());
            }
            else
            {
                _rigidbody.angularVelocity = Vector3.zero;
            }
        }

        private void FixedUpdate()
        {
            CheckActiveMovement(_actionsAsset.Player.Move.ReadValue<Vector2>());
        }

        protected virtual IEnumerator RotationCoroutine()
        {
            while (true)
            {
                yield return _waitForFixedUpdate;
                float destinationAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
                var destinationRotation = Quaternion.Euler(new Vector3(0, destinationAngle, 0));

                var delta = destinationRotation * Quaternion.Inverse(_rigidbody.rotation);

                delta.ToAngleAxis(out float angle, out _axisRotation);

                if (float.IsInfinity(_axisRotation.x))
                    continue;

                if (angle > 180f)
                    angle -= 360f;

                Vector3 angular = (0.9f * Mathf.Deg2Rad * angle / _intervalRotation) * _axisRotation.normalized;

                _rigidbody.angularVelocity = angular;
            }
        }

        private void CheckActiveMovement(Vector2 direction)
        {
            if (direction == Vector2.zero)
            {
                SetActive(false);
                return;
            }

            _direction.x = direction.x;
            _direction.z = direction.y;
            SetActive(true);
        }
    }
}
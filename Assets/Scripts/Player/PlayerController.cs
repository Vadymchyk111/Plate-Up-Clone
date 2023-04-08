using System;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerAnimation _playerAnimation;

        private void OnEnable()
        {
            _playerMovement.OnPlayerMove += _playerAnimation.SetMoving;
        }

        private void OnDisable()
        {
            _playerMovement.OnPlayerMove -= _playerAnimation.SetMoving;
        }
    }
}
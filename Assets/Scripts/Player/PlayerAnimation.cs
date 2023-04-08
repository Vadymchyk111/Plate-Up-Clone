using UnityEngine;

namespace Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        
        private const string MOVING = "IsMoving";
        
        public void SetMoving(bool isMoving)
        {
            _animator.SetBool(MOVING, isMoving);
        }
    }
}
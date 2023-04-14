using System;
using UnityEngine;

namespace InteractableObjects
{
    public class Interactable : MonoBehaviour
    {
        public event Action<bool> OnSelectedInteractable;
        
        private bool _isSelected;
        
        private bool IsSelected => _isSelected;

        public void SetSelectedInteractable()
        {
            _isSelected = true;
            OnSelectedInteractable?.Invoke(IsSelected);
        }
        
        public void SetUnselectedInteractable()
        {
            _isSelected = false;
            OnSelectedInteractable?.Invoke(IsSelected);
        }
    }
}
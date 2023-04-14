using System.Collections.Generic;
using InteractableObjects;
using UnityEngine;

namespace Player
{
    public class PlayerInteract : MonoBehaviour
    {
        private HashSet<Interactable> _interactables = new();
        private Interactable _selectedInteractable;

        private void OnTriggerStay(Collider other)
        {
            Interactable interactable = other.gameObject.GetComponent<Interactable>();
            if (interactable == null)
            {
                return;
            }
            
            SwitchCurrentInteractable();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.TryGetComponent(out Interactable interactable))
            {
                return;
            }
            
            _interactables.Add(interactable);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.TryGetComponent(out Interactable interactable))
            {
                return;
            }
            
            _interactables.Remove(interactable);
            SwitchCurrentInteractable();
        }

        private Interactable TryGetClosestInteractable()
        {
            float minDistance = float.MaxValue;
            Interactable closestInteractable = null;
            foreach (Interactable interactable in _interactables)
            {
                float distance = Vector3.Distance(interactable.gameObject.transform.position, gameObject.transform.position);
                if (!(distance < minDistance))
                {
                    continue;
                }
                
                minDistance = distance;
                closestInteractable = interactable;
            }

            return closestInteractable;
        }

        private void SwitchCurrentInteractable()
        {
            Interactable interactable = TryGetClosestInteractable();

            if (interactable == null)
            {
                _selectedInteractable.SetUnselectedInteractable();
                _selectedInteractable = interactable;
            }
            
            if (interactable == _selectedInteractable)
            {
                return;
            }

            if (_selectedInteractable == null)
            {
                _selectedInteractable = interactable;
                _selectedInteractable.SetSelectedInteractable();
            }
            
            _selectedInteractable.SetUnselectedInteractable();
            _selectedInteractable = interactable;
            _selectedInteractable.SetSelectedInteractable();
        }
    }
}
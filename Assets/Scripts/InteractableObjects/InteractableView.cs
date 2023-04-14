using UnityEngine;

namespace Counters
{
    public class InteractableView : MonoBehaviour
    {
        [SerializeField] private GameObject _unselectedInteractable;
        [SerializeField] private GameObject _selectedInteractable;

        public void SwitchSelection(bool isSelected)
        {
            _unselectedInteractable.SetActive(!isSelected);
            _selectedInteractable.SetActive(isSelected);
        }
    }
}
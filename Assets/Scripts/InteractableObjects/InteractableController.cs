using System;
using InteractableObjects;
using UnityEngine;

namespace Counters
{
    public class InteractableController : MonoBehaviour
    {
        [SerializeField] private Interactable interactable;
        [SerializeField] private InteractableView interactableView;

        private void OnEnable()
        {
            interactable.OnSelectedInteractable += interactableView.SwitchSelection;
        }

        private void OnDisable()
        {
            interactable.OnSelectedInteractable -= interactableView.SwitchSelection;
        }
    }
}
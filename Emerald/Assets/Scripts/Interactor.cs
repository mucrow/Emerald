using System.Collections.Generic;
using UnityEngine;

namespace Emerald {
  public class Interactor: MonoBehaviour {
    readonly HashSet<Interactable> _interactables = new HashSet<Interactable>();

    public void Interact() {
      foreach (var interactable in _interactables) {
        interactable.OnInteract.Invoke();
      }
    }

    void OnTriggerEnter(Collider other) {
      var interactable = other.GetComponent<Interactable>();
      if (interactable) {
        _interactables.Add(interactable);
      }
    }

    void OnTriggerExit(Collider other) {
      var interactable = other.GetComponent<Interactable>();
      if (interactable) {
        _interactables.Remove(interactable);
      }
    }
  }
}

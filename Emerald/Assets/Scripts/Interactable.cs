using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Emerald {
  public class Interactable: MonoBehaviour {
    public UnityEvent OnInteract;
    [SerializeField] SpriteRenderer _interactCue;

    readonly List<Collider> _interactors = new List<Collider>();

    void Start() {
      UpdateInteractCue();
    }

    void OnTriggerEnter(Collider other) {
      if (other.CompareTag("Interactor")) {
        _interactors.Add(other);
        UpdateInteractCue();
      }
    }

    void OnTriggerExit(Collider other) {
      if (other.CompareTag("Interactor")) {
        _interactors.Remove(other);
        UpdateInteractCue();
      }
    }

    void UpdateInteractCue() {
      _interactCue.enabled = _interactors.Count > 0;
    }
  }
}

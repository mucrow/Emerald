using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Emerald {
  public class TriggerArea: MonoBehaviour {
    [SerializeField] UnityEvent _onTriggerEnter;
    [SerializeField] UnityEvent _onTriggerExit;

    void OnTriggerEnter(Collider other) {
      var player = other.GetComponent<Player>();
      if (player) {
        _onTriggerEnter.Invoke();
      }
    }

    void OnTriggerExit(Collider other) {
      var player = other.GetComponent<Player>();
      if (player) {
        _onTriggerExit.Invoke();
      }
    }
  }
}

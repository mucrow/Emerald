using UnityEngine;
using UnityEngine.Events;

namespace Emerald {
  public abstract class GlobalEvent: MonoBehaviour {
    [SerializeField] UnityEvent _event = new UnityEvent();

    public void AddListener(UnityAction listener) {
      _event.AddListener(listener);
    }

    public void Invoke() {
      _event.Invoke();
    }
  }
}

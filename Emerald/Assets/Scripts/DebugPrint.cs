using UnityEngine;

namespace Emerald {
  public class DebugPrint: MonoBehaviour {
    public void Print(string message) {
      Debug.Log(message, gameObject);
    }
  }
}

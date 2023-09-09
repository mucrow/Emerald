using UnityEngine;

namespace Emerald {
  public class Utils {
    /**
     * Calls GetComponent<T> on the given gameObject. If no such component exists, calls
     * AddComponent<T> on the gameObject.
     *
     * Returns true if a new component was added.
     */
    public static bool EnsureComponent<T>(GameObject gameObject, out T component) where T: Component {
      component = gameObject.GetComponent<T>();
      if (component != null) {
        return false;
      }
      component = gameObject.AddComponent<T>();
      return true;
    }
  }
}

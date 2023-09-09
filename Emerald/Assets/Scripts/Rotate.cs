using UnityEngine;

namespace Emerald {
  public class Rotate: MonoBehaviour {
    public bool YClockwise = true;
    public float CycleDuration = 2f;

    void Update() {
      float dt = Time.deltaTime;
      float direction = YClockwise ? 1f : -1f;
      var angles = transform.localEulerAngles;
      angles.y += (360f / CycleDuration) * direction * dt;
      transform.localEulerAngles = angles;
    }
  }
}

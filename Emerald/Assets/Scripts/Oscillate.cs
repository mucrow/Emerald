using UnityEngine;

namespace Emerald {
  public class Oscillate: MonoBehaviour {
    public float YPlusMinus = 0.125f;
    public float CycleDuration = 0.3f;

    float _startY;
    float _phase = 0f;

    void Start() {
      _startY = transform.localPosition.y;
    }

    void Update() {
      _phase = (_phase + Time.deltaTime / CycleDuration) % (2f * Mathf.PI);
      var newLocalPosition = transform.localPosition;
      newLocalPosition.y = _startY + YPlusMinus * Mathf.Sin(_phase);
      transform.localPosition = newLocalPosition;
    }
  }
}

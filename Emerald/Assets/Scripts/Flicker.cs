using UnityEngine;

namespace Emerald {
  [RequireComponent(typeof(Light))]
  public class Flicker: MonoBehaviour {
    Light _light;

    [SerializeField] float _slowIntensityMin = 0.3f;
    [SerializeField] float _slowIntensityMax = 0.5f;
    [SerializeField] float _slowTimerMinDuration = 3.5f;
    [SerializeField] float _slowTimerMaxDuration = 5f;

    [SerializeField] float _flickerMin = -0.03f;
    [SerializeField] float _flickerMax = 0.03f;
    [SerializeField] float _flickerMinDuration = 0.05f;
    [SerializeField] float _flickerMaxDuration = 0.1f;

    float _slowIntervalStartIntensity;
    float _slowIntervalEndIntensity;
    float _nextSlowIntervalTimerInit;
    float _nextSlowIntervalTimer;

    float _flickerIntensity;
    float _flickerTimer;

    void Awake() {
      _light = GetComponent<Light>();
      _slowIntervalEndIntensity = Random.Range(_slowIntensityMin, _slowIntensityMax);
      StartNewFlicker();
      StartNewSlowInterval();
    }

    void StartNewFlicker() {
      _flickerIntensity = Random.Range(_flickerMin, _flickerMax);
      _flickerTimer = Random.Range(_flickerMinDuration, _flickerMaxDuration);
    }

    void StartNewSlowInterval() {
      _slowIntervalStartIntensity = _slowIntervalEndIntensity;
      _slowIntervalEndIntensity = Random.Range(_slowIntensityMin, _slowIntensityMax);
      _nextSlowIntervalTimerInit = Random.Range(_slowTimerMinDuration, _slowTimerMaxDuration);
      _nextSlowIntervalTimer = _nextSlowIntervalTimerInit;
    }

    void Update() {
      float dt = Time.deltaTime;
      _nextSlowIntervalTimer -= dt;
      _flickerTimer -= dt;

      if (_flickerTimer <= 0f) {
        StartNewFlicker();
      }

      if (_nextSlowIntervalTimer <= 0f) {
        StartNewSlowInterval();
      }

      float timerProgress = Mathf.Max(0f, 1f - (_nextSlowIntervalTimerInit - _nextSlowIntervalTimer));
      var slowIntensity = ((_slowIntervalEndIntensity - _slowIntervalStartIntensity) * timerProgress) + _slowIntervalStartIntensity;

      _light.intensity = slowIntensity + _flickerIntensity;
    }
  }
}

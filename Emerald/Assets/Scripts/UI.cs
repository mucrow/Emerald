using UnityEngine;

namespace Emerald {
  public class UI: MonoBehaviour {
    [SerializeField] ShowHideUIElement _pauseMenu;
    public ShowHideUIElement PauseMenu => _pauseMenu;

    [HideInInspector] public SolidColorOverlay SolidColorOverlay;

    bool _isWholeUIReady = false;

    void Awake() {
      SolidColorOverlay = GetComponentInChildren<SolidColorOverlay>();
    }

    void Start() {
      EnsureReady();
    }

    /**
     * Ensures the UI is ready for interaction.
     *
     * Must be called in the Start() method (or a subroutine of Start()).
     *
     * The first call to this method is slow, but after the first call completes successfully,
     * subsequent calls are cheap.
     */
    public void EnsureReady() {
      if (_isWholeUIReady) {
        return;
      }
      var objects = FindObjectsByType<OnEnsureUIReady>(FindObjectsSortMode.None);
      foreach (var obj in objects) {
        obj.Invoke();
      }
      _isWholeUIReady = true;
    }
  }
}

using Emerald;
using UnityEngine;
using Input = UnityEngine.Input;

namespace Emerald {
  public class Globals: MonoBehaviour {
    // These static fields are always set by the time Start() is called.
    public static EmeraldCamera Camera;
    public static Input Input;
    public static Player Player;
    public static UI UI {
      get {
        _ui.EnsureReady();
        return _ui;
      }
    }

    static Globals _instance;
    static UI _ui;

    [SerializeField] GameObject _eventSystemPrefab;

    EmeraldCamera _camera;
    Input _input;
    Player _player;

    // AudioManager _audioManager;
    // GameManager _gameManager;
    // MtdInput _input;

    void Awake() {
      if (_instance == null) {
        DontDestroyOnLoad(gameObject);
        _instance = this;
        DoGlobalsAwake();
      }
      else {
        Destroy(gameObject);
      }
    }

    /** The part of Awake() that only runs if we are the singleton instance of Globals */
    void DoGlobalsAwake() {
      // The event system needs to be instantiated after we know this is the true Globals instance,
      // otherwise Unity will protest in the console that there can only be one event system.
      Instantiate(_eventSystemPrefab, transform);

      FindChildren();
      CallGlobalsAwakeLifecycleMethod();
    }

    void FindChildren() {
      Camera = GetComponentInChildren<EmeraldCamera>();
      Input = GetComponentInChildren<Input>();
      Player = GetComponentInChildren<Player>();
      _ui = GetComponentInChildren<UI>();
    }

    void CallGlobalsAwakeLifecycleMethod() {
      var objects = FindObjectsByType<OnGlobalsAwake>(FindObjectsSortMode.None);
      foreach (var obj in objects) {
        obj.Invoke();
      }
    }
  }
}

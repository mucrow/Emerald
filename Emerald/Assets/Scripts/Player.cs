using UnityEngine;

namespace Emerald {
  public class Player: MonoBehaviour {
    [SerializeField] float _moveSpeed = 6f;
    [SerializeField] float _moveAcceleration = 30f;
    [SerializeField] float _rotateSpeed = 540f;

    CharacterController _characterController;
    Interactor _interactor;
    Vector3 _moveVelocity;
    float _fallSpeed = 0f;

    public void OnGlobalsAwake() {
      _characterController = GetComponent<CharacterController>();
      _interactor = GetComponentInChildren<Interactor>();
    }

    void Start() {
      // TODO idk if this being here is good...
      SetPauseMenuOpen(false);
    }

    void Update() {
      Globals.Input.Poll();

      HandleMenuInput();

      if (Time.timeScale <= 0f) {
        return;
      }

      float dt = Time.deltaTime;

      HandleInteractInput();
      // HandleMoveInput(dt);
      HandleLookInput();
    }

    void OnApplicationFocus(bool hasFocus) {
      if (!hasFocus) {
        SetPauseMenuOpen(true);
      }
    }

    public async void SetPauseMenuOpen(bool isOpen) {
      if (isOpen) {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        await Globals.UI.PauseMenu.Show();
      }
      else {
        Time.timeScale = 1f;
        await Globals.UI.PauseMenu.Hide();
        Cursor.lockState = CursorLockMode.Locked;
      }
    }

    public void SetPosition(Vector3 newPosition) {
      _characterController.enabled = false;
      transform.localPosition = newPosition;
      _characterController.enabled = true;
    }

    /** Set the player's velocity to zero. Handy if you're about to teleport them. */
    public void ZeroVelocity() {
      _moveVelocity = Vector3.zero;
    }

    void HandleMenuInput() {
      if (Globals.Input.Menu) {
        SetPauseMenuOpen(true);
      }
    }

    void HandleMoveInput(float dt) {
      var moveInput = Globals.Input.Move;

      if (moveInput.magnitude < 0.1f) {
        UpdateCharacterControllerVelocity(dt, Vector3.zero);
      }
      else {
        var localRotation = transform.localRotation;
        var moveInputQuaternion = Quaternion.LookRotation(new Vector3(moveInput.x, 0f, moveInput.y), Vector3.up);
        var newRotation = Quaternion.RotateTowards(localRotation, moveInputQuaternion, _rotateSpeed * dt);
        transform.localRotation = newRotation;

        if (Quaternion.Angle(newRotation, moveInputQuaternion) < 0.5f) {
          var targetVelocity = newRotation * Vector3.forward * _moveSpeed;
          UpdateCharacterControllerVelocity(dt, targetVelocity);
        }
        else {
          UpdateCharacterControllerVelocity(dt, Vector3.zero);
        }
      }
    }

    void HandleLookInput() {
      var moveInput = Globals.Input.Look;
      var eulerAngles = transform.localEulerAngles;
      eulerAngles.x -= moveInput.y;
      eulerAngles.y += moveInput.x;
      transform.localEulerAngles = eulerAngles;
    }

    void HandleInteractInput() {
      if (Globals.Input.Interact) {
        _interactor.Interact();
      }
    }

    void UpdateCharacterControllerVelocity(float dt, Vector3 targetVelocity) {
      if (targetVelocity.y != 0f) {
        Debug.LogWarning("targetVelocity with Y-component not yet supported");
        targetVelocity.y = 0f;
      }

      float skinWidth = _characterController.skinWidth;
      var ray = new Ray(transform.position + Vector3.up * skinWidth, Vector3.down * (skinWidth + 0.001f));
      if (Physics.Raycast(ray, out RaycastHit hit, 0.2f)) {
        transform.position = hit.point;
        _fallSpeed = 0f;
      }
      else {
        _fallSpeed += Physics.gravity.y * dt;
      }

      _moveVelocity = Vector3.MoveTowards(_moveVelocity, targetVelocity, _moveAcceleration * dt);
      var motion = AdjustVelocityToSlope(_moveVelocity * dt);
      motion.y += _fallSpeed * dt;
      _characterController.Move(motion);
    }

    Vector3 AdjustVelocityToSlope(Vector3 velocity) {
      var position = transform.position;
      var playerForwardRadius = transform.forward * _characterController.radius;
      for (int i = -1; i <= 1; ++i) {
        // this loop raycasts downward from just behind the player's feet, then downward from the
        // player's bottom-center, then downward from directly in front of the player's feet.
        var zOffset = playerForwardRadius * i;
        var ray = new Ray(position + zOffset, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 0.2f)) {
          var slopeRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
          var adjustedVelocity = slopeRotation * velocity;
          if (adjustedVelocity.y < 0f) {
            return adjustedVelocity;
          }
        }
      }
      return velocity;
    }
  }
}

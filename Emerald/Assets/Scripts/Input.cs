using System;
using UnityEngine;

namespace Emerald {
  public class Input: MonoBehaviour {
    public Vector2 Move { get; private set; }
    public Vector2 Look { get; private set; }
    public bool Interact { get; private set; }
    public bool Menu { get; private set; }

    /** To be called by the consumer of this code during the consumer's Update call. */
    public void Poll() {
      Interact = UnityEngine.Input.GetKeyDown(KeyCode.F);
      Menu = UnityEngine.Input.GetKeyDown(KeyCode.Escape);
      PollMove();
      PollLook();
    }

    void PollMove() {
      var temp = Vector2.zero;

      if (UnityEngine.Input.GetKey(KeyCode.A)) {
        temp.x -= 1f;
      }
      if (UnityEngine.Input.GetKey(KeyCode.D)) {
        temp.x += 1f;
      }

      if (UnityEngine.Input.GetKey(KeyCode.S)) {
        temp.y -= 1f;
      }
      if (UnityEngine.Input.GetKey(KeyCode.W)) {
        temp.y += 1f;
      }

      Move = temp.normalized;
    }

    void PollLook() {
      var temp = Vector2.zero;
      if (Cursor.lockState == CursorLockMode.Locked) {
        temp.x = UnityEngine.Input.GetAxis("Mouse X");
        temp.y = UnityEngine.Input.GetAxis("Mouse Y");
      }
      Look = temp;
    }
  }
}

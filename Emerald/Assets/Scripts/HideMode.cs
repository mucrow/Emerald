using UnityEngine;

namespace Emerald {
  public enum HideMode {
    Fade = 0,
    SlideLeft = 1,
    SlideRight = 2,
    SlideUp = 3,
    SlideDown = 4,
  }

  public static class HideModeExtensions {
    public static bool IsDirectional(this HideMode hideMode) {
      return hideMode != HideMode.Fade;
    }

    public static Vector2 ToVector2(this HideMode hideMode) {
      if (hideMode == HideMode.SlideLeft) {
        return Vector2.left;
      }
      if (hideMode == HideMode.SlideRight) {
        return Vector2.right;
      }
      if (hideMode == HideMode.SlideDown) {
        return Vector2.down;
      }
      if (hideMode == HideMode.SlideUp) {
        return Vector2.up;
      }
      return Vector2.zero;
    }
  }
}

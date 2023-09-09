using UnityEngine;
using UnityEngine.UI;

namespace Emerald {
  [RequireComponent(typeof(ShowHideUIElement))]
  public class SolidColorOverlay: MonoBehaviour {
    public Color Color {
      get => _image.color;
      set => _image.color = value;
    }
    public ShowHideUIElement ShowHide => _showHide;

    Image _image;
    ShowHideUIElement _showHide;

    void Awake() {
      _image = GetComponent<Image>();
      _showHide = GetComponent<ShowHideUIElement>();
    }
  }
}

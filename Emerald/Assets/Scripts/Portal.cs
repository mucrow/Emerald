using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Emerald {
  public class Portal: MonoBehaviour {
    [SerializeField] SceneReference _destination;
    [SerializeField] Vector3 _positionInDestination;

    // TODO move this somewhere good
    public async void DoMapTransitionEH() {
      Time.timeScale = 0f;
      var ui = Globals.UI;
      ui.SolidColorOverlay.Color = Color.black;
      await ui.SolidColorOverlay.ShowHide.Show();
      SceneManager.LoadScene(_destination.Path);
      Globals.Player.ZeroVelocity();
      Globals.Player.SetPosition(_positionInDestination);
      Globals.Camera.SetPosition(_positionInDestination);
      await ui.SolidColorOverlay.ShowHide.Hide();
      Time.timeScale = 1f;
    }
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Emerald {
  public class EmeraldDebugInfo: MonoBehaviour {
    void Start() {
      Debug.Log("renderPipelineAsset=" + UnityEngine.Rendering.GraphicsSettings.renderPipelineAsset.GetType().Name);
    }
  }
}

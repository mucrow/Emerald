using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Emerald {
  [RequireComponent(typeof(RectTransform))]
  public class ShowHideUIElement: UIBehaviour {
    public bool IsHidden { get; private set; }

    [SerializeField] HideMode _hideMode = HideMode.Fade;
    [SerializeField] float _tweenTime = 0.3f;

    CanvasGroup _canvasGroup;
    Vector2 _initialPosition;
    bool _isReady = false;
    RectTransform _rectTransform;
    Vector2 _size = new Vector2(16, 16);


    protected override void Awake() {
      _rectTransform = GetComponent<RectTransform>();

      if (_hideMode == HideMode.Fade) {
        Utils.EnsureComponent(gameObject, out _canvasGroup);
      }

      Utils.EnsureComponent<OnEnsureUIReady>(gameObject, out var uiReadyComponent);
      uiReadyComponent.AddListener(OnEnsureUIReady);
    }

    void OnEnsureUIReady() {
      if (_isReady) {
        return;
      }
      _initialPosition = _rectTransform.anchoredPosition;
      _size = _rectTransform.rect.size;
      HideInstant();
      _isReady = true;
    }

    protected override void OnRectTransformDimensionsChange() {
      base.OnRectTransformDimensionsChange();
      if (!_rectTransform) {
        return;
      }
      _size = _rectTransform.rect.size;
      if (IsHidden) {
        HideInstant();
      }
      else {
        ShowInstant();
      }
    }

    public void ShowInstant() {
      if (_hideMode == HideMode.Fade) {
        SetCanvasAlpha(1f);
      }
      else {
        _rectTransform.anchoredPosition = _initialPosition;
      }
      IsHidden = false;
    }

    public void HideInstant() {
      if (_hideMode == HideMode.Fade) {
        SetCanvasAlpha(0f);
      }
      else {
        var hiddenPosition = GetHiddenPosition();
        _rectTransform.anchoredPosition = hiddenPosition;
      }
      IsHidden = true;
    }

    public Task Show() {
      var tcs = new TaskCompletionSource<bool>();

      LTDescr tween;
      if (_hideMode == HideMode.Fade) {
        tween = TweenCanvasAlpha(1f, _tweenTime);
      }
      else {
        tween = LeanTween.move(_rectTransform, _initialPosition, _tweenTime);
      }

      tween.setOnComplete(() => {
        tcs.SetResult(true);
      }).setIgnoreTimeScale(true);

      IsHidden = false;
      return tcs.Task;
    }

    public Task Hide() {
      var tcs = new TaskCompletionSource<bool>();

      LTDescr tween;
      if (_hideMode == HideMode.Fade) {
        tween = TweenCanvasAlpha(0f, _tweenTime);
      }
      else {
        var hiddenPosition = GetHiddenPosition();
        tween = LeanTween.move(_rectTransform, hiddenPosition, _tweenTime);
      }

      tween.setOnComplete(() => {
        tcs.SetResult(true);
      }).setIgnoreTimeScale(true);

      IsHidden = true;
      return tcs.Task;
    }

    public async void ShowEH() {
      await Show();
    }

    public async void HideEH() {
      await Hide();
    }

    Vector2 GetHiddenPosition() {
      var direction = _hideMode.ToVector2();
      var slideDistance = GetSlideDistance();
      return _initialPosition + direction * slideDistance;
    }

    public float GetSlideDistance() {
      if (_hideMode == HideMode.Fade) {
        return 0f;
      }
      if (_hideMode == HideMode.SlideLeft || _hideMode == HideMode.SlideRight) {
        return _size.x;
      }
      if (_hideMode == HideMode.SlideUp || _hideMode == HideMode.SlideDown) {
        return _size.y;
      }
      Debug.LogWarning("Unhandled HideMode: " + _hideMode);
      return 0f;
    }

    LTDescr TweenCanvasAlpha(float alpha, float time) {
      return ChangeCanvasAlphaHelper(alpha, true, time);
    }

    void SetCanvasAlpha(float alpha) {
      ChangeCanvasAlphaHelper(alpha, false, 0f);
    }

    /**
     * A helper method that helps me avoid forgetting to update _canvasGroup.blocksRaycasts. Also
     * ensures I don't forget to make the tween use the "ease out circular" curve.
     *
     * If alpha is 0, _canvasGroup.blocksRaycasts is set to false.
     * Otherwise, _canvasGroup.blocksRaycasts is set to true.
     *
     * If time is greater than 0, an LTDescr is returned.
     * Otherwise, null is returned.
     */
    LTDescr ChangeCanvasAlphaHelper(float alpha, bool returnLTDescr, float time) {
      _canvasGroup.blocksRaycasts = alpha != 0;
      if (returnLTDescr) {
        // TODO this code is disgusting
        if (alpha == 0f) {
          return LeanTween.alphaCanvas(_canvasGroup, alpha, time).setEaseInCirc();
        }
        else {
          return LeanTween.alphaCanvas(_canvasGroup, alpha, time).setEaseOutCirc();
        }
      }
      _canvasGroup.alpha = alpha;
      return null;
    }
  }
}

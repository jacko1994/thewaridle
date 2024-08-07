using UnityEngine;
using UnityEngine.Events;

public interface IUIPopup
{
    int Id { get; }
    float Duration { get; }
    float DelayToNextPopup { get; set; }
    int NextPopupId { get; set; }
    bool EnableAutoTransition { get; set; }
    bool UseInstantShow { get; set; }
    bool HideOnTransition { get; set; }

    UnityEvent OnPopupShow { get; }
    UnityEvent OnPopupHide { get; }
    UnityEvent OnResetAutoTransitionTimer { get; }

    void Show();
    void Hide();
    void ForceHide();
    void InitialHide();
    void ActivateAutoTransition();
    void StopAutoTransition();
    float GetRemainingTransitionTime();
    void ResetAutoTransitionTimer();
}

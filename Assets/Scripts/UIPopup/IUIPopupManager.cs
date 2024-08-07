using System.Collections.Generic;

public interface IUIPopupManager
{
    UIPopup CurrentVisiblePopup { get; }
    int InitialPopupID { get; set; }
    int PreviousPopupID { get; }

    void Show();
    void ShowOnlyPopup(int id);
    void ShowPopup(int id);
    void HidePopupById(int id);
    UIPopup GetCurrentVisiblePopup();
    void ShowPopupSimple(int id);
    void ShowPreviousPopup();
    int GetPreviousPopupID();
    void ShowPopupWithTransition(int id, float transitionDuration);
    void HideAllPopups();
    void SetPopupAsActive(int id);
}

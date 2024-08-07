using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopupManager : MonoBehaviour
{
    private static UIPopupManager instance;
    public static UIPopupManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIPopupManager>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<UIPopupManager>();
                    singletonObject.name = typeof(UIPopupManager).ToString() + " (Singleton)";
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private List<UIPopup> panels;
    public int initialPanelID = 0;
    private UIPopup currentVisiblePanel;
    private UIPopup panelToHide;
    private int previousPanelID = -1;

    void Start()
    {
        panels = new List<UIPopup>(FindObjectsByType<UIPopup>(FindObjectsSortMode.None));
        foreach (var panel in panels)
        {
            panel.OnRequestPanelChange += ShowPanel;
            panel.InitialHide();
        }
        ShowPanel(initialPanelID);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void ShowOnlyPanel(int id)
    {
        foreach (var panel in panels)
        {
            if (panel.id != id)
            {
                panel.Hide();
            }
            else
            {
                panel.gameObject.SetActive(true);
                panel.Show();
                currentVisiblePanel = panel;
            }
        }
    }

    public void ShowPanel(int id)
    {
        foreach (var panel in panels)
        {
            if (panel.id == id)
            {
                if (currentVisiblePanel != null && currentVisiblePanel != panel)
                {
                    currentVisiblePanel.OnMovementComplete -= HideCurrentPanel;
                    currentVisiblePanel.StopAutoTransition();

                    if (currentVisiblePanel.hideOnTransition)
                    {
                        currentVisiblePanel.gameObject.SetActive(false);
                    }

                    panelToHide = currentVisiblePanel;
                }
                panel.gameObject.SetActive(true);
                panel.Show();
                panel.OnMovementComplete += HideCurrentPanel;

                // Store the current panel ID as previous before changing it
                previousPanelID = currentVisiblePanel != null ? currentVisiblePanel.id : -1;

                currentVisiblePanel = panel;
                break;
            }
        }
    }

    public void HidePanelById(int id)
    {
        foreach (var panel in panels)
        {
            if (panel.id == id)
            {
                panel.Hide();
                break;
            }
        }
    }

    private void HideCurrentPanel(UIPopup panel)
    {
        if (panelToHide && panelToHide == panel)
        {
            panelToHide.Hide();
            panelToHide = null;
        }
    }

    public UIPopup GetCurrentVisiblePanel()
    {
        return currentVisiblePanel;
    }

    public static int GetHighestPanelId()
    {
        var panels = FindObjectsOfType<UIPopup>();
        int highestId = 0;
        foreach (var panel in panels)
        {
            if (panel.id > highestId)
            {
                highestId = panel.id;
            }
        }
        return highestId;
    }

    public static bool HasDuplicateIds()
    {
        var panels = FindObjectsOfType<UIPopup>();
        HashSet<int> ids = new HashSet<int>();
        foreach (var panel in panels)
        {
            if (!ids.Add(panel.id))
            {
                return true;
            }
        }
        return false;
    }

    public static List<UIPopup> FindPanelsWithSameId(int id)
    {
        List<UIPopup> duplicatePanels = new List<UIPopup>();
        var panels = FindObjectsOfType<UIPopup>();
        foreach (var panel in panels)
        {
            if (panel.id == id)
            {
                duplicatePanels.Add(panel);
            }
        }
        return duplicatePanels;
    }

    public void ShowPanelSimple(int id)
    {
        foreach (var panel in panels)
        {
            if (panel.id == id)
            {
                panel.gameObject.SetActive(true);
                panel.transform.localPosition = Vector3.zero;
                panel.Show();
                break;
            }
        }
    }

    public void ShowPreviousPanel()
    {
        if (previousPanelID != -1)
        {
            ShowPanel(previousPanelID);
        }
        else
        {
            Debug.LogWarning("No previous panel to show.");
        }
    }

    public int GetPreviousPanelID()
    {
        return previousPanelID;
    }
    public void ShowPopupWithTransition(int id, float transitionDuration)
    {
        foreach (var panel in panels)
        {
            if (panel.id == id)
            {
                StartCoroutine(ShowPopupWithTransitionCoroutine(panel, transitionDuration));
                break;
            }
        }
    }

    private IEnumerator ShowPopupWithTransitionCoroutine(UIPopup panel, float duration)
    {
        if (currentVisiblePanel != null)
        {
            currentVisiblePanel.Hide();
        }

        panel.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        panel.Show();
        currentVisiblePanel = panel;
    }

    public void HideAllPopups()
    {
        foreach (var panel in panels)
        {
            panel.Hide();
        }
    }

    public void SetPopupAsActive(int id)
    {
        foreach (var panel in panels)
        {
            if (panel.id == id)
            {
                if (currentVisiblePanel != null && currentVisiblePanel != panel)
                {
                    currentVisiblePanel.Hide();
                }

                panel.gameObject.SetActive(true);
                currentVisiblePanel = panel;
                panel.Show();
                break;
            }
        }
    }
}

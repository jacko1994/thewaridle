using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    public Text totalCrownsText;
    public Button homeButton; 

    void OnEnable()
    {
        UpdateTotalCrownsDisplay();

        homeButton.onClick.AddListener(OnHomeButtonClicked);
    }
    private void OnDisable()
    {
        homeButton.onClick.RemoveListener(OnHomeButtonClicked);

    }
    private void UpdateTotalCrownsDisplay()
    {
        int totalCrowns = TheWarIdleManager.Instance.GetCurrentCrowns();
        totalCrownsText.text = "Crown: " + totalCrowns.ToString();
    }

    private void OnHomeButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

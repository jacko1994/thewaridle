using UnityEngine;
using MoreMountains.Feedbacks;

public class CharacterFeedbackManager : MonoBehaviour
{
    // Các feedback cần thiết
    [Header("Attack Feedback")]
    public MMFeedbacks attackFeedback;

    [Header("Take Damage Feedback")]
    public MMFeedbacks takeDamageFeedback;

    [Header("Die Feedback")]
    public MMFeedbacks dieFeedback;

    // Phương thức để kích hoạt feedback khi tấn công
    public void PlayAttackFeedback()
    {
        attackFeedback?.PlayFeedbacks();
    }

    // Phương thức để kích hoạt feedback khi nhận sát thương
    public void PlayTakeDamageFeedback()
    {
        takeDamageFeedback?.PlayFeedbacks();
    }

    // Phương thức để kích hoạt feedback khi chết
    public void PlayDieFeedback()
    {
        dieFeedback?.PlayFeedbacks();
    }
}

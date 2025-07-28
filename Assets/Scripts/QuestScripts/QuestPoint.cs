using UnityEngine;

public class QuestPoint : MonoBehaviour
{
    [Header("Quest")]
    public QuestInfoSO questInfoForPoint;
    public string questId;
    public QuestState currentQuestState;

    public void Awake()
    {
        questId = questInfoForPoint.id;

    }
    public void OnEnable()
    {
        if (GameEventsManager.Instance != null)
        {
            Debug.Log($"[QuestPoint] Subscribing to quest state changes for quest {questId}");
            GameEventsManager.Instance.questEvents.onQuestStateChange += QuestStateChange;
        }
    }
    public void OnDisable()
    {
        if (GameEventsManager.Instance != null)
        {
            GameEventsManager.Instance.questEvents.onQuestStateChange -= QuestStateChange;
        }

    }
    public void QuestStateChange(Quest quest)
    {
        Debug.Log($"[QuestPoint] Received quest state change for {quest.info.id} (Expected: {questId})");

    if (quest.info.id == questId)
    {
        currentQuestState = quest.state;
        Debug.Log($"[QuestPoint] Quest state updated to: {currentQuestState}");
    }
    }
}

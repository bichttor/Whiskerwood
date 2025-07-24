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
        if (quest.info.id == questId)
        {
            currentQuestState = quest.state;
            Debug.Log($"Quest state changed for {questId}: {currentQuestState}");
        }
    }
}

using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public string npcName;
    public QuestPoint questPoint;
    
    public void Interact()
    {
        if(questPoint == null)
        {
            Debug.LogWarning("No quest point assigned to this NPC.");
            return;
        }
        if (questPoint != null && questPoint.currentQuestState == QuestState.CAN_START)
        {
            // Start the quest if it hasn't been started yet
            Debug.Log($"Starting quest: {questPoint.questId}");
            GameEventsManager.Instance.questEvents.StartQuest(questPoint.questId);
        }
        else if (questPoint != null && questPoint.currentQuestState == QuestState.CAN_FINISH)
        {
            GameEventsManager.Instance.questEvents.CompleteQuest(questPoint.questId);
            Debug.Log($"Completing quest: {questPoint.questId}");
        }

    }
}

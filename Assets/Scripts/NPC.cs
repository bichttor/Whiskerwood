using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public QuestPoint questPoint;
    public Dialogue dialogue;
    public void Interact()
    {
        //No quest point assigned, just start dialogue
        if (questPoint == null)
        {
            FindFirstObjectByType<DialogueManager>().StartDialogue(dialogue);
        }
        else if (questPoint != null && questPoint.currentQuestState == QuestState.CAN_START)
        {
            //If quest point is assigned and the quest can be started, start the quest
            Debug.Log($"Starting quest: {questPoint.questId}");
            FindFirstObjectByType<DialogueManager>().StartDialogue(dialogue);
            GameEventsManager.Instance.questEvents.StartQuest(questPoint.questId);
        }
         else if (questPoint != null && questPoint.currentQuestState == QuestState.CAN_FINISH)
        {
            //If quest point is assigned and the quest can be finished, finish the quest
            FindFirstObjectByType<DialogueManager>().FinishQuestDialogue(dialogue);
            GameEventsManager.Instance.questEvents.CompleteQuest(questPoint.questId);
            Debug.Log($"Completing quest: {questPoint.questId}");
        }

    }
}

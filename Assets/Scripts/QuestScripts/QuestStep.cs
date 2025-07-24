using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    public bool isFinished = false;
    public string questId;

    public void InitalizeQuestStep(string id)
    {
        this.questId = id;
    }
    public void FinishQuestStep()
    {
        if (!isFinished)
        {
            isFinished = true;
            GameEventsManager.Instance.questEvents.AdvanceQuest(questId);
            Destroy(this.gameObject);
        }


    }
}

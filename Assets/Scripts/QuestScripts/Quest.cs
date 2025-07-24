using UnityEngine;

public class Quest
{
    public QuestInfoSO info;

    public QuestState state;

    public int currentQuestStepIndex;

    public Quest(QuestInfoSO info)
    {
        this.info = info;
        state = QuestState.REQUIREMENTS_NOT_MET;
        currentQuestStepIndex = 0;
    }

    public void MoveToNextStep()
    {
        if (currentQuestStepIndex < info.questSteps.Length)
        {
            currentQuestStepIndex++;
        }

    }
    public bool CurrentStepExists()
    {
        return currentQuestStepIndex < info.questSteps.Length;
    }
    public void InstantiateCurrentQuestStep(Transform parentTransform)
    {
        GameObject questStepObject = GetCurrentQuestStepGameObject();
        if (questStepObject != null)
        {
            QuestStep queststep = GameObject.Instantiate(questStepObject, parentTransform).GetComponent<QuestStep>();
            queststep.InitalizeQuestStep(info.id);
        }
    }
    public GameObject GetCurrentQuestStepGameObject()
    {
        if (CurrentStepExists())
        {
            return info.questSteps[currentQuestStepIndex].gameObject;
        }
        return null;
    }
}

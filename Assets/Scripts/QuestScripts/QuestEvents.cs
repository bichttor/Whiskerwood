using UnityEngine;
using System;
public class QuestEvents
{
    public event Action<string> onStartQuest;
    public event Action<string> onAdvanceQuest;
    public event Action<string> onCompleteQuest;
    public event Action<Quest> onQuestStateChange;
    public void StartQuest(string id)
    {
        if (onStartQuest != null)
        {
            onStartQuest.Invoke(id);
        }
    }

    public void AdvanceQuest(string id)
    {
        if (onAdvanceQuest != null)
        {
            onAdvanceQuest.Invoke(id);
        }
    }
    public void CompleteQuest(string id)
    {
        if (onCompleteQuest != null)
        {
            onCompleteQuest.Invoke(id);
        }
    }

    public void ChangeQuestState(Quest quest)
    {
        if (onQuestStateChange != null)
        {
            onQuestStateChange.Invoke(quest);
        }
    }
}

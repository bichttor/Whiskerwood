using UnityEngine;
using System.Collections.Generic;
public class QuestManager : MonoBehaviour
{
    public Dictionary<string, Quest> questMap;
    public int currentPlayerLevel;
    public void Awake()
    {
        questMap = CreateQuestMap();
    }

    public void onEnable()
    {
        if (GameEventsManager.Instance != null)
        {
            GameEventsManager.Instance.questEvents.onStartQuest += StartQuest;
            GameEventsManager.Instance.questEvents.onAdvanceQuest += AdvanceQuest;
            GameEventsManager.Instance.questEvents.onCompleteQuest += CompleteQuest;
            GameEventsManager.Instance.OnPlayerLevelUp += PlayerLevelChange;
        }
    }

    public void onDisable()
    {
        if (GameEventsManager.Instance != null)
        {
            GameEventsManager.Instance.questEvents.onStartQuest -= StartQuest;
            GameEventsManager.Instance.questEvents.onAdvanceQuest -= AdvanceQuest;
            GameEventsManager.Instance.questEvents.onCompleteQuest -= CompleteQuest;
            GameEventsManager.Instance.OnPlayerLevelUp -= PlayerLevelChange;
        }
    }

    public void Start()
    {
        foreach(Quest quest in questMap.Values)
        {
           GameEventsManager.Instance.questEvents.ChangeQuestState(quest);
        }
    }
    //FIXME MAYBE
    public void PlayerLevelChange()
    {
        //currentPlayerLevel = level;
       // Debug.Log($"Player level changed to: {currentPlayerLevel}");
    }
    public bool CheckRequirementsMet(Quest quest)
    {
        bool meetsRequirements = true;
        if (currentPlayerLevel < quest.info.requiredLevel)
        {
            meetsRequirements = false;
        }
        foreach (QuestInfoSO prereq in quest.info.questPrerequisites)
        {
            if (GetQuestById(prereq.id).state != QuestState.FINISHED)
            {
                meetsRequirements = false;
            }
        }
        return meetsRequirements;
    }
    public void Update()
    {
        foreach (Quest quest in questMap.Values)
        {
            if(quest.state == QuestState.REQUIREMENTS_NOT_MET && CheckRequirementsMet(quest))
            {
              ChangeQuestState(quest.info.id, QuestState.CAN_START);
            }
        }
    }
    public void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuestById(id);
        quest.state = state;
        GameEventsManager.Instance.questEvents.ChangeQuestState(quest);
    }
    public void StartQuest(string id)
    {
        Quest quest = GetQuestById(id);
        quest.InstantiateCurrentQuestStep(this.transform);
        ChangeQuestState(id, QuestState.IN_PROGRESS);
        Debug.Log($"Starting quest with ID: {id}");
    }
    public void AdvanceQuest(string id)
    {
        Quest quest = GetQuestById(id);
        quest.MoveToNextStep();
        if(quest.CurrentStepExists())
        {
            quest.InstantiateCurrentQuestStep(this.transform);
       
        }
        else
        {
            ChangeQuestState(id, QuestState.CAN_FINISH);
        }
        Debug.Log($"Advancing quest with ID: {id}");
    }
    public void CompleteQuest(string id)
    {
        Quest quest = GetQuestById(id);
        ClaimRewards(quest);
        ChangeQuestState(id, QuestState.FINISHED);
        Debug.Log($"Completing quest with ID: {id}");
    }
    public void ClaimRewards(Quest quest)
    {
        /*MAKE EVENTS*/
       // GameEventsManager.Instance.OnBottleCapsGained(quest.info.bottleCapsReward);
        // GameEventsManager.Instance.OnExperienceGained(quest.info.experienceReward);
        Debug.Log($"Claiming rewards for quest: {quest.info.id}");
       
    }
    public Dictionary<string, Quest> CreateQuestMap()
    {
        QuestInfoSO[] questInfos = Resources.LoadAll<QuestInfoSO>("Quests");

        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();
        foreach (QuestInfoSO questInfo in questInfos)
        {
            Debug.Log($"Loading quest: {questInfo.id}");
            if (idToQuestMap.ContainsKey(questInfo.id))
            {
                Debug.LogError($"Duplicate quest ID found: {questInfo.id}. Please ensure all quest IDs are unique.");
                continue;
            }
            idToQuestMap.Add(questInfo.id, new Quest(questInfo));
        }
        return idToQuestMap;
    }

    public Quest GetQuestById(string id)
    {
        Quest quest = questMap[id];
        if (quest == null)
        {
            Debug.LogError($"Quest with ID {id} not found.");

        }
        return quest;
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "QuestInfoSO", menuName = "Scriptable Objects/QuestInfoSO")]
public class QuestInfoSO : ScriptableObject
{
    public string id;
    [Header("Quest Details")]
    public string displayName;

    [Header("Quest requirements")]
    public int levelRequirement;
    public QuestInfoSO[] questPrerequisites;
    [Header("Quest Steps")]
    public GameObject[] questSteps;
    [Header("Quest Rewards")]
    public int experienceReward;
    public int bottleCapsReward;

}

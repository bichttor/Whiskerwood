using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public QuestPoint questPoint;
    public Dialogue dialogue;
    public float walkPointRange;
    public bool walkPointSet, wanderingNPC;
    public Vector3 walkPoint;
    public LayerMask groundMask;
     public UnityEngine.AI.NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }
    void Update()
    {
        if (wanderingNPC)
        {
            Walking();
        }
        
    }
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
    public void Walking()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }
        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }
         Vector3 distanceToPoint = transform.position - walkPoint;
        if (distanceToPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }
     public void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, Vector3.down, 2f, groundMask))
        {
            walkPointSet = true;
        }
    }
}

using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public QuestPoint questPoint;
    public Dialogue dialogue;
    public float walkPointRange;
    public bool walkPointSet;
    public bool wanderingNPC = false;
    public Vector3 walkPoint;
    public LayerMask groundMask;
    public UnityEngine.AI.NavMeshAgent agent;
    public AnimationStateChanger animationStateChanger;
    public bool isPaused = false;
    public float pauseTimer = 0f;
    public float pauseDuration = 2f;
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
        if (!walkPointSet && !isPaused)
        {
            animationStateChanger.ChangeState("Breathing Idle", 1f);
            SearchWalkPoint();
            return;
        }

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);

            Vector3 distanceToPoint = transform.position - walkPoint;
            if (distanceToPoint.magnitude < 1f)
            {
                if (!isPaused)
                {
                    isPaused = true;
                    pauseTimer = pauseDuration;
                    agent.ResetPath(); 
                    animationStateChanger.ChangeState("Breathing Idle", 1f);
                }
            }
            else
            {
                if (agent.velocity.magnitude > 0.1f)
                {
                    animationStateChanger.ChangeState("Walking", 1f);
                }
            }
        }

        if (isPaused)
        {
            pauseTimer -= Time.deltaTime;
            if (pauseTimer <= 0f)
            {
                isPaused = false;
                walkPointSet = false;
            }
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

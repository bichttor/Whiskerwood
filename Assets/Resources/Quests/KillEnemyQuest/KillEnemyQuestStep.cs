using UnityEngine;

public class KillEnemyQuestStep : QuestStep
{
    public int enemiesToKill = 6;
    public int enemiesKilled = 0;

    public void OnEnable()
    {
        if (GameEventsManager.Instance != null)
        {
            GameEventsManager.Instance.OnEnemyKilled += OnEnemyKilled;
        }
    }
    public void OnDisable()
    {
        if (GameEventsManager.Instance != null)
        {
            GameEventsManager.Instance.OnEnemyKilled -= OnEnemyKilled;
        }
    }
    public void OnEnemyKilled(float experience, int bottlecaps)
    {
        Debug.Log($"Enemy killed: Experience={experience}, BottleCaps={bottlecaps}");
        enemiesKilled++;
        if (enemiesKilled >= enemiesToKill)
        {
            FinishQuestStep();
        }
    }
}

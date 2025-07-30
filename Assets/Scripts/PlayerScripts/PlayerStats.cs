using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerStats : MonoBehaviour
{

    public float maxHealth, currentHealth;
    public StatsBar healthBar, staminaBar, experienceBar;
    public HUD hud;
    public float currentStamina, maxStamina;
    public float damage = 5f; // base damage with no weapons
    public float currentDamage; //for weapons 
    public int currentLevel = 1;
    public float currentExperience = 0;
    public float nextLevelsExperience = 200;
    private Coroutine rechargeCoroutine;
    public bool isRecharging = false;
    public float chargeRate = 30;
    public AudioSource audioSource;
    public AudioClip takeDamageSound;
    public AudioClip levelUpSound;
    public int BottleCaps;

    void Start()
    {
        currentHealth = maxHealth;
        hud.healthBar.SetSliderMax(maxHealth);

        currentStamina = maxStamina;
        hud.staminaBar.SetSliderMax(maxStamina);

        currentDamage = damage;
        hud.experienceBar.SetSliderMax(nextLevelsExperience);
        hud.experienceBar.SetSlider(currentExperience);
        hud.SetBottleCaps(BottleCaps);
        hud.SetLevel(currentLevel);
    }
    void OnEnable()
    {
        if (GameEventsManager.Instance != null)
        {
            GameEventsManager.Instance.OnPlayerDamaged += TakeDamage;
            GameEventsManager.Instance.OnPlayerHealed += Heal;
            GameEventsManager.Instance.OnEnemyKilled += HandleEnemyKilled;
            GameEventsManager.Instance.OnPlayerLevelUp += UpdateLevel;
            GameEventsManager.Instance.OnBottleCapsGained += AddBottleCaps;
            GameEventsManager.Instance.OnExperienceGained += AddExperience;
        }
    }

    void OnDisable()
    {
        if (GameEventsManager.Instance != null)
        {
            GameEventsManager.Instance.OnPlayerDamaged -= TakeDamage;
            GameEventsManager.Instance.OnPlayerHealed -= Heal;
            GameEventsManager.Instance.OnEnemyKilled -= HandleEnemyKilled;
            GameEventsManager.Instance.OnPlayerLevelUp -= UpdateLevel;
            GameEventsManager.Instance.OnBottleCapsGained -= AddBottleCaps;
            GameEventsManager.Instance.OnExperienceGained -= AddExperience;
        }
    }
    public void HandleEnemyKilled(float xp, int bottlecaps)
    {
        AddExperience(xp);
        AddBottleCaps(bottlecaps);
    }
    /*METHODS FOR HEALTH*/
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            //dead
            Debug.Log("DEAD");
        }
        PlaySFX(takeDamageSound);
        hud.healthBar.SetSlider(currentHealth);
    }

    public void Heal(float heal)
    {
        currentHealth += heal;
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        hud.healthBar.SetSlider(currentHealth);
    }
    /*METHODS FOR STAMINA*/
    public void SpendStamina(float amount)
    {
        currentStamina -= amount;
        if (currentStamina < 0)
            currentStamina = 0;

        hud.staminaBar.SetSlider(currentStamina);

        if (rechargeCoroutine != null)
        {
            StopCoroutine(rechargeCoroutine);
        }

        rechargeCoroutine = StartCoroutine(RechargeStamina());
    }

    public void AddStamina(float stamina)
    {
        currentStamina += stamina;
        if (currentStamina >= maxStamina)
        {
            currentStamina = maxStamina;
        }
        hud.staminaBar.SetSlider(currentStamina);
    }
    public IEnumerator RechargeStamina()
    {
        isRecharging = true;
        yield return new WaitForSeconds(1.5f);

        while (currentStamina < maxStamina)
        {
            currentStamina += chargeRate * Time.deltaTime;
            if (currentStamina > maxStamina)
            {
                currentStamina = maxStamina;

            }
            staminaBar.SetSlider(currentStamina);
            yield return null;
        }
        isRecharging = false;
    }
    /*METHODS FOR EXPERIENCE*/
    public void AddExperience(float amount)
    {
        currentExperience += amount;
        CheckForLevelUp();
        hud.experienceBar.SetSlider(currentExperience);
    }
    public void CheckForLevelUp()
    {
        if (currentExperience >= nextLevelsExperience)
        {
            UpdateLevel();
            PlaySFX(levelUpSound);

        }
    }
    public void UpdateLevel()
    {
        currentLevel++;
        nextLevelsExperience *= 1.2f;
        currentExperience = 0;

        maxHealth += 10; // Increase max health on level up
        maxStamina += 5; // Increase max stamina on level up
        currentHealth = maxHealth; // Restore health on level up
        currentStamina = maxStamina; // Restore stamina on level up
        hud.SetLevel(currentLevel);
        hud.experienceBar.SetSliderMax(nextLevelsExperience);
        hud.experienceBar.SetSlider(currentExperience);
    }
    /*METHODS FOR BOTTLE CAPS*/
    public void AddBottleCaps(int amount)
    {
        BottleCaps += amount;
        hud.SetBottleCaps(BottleCaps);
    }

    public void SpendBottleCaps(int amount)
    {
        BottleCaps -= amount;
        hud.SetBottleCaps(BottleCaps);
    }
    public void PlaySFX(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerStats : MonoBehaviour
{

    public float maxHealth,currentHealth; 
    public StatsBar healthBar, staminaBar, experienceBar;
    public float currentStamina, maxStamina;
    public float damage = 5f; // base damage with no weapons
    public float currentDamage; //for weapons 
    public int currentLevel = 1;
    public float currentExperience = 0;
    public float nextLevelsExperience = 200;
    private Coroutine rechargeCoroutine;
    public bool isRecharging = false;
    public float chargeRate = 30;
    public AudioSource takedamgeSFX;
    public AudioSource LevelUpSFX;
    public int BottleCaps;
    public void PlayTakeDamage() => takedamgeSFX.Play();
    public void PlayLevelUp() => LevelUpSFX.Play();
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetSliderMax(maxHealth);

        currentStamina = maxStamina;
        staminaBar.SetSliderMax(maxStamina);

        currentDamage = damage;

        experienceBar.SetSliderMax(nextLevelsExperience);
        experienceBar.SetSlider(currentExperience);
    }
     void OnEnable()
    {
        if (GameEventsManager.Instance != null)
        {
            GameEventsManager.Instance.OnPlayerDamaged += TakeDamage;
            GameEventsManager.Instance.OnPlayerHealed += Heal;
            GameEventsManager.Instance.OnEnemyKilled += HandleEnemyKilled;
            GameEventsManager.Instance.OnPlayerLevelUp += UpdateLevel;
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
        }
    }
      private void HandleEnemyKilled(float xp, int bottlecaps)
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
        // PlayTakeDamage();
        healthBar.SetSlider(currentHealth);
    }

    public void Heal(float heal)
    {
        currentHealth += heal;
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthBar.SetSlider(currentHealth);
    }
    /*METHODS FOR STAMINA*/
    public void SpendStamina(float amount)
    {
        currentStamina -= amount;
        if (currentStamina < 0)
            currentStamina = 0;

        staminaBar.SetSlider(currentStamina);

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
        staminaBar.SetSlider(currentStamina);
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
        experienceBar.SetSlider(currentExperience);
    }
        public void CheckForLevelUp()
    {
        if (currentExperience >= nextLevelsExperience)
        {
            currentLevel++;
            UpdateLevel();
           // PlayLevelUp();

        }
    }
    public void UpdateLevel()
    {
        nextLevelsExperience *= 1.2f;
        currentExperience = 0;
        experienceBar.SetSliderMax(nextLevelsExperience);
        experienceBar.SetSlider(currentExperience);
    }
    /*METHODS FOR BOTTLE CAPS*/
    public void AddBottleCaps(int amount)
    {
        BottleCaps += amount;
    }

    public void SpendBottleCaps(int amount)
    {
        BottleCaps -= amount;
    }
}

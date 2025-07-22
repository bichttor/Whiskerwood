using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerStats : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

    // Update is called once per frame
    void Update()
    {

    }

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

    public void AddExperience(float amount)
    {
        currentExperience += amount;
        CheckForLevelUp();
        experienceBar.SetSlider(currentExperience);
    }
    public void AddBottleCaps(int amount)
    {
        BottleCaps += amount;
    }

    public void SpendBottleCaps(int amount)
    {
        BottleCaps -= amount;
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
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HUD : MonoBehaviour
{
    public StatsBar healthBar, staminaBar, experienceBar;
    public TMP_Text bottleCapsText;
    public TMP_Text levelText;

    public void SetBottleCaps(int amount)
    {
        if (bottleCapsText != null)
        {
            bottleCapsText.text = "Bottle Caps: " + amount.ToString();
        }
    }
    public void SetLevel(int level)
    {
        if (levelText != null)
        {
            levelText.text = "Level: " + level.ToString();
        }
    }
    
}

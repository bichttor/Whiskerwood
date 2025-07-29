using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StatsBar : MonoBehaviour
{
    public Slider slider;
    public TMP_Text textAmount;
    public void SetSlider(float amount)
    {
        slider.value = amount;
        textAmount.text = amount.ToString();
    }

    public void SetSliderMax(float amount)
    {

        slider.maxValue = amount;
        textAmount.text = amount.ToString();
        SetSlider(amount);
    }
    
}

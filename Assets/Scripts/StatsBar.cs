using UnityEngine;
using UnityEngine.UI;

public class StatsBar : MonoBehaviour
{
    public Slider slider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetSlider(float amount)
    {
        slider.value = amount;
    }

    public void SetSliderMax(float amount)
    {
        slider.maxValue = amount;
        SetSlider(amount);
    }
}

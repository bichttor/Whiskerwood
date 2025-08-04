using UnityEngine;

public class UIWindow : MonoBehaviour
{
    Canvas canvas;
    void Awake()
    {
        canvas = GetComponent<Canvas>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Show()
    {
        canvas.enabled = true;
    }
    public void Hide()
    {
        canvas.enabled = false;
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderToValue : MonoBehaviour 
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _sliderText;
    [SerializeField] private bool percentage;


    void Start()
    {
        _slider.onValueChanged.AddListener((v) => _sliderText.text = BuildString(v));
    }

    private string BuildString(float value)
    {
        if (percentage)
        {
            return (value / _slider.maxValue * 100).ToString("0") + "%";
        }
        
        if (!_slider.wholeNumbers)
        {
            return value.ToString("0.00");
        }
        
        return value.ToString();
    }

    // Kostyl` blyat
    public static void updateManually(Slider _slider, bool percentage)
    {
        _slider.GetComponentInChildren<TextMeshProUGUI>().text = (percentage ? (_slider.value / _slider.maxValue * 100).ToString("0") + "%" : _slider.value.ToString("0.00"));
    }
}

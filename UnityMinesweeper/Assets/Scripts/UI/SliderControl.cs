using UnityEngine;
using UnityEngine.UI;

public class SliderControl : MonoBehaviour
{
    [SerializeField] private Slider widthSlider;
    [SerializeField] private Slider heightSlider;
    [SerializeField] private Slider minesSlider;


    public void ControlValues()
    {
        int area = (int) (widthSlider.value * heightSlider.value);
        minesSlider.maxValue = Mathf.Min(area - 9, 999);
        if (minesSlider.value > area - 9) { minesSlider.value = area - 9; }
    }
}

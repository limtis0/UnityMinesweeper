using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{

    [SerializeField] private Slider widthSlider;
    [SerializeField] private Slider heightSlider;
    [SerializeField] private Slider minesSlider;


    public void PlayGame()
    {
        PlayerPrefs.SetInt("fieldWidth", (int) widthSlider.value);
        PlayerPrefs.SetInt("fieldHeight", (int) heightSlider.value);
        PlayerPrefs.SetInt("mineCount", (int) minesSlider.value);

        SceneManager.LoadScene("Minesweeper");
    }

    public void SetBeginnerPreset()
    {
        widthSlider.value = 9;
        heightSlider.value = 9;
        minesSlider.value = 10;
    }

    public void SetIntermediatePreset()
    {
        widthSlider.value = 16;
        heightSlider.value = 16;
        minesSlider.value = 40;
    }

    public void SetExpertPreset()
    {
        widthSlider.value = 30;
        heightSlider.value = 16;
        minesSlider.value = 99;
    }

    public void SetRandomPreset()
    {
        int w = Random.Range(5, 80);
        int h = Random.Range(5, 80);
        int area = w * h;

        widthSlider.value = w;
        heightSlider.value = h;
        minesSlider.value = Mathf.Min(Random.Range((int) (area * 0.1), (int) (area * 0.4)), 999);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private Slider mouseSensivitySlider;
    [SerializeField] private Slider screenBorderSlider;
    [SerializeField] private Slider mouseCamSpeedSlider;
    [SerializeField] private Slider cameraZoomSpeedSlider;
    [SerializeField] private Slider keyboardCamSpeedSlider;
    [SerializeField] private Slider musicSlider; 
    [SerializeField] private Slider sfxSlider;

    [SerializeField] private Toggle invertedToggle;
    [SerializeField] private Toggle nightThemeToggle;


    void Start()
    {
        loadSettings();
        updateSliders();
    }

    public void saveSettings()
    {
        PlayerPrefs.SetFloat("mouseSensivity", mouseSensivitySlider.value);
        PlayerPrefs.SetFloat("screenBorderSize", screenBorderSlider.value);
        PlayerPrefs.SetFloat("cameraMouseMovementSpeed", mouseCamSpeedSlider.value);
        PlayerPrefs.SetFloat("cameraZoomSpeed", cameraZoomSpeedSlider.value);
        PlayerPrefs.SetFloat("cameraKeyboardMovementSpeed", keyboardCamSpeedSlider.value);
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("sfxVolume", sfxSlider.value);

        PlayerPrefs.SetInt("invertedControls", invertedToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("nightTheme", nightThemeToggle.isOn ? 1 : 0);
    } 

    public void loadSettings()
    {
        mouseSensivitySlider.value = PlayerPrefs.GetFloat("mouseSensivity", 1.0f);
        screenBorderSlider.value = PlayerPrefs.GetFloat("screenBorderSize", 0.03f);
        mouseCamSpeedSlider.value = PlayerPrefs.GetFloat("cameraMouseMovementSpeed", 0.15f);
        cameraZoomSpeedSlider.value = PlayerPrefs.GetFloat("cameraZoomSpeed", 0.3f);
        keyboardCamSpeedSlider.value = PlayerPrefs.GetFloat("cameraKeyboardMovementSpeed", 10f);
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 0.25f);
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 0.4f);

        invertedToggle.isOn = PlayerPrefs.GetInt("invertedControls", 0) == 1;
        nightThemeToggle.isOn = PlayerPrefs.GetInt("nightTheme", 0) == 1;
    }

    private void updateSliders()
    {
        SliderToValue.updateManually(mouseSensivitySlider, false);
        SliderToValue.updateManually(screenBorderSlider, true);
        SliderToValue.updateManually(mouseCamSpeedSlider, true);
        SliderToValue.updateManually(cameraZoomSpeedSlider, true);
        SliderToValue.updateManually(keyboardCamSpeedSlider, true);
        SliderToValue.updateManually(musicSlider, true);
        SliderToValue.updateManually(sfxSlider, true);
    }
}

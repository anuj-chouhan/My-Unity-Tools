using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Anuj.Utility.Ui
{
    public class UISettingsManager : MonoBehaviour
    {
        [Header("Audio Mixer & UI Elements")]
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private Slider sliderMaster;
        [SerializeField] private Slider sliderMusic;
        [SerializeField] private Slider sliderVolume;
        [SerializeField] private Slider sliderSoundEffects;
        [SerializeField] private Toggle toggleMaster;
        [SerializeField] private Toggle toggleMusic;
        [SerializeField] private Toggle toggleVolume;
        [SerializeField] private Toggle toggleSoundEffects;
        [SerializeField] private TMP_Dropdown dropdownQuality;
        [SerializeField] private Button buttonReset;
        [SerializeField] private Button buttonDeleteSave;

        //This should match in audio mixer
        private string _audioMaster = "Master";
        private string _audioGroupMusic = "Music";
        private string _audioGroupSFX = "SFX";
        private string _audioGroupUI = "UI";

        //Player Prefs
        private const string MASTER_KEY = "volume_master";
        private const string MUSIC_KEY = "volume_music";
        private const string UI_KEY = "volume_ui";
        private const string SFX_KEY = "volume_sfx";
        private const string MUTE_MASTER_KEY = "mute_master";
        private const string MUTE_MUSIC_KEY = "mute_music";
        private const string MUTE_UI_KEY = "mute_ui";
        private const string MUTE_SFX_KEY = "mute_sfx";
        private const string QUALITY_KEY = "quality_index";

        private void Start()
        {
            AddListener();
            LoadSettings();
        }

        private void AddListener()
        {
            // Setup event listeners
            sliderMaster.onValueChanged.AddListener((v) => OnAudioSliderChange(v, _audioMaster, MASTER_KEY));
            sliderMusic.onValueChanged.AddListener((v) => OnAudioSliderChange(v, _audioGroupMusic, MUSIC_KEY));
            sliderVolume.onValueChanged.AddListener((v) => OnAudioSliderChange(v, _audioGroupUI, UI_KEY));
            sliderSoundEffects.onValueChanged.AddListener((v) => OnAudioSliderChange(v, _audioGroupSFX, SFX_KEY));

            toggleMaster.onValueChanged.AddListener((b) => OnAudioToggleChange(b, _audioMaster, MUTE_MASTER_KEY));
            toggleMusic.onValueChanged.AddListener((b) => OnAudioToggleChange(b, _audioGroupMusic, MUTE_MUSIC_KEY));
            toggleVolume.onValueChanged.AddListener((b) => OnAudioToggleChange(b, _audioGroupUI, MUTE_UI_KEY));
            toggleSoundEffects.onValueChanged.AddListener((b) => OnAudioToggleChange(b, _audioGroupSFX, MUTE_SFX_KEY));

            dropdownQuality.onValueChanged.AddListener(OnQualityChange);
            buttonReset.onClick.AddListener(ResetToDefaults);
            buttonDeleteSave.onClick.AddListener(DeleteAllSaves);
        }

        private void OnAudioSliderChange(float value, string group, string key)
        {
            float volume = FloatToDecibels(value);
            audioMixer.SetFloat(group, volume);
            PlayerPrefs.SetFloat(key, value);
        }

        private void OnAudioToggleChange(bool isOn, string group, string key)
        {
            audioMixer.SetFloat(group, isOn ? 0 : -80);
            PlayerPrefs.SetInt(key, isOn ? 1 : 0);
        }
        private float FloatToDecibels(float Value)
        {
            return Mathf.Log10(Mathf.Max(Value, 0.0001f)) * 20f;
        }
        private void OnQualityChange(int index)
        {
            QualitySettings.SetQualityLevel(index);
            PlayerPrefs.SetInt(QUALITY_KEY, index);
        }

        private void LoadSettings()
        {
            sliderMaster.value = PlayerPrefs.GetFloat(MASTER_KEY, 1f);
            sliderMusic.value = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
            sliderVolume.value = PlayerPrefs.GetFloat(UI_KEY, 1f);
            sliderSoundEffects.value = PlayerPrefs.GetFloat(SFX_KEY, 1f);

            toggleMaster.isOn = PlayerPrefs.GetInt(MUTE_MASTER_KEY, 1) == 1;
            toggleMusic.isOn = PlayerPrefs.GetInt(MUTE_MUSIC_KEY, 1) == 1;
            toggleVolume.isOn = PlayerPrefs.GetInt(MUTE_UI_KEY, 1) == 1;
            toggleSoundEffects.isOn = PlayerPrefs.GetInt(MUTE_SFX_KEY, 1) == 1;

            dropdownQuality.value = PlayerPrefs.GetInt(QUALITY_KEY, QualitySettings.GetQualityLevel());

            // Apply loaded values to AudioMixer
            OnAudioSliderChange(sliderMaster.value, _audioMaster, MASTER_KEY);
            OnAudioSliderChange(sliderMusic.value, _audioGroupMusic, MUSIC_KEY);
            OnAudioSliderChange(sliderVolume.value, _audioGroupUI, UI_KEY);
            OnAudioSliderChange(sliderSoundEffects.value, _audioGroupSFX, SFX_KEY);

            OnAudioToggleChange(toggleMaster.isOn, _audioMaster, MUTE_MASTER_KEY);
            OnAudioToggleChange(toggleMusic.isOn, _audioGroupMusic, MUTE_MUSIC_KEY);
            OnAudioToggleChange(toggleVolume.isOn, _audioGroupUI, MUTE_UI_KEY);
            OnAudioToggleChange(toggleSoundEffects.isOn, _audioGroupSFX, MUTE_SFX_KEY);
        }

        private void ResetToDefaults()
        {
            sliderMaster.value = 1f;
            sliderMusic.value = 1f;
            sliderVolume.value = 1f;
            sliderSoundEffects.value = 1f;

            toggleMaster.isOn = true;
            toggleMusic.isOn = true;
            toggleVolume.isOn = true;
            toggleSoundEffects.isOn = true;

            dropdownQuality.value = 2; // Mid-quality as default
        }

        private void DeleteAllSaves()
        {
            PlayerPrefs.DeleteKey(MASTER_KEY);
            PlayerPrefs.DeleteKey(MUSIC_KEY);
            PlayerPrefs.DeleteKey(UI_KEY);
            PlayerPrefs.DeleteKey(SFX_KEY);

            PlayerPrefs.DeleteKey(MUTE_MASTER_KEY);
            PlayerPrefs.DeleteKey(MUTE_MUSIC_KEY);
            PlayerPrefs.DeleteKey(MUTE_UI_KEY);
            PlayerPrefs.DeleteKey(MUTE_SFX_KEY);

            PlayerPrefs.DeleteKey(QUALITY_KEY);

            ResetToDefaults();
            LoadSettings();
        }
    }

}
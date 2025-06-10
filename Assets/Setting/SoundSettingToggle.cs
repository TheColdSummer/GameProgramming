using UnityEngine;
using UnityEngine.UI;


public class SoundSettingToggle : MonoBehaviour
{
    public enum SoundType { BGM, SoundEffect }
    public SoundType soundType;

    private Toggle _toggle;

    void Start()
    {
        _toggle = GetComponent<Toggle>();
        if (soundType == SoundType.BGM)
            _toggle.isOn = SoundSetting.BGMEnabled;
        else if (soundType == SoundType.SoundEffect)
            _toggle.isOn = SoundSetting.SoundEffectEnabled;

        _toggle.onValueChanged.AddListener(OnToggleChanged);
    }

    void OnToggleChanged(bool isOn)
    {
        if (soundType == SoundType.BGM)
            SoundSetting.BGMEnabled = isOn;
        else if (soundType == SoundType.SoundEffect)
            SoundSetting.SoundEffectEnabled = isOn;
    }
}
public static class SoundSetting
{
    private static bool _BGMEnabled = true;
    private static bool _SoundEffectEnabled = true;
    
    public static bool BGMEnabled
    {
        get => _BGMEnabled;
        set
        {
            _BGMEnabled = value;
        }
    }
    
    public static bool SoundEffectEnabled
    {
        get => _SoundEffectEnabled;
        set
        {
            _SoundEffectEnabled = value;
        }
    }
}
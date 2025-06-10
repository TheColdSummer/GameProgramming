using UnityEngine;

public static class Difficulty
{
    private static float _difficulty;
    public static void SetDifficulty(float difficulty)
    {
        _difficulty = difficulty;
    }
    
    public static float GetDifficulty()
    {
        return _difficulty > 0.1f ? _difficulty : 0.1f;
    }
}
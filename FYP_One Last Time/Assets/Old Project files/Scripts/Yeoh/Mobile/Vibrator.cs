using UnityEngine;

public static class Vibrator
{
    public static AndroidJavaClass unityPlayer;
    public static AndroidJavaObject currentActivity;
    public static AndroidJavaObject vibrator;

    static Vibrator()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#endif
    }

    public static void Vibrate(long milliseconds = 100)
    {
        if(IsAndroid()) vibrator.Call("vibrate", milliseconds);

#if UNITY_ANDROID && !UNITY_EDITOR
        else Handheld.Vibrate();
#endif

    }

    public static void Cancel()
    {
        if(IsAndroid()) vibrator.Call("cancel");
    }

    public static bool IsAndroid()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return true;
#else
        return false;
#endif
    }
}

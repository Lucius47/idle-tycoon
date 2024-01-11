using UnityEngine;

public static class VibrationManager
{
    public static void Vibrate(int milliseconds = 100)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJNIHelper.debug = true;

        using AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        using AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");
        using AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");

        using AndroidJavaClass newClass = new AndroidJavaClass("com.ehs.idle.VibrationHelper");
        newClass.CallStatic("vibrate", context, milliseconds);
#endif
    }
}

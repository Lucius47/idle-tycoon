using UnityEngine;

public static class VibrationManager
{
    public static void Vibrate(int milliseconds = 100)
    {
        AndroidJNIHelper.debug = true;

        using AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");

        using AndroidJavaClass newClass = new AndroidJavaClass("com.ehs.idle.VibrationHelper");
        newClass.CallStatic("vibrate", context, milliseconds);
    }
}

using UnityEngine;

public static class VibrationManager
{
    public static void Vibrate(long milliseconds)
    {
        Handheld.Vibrate();

        //#if UNITY_ANDROID && !UNITY_EDITOR
        //using var vibrationHelper = new AndroidJavaClass("com.ehs.idle.VibrationHelper");
        //Debug.LogError("Vibrate " + vibrationHelper);
        //vibrationHelper.CallStatic("vibrate", new object[] { milliseconds });
        //#endif
    }
}

using UnityEngine;

public class JavaExamples : MonoBehaviour
{
    private void Start()
    {
        Handheld.Vibrate();
    }

    private void Call()
    {
        AndroidJNIHelper.debug = true;

        using AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");

        using AndroidJavaClass newClass = new AndroidJavaClass("com.ehs.idle.VibrationHelper");
        newClass.CallStatic("vibrate", context, 1000);
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 100), "Vibrate"))
        {
            Call();
        }
    }
}

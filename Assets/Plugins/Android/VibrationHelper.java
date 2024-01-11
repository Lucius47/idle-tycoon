package com.ehs.idle;

import android.content.Context;
import android.os.Vibrator;

public class VibrationHelper
{
    public static void vibrate(Context context, int duration)
    {
        Vibrator vibrator = (Vibrator) context.getSystemService(Context.VIBRATOR_SERVICE);
        if (vibrator != null) {
            vibrator.vibrate(duration);
        }
    }
}
using UnityEngine;

public static class Vibrator
{
    public static AndroidJavaClass unityPlayer = new("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaClass>("getSystemService", "vibrator");

    public static void Vibrate(long milliseconds)
    {
        vibrator.Call("vibrate", milliseconds);
    }
}

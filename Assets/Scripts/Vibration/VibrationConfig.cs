using System;
using MoreMountains.NiceVibrations;
using UnityEngine;

[CreateAssetMenu(fileName = "Vibration", menuName = "Settings/Vibration", order = 4)]
public class VibrationConfig : ScriptableObject
{
    [SerializeField] HapticTypes vibroTypeIos;
    [SerializeField] HapticTypes vibroTypeAndroid;
    [SerializeField] float vibrationDelayIos;
    [SerializeField] float vibrationDelayAndroid;

    private float allowVibroTime;

    public void Init(HapticTypes haptTypes, float delay)
    {
        vibroTypeAndroid = vibroTypeIos = haptTypes;
        vibrationDelayAndroid = vibrationDelayIos = delay;
    }

    public void Vibrate()
    {
        if (Time.time < allowVibroTime)
            return;
#if UNITY_IOS
        allowVibroTime = Time.time + vibrationDelayIos;
        MMVibrationManager.Haptic(vibroTypeIos);
#elif UNITY_ANDROID
        allowVibroTime = Time.time + vibrationDelayAndroid;
        MMVibrationManager.Haptic(vibroTypeAndroid);
#endif
    }
    public void VibrateLong()
    {
        if (Time.time < allowVibroTime)
            return;
#if UNITY_IOS
        allowVibroTime = Time.time + vibrationDelayIos;
        MMVibrationManager.Haptic(vibroTypeIos);
#elif UNITY_ANDROID
        
        MMVibrationManager.AndroidVibrate(new long[] { 50, 50, 50, 50 }, 20);
#endif
    }
   

#if UNITY_EDITOR || DEVELOPMENT_BUILD
public HapticTypes VibroTypeIos => vibroTypeIos;
    public HapticTypes VibroTypeAndroid => vibroTypeAndroid;
    public float VibrationDelayIos => vibrationDelayIos;
    public float VibrationDelayAndroid => vibrationDelayAndroid;

    public void ToggleHapticIos()
    {
        vibroTypeIos = VibroTypeIos.Next();
    }

    public void ToggleHapticAndroid()
    {
        vibroTypeAndroid = VibroTypeAndroid.Next();
    }

    public void ToggleHapticIosBack()
    {
        vibroTypeIos = VibroTypeIos.Prev();
    }

    public void ToggleHapticAndroidBack()
    {
        vibroTypeAndroid = VibroTypeAndroid.Prev();
    }

    public void ChangeCooldownIos(float n)
    {
        vibrationDelayIos += n;
    }

    public void ChangeCooldownAndroid(float n)
    {
        vibrationDelayAndroid += n;
    }
#endif
}

public static class Extensions
{
    public static T Next<T>(this T src) where T : struct
    {
        if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

        T[] Arr = (T[]) Enum.GetValues(src.GetType());
        int j = Array.IndexOf<T>(Arr, src) + 1;
        return (Arr.Length == j) ? Arr[0] : Arr[j];
    }

    public static T Prev<T>(this T src) where T : struct
    {
        if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

        T[] Arr = (T[]) Enum.GetValues(src.GetType());
        int j = Array.IndexOf<T>(Arr, src) - 1;
        return (j < 1) ? Arr[Arr.Length - 1] : Arr[j];
    }
}
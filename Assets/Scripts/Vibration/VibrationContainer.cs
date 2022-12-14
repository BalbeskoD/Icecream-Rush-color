using System;
using System.Linq;
using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

[CreateAssetMenu(fileName = "VibrationContainer", menuName = "Configs/VibrationContainer", order = 0)]
public class VibrationContainer : ScriptableObject
{
    [SerializeField] private Vibration[] vibrations;
    [SerializeField] private VibrationItem[] vibrationItems;

    public void Vibrate(VibrationPlace place)
    {
        var type = vibrations.FirstOrDefault(x => x.Place == place)!.Type;
        vibrationItems.FirstOrDefault(x => x.Type == type)?.Config.Vibrate();
    }
}

public enum VibrationType
{
    None,
    Easy,
    Mistake,
    SingleLight,
    Complete,
    ShortComplete,
    NormalImpact,
    FastImpact,
}

[Serializable]
public class VibrationItem
{
    [SerializeField] private VibrationType type;
    [SerializeField] private VibrationConfig config;

    public VibrationType Type => type;

    public VibrationConfig Config => config;
}
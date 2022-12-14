using System;
using UnityEngine;
using Zenject;

public class VibrationController
{
    private const string KEY = "Vibration";
    private readonly VibrationContainer _container;

    public VibrationController(VibrationContainer container)
    {
        _container = container;
    }

    public bool IsVibration
    {
        get => PlayerPrefs.GetInt(KEY, 1) == 1;
        set => PlayerPrefs.SetInt(KEY, value ? 1 : 0);
    }

    public void Vibrate(VibrationPlace type)
    {
        if (IsVibration)
            _container.Vibrate(type);
    }
}

[Serializable]
public class Vibration
{
    [SerializeField] private VibrationPlace place;
    [SerializeField] private VibrationType type;

    public VibrationPlace Place => place;

    public VibrationType Type => type;
}

public enum VibrationPlace
{
    None,
    RollInProgress,
    RollCompleted,
    GatePass,
    FinishBallOnPlace
}
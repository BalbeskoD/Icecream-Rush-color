using Configs;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GameConfigsInstaller", menuName = "Installers/GameConfigsInstaller")]
public class GameConfigsInstaller : ScriptableObjectInstaller<GameConfigsInstaller>
{
    [SerializeField] private PlayerSettings playerSettings;
    [SerializeField] private VibrationContainer vibrationContainer;
    public override void InstallBindings()
    {
        Container.BindInstances(
            playerSettings,
            vibrationContainer
        );
    }
}
using Helpers;
using Location;
using Managers;
using Rolls;
using UnityEngine;
using Zenject;
using Zenject.Signals;

public class GameSceneInstaller : MonoInstaller<GameSceneInstaller>
{
    [SerializeField] private Player.Player player;
    [SerializeField] private UI.UiController controller;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private Roll roll;

    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        BinsSignals();
        Container.BindInterfacesAndSelfTo<VibrationController>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<GameManager>().AsSingle().NonLazy();
        Container.BindInstances(player, controller, cameraController);
        Container.BindFactory<Line, Roll, Roll.Factory>().FromComponentInNewPrefab(roll);
    }

    private void BinsSignals()
    {
        Container.DeclareSignal<GameStateChangeSignal>();
        Container.DeclareSignal<GameStartSignal>();
        Container.DeclareSignal<GameRestartSignal>();
        Container.DeclareSignal<GameEndSignal>();
        Container.DeclareSignal<LevelWinSignal>();
        Container.DeclareSignal<PlayerProgressBarSignal>();
        Container.DeclareSignal<PlayerFailSignal>();
        Container.DeclareSignal<GetRewardSignal>();
        Container.DeclareSignal<CoinsUpdateSignal>();
        Container.DeclareSignal<PlayerPointsSignal>();
        Container.DeclareSignal<FinishBonusFailSignal>();
        Container.DeclareSignal<CreateLevelSignal>();
        Container.DeclareSignal<PlayerFinishActionSignal>();
        Container.DeclareSignal<FinishBonusAddSignal>();
        Container.DeclareSignal<FinishAddSignal>();
    }
}
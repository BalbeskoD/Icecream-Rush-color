using System.Threading;
using Cysharp.Threading.Tasks;
using Managers;
using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller<ProjectInstaller>
{
    [SerializeField] private GameObject appMetricsPrefab;
    [SerializeField] private GameObject appsFlyer;
    [SerializeField] private GameObject fbLoading;
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GameSetup>().AsSingle().NonLazy();
        Container.InstantiatePrefab(appMetricsPrefab);
        Container.InstantiatePrefab(appsFlyer);
        Container.InstantiatePrefab(fbLoading);
    }
    public class SyncContextInjecter
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Inject()
        {
            SynchronizationContext.SetSynchronizationContext(new UniTaskSynchronizationContext());
        }
    }
}
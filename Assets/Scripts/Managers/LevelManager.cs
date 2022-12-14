using UnityEngine;
using Zenject;
using Zenject.Signals;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelController[] levelControllers;
        private LevelController _controller;

        private SignalBus _signalBus;
        private DiContainer _container;

        [Inject]
        public void Construct(SignalBus signalBus, DiContainer container)
        {
            _container = container;
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _signalBus.Subscribe<CreateLevelSignal>(CreateLevel);
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<CreateLevelSignal>(CreateLevel);
        }

        private void CreateLevel(CreateLevelSignal signal)
        {
            if (_controller)
            {
                Destroy(_controller.gameObject);
            }

            var index = signal.Index - 1;
            index %= levelControllers.Length;
            _controller = _container.InstantiatePrefab(levelControllers[index].gameObject).GetComponent<LevelController>();
        }
    }
}
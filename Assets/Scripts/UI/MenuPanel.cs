using UnityEngine;
using Zenject;
using Zenject.Signals;

namespace UI
{
    public class MenuPanel : MonoBehaviour
    {
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void OnStartButtonClick()
        {
            _signalBus.Fire<GameStartSignal>();
        }
    }
}
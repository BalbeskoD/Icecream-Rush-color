using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Zenject.Signals;

namespace UI
{
    public class WinPanel : MonoBehaviour
    {
        private Button _restartButton;

        private SignalBus _signalBus;

        private int _rewardCount;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _restartButton = GetComponentInChildren<Button>();
            _restartButton.onClick.AddListener(Restart);
        }

        private void OnDestroy()
        {
            _restartButton.onClick.RemoveAllListeners();
        }

        private void Restart()
        {
            _signalBus.Fire<GameRestartSignal>();
        }
    }
}
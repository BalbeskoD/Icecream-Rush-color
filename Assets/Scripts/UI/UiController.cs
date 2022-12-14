using Enums;
using UnityEngine;
using Zenject;
using Zenject.Signals;

namespace UI
{
    public class UiController : MonoBehaviour
    {
        private MenuPanel _menuPanel;
        private GamePanel _gamePanel;
        private LostPanel _lostPanel;
        private WinPanel _winPanel;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _menuPanel = GetComponentInChildren<MenuPanel>(true);
            _gamePanel = GetComponentInChildren<GamePanel>(true);
            _lostPanel = GetComponentInChildren<LostPanel>(true);
            _winPanel = GetComponentInChildren<WinPanel>(true);
            SubscribeSignals();
        }

        private void OnDestroy()
        {
            UnsubscribeSignals();
        }

        private void SubscribeSignals()
        {
            _signalBus.Subscribe<GameStateChangeSignal>(OnGameStateChange);
        }

        private void UnsubscribeSignals()
        {
            _signalBus.Unsubscribe<GameStateChangeSignal>(OnGameStateChange);
        }

        private void OnGameStateChange(GameStateChangeSignal signal)
        {
            _menuPanel.gameObject.SetActive(signal.GameStates == GameStates.Menu);
            _gamePanel.gameObject.SetActive(signal.GameStates == GameStates.Game);
            _lostPanel.gameObject.SetActive(signal.GameStates == GameStates.Lost);
            _winPanel.gameObject.SetActive(signal.GameStates == GameStates.Win);
        }
    }
}
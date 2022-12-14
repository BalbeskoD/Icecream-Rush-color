using Managers;
using TMPro;
using UnityEngine;
using Zenject;
using Zenject.Signals;

namespace UI
{
    public class PlayerPoints : MonoBehaviour
    {
        private TextMeshProUGUI _text;

        private SignalBus _signalBus;
        private GameManager _gameManager;

        [Inject]
        public void Construct(SignalBus signalBus, GameManager gameManager)
        {
            _signalBus = signalBus;
            _gameManager = gameManager;
        }

        private void Awake()
        {
            _text = GetComponentInChildren<TextMeshProUGUI>();
            _text.text = _gameManager.Coins.ToString();
            _signalBus.Subscribe<CoinsUpdateSignal>(OnPoints);
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<CoinsUpdateSignal>(OnPoints);
        }

        private void OnPoints(CoinsUpdateSignal signal)
        {
            _text.text = signal.CoinsCount.ToString();
        }
    }
}
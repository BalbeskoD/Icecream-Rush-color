using System;
using Configs;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Enums;
using Finish;
using Location;
using Managers;
using Rolls;
using UI;
using UnityEngine;
using Zenject;
using Zenject.Signals;
using Random = UnityEngine.Random;

namespace Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] public ParticleSystem confetti;
        [SerializeField] public ParticleSystem capli;
        [SerializeField] public ParticleSystem capli2;
        private GameManager _gameManager;
        private PlayerSettings _playerSettings;
        private PlayerCameraController _cameraController;
        private PlayerAnimationController _animationController;
        private PlayerMoveController _moveController;
        private UiController _uiController;
        private SignalBus _signalBus;
        private PlayerView _playerView;
        private bool _isActive;
        private int _levelProgress;
        private int _levelCompletedValue;
        private int _totalPointsOnLevel;

        private float ProgressValue => (float) transform.position.z / _finishPos;

        private float _finishPos;
        private bool _isFinishAction;

        private Roll.Factory _rollFactory;
        private Roll _currentRoll;
        private ColorSet _colorSet;
        public Vector3 RollOffset => transform.position + _playerSettings.RollOffset;

        private bool _isWasShake;
        private VibrationController _vibrationController;

        [Inject]
        public void Construct(PlayerSettings playerSettings, UiController ui, SignalBus signalBus,
            GameManager gameManager, Roll.Factory rollFactory, VibrationController vibrationController)
        {
            _playerSettings = playerSettings;
            _uiController = ui;
            _signalBus = signalBus;
            _gameManager = gameManager;
            _rollFactory = rollFactory;
            _vibrationController = vibrationController;
        }

        public void Awake()
        {
            _playerView = GetComponentInChildren<PlayerView>();
            _cameraController = GetComponent<PlayerCameraController>();
            _animationController = GetComponent<PlayerAnimationController>();
            _moveController = GetComponent<PlayerMoveController>();
            SubscribeSignals();
        }

        private void OnDestroy()
        {
            UnsubscribeSignals();
        }

        private void LateUpdate()
        {
            if (_isActive && !_isFinishAction)
                _signalBus.Fire(new PlayerProgressBarSignal() {Value = ProgressValue});
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
            _isActive = signal.GameStates == GameStates.Game;
            _moveController.SetActive(_isActive, _isActive ? FindObjectOfType<LevelController>().LevelLimits : null);
            if (_isActive)
            {
                _finishPos = FindObjectOfType<FinishLine>().transform.position.z;
                _isFinishAction = false;
                _playerView.SwitchTrails(true);
            }
            else
            {
                _playerView.SwitchTrails(false);
                confetti.gameObject.SetActive(false);
            }

            if (signal.GameStates == GameStates.Menu)
            {
                _cameraController.SwitchCamera(true);
                transform.position = Vector3.zero + new Vector3(0, 0.5f, 0f);
                _playerView.gameObject.SetActive(true);
            }
        }

        public void OnGate(Gate gate)
        {
            _playerView.BlinkWithoutRotate();
            _colorSet = gate.ColorSet;
            _playerView.ChangeColor(gate.ColorSet);
            _moveController.OnGate(gate.ColorSet);
            _vibrationController.Vibrate(VibrationPlace.GatePass);
        }

        public void AddCoin()
        {
            _totalPointsOnLevel++;
            _signalBus.Fire(new PlayerPointsSignal() {Count = 1});
            _vibrationController.Vibrate(VibrationPlace.RollCompleted);
        }

        public async void OnFinishLine(FinishLine finishLine)
        {
            _moveController.SetActive(false);
            _playerView.gameObject.SetActive(false);
            _playerView.SwitchTrails(false);
            finishLine.Finish.StakanStart();
            await UniTask.Delay(TimeSpan.FromSeconds(1.4));
            finishLine.Finish.StakanView();
            _cameraController.SwitchCamera(false);
            _isFinishAction = true;
            _signalBus.Fire<PlayerFinishActionSignal>();
            await UniTask.Delay(TimeSpan.FromSeconds(2));
            finishLine.Finish.StartAction();
        }

        public void FinishBonusEnd(int bonus)
        {
            _signalBus.Fire(new PlayerPointsSignal() {Count = _totalPointsOnLevel * (bonus + 1)});
            _signalBus.Fire<LevelWinSignal>();
        }

        public void OnLine(Line line)
        {
            if (_moveController.OnLine(line))
            {
                _currentRoll = _rollFactory.Create(line);
                line.OnStartAction(_playerSettings.SpeedForwardOnLine, _playerSettings.DelayForOtherLine);
                line.FxColor(capli);
                line.FxColor(capli2);
                //confetti.gameObject.SetActive(true);
                capli.gameObject.SetActive(true);
                capli2.gameObject.SetActive(true);
            }
            else if (line.IsCanStart)
            {
                _playerView.Blink();
                //confetti.gameObject.SetActive(false);
                capli.gameObject.SetActive(false);
                capli2.gameObject.SetActive(false);
            }
        }

        public void OnLineMove(float value)
        {
            _playerView.OnRoll(value);
            _vibrationController.Vibrate(VibrationPlace.RollInProgress);
        }

        public void ThrowRoll()
        {
            _playerView.ThrowRoll();
            //confetti.gameObject.SetActive(false);
            capli.gameObject.SetActive(false);
            capli2.gameObject.SetActive(false);
        }
    }
}
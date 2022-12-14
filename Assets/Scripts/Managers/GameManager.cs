using System;
using System.Collections.Generic;
using System.Threading;
using Configs;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Enums;
using UI;
using UnityEngine;
using Zenject;
using Zenject.Signals;

namespace Managers
{
    public class GameManager : IInitializable, IDisposable, ITickable
    {
        private const string COINS_KEY = "Coins";
        private const string LEVEL_KEY = "Level";
        private const string LEVEL_COUNT_KEY = "LevelCount";

        private readonly DiContainer _diContainer;
        private readonly ProjectSettings _projectSettings;
        private readonly SignalBus _signalBus;

        private GameStates _gameStates;
        private List<string> _names;
        private List<Color> _colors;
        private float _timer;
        private bool _isGame;

        private GameStates GameState
        {
            get => _gameStates;
            set
            {
                if (_gameStates == value)
                    return;
                _gameStates = value;
                _signalBus.Fire(new GameStateChangeSignal() {GameStates = _gameStates});
                switch (_gameStates)
                {
                    case GameStates.Menu:
                        _signalBus.Fire(new CreateLevelSignal() {Index = Level});
                        break;
                    case GameStates.Game:
                        OnGame();
                        break;
                    case GameStates.Win:
                        OnGameEnd(true);
                        break;
                    case GameStates.Lost:
                        OnGameEnd(false);
                        break;
                }
            }
        }

        public int Coins
        {
            get => PlayerPrefs.GetInt(COINS_KEY, 0);
            private set
            {
                PlayerPrefs.SetInt(COINS_KEY, value);
                _signalBus.Fire(new CoinsUpdateSignal() {CoinsCount = value});
            }
        }

        public int Level
        {
            get => PlayerPrefs.GetInt(LEVEL_KEY, 1);
            private set => PlayerPrefs.SetInt(LEVEL_KEY, value);
        }

        public int LevelCount
        {
            get => PlayerPrefs.GetInt(LEVEL_COUNT_KEY, 1);
            private set => PlayerPrefs.SetInt(LEVEL_COUNT_KEY, value);
        }

        public GameManager(DiContainer diContainer, ProjectSettings projectSettings, SignalBus signalBus)
        {
            _diContainer = diContainer;
            _projectSettings = projectSettings;
            _signalBus = signalBus;
        }

        public async void Initialize()
        {
            SubscribeSignals();
            await UniTask.Yield();
            ChangeGameState(GameStates.Menu);
        }

        public void Dispose()
        {
            UnsubscribeSignals();
        }

        private void SubscribeSignals()
        {
            _signalBus.Subscribe<GameStartSignal>(OnGameStart);
            _signalBus.Subscribe<LevelWinSignal>(OnWin);
            _signalBus.Subscribe<PlayerFailSignal>(OnFail);
            _signalBus.Subscribe<PlayerPointsSignal>(OnGetReward);
            _signalBus.Subscribe<GameRestartSignal>(OnGameRestart);
        }

        private void UnsubscribeSignals()
        {
            _signalBus.Unsubscribe<GameStartSignal>(OnGameStart);
            _signalBus.Unsubscribe<LevelWinSignal>(OnWin);
            _signalBus.Unsubscribe<PlayerFailSignal>(OnFail);
            _signalBus.Unsubscribe<PlayerPointsSignal>(OnGetReward);
            _signalBus.Unsubscribe<GameRestartSignal>(OnGameRestart);
        }

        private void OnGameRestart()
        {
            ChangeGameState(GameStates.Menu);
            Resources.UnloadUnusedAssets();
            GC.Collect();
            DOTween.Clear();
        }

        private void OnGetReward(PlayerPointsSignal signal)
        {
            Coins += signal.Count;
        }

        private void OnWin()
        {
            ChangeGameState(GameStates.Win);
        }

        private void OnFail()
        {
            ChangeGameState(GameStates.Lost);
        }

        private void ChangeGameState(GameStates gameStates)
        {
            GameState = gameStates;
        }

        private void OnGameStart()
        {
            ChangeGameState(GameStates.Game);
        }

        private void OnGame()
        {
            var data = new Dictionary<string, object>
            {
                {"level_number", Level},
                {"level_count", LevelCount++},
            };
            AppMetrica.Instance.ReportEvent("level_start", data);
            AppMetrica.Instance.SendEventsBuffer();
            _isGame = true;
            _timer = 0;
        }

        private void OnGameEnd(bool isWin)
        {
            if (isWin)
            {
                Level++;
            }

            var data = new Dictionary<string, object>
            {
                {"level_number", Level},
                {"level_count", LevelCount},
                {"result", isWin ? "win" : "lose"},
                {"time", (int) _timer},
            };
            AppMetrica.Instance.ReportEvent("level_finish", data);
            AppMetrica.Instance.SendEventsBuffer();
            _isGame = false;
        }

        public void Tick()
        {
            if (!_isGame)
                return;
            _timer += Time.deltaTime;
        }
    }
}
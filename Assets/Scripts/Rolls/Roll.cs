using System;
using Configs;
using DG.Tweening;
using Location;
using UnityEngine;
using Zenject;
using Zenject.Signals;
using Random = UnityEngine.Random;

namespace Rolls
{
    public class Roll : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<Line, Roll>
        {
        }

        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

        [SerializeField] private MoveAwaySet[] sets;
        [SerializeField] private VibrationType vibrationType;
        private Player.Player _player;
        private Line _line;
        private bool _isActive;
        private SignalBus _signalBus;
        private float _lineScale;
        private PlayerSettings _playerSettings;
        private TrailRenderer _trailRenderer;
        private VibrationController _vibrationController;

        [Inject]
        public void Construct(Player.Player player, Line line, SignalBus signalBus, PlayerSettings playerSettings, VibrationController vibrationController)
        {
            _player = player;
            _line = line;
            _signalBus = signalBus;
            _playerSettings = playerSettings;
            _vibrationController = vibrationController;
        }

        private void Awake()
        {
            _signalBus.Subscribe<PlayerFinishActionSignal>(OnStartFinish);
            _signalBus.Subscribe<GameRestartSignal>(OnRestart);
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<PlayerFinishActionSignal>(OnStartFinish);
            _signalBus.Unsubscribe<GameRestartSignal>(OnRestart);
        }

        private void Start()
        {
            _trailRenderer = GetComponentInChildren<TrailRenderer>();
            _trailRenderer.material.SetColor(BaseColor, _line.ActiveColor.Color);
            _trailRenderer.enabled = false;
            GetComponentInChildren<MeshRenderer>().material
                .SetColor(BaseColor, _line.ActiveColor.Color);
            _lineScale = _line.transform.parent.localScale.z;
            _isActive = true;
            transform.localScale = Vector3.zero;
        }

        private void Update()
        {
            if (!_isActive)
                return;
            transform.position = new Vector3(_line.transform.position.x, _player.RollOffset.y,
                _player.RollOffset.z);
            if (!_line.IsCompleted)
            {
                var targetScale = _playerSettings.RollStartScale.z +
                                  (_lineScale - _line.transform.parent.localScale.z);
                transform.localScale = new Vector3(1, targetScale, targetScale);
            }
            else
            {
                MoveAway();
            }
        }

        private void MoveAway()
        {
            _trailRenderer.enabled = true;
            _isActive = false;
            var set = sets[Random.Range(0, sets.Length)];
            transform.DOJump(
                transform.position + (transform.position.x > 0 ? Vector3.right : Vector3.left) * set.SideOffset,
                set.UpOffset,
                1, set.Duration);
            transform.DORotate(set.Rotate, set.Duration);
            _player.AddCoin();
        }

        private void OnStartFinish()
        {
            var finish = FindObjectOfType<Finish.Finish>();
            var point2 = finish.GetRollPoint2();
            gameObject.transform.position = point2.position;
            var point = finish.GetRollPoint();
            transform.localScale = Vector3.one;
            var s = DOTween.Sequence();
            s.AppendInterval(0.3f * finish.GetStartIndex());
            s.Append(transform.DOJump(point.position, 2, 1, 0.3f)
                .Join(transform.DORotateQuaternion(point.rotation, 0.3f)));
        }

        private void OnRestart()
        {
            Destroy(gameObject);
        }

        [Serializable]
        public class MoveAwaySet
        {
            [SerializeField] private float duration = 0.5f;
            [SerializeField] private Vector3 rotate = new Vector3(0, -7, 104);
            [SerializeField] private float upOffset = 1;
            [SerializeField] private float sideOffset = 3;

            public float Duration => duration;

            public Vector3 Rotate => rotate;

            public float UpOffset => upOffset;

            public float SideOffset => sideOffset;
        }
    }
}
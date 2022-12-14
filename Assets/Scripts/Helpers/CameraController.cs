using Cinemachine;
using Configs;
using DG.Tweening;
using Finish;
using Player;
using UnityEngine;
using Zenject;
using Zenject.Signals;

namespace Helpers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera gameCamera;
        [SerializeField] private CinemachineVirtualCamera finishCamera;
        private CinemachineComposer _gameComposer;
        private CinemachineComposer _finishComposer;
        private CinemachineTransposer _finishTransposer;

        private Tween _shakeTween;

        private SignalBus _signalBus;
        private PlayerSettings _playerSettings;

        private Vector3 _startComposerOffset;
        private Vector3 _startTransposerOffset;

        [Inject]
        public void Construct(SignalBus signalBus, PlayerSettings playerSettings)
        {
            _signalBus = signalBus;
            _playerSettings = playerSettings;
        }

        public void Awake()
        {
            _gameComposer = gameCamera.GetCinemachineComponent<CinemachineComposer>();
            _finishComposer = finishCamera.GetCinemachineComponent<CinemachineComposer>();
            _finishTransposer = finishCamera.GetCinemachineComponent<CinemachineTransposer>();
            _signalBus.Subscribe<FinishBonusAddSignal>(OnFinishBall);
            _signalBus.Subscribe<FinishAddSignal>(OnFinish);
            _startComposerOffset = _finishComposer.m_TrackedObjectOffset;
            _startTransposerOffset = _finishTransposer.m_FollowOffset;
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<FinishBonusAddSignal>(OnFinishBall);
            _signalBus.Unsubscribe<FinishAddSignal>(OnFinish);
        }

        public void SwitchCamera(bool isGame)
        {
            gameCamera.gameObject.SetActive(isGame);
            finishCamera.gameObject.SetActive(!isGame);
            if (!isGame)
            {
                var finish = FindObjectOfType<FinishStakan>().transform.transform;
                _finishComposer.m_TrackedObjectOffset = _startComposerOffset;
                _finishTransposer.m_FollowOffset = _startTransposerOffset;
                finishCamera.m_Follow = finish;
                finishCamera.m_LookAt = finish;
            }
        }

        public void Shake(CameraShakeConfig config)
        {
            _shakeTween?.Kill();
            var startPos = _gameComposer.m_TrackedObjectOffset;
            _shakeTween = DOTween
                .To(() => _gameComposer.m_TrackedObjectOffset, x => _gameComposer.m_TrackedObjectOffset = x,
                    config.Shake, config.Duration)
                .OnComplete(() => DOTween.To(() => _gameComposer.m_TrackedObjectOffset,
                    x => _gameComposer.m_TrackedObjectOffset = x, startPos, config.Duration));
        }

        private void OnFinishBall()
        {
            DOTween.To(() => _finishComposer.m_TrackedObjectOffset, x => _finishComposer.m_TrackedObjectOffset = x,
                _finishComposer.m_TrackedObjectOffset + _playerSettings.FinishUpOffset, 0.5f);
            DOTween.To(() => _finishTransposer.m_FollowOffset, x => _finishTransposer.m_FollowOffset = x,
                _finishTransposer.m_FollowOffset + _playerSettings.FinishUpOffset2, 0.5f);
        }

        private void OnFinish()
        {
            DOTween.To(() => _finishComposer.m_TrackedObjectOffset, x => _finishComposer.m_TrackedObjectOffset = x,
                _finishComposer.m_TrackedObjectOffset = new Vector3(0, 2.55f, 0), 1.5f);
            DOTween.To(() => _finishTransposer.m_FollowOffset, x => _finishTransposer.m_FollowOffset = x,
                _finishTransposer.m_FollowOffset = new Vector3(4, 8, -9), 1.5f);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;
using Zenject.Signals;
using Random = UnityEngine.Random;

namespace Finish
{
    public class Finish : MonoBehaviour
    {
        [SerializeField] public GameObject stakan;
        private Transform[] _rollPoint;
        private FinishBall[] _bonus;
        private Sequence _sequence;

        private bool _isAction;
        private int _index;

        private SignalBus _signalBus;

        private List<FinishRollPoint> _points;
        private List<FinishStartRollPoint> _points2;
        private FinishBoy _finishBoy;
        private int _startIndex;
        private VibrationController _vibrationController;

        [Inject]
        public void Construct(SignalBus signalBus, VibrationController vibrationController)
        {
            _signalBus = signalBus;
            _vibrationController = vibrationController;
        }

        private void Awake()
        {
            _bonus = GetComponentsInChildren<FinishBall>();
            _finishBoy = GetComponentInChildren<FinishBoy>();
            foreach (var rb in _bonus)
            {
                rb.gameObject.SetActive(false);
            }

            _points = GetComponentsInChildren<FinishRollPoint>().ToList();
            _points2 = GetComponentsInChildren<FinishStartRollPoint>().ToList();
            _startIndex = 0;
        }

        private void Update()
        {
            if (!_isAction) return;

            if (Input.GetMouseButtonDown(0))
            {
                TryAction();
            }
        }

        private void TryAction()
        {
            if (_bonus[_index].IsCanBeDrop)
            {
                Drop();
            }
        }

        public void StartAction()
        {
            _isAction = true;
            _index = 0;
            StartMove(_index);
        }

        private void StartMove(int index)
        {
            if (index < _bonus.Length)
            {
                _bonus[index].gameObject.SetActive(true);
            }
        }

        public void StakanStart()
        {
            _finishBoy.StakanTake();
        }

        public void StakanView()
        {
            stakan.gameObject.SetActive(true);
        }

        private async void Drop()
        {
            var isCompleted = await _bonus[_index].Drop();
            if (isCompleted)
            {
                _vibrationController.Vibrate(VibrationPlace.FinishBallOnPlace);
                if (++_index == _bonus.Length)
                {
                    BonusEnd();
                    _signalBus.Fire(new FinishAddSignal());
                }
                else
                {
                    _signalBus.Fire(new FinishBonusAddSignal() {Value = _index + 1});
                    StartMove(_index);
                }
            }
            else
            {
                BonusEnd();
            }
        }

        public async void BonusEnd()
        {
            _isAction = false;
            if (_index != _bonus.Length)
                _bonus[_index].gameObject.SetActive(false);
            await MoveToBoy();
            FindObjectOfType<Player.Player>().FinishBonusEnd(_index);
        }

        private async UniTask MoveToBoy()
        {
            var boy = _finishBoy.OpenMouth();
            var target = boy.position;
            var stakan = GetComponentInChildren<FinishStakan>().transform;
            var points = GetComponentsInChildren<FinishRollPoint>();
            foreach (var finishRollPoint in points)
            {
                finishRollPoint.transform.SetParent(stakan);
            }

            var balls = GetComponentsInChildren<FinishBall>().Reverse().Select(x => x.transform).ToArray();

            var list = new List<Transform> {stakan};
            list.AddRange(balls);

            for (int i = 0; i < list.Count; i++)
            {
                var t = target;
                t.z -= 1;
                t.y += 1 + 1 * i;

                list[i].DOMove(t, 0.5f);
            }

            await UniTask.Delay(TimeSpan.FromSeconds(0.51f));
            for (int i = 0; i < list.Count; i++)
            {
                var index = i;
                list[index].DOMove(target, 0.5f + 0.5f * index).OnComplete(() =>
                {
                    _finishBoy.DoScale();
                    list[index].gameObject.SetActive(false);
                });
            }

            await UniTask.Delay(TimeSpan.FromSeconds(0.5f * list.Count));
            _finishBoy.CloseMouth();
        }

        public Transform GetRollPoint()
        {
            if (_points.Count == 0)
                return transform;
            var point = _points[Random.Range(0, _points.Count)];
            _points.Remove(point);
            return point.transform;
        }

        public Transform GetRollPoint2()
        {
            if (_points2.Count == 0)
                return transform;
            var point2 = _points2[Random.Range(0, _points2.Count)];
            _points2.Remove(point2);
            return point2.transform;
        }

        public int GetStartIndex()
        {
            return ++_startIndex;
        }
    }
}
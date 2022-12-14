using System;
using Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Location
{
    public class Segment : MonoBehaviour
    {
        [SerializeField] private ColorsConfig config;
        private Line[] _lines;

        private void Awake()
        {
            _lines = GetComponentsInChildren<Line>();
            for (int i = 0; i < _lines.Length; i++)
            {
                _lines[i].SetColor(config.Colors[i]);
            }
        }

        public async void OnLineAction(Line line, float speed, float startDelay)
        {
            line.ChangeState(speed, true);
            await UniTask.Delay(TimeSpan.FromSeconds(startDelay));
            for (int i = 0; i < _lines.Length; i++)
            {
                if (_lines[i] != line)
                    _lines[i].ChangeState(speed, false);
            }
        }
    }
}
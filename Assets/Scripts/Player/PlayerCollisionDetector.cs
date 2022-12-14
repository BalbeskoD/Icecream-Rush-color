using System;
using Finish;
using Location;
using UnityEngine;

namespace Player
{
    public class PlayerCollisionDetector : MonoBehaviour
    {
        private Player _player;

        private void Awake()
        {
            _player = GetComponent<Player>();
        }

        private void OnTriggerEnter(Collider other)
        {
            var gate = other.GetComponent<Gate>();
            if (gate)
            {
                if (!gate.IsPass)
                {
                    gate.Pass();
                    _player.OnGate(gate);
                }
                
            }

            var line = other.GetComponent<Line>();
            if (line)
            {
                _player.OnLine(line);
            }

            var finishLine = other.GetComponent<FinishLine>();
            if (finishLine)
            {
                _player.OnFinishLine(finishLine);
            }

            var horizontalPlatform = other.GetComponent<HorizontalPlatform>();
            if (horizontalPlatform)
            {
                horizontalPlatform.DoMove();
            }

            var rotatePlatformButton = other.GetComponent<RotatePlatformButton>();
            if (rotatePlatformButton)
            {
                rotatePlatformButton.OnClick();
            }
        }
    }
}
using System;
using System.Threading.Tasks;
using Cinemachine;
using Cysharp.Threading.Tasks;
using Helpers;
using UnityEngine;
using Zenject;
using Zenject.Signals;

namespace Player
{
    public class PlayerCameraController : MonoBehaviour
    {
        [SerializeField] private CameraShakeConfig shake;
        private CameraController _cameraController;


        private void Awake()
        {
            _cameraController = FindObjectOfType<CameraController>();
        }

        public void Shake()
        {
            _cameraController.Shake(shake);
        }

        public void SwitchCamera(bool isGame)
        {
            _cameraController.SwitchCamera(isGame);
        }
    }

    [Serializable]
    public class CameraShakeConfig
    {
        [SerializeField] private Vector3 shake;
        [SerializeField] private float duration;

        public Vector3 Shake => shake;

        public float Duration => duration;
    }
}
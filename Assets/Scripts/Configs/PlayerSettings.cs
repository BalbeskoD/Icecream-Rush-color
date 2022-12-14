using System;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "Configs/PlayerSettings", order = 0)]
    public class PlayerSettings : ScriptableObject
    {
        [SerializeField] private float speedForward = 5;
        [SerializeField] private float speedForwardOnLine = 10;
        [SerializeField] private float delayForOtherLine = 0.1f;
        [SerializeField] private float speedSide = 100;
        [SerializeField] private float deathCubeSpeed = 3;
        [SerializeField] private Vector3 rollOffset = new Vector3(0, -0.6f, 0.7f);
        [SerializeField] private Vector3 rollStartScale = new Vector3(1, 0.25f, 0.25f);
        [SerializeField] private Vector3 finishUpOffset = new Vector3(0, 0.5f, 0f);
        [SerializeField] private Vector3 finishUpOffset2 = new Vector3(0, 0.5f, 0f);
        [SerializeField] private Vector3 baseRotation = new Vector3(-75, 0, 0);
        [SerializeField] private Vector3 completedRollRotation = new Vector3(-85, 0, 0);
        [SerializeField] private Vector3 completedRollPosition = new Vector3(0, -0.5f, 1);
        [SerializeField] private Vector3 wrongRollRotation = new Vector3(-65, 0, 0);
        [SerializeField] private Vector3 basePosition = new Vector3(0, -0.6f, 1);
        [SerializeField] private Vector3 throwRollPosition = new Vector3(0, 0, 1);
       

        public float SpeedForward => speedForward;
       

        public float SpeedSide => speedSide;
        public float DeathCubeSpeed => deathCubeSpeed;

        public Vector3 RollOffset => rollOffset;

        public float SpeedForwardOnLine => speedForwardOnLine;

        public Vector3 RollStartScale => rollStartScale;

        public Vector3 FinishUpOffset => finishUpOffset;
        public Vector3 FinishUpOffset2 => finishUpOffset2;

        public float DelayForOtherLine => delayForOtherLine;

        public Vector3 BaseRotation => baseRotation;

        public Vector3 CompletedRollRotation => completedRollRotation;

        public Vector3 BasePosition => basePosition;

        public Vector3 ThrowRollPosition => throwRollPosition;

        public Vector3 WrongRollRotation => wrongRollRotation;

        public Vector3 CompletedRollPosition => completedRollPosition;
    }
}
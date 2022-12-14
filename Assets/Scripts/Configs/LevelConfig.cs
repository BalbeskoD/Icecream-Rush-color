using System;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/LevelConfig", order = 0)]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private LevelLimits levelLimits;
        public LevelLimits LevelLimits => levelLimits;
    }

    [Serializable]
    public class LevelLimits
    {
        [SerializeField] private float leftLimit = -2;
        [SerializeField] private float rightLimit = 2;

        public float LeftLimit => leftLimit;
        public float RightLimit => rightLimit;
    }
}
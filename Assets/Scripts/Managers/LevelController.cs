using Configs;
using Location;
using UnityEngine;

namespace Managers
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private LevelLimits levelLimits;
        public LevelLimits LevelLimits => levelLimits;
    }
}
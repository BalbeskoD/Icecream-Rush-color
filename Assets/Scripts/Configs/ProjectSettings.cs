using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Configs/GameSettings", order = 0)]
    public class ProjectSettings : ScriptableObject
    {
        [SerializeField] private int targetFps = 60;
        [SerializeField] private bool multiTouchEnable = false;
      

        public int TargetFps => targetFps;

        public bool MultiTouchEnable => multiTouchEnable;
        
    }
}
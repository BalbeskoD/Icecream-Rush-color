using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "ColorsConfig", menuName = "Configs/ColorsConfig", order = 0)]
    public class ColorsConfig : ScriptableObject
    {
        [SerializeField] private ColorSet[] colors;
        public ColorSet[] Colors => colors;
    }
}
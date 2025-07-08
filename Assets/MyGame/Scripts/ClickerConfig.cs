using UnityEngine;

namespace MyGame.Scripts
{
    [CreateAssetMenu(fileName = "ClickerConfig", menuName = "Config/Clicker")]
    public class ClickerConfig : ScriptableObject
    {
        public int maxEnergy = 1000;
        public int energyPerTick = 10;
        public float energyRestoreInterval = 10f;
        public float autoClickInterval = 3f;
    }
}
using UnityEngine;

namespace MyGame.Scripts
{
    [CreateAssetMenu(fileName = "ClickerConfig", menuName = "Config/Clicker")]
    public class ClickerConfig : ScriptableObject
    {
        [field: SerializeField] public int MaxEnergy { get; private set; }
        [field: SerializeField] public int EnergyPerTick { get; private set; }
        [field: SerializeField] public float EnergyRestoreInterval { get; private set; }
        [field: SerializeField] public float AutoClickInterval { get; private set; }
    }
}
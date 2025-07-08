using UnityEngine;

namespace MyGame.Scripts
{
    public class ClickerModel
    {
        public int Currency { get; private set; }
        public int Energy { get; private set; }

        public readonly ClickerConfig Config;

        public ClickerModel(ClickerConfig config)
        {
            Config = config;
            Energy = config.maxEnergy;
        }

        public bool TryClick()
        {
            if (Energy <= 0) return false;
            Energy -= 1;
            Currency += 1;
            return true;
        }

        public void RestoreEnergy()
        {
            Energy = Mathf.Min(Energy + Config.energyPerTick, Config.maxEnergy);
        }
    }
}
using UnityEngine;

namespace MyGame.Scripts
{
    public class ClickerModel
    {
        public readonly ClickerConfig Config;

        public ClickerModel(ClickerConfig config)
        {
            Config = config;
            Energy = config.MaxEnergy;
        }

        public int Currency { get; private set; }
        public int Energy { get; private set; }

        public bool TryClick()
        {
            if (Energy <= 0) return false;
            
            Energy -= 1;
            Currency += 1;
            
            return true;
        }

        public void RestoreEnergy()
        {
            Energy = Mathf.Min(Energy + Config.EnergyPerTick, Config.MaxEnergy);
        }
    }
}
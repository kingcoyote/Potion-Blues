using System;
using System.Collections.Generic;
using System.Linq;
using PotionBlues.Definitions;
using UnityEngine;

namespace PotionBlues
{
    [Serializable]
    public class GameData
    {
        public string ProfileName;

        public List<UpgradeCardDefinition> Upgrades;
        public RunData ActiveRun;
        public List<RunData> RunHistory;

        public GameData(string profileName)
        {
            ProfileName = profileName;
            Upgrades = PotionBlues.I().Upgrades
                .Where(x => x.GetType() != typeof(DailyUpgradeDefinition))
                .Where(x => x.Rarity.Weight >= 0.5)
                .ToList();
        }

        public RunData GenerateRunData()
        {
            var data = new RunData();
            data.Upgrades = new List<RunUpgradeCard>()
            {
                new RunUpgradeCard(Resources.Load<UpgradeCardDefinition>("Upgrades/Door/Plain Door")) { IsSelected = true },
                new RunUpgradeCard (Resources.Load < UpgradeCardDefinition >("Upgrades/Counter/Plain Counter")) { IsSelected = true },
                new RunUpgradeCard(Resources.Load < UpgradeCardDefinition >("Upgrades/Cauldron/Plain Cauldron")) { IsSelected = true },
                new RunUpgradeCard(Resources.Load < UpgradeCardDefinition >("Upgrades/Ingredients/Mandrake Root")) { IsSelected = true }
            };
            data.Day = 1;
            data.RunDuration = 5;
            data.Gold = 100;
            data.Reputation = 5;

            return data;
        }
    }
}
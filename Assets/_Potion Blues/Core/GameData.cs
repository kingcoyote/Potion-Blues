using System;
using System.Collections.Generic;
using System.Linq;
using PotionBlues;
using PotionBlues.Definitions;
using UnityEditor.Rendering.Universal;
using UnityEngine;

[Serializable]
public class GameData
{
    public List<UpgradeCardDefinition> Upgrades;
    public RunData ActiveRun;
    public List<RunData> RunHistory;

    public static GameData Load(string name)
    {
        var gd = new GameData();
        gd.Upgrades = Resources.LoadAll<UpgradeCardDefinition>("Upgrades")
            .Where(x => x.GetType() != typeof(DailyUpgradeDefinition))
            .Where(x => x.Rarity.Weight >= 0.5)
            .ToList();

        return gd;
    }

    public void Save(string name)
    {

    }

    public RunData GenerateRunData()
    {
        var data = new RunData();
        data.Upgrades = new List<UpgradeCardDefinition>()
        {
            Resources.Load<UpgradeCardDefinition>("Upgrades/Door/Plain Door"),
            Resources.Load<UpgradeCardDefinition>("Upgrades/Counter/Plain Counter"),
            Resources.Load<UpgradeCardDefinition>("Upgrades/Cauldron/Plain Cauldron"),
            Resources.Load<UpgradeCardDefinition>("Upgrades/Ingredients/Mandrake Root")
        };
        data.Day = 1;
        data.RunDuration = 5;
        data.Gold = 100;
        data.Reputation = 100;

        return data;
    }
}

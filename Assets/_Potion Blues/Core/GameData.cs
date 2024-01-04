using System;
using System.Collections.Generic;
using PotionBlues;
using PotionBlues.Definitions;

[Serializable]
public class GameData
{
    public List<UpgradeCardDefinition> Upgrades;
    public RunData ActiveRun;
    public List<RunData> RunHistory;

    public GameData()
    {
               
    }

    public static RunData GenerateRunData()
    {
        throw new NotImplementedException();
    }
}

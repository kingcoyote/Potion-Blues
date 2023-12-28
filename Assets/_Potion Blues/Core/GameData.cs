using System;
using System.Collections.Generic;
using PotionBlues;
using PotionBlues.Definitions;

public class GameData
{
    public List<UpgradeCardDefinition> Upgrades;
    public RunData ActiveRun;
    public List<RunData> RunHistory;

    public GameData()
    {
               
    }

    public RunData GenerateRunData()
    {
        throw new NotImplementedException();
    }
}

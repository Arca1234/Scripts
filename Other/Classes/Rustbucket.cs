/*
name: Rustbucket (Class)
description: This script will get ProtoSartorium class.
tags: Rustbucket, Early Game, class
*/
//cs_include Scripts/CoreBots.cs
//cs_include Scripts/CoreFarms.cs
//cs_include Scripts/CoreAdvanced.cs
using Skua.Core.Interfaces;

public class Rustbucket
{
    public IScriptInterface Bot => IScriptInterface.Instance;
    public CoreBots Core => CoreBots.Instance;
    public CoreFarms Farm => new CoreFarms();
    public CoreAdvanced Adv = new CoreAdvanced();

    public void ScriptMain(IScriptInterface bot)
    {
        Core.SetOptions();

        GetRustbucket();

        Core.SetOptions(false);
    }

    public void GetRustbucket(bool rankUpClass = true)
    {
        if (Core.CheckInventory("Rustbucket"))
        {
            if (rankUpClass)
                Adv.RankUpClass("Rustbucket");
            return;
        }

        Core.AddDrop("Rustbucket");

        if (!Core.CheckInventory("Rustbucket"))
        {
            Core.GetMapItem(12756, 1, "crashsite");
            Bot.Wait.ForPickup("Rustbucket");
        }

        if (rankUpClass)
            Adv.RankUpClass("Rustbucket");
    }
}

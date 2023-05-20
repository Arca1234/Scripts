/*
name: null
description: null
tags: null
*/
//cs_include Scripts/CoreBots.cs
//cs_include Scripts/CoreFarms.cs
//cs_include Scripts/CoreAdvanced.cs
using Skua.Core.Interfaces;

public class CoreAwe
{
    private IScriptInterface Bot => IScriptInterface.Instance;
    private CoreBots Core => CoreBots.Instance;
    private CoreFarms Farm = new();
    private CoreAdvanced Adv = new();

    public void ScriptMain(IScriptInterface bot)
    {
        Core.RunCore();
    }

    public void GetAweRelic(string Item, int LegendQuest, int FragmentAmount, int ShardAmount, string Map, string Monster)
    {
        if (Core.CheckInventory($"{Item} Relic"))
            return;

        int QuestID;
        if (Bot.Flash.GetGameObject<int>("world.myAvatar.objData.intAQ") > 0)
        {
            Farm.BladeofAweREP(5, false);
            Farm.Experience(35);
            QuestID = LegendQuest + 1;
        }
        else
        {
            if (!Core.IsMember)
            {
                Farm.BladeofAweREP(10, false);
                Farm.Experience(55);
            }

            Core.BuyItem("museum", 1130, Core.IsMember ? "Legendary Awe Pass" : "Armor of Awe Pass");
            QuestID = Core.IsMember ? LegendQuest : LegendQuest + 2;
        }

        Core.EquipClass(ClassType.Solo);
        Core.AddDrop($"{Item} Shard");
        Core.FarmingLogger($"{Item} Fragment", ShardAmount);

        Core.RegisterQuests(QuestID);
        if (Map.ToLower() == "doomvault" || Map.ToLower() == "doomvaultb")
            Adv.KillUltra(Map, Map.ToLower().EndsWith('b') ? "r26" : "r5", "Left", Monster, $"{Item} Fragment", ShardAmount, false);
        else Adv.BoostHuntMonster(Map, Monster, $"{Item} Fragment", ShardAmount, false);
        Core.CancelRegisteredQuests();

        Core.BuyItem("museum", 1129, $"{Item} Relic");
    }
}

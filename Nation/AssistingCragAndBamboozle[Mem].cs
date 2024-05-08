/*
name: AssistingCragAndBamboozle[Mem]
description: null
tags: null
*/
//cs_include Scripts/CoreBots.cs
//cs_include Scripts/CoreFarms.cs
//cs_include Scripts/CoreDailies.cs
//cs_include Scripts/Nation/CoreNation.cs
using Skua.Core.Interfaces;
using Skua.Core.Models.Items;
using System.Collections.Generic;
using Skua.Core.Options;
using System.Linq;
using Skua.Core.Models.Quests;

public class AssistingCragAndBamboozle
{
    public IScriptInterface Bot => IScriptInterface.Instance;
    public CoreBots Core => CoreBots.Instance;
    public CoreDailies Daily = new();
    public CoreNation Nation = new();

    readonly Rewards[] rewardsToCheck = (Rewards[])Enum.GetValues(typeof(Rewards));
    readonly string[] ACaBItems = {
                    "Sword of Nulgath", "Gem of Nulgath", "Tainted Gem", "Dark Crystal Shard", "Diamond of Nulgath",
                    "Totem of Nulgath", "Blood Gem of the Archfiend", "Unidentified 19", "Elders' Blood", "Voucher of Nulgath",
                    "Voucher of Nulgath (non-mem)", "Archfiend's Favor", "Essence of Nulgath", "Nulgath Larvae",
                    "Gem of Domniation", "Fiend Seal"};



    public string OptionsStorage = "AssistingCragAndBamboozle";
    public bool DontPreconfigure = true;
    public List<IOption> Options = new()
    {
        new Option<Rewards>("PickReward", "Pick your reward", "This quest is a daily.. unless you have multiple sparrows blood then itll keep running.", Rewards.Get_whats_not_maxed),
        CoreBots.Instance.SkipOptions,
    };


    public void ScriptMain(IScriptInterface bot)
    {
        Quest QuestData = Core.EnsureLoad(5817);
        List<ItemBase> AcceptReqandQuestReq = QuestData.Requirements.Concat(QuestData.AcceptRequirements).ToList();
        Core.BankingBlackList.AddRange(AcceptReqandQuestReq.Select(item => item.ToString()).Concat(ACaBItems));

        Core.SetOptions();

        AssistingCandB(Bot.Config!.Get<Rewards>("PickReward"));

        Core.SetOptions(false);
    }

    public void AssistingCandB(Rewards reward = new())
    {
        if (!Core.IsMember || !Core.CheckInventory(Nation.CragName) || !Core.CheckInventory("Sparrow's Blood") && !Daily.CheckDaily(803, true, true, "Sparrow's Blood"))
            return;

        ItemBase? Item = Bot.Quests.EnsureLoad(5817)!.Rewards
        .FirstOrDefault(r => (int)reward == 1
        ? r.Quantity < r.MaxStack
        : r.ID == (int)reward);


        Core.AddDrop("Nulgath Larvae",
                     "Sword of Nulgath", "Gem of Nulgath", "Tainted Gem", "Dark Crystal Shard", "Diamond of Nulgath",
                     "Totem of Nulgath", "Blood Gem of the Archfiend", "Unidentified 19", "Elders' Blood", "Voucher of Nulgath", "Voucher of Nulgath (non-mem)");

        Core.FarmingLogger(Item!.Name, 1);
        bool continueFarming = true;
        while (continueFarming)
        {
            Core.EnsureAccept(5817);

            //Required to "Accept"
            if (!Core.CheckInventory("Tendurrr The Assistant"))
                Core.KillMonster("tercessuinotlim", "m2", "Left", "*", "Tendurrr The Assistant", isTemp: false);

            Daily.SparrowsBlood();
            if (!Core.CheckInventory("Sparrow's Blood"))
                Core.Logger("This bot requires you to have at least 1 Sparrow's Blood", stopBot: true);

            Nation.EssenceofNulgath(20);
            Nation.ApprovalAndFavor(100, 100);

            //medal required to get seals
            Nation.NationRound4Medal();
            Core.Logger("Accepting \"Nation Recruits: Seal Your Fate[4748]\"\n" +
            "to allow \"Fiend Seal\" to drop.");
            Core.EnsureAccept(4748);
            Core.HuntMonster("shadowblast", "Legion Fenrir", "Fiend Seal", 10, isTemp: false);

            if (Bot.Config!.Get<Rewards>("PickReward") == Rewards.Get_whats_not_maxed)
            {
                foreach (Rewards Reward in rewardsToCheck)
                {
                    string itemName = reward.ToString().Replace("_", " ");
                    int requiredCount = GetRequiredCount(reward);
                    Core.FarmingLogger(itemName, requiredCount);

                    if (!Core.CheckInventory(itemName, requiredCount))
                    {
                        Core.EnsureComplete(5817, (int)reward);

                        if (!Core.CheckInventory("Sparrow's Blood"))
                        {
                            continueFarming = false;
                            break;
                        }
                        Core.FarmingLogger(itemName, requiredCount);
                    }
                    else
                    {
                        Core.Logger($"{itemName} owned in max quant: {requiredCount}");
                    }
                }
            }
            else
            {
                Core.FarmingLogger(Item!.Name, Item.MaxStack);
                Core.ChainComplete(5817, (int)Bot.Config!.Get<Rewards>("PickReward"));

                if (!Core.CheckInventory("Sparrow's Blood"))
                {
                    continueFarming = false;
                }
            }
            if (!continueFarming)
                Core.Logger($"Not enough \"Sparrow's Blood\", please do the daily 1 more time (not today)");
        }
    }

    private static int GetRequiredCount(Rewards reward)
    {
        return reward switch
        {
            Rewards.Sword_of_Nulgath => 1,
            Rewards.Gem_of_Nulgath => 300,
            Rewards.Tainted_Gem or Rewards.Dark_Crystal_Shard or Rewards.Diamond_of_Nulgath => 1000,
            _ => 0, // Default case or handle other rewards as needed
        };
    }

    public enum Rewards
    {
        Blood_Gem_of_the_Archfiend = 22332,
        Gem_of_Nulgath = 6136,
        Elders_Blood = 5586,
        Totem_of_Nulgath = 5357,
        Voucher_of_Nulgath_non_mem = 4862,
        Voucher_of_Nulgath = 4861,
        Diamond_of_Nulgath = 4771,
        Dark_Crystal_Shard = 4770,
        Tainted_Gem = 4769,
        Unidentified_19 = 4752,
        Sword_of_Nulgath = 4670,
        Get_whats_not_maxed = 1
    }
}

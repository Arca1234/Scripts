//cs_include Scripts/CoreBots.cs
//cs_include Scripts/CoreFarms.cs
//cs_include Scripts/CoreAdvanced.cs
using RBot;

public class Paladin
{
    public ScriptInterface Bot => ScriptInterface.Instance;
    public CoreBots Core => CoreBots.Instance;
    public CoreFarms Farm = new CoreFarms();
    public CoreAdvanced Adv = new CoreAdvanced();

    public void ScriptMain(ScriptInterface bot)
    {
        Core.SetOptions();

        GetPaladin();

        Core.SetOptions(false);
    }

    public void GetPaladin(bool rankUpClass = true)
    {
        if (Core.CheckInventory("Paladin"))
            return;

        Core.BuyItem("necropolis", 26, "Warrior");
        Core.BuyItem("necropolis", 26, "Healer");

        Adv.rankUpClass("Warrior");
        Adv.rankUpClass("Healer");

        Farm.GoodREP(5);

        Core.BuyItem("necropolis", 26, "Paladin");

        if (rankUpClass)
            Adv.rankUpClass("Paladin");
    }
}
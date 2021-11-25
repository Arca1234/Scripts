//cs_include Scripts/CoreBots.cs
//cs_include Scripts/Legion/CoreLegion.cs
using RBot;

public class EmblemofDage
{
    public ScriptInterface Bot => ScriptInterface.Instance;
    public CoreBots Core => CoreBots.Instance;
    public CoreLegion Legion = new CoreLegion();
    public void ScriptMain(ScriptInterface bot)
    {
        Core.SetOptions();

        Legion.EmblemofDage();

        Core.SetOptions(false);
    }
}
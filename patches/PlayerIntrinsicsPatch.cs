using HarmonyLib;
using Miniscript;
using System;
using System.Reflection;

[HarmonyPatch]
public class PlayerIntrinsicsPatch
{
    [HarmonyPatch(typeof(PlayerIntrinsics), "AddInstrinsics")]
    class AddInstrinsicsPatch
    {
        private static bool intrinsicsAdded;

        static void Postfix()
        {
            if (intrinsicsAdded == true)
                return;
            intrinsicsAdded = true;
            Intrinsic.Create("bios_version").code = delegate (TAC.Context context, Intrinsic.Result partialResult)
            {
                Type gameConfigType = AccessTools.TypeByName("Util.GameConfig");
                FieldInfo fieldInfo = gameConfigType?.GetField("gameVersion", BindingFlags.Public | BindingFlags.Static);
                return new Intrinsic.Result((string)fieldInfo.GetValue(null));
            };
        }
    }
}
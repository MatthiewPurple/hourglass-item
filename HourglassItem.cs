using MelonLoader;
using HarmonyLib;
using Il2Cpp;
using hourglass_item;
using Il2Cppresult2_H;
using Il2Cppnewdata_H;
using Il2Cppfacility_H;

[assembly: MelonInfo(typeof(HourglassItem), "Hourglass item", "1.0.0", "Matthiew Purple")]
[assembly: MelonGame("アトラス", "smt3hd")]

namespace hourglass_item;
public class HourglassItem : MelonMod
{
    // After creating the shop
    [HarmonyPatch(typeof(fclShopCalc), nameof(fclShopCalc.shpCreateItemList))]
    private class Patch
    {
        public static void Postfix(ref fclDataShop_t pData)
        {
            pData.BuyItemList[pData.BuyItemCnt++] = 57; // Adds the hourglass
        }
    }

    // After getting the name of a skill
    [HarmonyPatch(typeof(datItemName), nameof(datItemName.Get))]
    private class Patch2
    {
        public static void Postfix(ref int id, ref string __result)
        {
            // If searching for the hourglass, returns its name
            if (id == 57) __result = "Hourglass";
        }
    }

    // After getting the name of a skill
    [HarmonyPatch(typeof(datItemHelp_msg), nameof(datItemHelp_msg.Get))]
    private class Patch3
    {
        public static void Postfix(ref int id, ref string __result)
        {
            // If searching for the hourglass, returns its description
            if (id == 57) __result = "Lets time pass \nuntil a full Kagutsuchi.";
        }
    }

    // After apply a skill effect outside of battle
    [HarmonyPatch(typeof(datCalc), nameof(datCalc.datExecSkill))]
    private class Patch4
    {
        public static void Postfix(ref int nskill)
        {
            // If using an hourglass, set the Kagutsuchi phase to full
            if (nskill == 78) dds3GlobalWork.DDS3_GBWK.Moon.Age = 8;
        }
    }

    // When launching the game
    public override void OnInitializeMelon()
    {
        // Creates the item
        datItem.tbl[57].flag = 4; // Normal item
        datItem.tbl[57].price = 500; // 500 macca each
        datItem.tbl[57].skillid = 78; // Triggers the skill n°78
        datItem.tbl[57].use = 1; // Can only be used out of battle

        // Creates the skill n°78
        datSkill.tbl[78].capacity = 4;
        datSkill.tbl[78].skillattr = 15; // Utility skill
        datNormalSkill.tbl[78].koukatype = 1; // Not Physical
        datNormalSkill.tbl[78].program = 14; // Phase shift
        datNormalSkill.tbl[78].targetcntmax = 1;
        datNormalSkill.tbl[78].targetcntmin = 1;
        datNormalSkill.tbl[78].targettype = 3; // Field
    }
}

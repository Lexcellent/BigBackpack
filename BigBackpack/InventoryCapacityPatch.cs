using System;
using HarmonyLib;
using UnityEngine;

namespace BigBackpack
{
    [HarmonyPatch(typeof(CharacterMainControl))]
    public static class InventoryCapacityPatch
    {
        [HarmonyPatch("InventoryCapacity", MethodType.Getter)]
        [HarmonyPostfix]
        public static void PostfixInventoryCapacity(CharacterMainControl __instance, ref float __result)
        {
            try
            {
                // Debug.Log($"BigBackpack模组：角色所属阵营{__instance.Team},原始背包容量：{__result}");
                if (__instance != null && __instance.Team == Teams.player)
                {
                    __result += ModBehaviour.InventoryCapacityIncrease;
                }
            }
            catch (Exception e)
            {
                Debug.Log($"BigBackpack模组：错误：{e.Message}");
            }
        }

        [HarmonyPatch("MaxWeight", MethodType.Getter)]
        [HarmonyPostfix]
        public static void PostfixMaxWeight(CharacterMainControl __instance, ref float __result)
        {
            try
            {
                if (__instance != null && __instance.Team == Teams.player)
                {
                    __result += ModBehaviour.MaxWeightIncrease;
                }
            }
            catch (Exception e)
            {
                Debug.Log($"BigBackpack模组：错误：{e.Message}");
            }
        }
    }
}
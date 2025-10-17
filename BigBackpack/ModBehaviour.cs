using System;
using UnityEngine;
using HarmonyLib;

namespace BigBackpack
{
    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        private bool _isInit = false;
        private string? _harmonyId = null;

        protected override void OnAfterSetup()
        {
            Debug.Log("BigBackpack模组：OnAfterSetup方法被调用");
            if (!_isInit)
            {
                Debug.Log("BigBackpack模组：执行修补");
                _harmonyId = Harmony.CreateAndPatchAll(typeof(BigBackpack.ModBehaviour)).Id;
                _isInit = true;
                Debug.Log("BigBackpack模组：修补完成");
            }
        }

        protected override void OnBeforeDeactivate()
        {
            Debug.Log("BigBackpack模组：OnBeforeDeactivate方法被调用");
            if (_isInit)
            {
                Debug.Log("BigBackpack模组：执行取消修补");
                if (_harmonyId != null)
                {
                    Harmony.UnpatchID(_harmonyId);
                }
                Debug.Log("BigBackpack模组：执行取消修补完毕");
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(CharacterMainControl), "InventoryCapacity", MethodType.Getter)]
        public static void BigBackpackPatch(CharacterMainControl __instance,ref float __result)
        {
            try
            {
                // Debug.Log($"BigBackpack模组：角色所属阵营{__instance.Team},原始背包容量：{__result}");
                if (__instance != null && __instance.Team == Teams.player)
                {
                    __result += 200;
                }
            }
            catch (Exception e)
            {
                Debug.Log($"BigBackpack模组：错误：{e.Message}");
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(CharacterMainControl), "MaxWeight", MethodType.Getter)]
        public static void MaxWeightPatch(CharacterMainControl __instance,ref float __result)
        {
            try
            {
                if (__instance != null && __instance.Team == Teams.player)
                {
                    __result += 200;
                }
            }
            catch (Exception e)
            {
                Debug.Log($"BigBackpack模组：错误：{e.Message}");
            }
        }
    }
}
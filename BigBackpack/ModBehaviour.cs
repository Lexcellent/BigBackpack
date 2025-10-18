using System;
using UnityEngine;
using HarmonyLib;
using System.IO;

namespace BigBackpack
{
    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        private bool _isInit = false;
        private string? _harmonyId = null;
        private static float _inventoryCapacityIncrease = 200f;
        private static float _maxWeightIncrease = 200f;

        protected override void OnAfterSetup()
        {
            Debug.Log("BigBackpack模组：OnAfterSetup方法被调用");
            if (!_isInit)
            {
                LoadConfig();
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

        private void LoadConfig()
        {
            try
            {
                string configPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "info.ini");
                if (File.Exists(configPath))
                {
                    string[] lines = File.ReadAllLines(configPath);
                    foreach (string line in lines)
                    {
                        if (line.StartsWith("InventoryCapacity="))
                        {
                            string value = line.Substring("InventoryCapacity=".Length).Trim();
                            if (float.TryParse(value, out float capacity))
                            {
                                _inventoryCapacityIncrease = capacity;
                                Debug.Log($"BigBackpack模组：已从配置文件读取InventoryCapacity值: {capacity}");
                            }
                        }
                        else if (line.StartsWith("MaxWeight="))
                        {
                            string value = line.Substring("MaxWeight=".Length).Trim();
                            if (float.TryParse(value, out float weight))
                            {
                                _maxWeightIncrease = weight;
                                Debug.Log($"BigBackpack模组：已从配置文件读取MaxWeight值: {weight}");
                            }
                        }
                    }
                }
                else
                {
                    Debug.Log("BigBackpack模组：未找到config.ini文件，使用默认值");
                }
            }
            catch (Exception e)
            {
                Debug.Log($"BigBackpack模组：读取配置文件时出错：{e.Message}，使用默认值");
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(CharacterMainControl), "InventoryCapacity", MethodType.Getter)]
        public static void BigBackpackPatch(CharacterMainControl __instance, ref float __result)
        {
            try
            {
                // Debug.Log($"BigBackpack模组：角色所属阵营{__instance.Team},原始背包容量：{__result}");
                if (__instance != null && __instance.Team == Teams.player)
                {
                    __result += _inventoryCapacityIncrease;
                }
            }
            catch (Exception e)
            {
                Debug.Log($"BigBackpack模组：错误：{e.Message}");
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(CharacterMainControl), "MaxWeight", MethodType.Getter)]
        public static void MaxWeightPatch(CharacterMainControl __instance, ref float __result)
        {
            try
            {
                if (__instance != null && __instance.Team == Teams.player)
                {
                    __result += _maxWeightIncrease;
                }
            }
            catch (Exception e)
            {
                Debug.Log($"BigBackpack模组：错误：{e.Message}");
            }
        }
    }
}

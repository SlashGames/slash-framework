namespace Slash.Unity.StrangeIoC.Modules.Utils
{
    using System.Collections.Generic;
    using Slash.Unity.StrangeIoC.Configs;
    using UnityEngine;

    public static class ModuleUtils
    {
        public static IEnumerable<IModuleInstaller> FindModuleConfigs(GameObject rootGameObject)
        {
            // Find module configs.
            return rootGameObject.GetComponentsInChildren<StrangeConfig>();
        }
    }
}
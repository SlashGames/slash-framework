namespace Slash.Unity.Editor.Common.Utils
{
    using System.Collections.Generic;

    using UnityEditor;

    using UnityEditorInternal;

    using UnityEngine;

    public static class LayerMaskUtils
    {
        private static readonly List<int> layerNumbers = new List<int>();

        public static LayerMask LayerMaskField(string label, LayerMask layerMask)
        {
            var layers = InternalEditorUtility.layers;

            layerNumbers.Clear();

            foreach (var layerName in layers)
            {
                layerNumbers.Add(LayerMask.NameToLayer(layerName));
            }

            var maskWithoutEmpty = 0;
            for (var i = 0; i < layerNumbers.Count; i++)
            {
                if (((1 << layerNumbers[i]) & layerMask.value) > 0)
                {
                    maskWithoutEmpty |= (1 << i);
                }
            }

            maskWithoutEmpty = EditorGUILayout.MaskField(label, maskWithoutEmpty, layers);

            var mask = 0;
            for (var i = 0; i < layerNumbers.Count; i++)
            {
                if ((maskWithoutEmpty & (1 << i)) > 0)
                {
                    mask |= (1 << layerNumbers[i]);
                }
            }
            layerMask.value = mask;

            return layerMask;
        }
    }
}
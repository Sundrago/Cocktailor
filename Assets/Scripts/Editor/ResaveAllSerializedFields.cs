using UnityEditor;
using UnityEngine;

namespace Cocktailor
{
    internal class EditorForceSaveAll : Editor
    {
        [MenuItem("Tools/Force Reserialize Assets")]
        private static void ForceReserialzed()
        {
            AssetDatabase.ForceReserializeAssets();
        }
    }
}
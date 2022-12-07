using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;

namespace PureOdinTools
{
    public class PureOdinTools : OdinMenuEditorWindow
    {
        [MenuItem("PureTools/PureOdinTools")]
        private static void OpenWindow()
        {
            var window = GetWindow<PureOdinTools>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            OdinMenuTree tree = new OdinMenuTree(supportsMultiSelect: false)
            {
                { "Temporary", TemporaryTool.Instance, EditorIcons.Clock },
                { "Auto Set Asset Bundle", AutoSetAssetBundleTool.Instance, EditorIcons.Airplane },
            };

            tree.SortMenuItemsByName();



            return tree;
        }
    }
}

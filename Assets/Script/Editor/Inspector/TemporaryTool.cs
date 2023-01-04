using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LitJson;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Sirenix.Utilities;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Util;

namespace PureOdinTools
{
    [GlobalConfig(assetPath: "Odin")]
    public class TemporaryTool : GlobalConfig<TemporaryTool>
    {
        [Button("Check Is Match")]
        public void RegexTest(string content, string pattern)
        {
            Debug.Log(string.Format("content = {0} pattern = {1}", content, pattern));
            Regex regex = new Regex(pattern);
            Debug.Log(regex.IsMatch(content));
        }
    }
}

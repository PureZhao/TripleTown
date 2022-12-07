using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

public static class ComponentEx
{
    public static void MoveToTop(this Component comp)
    {

        if (comp == null)
        {
            return;
        }
#if UNITY_EDITOR
        List<Component> comps = new List<Component>(comp.transform.GetComponents<Component>());
        // Only To Second Because the first Component is Transform, this is always first Component
        while (comps.IndexOf(comp) > 1)
        {
            ComponentUtility.MoveComponentUp(comp);
            comps.Clear();
            comps.AddRange(comp.transform.GetComponents<Component>());
        }
        comps.Clear();
#endif
    }

    public static void MoveToBottom(this Component comp)
    {
        if(comp == null)
        {
            return;
        }
#if UNITY_EDITOR
        List<Component> comps = new List<Component>(comp.transform.GetComponents<Component>());
        while (comps.IndexOf(comp) < comps.Count - 1)
        {
            ComponentUtility.MoveComponentDown(comp);
            comps.Clear();
            comps.AddRange(comp.transform.GetComponents<Component>());
        }
        comps.Clear();
#endif
    }

    public static void MoveToIndex(this Component comp, int index)
    {
        if (comp == null)
        {
            return;
        }
#if UNITY_EDITOR
        List<Component> comps = new List<Component>(comp.transform.GetComponents<Component>());
        if (index >= comps.Count - 1)
        {
            ComponentEx.MoveToBottom(comp);
            return;
        }
        if(index <= 1)
        {
            ComponentEx.MoveToTop(comp);
            return;
        }
        while (comps.IndexOf(comp) != index)
        {
            if(comps.IndexOf(comp) > index)
            {
                ComponentUtility.MoveComponentUp(comp);
            }
            else
            {
                ComponentUtility.MoveComponentDown(comp);
            }
            comps.Clear();
            comps.AddRange(comp.transform.GetComponents<Component>());
        }
        comps.Clear();
#endif
    }

}

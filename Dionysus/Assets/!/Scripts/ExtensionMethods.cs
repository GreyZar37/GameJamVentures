using System.Collections.Generic;
using UnityEngine;
using System;
using Random = System.Random;

public static class ExtensionMethods
{
    private static readonly Random random = new Random();
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static List<T> GetChildrenRecursive<T>(this Transform _this, bool returnParent = true) where T : class
    {
        List<T> returns = new List<T>();
        if (returnParent == true)
        {
            T[] components = _this.GetComponents<T>();
            if (components != null)
            {
                foreach (T component in components)
                {
                    returns.Add(component);
                }
            }
        }

        foreach (Transform t in _this)
        {
            returns.AddRange(t.GetChildrenRecursive<T>(true));
        }

        return returns;
    }
}
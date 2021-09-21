using Playground.IoC;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Playground.Generation
{
    public class TypeColorProvider : MonoBehaviour, IService
    {
        private Dictionary<Type, Color> typeColor;

        private void Awake()
        {
            typeColor = new Dictionary<Type, Color>();
        }

        public Color GetColor(Type type)
        {
            if (typeColor.TryGetValue(type, out var col))
            {
                return col;
            }

            var hue = (((double)type.GUID.GetHashCode() / int.MaxValue) + 1.0) / 2.0;
            var color = Color.HSVToRGB((float)hue, 1, 1);
            typeColor.Add(type, color);
            return color;
        }
    }
}

using System;
using UnityEngine;

namespace MyGame.Scripts.Core
{
    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            var newJson = $"{{\"array\":{json}}}";
            return JsonUtility.FromJson<Wrapper<T>>(newJson).array;
        }

        [Serializable] 
        private class Wrapper<T>
        {
            public T[] array;
        }
    }
}
using System;
using UnityEngine;

namespace MyGame.Scripts
{
    public static class JsonHelper
    {
        [Serializable]
        private class Wrapper<T>
        {
            public T[] array;
        }

        public static T[] FromJson<T>(string json)
        {
            var newJson = $"{{\"array\":{json}}}";
            return JsonUtility.FromJson<Wrapper<T>>(newJson).array;
        }
    }
}
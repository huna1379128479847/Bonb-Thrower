using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BombThrower.Resource
{
    /// <summary>
    /// 外部からリソースを取得するためのヘルパークラス
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResourceHelper<T> where T : UnityEngine.Object
    {
        private readonly Type _parent;

        public ResourceHelper(Type parent)
        {
            if (parent == null)
            {
                throw new ArgumentNullException(nameof(parent), "Parent type cannot be null.");
            }
            _parent = parent;
        }

        /// <summary>
        /// 指定した型の静的フィールドからリソースを取得し、辞書形式で返す。
        /// </summary>
        /// <returns>フィールド名とそのリソースの辞書</returns>
        public Dictionary<string, T> GetResources()
        {
            var result = new Dictionary<string, T>();

            // 静的フィールドを取得
            var fields = _parent.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            foreach (var field in fields)
            {
                if (field.Name == "ResourceHelper") continue;
                // フィールドが指定された型 T である場合のみ追加
                if (field.FieldType == typeof(T))
                {
                    var fieldValue = field.GetValue(null) as T;
                    if (fieldValue != null)
                    {
                        result[field.Name] = fieldValue;
                    }
                    else
                    {
                        Debug.LogWarning($"ResourceHelper: Field '{field.Name}' is null or could not be cast to {typeof(T).Name}.");
                    }
                }
                else
                {
                    Debug.LogWarning($"ResourceHelper: Field '{field.Name}' is not of type {typeof(T).Name} and was skipped.");
                }
            }
            return result;
        }
    }
}

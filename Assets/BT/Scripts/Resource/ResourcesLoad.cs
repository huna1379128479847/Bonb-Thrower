using System;
using UnityEngine;
using System.Reflection;
using System.IO;

namespace BombThrower.Resource
{
    internal static class ResourcesLoader
    {
        /// <summary>
        /// 指定された型の全てのUnityEngine.Objectフィールドに対してリソースをロードし、設定します。
        /// フィールド名はリソース名と一致している必要があります。
        /// 静的クラス限定
        /// </summary>
        /// <param name="type">対象オブジェクトの型</param>
        /// <param name="path">カスタムパス</param>
        public static void ResourcesSupport(Type type, string path = default)
        {

            // パブリックおよび非パブリック、インスタンスおよび静的フィールドを取得
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

            foreach (var field in fields)
            {
                Type fieldType = field.FieldType;
                string resourceName = field.Name;
                if (resourceName == "ResourceHelper") continue;
                object loadedResource = null;
                // UnityEngine.Objectのサブクラスかどうかをチェック
                if (typeof(UnityEngine.Object).IsAssignableFrom(fieldType))
                {

                    loadedResource = InternalUnityObjectResourcesLoader(fieldType, resourceName, path);

                    if (loadedResource != null)
                    {
                        field.SetValue(null, loadedResource);
                    }
                    else
                    {
                        Debug.LogWarning($"ResourcesSupport: Failed to load resource for field '{field.Name}'.");
                    }
                }
                else if (false)
                {

                }
                else
                {
                    Debug.LogWarning($"ResourcesSupport: Failed to load resource for Type '{fieldType}'.");
                }
            }
        }

        /// <summary>
        /// 指定された型とファイル名に基づいてリソースをロードします。
        /// </summary>
        /// <param name="type">リソースの型</param>
        /// <param name="fileName">リソースのファイル名（Resourcesフォルダ内のパス）</param>
        /// <returns>ロードされたリソース。見つからない場合はnull。</returns>
        private static UnityEngine.Object InternalUnityObjectResourcesLoader(Type type, string fileName, string path = "")
        {
            // パスが空でなければ、ディレクトリとファイル名を組み合わせる
            string fullPath = string.IsNullOrEmpty(path) ? fileName : Path.Combine(path, fileName);

            Debug.Log($"Attempting to load '{fullPath}' from Resources as {type.Name}.");

            // Resourcesからリソースをロード
            var resource = Resources.Load(fullPath, type);

            if (resource != null)
            {
                Debug.Log($"File loaded successfully: '{fullPath}' as {type.Name}.");
                return resource;
            }
            else
            {
                Debug.LogError($"Could not find file: '{fullPath}' in Resources as {type.Name}.");
                return null;
            }
        }


        /// <summary>
        /// Unity.Object以外のファイル読み込み。未開発
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        private static T InternalResourcesLoader<T>(string fileName, string extension)
        {
            Debug.Log($"Attempting to load '{fileName}' from Resources as {nameof(T)}.");
            T resource = default;

            return resource;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System;

namespace BombThrower.Utilities
{
    /// <summary>
    /// ゲーム内で使う汎用的なヘルパーメソッドをまとめたクラス。
    /// </summary>
    public static class CollectionsHelper
    {
        private static readonly Random random = new Random();
        private static readonly object randomLock = new object();

        /// <summary>
        /// リストからランダムに1つの要素を選ぶ。
        /// </summary>
        /// <typeparam name="T">リストの要素の型。</typeparam>
        /// <param name="values">要素を持つリスト。</param>
        /// <returns>ランダムに選ばれた要素。リストが空の場合は例外を投げる。</returns>
        public static T RandomPick<T>(List<T> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), "The provided list is null.");
            if (values.Count == 0)
                throw new InvalidOperationException("The provided list is empty. Cannot pick a random element.");

            lock (randomLock)
            {
                return values[random.Next(values.Count)];
            }
        }

        /// <summary>
        /// 辞書の値からランダムに1つの要素を選ぶ。
        /// </summary>
        public static V RandomPick<T, V>(Dictionary<T, V> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), "The provided dictionary is null.");
            if (values.Count == 0)
                throw new InvalidOperationException("The provided dictionary is empty. Cannot pick a random value.");

            lock (randomLock)
            {
                return values.Values.ElementAt(random.Next(values.Count));
            }
        }

        /// <summary>
        /// 辞書のキーからランダムに1つの要素を選ぶ。
        /// </summary>
        public static T RandomKey<T, V>(Dictionary<T, V> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), "The provided dictionary is null.");
            if (values.Count == 0)
                throw new InvalidOperationException("The provided dictionary is empty. Cannot pick a random key.");

            lock (randomLock)
            {
                return values.Keys.ElementAt(random.Next(values.Count));
            }
        }
    }
}
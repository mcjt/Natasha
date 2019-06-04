﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Natasha
{
    public static class Snapshot<T>
    {
        public static readonly ConcurrentDictionary<T, T> SnapshotCache;

        static Snapshot() => SnapshotCache = new ConcurrentDictionary<T, T>();
        public static void MakeSnapshot(T needSnapshot)
        {
            SnapshotCache[needSnapshot] = DeepClone.Clone(needSnapshot);
        }

        public static Func<T, T, Dictionary<string, DiffModel>> CompareFunc;

        public static Dictionary<string, DiffModel> Diff(T newInstance, T oldInstance)
        {
            if (CompareFunc == null)
            {
                CompareFunc = (Func<T, T, Dictionary<string, DiffModel>>)(new SnapshotBuilder(typeof(T)).Create());
            }
            return CompareFunc(newInstance, oldInstance);
        }
        public static bool IsDiffernt(T instance)
        {
            return Compare(instance).Count != 0;
        }

        public static Dictionary<string, DiffModel> Compare(T instance)
        {
            if (CompareFunc == null)
            {
                CompareFunc = (Func<T, T, Dictionary<string, DiffModel>>)(new SnapshotBuilder(typeof(T)).Create());
            }
            return CompareFunc(instance, SnapshotCache[instance]);
        }
    }
    public static class Snapshot
    {
        public static Dictionary<string, DiffModel> Diff<T>(T newInstance, T oldInstance)
        {
            return Snapshot<T>.Diff(newInstance, oldInstance);
        }
        public static bool IsDiffernt<T>(T instance)
        {
            return Snapshot<T>.Compare(instance).Count != 0;
        }
        public static Dictionary<string, DiffModel> Compare<T>(T instance)
        {
            return Snapshot<T>.Compare(instance);
        }
        public static void MakeSnapshot<T>(T needSnapshot)
        {
            Snapshot<T>.MakeSnapshot(needSnapshot);
        }
    }
}

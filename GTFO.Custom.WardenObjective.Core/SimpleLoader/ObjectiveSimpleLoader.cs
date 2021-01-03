﻿using BepInEx;
using System;
using System.IO;
using System.Reflection;

namespace CustomObjectives.SimpleLoader
{
    internal static class ObjectiveSimpleLoader
    {
        public static void Setup()
        {
            var lookupPaths = new string[]
            {
                Paths.PluginPath,
                Path.Combine(Paths.PluginPath, "CustomObjectives")
            };

            foreach (var path in lookupPaths)
            {
                Logger.Verbose($"Searching Plugin Path: {path}");

                if (!Directory.Exists(path))
                    continue;

                var files = Directory.GetFiles(path, "*.dll", SearchOption.TopDirectoryOnly);
                foreach (var file in files)
                {
                    try
                    {
                        var asm = Assembly.LoadFile(file);
                        LoadAssembly(asm);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }

        private static void LoadAssembly(Assembly asm)
        {
            var attribute = asm.GetCustomAttribute<CustomObjectiveAttribute>();

            if (attribute == null)
                return;

            var type = attribute.Entry;

            if (type == null)
                return;

            if (type.IsAbstract)
                return;

            if (type.BaseType == null)
                return;

            if (type.BaseType != typeof(ObjectiveSimpleEntry))
                return;

            Logger.Verbose("Loading Simple Entry dll: {0} / type: {1}", asm.GetName().Name, type.Name);
            var entry = Activator.CreateInstance(type) as ObjectiveSimpleEntry;
            entry.OnStart();
        }
    }
}
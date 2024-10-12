﻿using MNetUtil.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;

namespace EpohUI.Core
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int port = 8080;

            AllServer fileServer = new AllServer(port);
            fileServer.Start().Wait();
        }
    }

    class AllServer : FileServer
    {

        internal AllServer(int port) : base(port, "")
        {
        }

        public override void ProcessRequest(HttpListenerContext context)
        {
            var url = context.Request.Url;
            if (url.AbsolutePath.Contains("/api/"))
            {
                ProcessApi(context);
                return;
            }
            base.ProcessRequest(context);
        }

        private void ProcessApi(HttpListenerContext context)
        {
            var apiUri = context.Request.Url.AbsolutePath.TrimStart("/api/".ToCharArray());
            var methodId = DllHelper.GetMethodId(apiUri);
            var result = DllHelper.Invoke(methodId, new object[] { });
            using (StreamWriter streamWriter = new StreamWriter(context.Response.OutputStream))
            {
                streamWriter.WriteLine(result.ToString());
                context.Response.ContentType = "text/plain; charset=UTF-8";
                context.Response.StatusCode = 200;
            }
            context.Response.Close();
        }

    }

    class DllHelper
    {

        private static readonly object _lock = new object();
        private static readonly Dictionary<string, Assembly> _assemblyCache = new Dictionary<string, Assembly>();
        private static readonly Dictionary<string, Type> _typeCache = new Dictionary<string, Type>();
        private static readonly ConcurrentDictionary<string, MethodInfo> _methodCache = new ConcurrentDictionary<string, MethodInfo>();

        static DllHelper()
        {
            lock (_lock)
            {
                LoadAssembly();
                LoadMethod();
            }
        }

        private static void LoadAssembly()
        {
            var dlls = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "DLLs"), "*.dll");
            foreach (var dll in dlls)
            {
                try
                {
                    var assembly = Assembly.LoadFrom(dll);
                    _assemblyCache[dll] = assembly;
                }
                catch
                {

                }
            }
        }

        private static void LoadMethod()
        {
            foreach (var assembly in _assemblyCache.Values)
            {
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    _typeCache[type.FullName] = type;
                }
            }
        }

        public static string GetMethodId(string _)
        {
            return "TODO";
        }

        public static object Invoke(string methodId, params object[] args)
        {
            string typeName;
            Type type;
            _methodCache.TryGetValue(methodId, out var method);

            if (method == null)
            {
                lock (_lock)
                {
                    _methodCache.TryGetValue(methodId, out method);
                    if (method == null)
                    {
                        ParseMethodId(methodId, out typeName, out string methodName);
                        _typeCache.TryGetValue(typeName, out type);
                        if (type == null)
                        {
                            throw new ArgumentException($"Type did not exist: {typeName}");
                        }
                        try
                        {
                            method = type.GetMethod(methodName);
                        }
                        catch
                        {
                            throw new ArgumentException($"Method did not exist: {methodId}");
                        }
                        _methodCache[methodId] = method;
                    }
                }
            }
            if (method.IsStatic)
            {
                return method.Invoke(null, args);
            }

            ParseMethodId(methodId, out typeName, out _);
            type = _typeCache[typeName];
            var instance = Activator.CreateInstance(type);
            return method.Invoke(instance, args);
        }

        private static void ParseMethodId(string methodId, out string typeName, out string methodName)
        {
            var methodIdSplit = methodId.Split('#');
            typeName = methodIdSplit[0];
            methodName = methodIdSplit[1];
        }

    }

}

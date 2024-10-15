using MNetUtil.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EpohUI.Core
{
    internal class Program
    {
        static void Main(string[] _)
        {
            var pidPath = Path.Combine(Directory.GetCurrentDirectory(), ".pid");
            var pid = File.Exists(pidPath) ? File.ReadAllText(pidPath) : null;
            if (!string.IsNullOrEmpty(pid))
            {
                try
                {
                    var process = Process.GetProcessById(int.Parse(pid));
                    if (process != null && !process.HasExited)
                    {
                        process.Kill();
                    }
                }
                catch { }
            }

            int port = 33;

            AllServer fileServer = new AllServer(port);
            var task = Task.Run(async () =>
            {
                await fileServer.Start();
            });
            Process.Start($"http://localhost:{port}");
            File.WriteAllText(pidPath, Process.GetCurrentProcess().Id.ToString());
            task.Wait();
        }
    }

    class AllServer : FileServer
    {

        internal AllServer(int port) : base(port, "")
        {
        }

        public override void ProcessRequest(HttpListenerContext context)
        {
            if (ProcessApi(context))
            {
                return;
            }
            base.ProcessRequest(context);
        }

        private bool ProcessApi(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;
            var url = request.Url;
            var path = url.AbsolutePath;
            path = HttpUtility.UrlDecode(path);
            var apiPrefix = "/api/";
            if (!path.StartsWith(apiPrefix))
            {
                return false;
            }
            var queryString = HttpUtility.ParseQueryString(url.Query, Encoding.UTF8);

            var apiUri = path.TrimStart(apiPrefix.ToCharArray());
            apiUri = apiUri.Trim('/');

            var invokeParams = queryString["invokeParams"];
            using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
            {
                invokeParams += reader.ReadToEnd();
            }

            using (var streamWriter = new StreamWriter(response.OutputStream, request.ContentEncoding))
            {
                MethodHelper.GetMethodId(apiUri, out var methodId);
                if (methodId != null)
                {
                    try
                    {
                        var result = MethodHelper.Invoke(methodId, invokeParams);
                        if (result is Stream resultStream)
                        {
                            response.StatusCode = 200;
                            response.ContentType = "application/octet-stream";
                            resultStream.CopyTo(response.OutputStream);
                        }
                        else
                        {
                            response.StatusCode = 200;
                            response.ContentType = "text/plain; charset=UTF-8";
                            streamWriter.WriteLine(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        streamWriter.WriteLine(ex.ToString());
                    }
                }
                else
                {
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }

            response.Close();
            return true;
        }

    }

    public class MethodHelper
    {

        private static readonly object _lock = new object();
        private static readonly Dictionary<string, string> _methodIdMap = new Dictionary<string, string>();
        private static readonly Dictionary<string, Assembly> _assemblyCache = new Dictionary<string, Assembly>();
        private static readonly Dictionary<string, Type> _typeCache = new Dictionary<string, Type>();
        private static readonly ConcurrentDictionary<string, MethodInfo> _methodCache = new ConcurrentDictionary<string, MethodInfo>();

        static MethodHelper()
        {
            lock (_lock)
            {
                LoadAssembly();
                LoadType();
                LoadMethodIdMap();
            }
        }

        private static void LoadAssembly()
        {
            var dllPath = Path.Combine(Directory.GetCurrentDirectory(), "DLLs");
            Directory.CreateDirectory(dllPath);
            var dlls = Directory.GetFiles(dllPath, "*.dll");
            foreach (var dll in dlls)
            {
                try
                {
                    if (_assemblyCache.ContainsKey(dll))
                    {
                        continue;
                    }
                    var assembly = Assembly.LoadFrom(dll);
                    _assemblyCache[dll] = assembly;
                }
                catch { }
            }
        }

        private static void LoadType()
        {
            foreach (var assembly in _assemblyCache.Values)
            {
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    if (_typeCache.ContainsKey(type.FullName))
                    {
                        continue;
                    }
                    _typeCache[type.FullName] = type;
                }
            }
        }

        private static void LoadMethodIdMap()
        {
            foreach (var type in _typeCache.Values)
            {
                var method = type.GetMethod("GetMethodIdMap");
                if (method == null || !method.IsStatic)
                {
                    continue;
                }
                object ret = method.Invoke(null, new object[0]);
                if (ret == null)
                {
                    continue;
                }
                var lines = ret.ToString().Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (var line in lines)
                {
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }
                    var kv = line.Split('=');
                    var k = kv[0].Trim();
                    if (_methodIdMap.ContainsKey(k))
                    {
                        continue;
                    }
                    _methodIdMap[k] = kv[1].Trim();
                }
            }
        }

        public static void GetMethodId(string uri, out string methodId)
        {
            _methodIdMap.TryGetValue(uri, out methodId);
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
                        method = type.GetMethod(methodName);
                        _methodCache[methodId] = method ?? throw new ArgumentException($"Method did not exist: {methodId}");
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

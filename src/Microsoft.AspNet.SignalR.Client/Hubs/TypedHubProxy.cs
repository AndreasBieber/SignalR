using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.AspNet.SignalR.Client.Hubs
{
    public class TypedHubProxy<TServerContract, TClientContract> : ITypedHubProxy<TServerContract, TClientContract>
            where TServerContract : class
            where TClientContract : class
    {
        private readonly IHubProxy _legacyHubProxy;
        private readonly MethodInfo _invokeEventMethod;

        public TypedHubProxy(IHubProxy legacyHubProxy)
        {
#if NETFX_CORE
            if (!typeof(TServerContract).GetTypeInfo().IsInterface)
#else
            if (!typeof(TServerContract).IsInterface)
#endif
            {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, Resources.Error_NotAnInterface, typeof(TServerContract).Name));
            }
#if NETFX_CORE
            if (!typeof(TServerContract).GetTypeInfo().IsInterface)
#else
            if (!typeof(TClientContract).IsInterface)
#endif
            {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, Resources.Error_NotAnInterface, typeof(TClientContract).Name));
            }

            _legacyHubProxy = legacyHubProxy;

            _invokeEventMethod = _legacyHubProxy.GetType()
#if PORTABLE
                .GetMethod("InvokeEvent", new[] {typeof (string), typeof (IList<JToken>)});
#elif NETFX_CORE
                .GetRuntimeMethod("InvokeEvent", new[] {typeof (string), typeof (IList<JToken>)});
#else
                .GetMethod("InvokeEvent", BindingFlags.Public | BindingFlags.Instance, null,
                    new[] { typeof(string), typeof(IList<JToken>) }, null);
#endif
        }

        public TypedHubProxy(IHubConnection hubConnection, string hubName)
            : this(new HubProxy(hubConnection, hubName))
        {
        }

        public JToken this[string name]
        {
            get { return _legacyHubProxy[name]; }
            set { _legacyHubProxy[name] = value; }
        }

        public JsonSerializer JsonSerializer
        {
            get { return _legacyHubProxy.JsonSerializer; }
        }

        public Task Invoke(Expression<Action<TServerContract>> method)
        {
            if (method == null)
            {
                throw new ArgumentNullException("method");
            }

            string methodName;
            object[] args;
            method.GetInvocationDetails(out methodName, out args);

            return _legacyHubProxy.Invoke(methodName, args);
        }

        public Task<TResult> Invoke<TResult>(Expression<Func<TServerContract, TResult>> method)
        {
            if (method == null)
            {
                throw new ArgumentNullException("method");
            }

            string methodName;
            object[] args;
            method.GetInvocationDetails(out methodName, out args);

            return _legacyHubProxy.Invoke<TResult>(methodName, args);
        }

        public Task Invoke<TProgress>(Expression<Action<TServerContract>> method, Action<TProgress> onProgress)
        {
            if (method == null)
            {
                throw new ArgumentNullException("method");
            }

            string methodName;
            object[] args;
            method.GetInvocationDetails(out methodName, out args);

            return _legacyHubProxy.Invoke(methodName, onProgress, args);
        }

        public Task<TResult> Invoke<TResult, TProgress>(Expression<Func<TServerContract, TResult>> method, Action<TProgress> onProgress)
        {
            if (method == null)
            {
                throw new ArgumentNullException("method");
            }

            string methodName;
            object[] args;
            method.GetInvocationDetails(out methodName, out args);

            return _legacyHubProxy.Invoke<TResult>(methodName, onProgress, args);
        }

        public void InvokeEvent(Expression<Action<TClientContract>> eventMethod)
        {
            string methodName;
            object[] args;
            eventMethod.GetInvocationDetails(out methodName, out args);

            _invokeEventMethod.Invoke(_legacyHubProxy, new object[] { methodName, ConvertToJToken(args) });
        }

        public IDisposable On(Expression<Func<TClientContract, Action>> eventMethod, Action onData)
        {
            return CreateSubscription(eventMethod.GetMethodName(), onData);
        }

        public IDisposable On<T>(Expression<Func<TClientContract, Action<T>>> eventMethod, Action<T> onData)
        {
            return CreateSubscription(eventMethod.GetMethodName(), onData);
        }

        public IDisposable On<T1, T2>(Expression<Func<TClientContract, Action<T1, T2>>> eventMethod, Action<T1, T2> onData)
        {
            return CreateSubscription(eventMethod.GetMethodName(), onData);
        }

        public IDisposable On<T1, T2, T3>(Expression<Func<TClientContract, Action<T1, T2, T3>>> eventMethod, Action<T1, T2, T3> onData)
        {
            return CreateSubscription(eventMethod.GetMethodName(), onData);
        }

        public IDisposable On<T1, T2, T3, T4>(Expression<Func<TClientContract, Action<T1, T2, T3, T4>>> eventMethod, Action<T1, T2, T3, T4> onData)
        {
            return CreateSubscription(eventMethod.GetMethodName(), onData);
        }

        public IDisposable On<T1, T2, T3, T4, T5>(Expression<Func<TClientContract, Action<T1, T2, T3, T4, T5>>> eventMethod, Action<T1, T2, T3, T4, T5> onData)
        {
            return CreateSubscription(eventMethod.GetMethodName(), onData);
        }

        public IDisposable On<T1, T2, T3, T4, T5, T6>(Expression<Func<TClientContract, Action<T1, T2, T3, T4, T5, T6>>> eventMethod, Action<T1, T2, T3, T4, T5, T6> onData)
        {
            return CreateSubscription(eventMethod.GetMethodName(), onData);
        }
        public IDisposable On<T1, T2, T3, T4, T5, T6, T7>(Expression<Func<TClientContract, Action<T1, T2, T3, T4, T5, T6, T7>>> eventMethod, Action<T1, T2, T3, T4, T5, T6, T7> onData)
        {
            return CreateSubscription(eventMethod.GetMethodName(), onData);
        }


        private IDisposable CreateSubscription(string eventName, Delegate onDataCallback)
        {
#if NETFX_CORE
            Type[] callbackGenericArguments = onDataCallback.GetType().GenericTypeArguments;
#else
            Type[] callbackGenericArguments = onDataCallback.GetType().GetGenericArguments();
#endif
            MethodInfo relatedOnMethod = GetRelatedOnMethod(callbackGenericArguments);

            return (IDisposable)relatedOnMethod.Invoke(null, new object[] { _legacyHubProxy, eventName, onDataCallback });
        }

        private MethodInfo GetRelatedOnMethod(params Type[] argumentTypes)
        {
            var methodInfos = typeof(HubProxyExtensions)
#if NETFX_CORE
                .GetRuntimeMethods()
#else
                .GetMethods(BindingFlags.Static | BindingFlags.Public)
#endif
                .Where(m => m.Name.Equals(nameof(HubProxyExtensions.On)));

            if (argumentTypes.Any())
            {
                methodInfos = methodInfos.Where(
                    m => m.GetParameters()
                        .Last()
                        .ParameterType
#if NETFX_CORE
                        .GenericTypeArguments
#else
                        .GetGenericArguments()
#endif
                        .Count()
                        .Equals(argumentTypes.Count()));
            }

            var method = methodInfos.First();

            return method.IsGenericMethodDefinition ? method.MakeGenericMethod(argumentTypes) : method;
        }

        private IList<JToken> ConvertToJToken(params object[] args)
        {
            return args.Select(arg => JToken.FromObject(arg, JsonSerializer)).ToList();
        }
    }
}
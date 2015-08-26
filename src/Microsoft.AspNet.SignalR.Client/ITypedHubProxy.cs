using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.AspNet.SignalR.Client
{
    /// <summary>
    ///  A client side strongly typed proxy for a server side hub.
    /// </summary>
    public interface ITypedHubProxy<TServerContract, TClientContract>
            where TServerContract : class
            where TClientContract : class
    {
        /// <summary>
        /// Gets or sets state on the hub.
        /// </summary>
        /// <param name="name">The name of the field.</param>
        /// <returns>The value of the field</returns>
        JToken this[string name] { get; set; }

        /// <summary>
        /// Executes a method on the server side hub asynchronously.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns>A task that represents when invocation returned.</returns>
        Task Invoke(Expression<Action<TServerContract>> method);

        /// <summary>
        /// Executes a method on the server side hub asynchronously.
        /// </summary>
        /// <typeparam name="TResult">The type of result returned from the hub.</typeparam>
        /// <param name="method">The method.</param>
        /// <returns>A task that represents when invocation returned.</returns>
        Task<TResult> Invoke<TResult>(Expression<Func<TServerContract, TResult>> method);

        /// <summary>
        /// Executes a method on the server side hub asynchronously with progress updates.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="onProgress">The callback to invoke when progress updates are received.</param>
        /// <returns>A task that represents when invocation returned.</returns>
        Task Invoke<TProgress>(Expression<Action<TServerContract>> method, Action<TProgress> onProgress);

        /// <summary>
        /// Executes a method on the server side hub asynchronously with progress updates.
        /// </summary>
        /// <typeparam name="TResult">The type of result returned from the hub.</typeparam>
        /// <typeparam name="TProgress">The type of progress update value.</typeparam>
        /// <param name="method">The method.</param>
        /// <param name="onProgress">The callback to invoke when progress updates are received.</param>
        /// <returns>A task that represents when invocation returned.</returns>
        Task<TResult> Invoke<TResult, TProgress>(Expression<Func<TServerContract, TResult>> method, Action<TProgress> onProgress);

        /// <summary>
        /// Registers for an event with the specified method and callback.
        /// </summary>
        /// <param name="eventMethod">The event method exposed by the server hub interface.</param>
        /// <param name="onData">The callback.</param>
        /// <returns>An <see cref="IDisposable"/> that represents this subscription.</returns>
        IDisposable On(Expression<Func<TClientContract, Action>> eventMethod, Action onData);

        /// <summary>
        /// Registers for an event with the specified method and callback.
        /// </summary>
        /// <param name="eventMethod">The event method exposed by the server hub interface.</param>
        /// <param name="onData">The callback.</param>
        /// <returns>An <see cref="IDisposable"/> that represents this subscription.</returns>
        IDisposable On<T>(Expression<Func<TClientContract, Action<T>>> eventMethod, Action<T> onData);

        /// <summary>
        /// Registers for an event with the specified method and callback.
        /// </summary>
        /// <param name="eventMethod">The event method exposed by the server hub interface.</param>
        /// <param name="onData">The callback.</param>
        /// <returns>An <see cref="IDisposable"/> that represents this subscription.</returns>
        IDisposable On<T1, T2>(Expression<Func<TClientContract, Action<T1, T2>>> eventMethod,
            Action<T1, T2> onData);

        /// <summary>
        /// Registers for an event with the specified method and callback.
        /// </summary>
        /// <param name="eventMethod">The event method exposed by the server hub interface.</param>
        /// <param name="onData">The callback.</param>
        /// <returns>An <see cref="IDisposable"/> that represents this subscription.</returns>
        IDisposable On<T1, T2, T3>(Expression<Func<TClientContract, Action<T1, T2, T3>>> eventMethod,
            Action<T1, T2, T3> onData);

        /// <summary>
        /// Registers for an event with the specified method and callback.
        /// </summary>
        /// <param name="eventMethod">The event method exposed by the server hub interface.</param>
        /// <param name="onData">The callback.</param>
        /// <returns>An <see cref="IDisposable"/> that represents this subscription.</returns>
        IDisposable On<T1, T2, T3, T4>(Expression<Func<TClientContract, Action<T1, T2, T3, T4>>> eventMethod,
            Action<T1, T2, T3, T4> onData);

        /// <summary>
        /// Registers for an event with the specified method and callback.
        /// </summary>
        /// <param name="eventMethod">The event method exposed by the server hub interface.</param>
        /// <param name="onData">The callback.</param>
        /// <returns>An <see cref="IDisposable"/> that represents this subscription.</returns>
        IDisposable On<T1, T2, T3, T4, T5>(Expression<Func<TClientContract, Action<T1, T2, T3, T4, T5>>> eventMethod,
            Action<T1, T2, T3, T4, T5> onData);

        /// <summary>
        /// Registers for an event with the specified method and callback.
        /// </summary>
        /// <param name="eventMethod">The event method exposed by the server hub interface.</param>
        /// <param name="onData">The callback.</param>
        /// <returns>An <see cref="IDisposable"/> that represents this subscription.</returns>
        IDisposable On<T1, T2, T3, T4, T5, T6>(Expression<Func<TClientContract, Action<T1, T2, T3, T4, T5, T6>>> eventMethod,
            Action<T1, T2, T3, T4, T5, T6> onData);

        /// <summary>
        /// Registers for an event with the specified method and callback.
        /// </summary>
        /// <param name="eventMethod">The event method exposed by the server hub interface.</param>
        /// <param name="onData">The callback.</param>
        /// <returns>An <see cref="IDisposable"/> that represents this subscription.</returns>
        IDisposable On<T1, T2, T3, T4, T5, T6, T7>(Expression<Func<TClientContract, Action<T1, T2, T3, T4, T5, T6, T7>>> eventMethod,
            Action<T1, T2, T3, T4, T5, T6, T7> onData);

        /// <summary>
        /// Gets the serializer used by the connection.
        /// </summary>
        JsonSerializer JsonSerializer { get; }
    }
}
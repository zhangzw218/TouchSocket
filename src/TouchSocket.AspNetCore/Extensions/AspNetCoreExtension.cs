//------------------------------------------------------------------------------
//  此代码版权（除特别声明或在XREF结尾的命名空间的代码）归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  Gitee源代码仓库：https://gitee.com/RRQM_Home
//  Github源代码仓库：https://github.com/RRQM
//  API首页：https://touchsocket.net/
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using TouchSocket.Dmtp;
using TouchSocket.Http;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// AspNetCoreContainerExtension
/// </summary>
public static class AspNetCoreExtension
{
    #region TcpDmtpService

    /// <summary>
    /// 添加TcpDmtpService服务。
    /// 此方法用于通过依赖注入将TcpDmtpService服务及其实现类注册到服务集合中。
    /// 它允许指定服务接口TService和该服务的具体实现类TImpService，
    /// 并通过提供的配置操作委托对相关配置进行定制。
    /// </summary>
    /// <typeparam name="TService">服务的接口类型。</typeparam>
    /// <typeparam name="TImpService">服务的具体实现类。</typeparam>
    /// <param name="services">服务集合，用于存储应用程序中所有注册的服务。</param>
    /// <param name="actionConfig">配置操作委托，用于定制TouchSocket的配置。</param>
    /// <returns>返回扩展后的服务集合，允许方法链式调用。</returns>
    public static IServiceCollection AddTcpDmtpServiceV4<TService, [DynamicallyAccessedMembers(AOT.Container)] TImpService>(this IServiceCollection services, Action<TouchSocketConfigV4> actionConfig)
        where TService : class, ITcpDmtpServiceBase
        where TImpService : class, TService
    {
        // 调用AddTcpService方法来注册TcpDmtpService服务，
        // 该方法是实际执行服务添加的地方。
        return services.AddTcpServiceV4<TService, TImpService>(actionConfig);
    }

    /// <summary>
    /// 添加TcpDmtpService服务。并使用<see cref="ITcpDmtpServiceV4"/>注册服务。
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="actionConfig">配置操作委托</param>
    /// <returns>返回服务集合</returns>
    public static IServiceCollection AddTcpDmtpServiceV4(this IServiceCollection services, Action<TouchSocketConfigV4> actionConfig)
    {
        // 使用泛型方法AddTcpDmtpService，注册TcpDmtpService服务，实现ITcpDmtpService接口
        return services.AddTcpDmtpServiceV4<ITcpDmtpServiceV4, TcpDmtpServiceV4>(actionConfig);
    }

    #endregion TcpDmtpService

    #region TcpDmtpClient

    /// <summary>
    /// 添加Scoped TcpDmtpClient服务。
    /// </summary>
    /// <typeparam name="TClient"></typeparam>
    /// <typeparam name="TImpClient"></typeparam>
    /// <param name="services"></param>
    /// <param name="actionConfig"></param>
    /// <returns></returns>
    public static IServiceCollection AddScopedTcpDmtpClientV4<TClient, [DynamicallyAccessedMembers(AOT.Container)] TImpClient>(this IServiceCollection services, Action<TouchSocketConfigV4> actionConfig)
        where TClient : class, ITcpDmtpClientV4
        where TImpClient : class, TClient
    {
        return services.AddScopedSetupConfigObjectV4<TClient, TImpClient>(actionConfig);
    }

    /// <summary>
    /// 添加Scoped TcpDmtpClient服务。并使用<see cref="ITcpDmtpClientV4"/>注册服务。
    /// </summary>
    /// <param name="services"></param>
    /// <param name="actionConfig"></param>
    /// <returns></returns>
    public static IServiceCollection AddScopedTcpDmtpClientV4(this IServiceCollection services, Action<TouchSocketConfigV4> actionConfig)
    {
        return services.AddScopedTcpDmtpClientV4<ITcpDmtpClientV4, TcpDmtpClientV4>(actionConfig);
    }

    /// <summary>
    /// 添加单例TcpDmtpClient服务。
    /// </summary>
    /// <typeparam name="TClient"></typeparam>
    /// <typeparam name="TImpClient"></typeparam>
    /// <param name="services"></param>
    /// <param name="actionConfig"></param>
    /// <returns></returns>
    public static IServiceCollection AddSingletonTcpDmtpClientV4<TClient, [DynamicallyAccessedMembers(AOT.Container)] TImpClient>(this IServiceCollection services, Action<TouchSocketConfigV4> actionConfig)
        where TClient : class, ITcpDmtpClientV4
        where TImpClient : class, TClient
    {
        return services.AddSingletonSetupConfigObjectV4<TClient, TImpClient>(actionConfig);
    }

    /// <summary>
    /// 添加单例TcpDmtpClient服务。并使用<see cref="ITcpDmtpClientV4"/>注册服务。
    /// </summary>
    /// <param name="services"></param>
    /// <param name="actionConfig"></param>
    /// <returns></returns>
    public static IServiceCollection AddSingletonTcpDmtpClient(this IServiceCollection services, Action<TouchSocketConfigV4> actionConfig)
    {
        return services.AddSingletonTcpDmtpClientV4<ITcpDmtpClientV4, TcpDmtpClientV4>(actionConfig);
    }

    /// <summary>
    /// 添加瞬态TcpDmtpClient服务。
    /// </summary>
    /// <typeparam name="TClient"></typeparam>
    /// <typeparam name="TImpClient"></typeparam>
    /// <param name="services"></param>
    /// <param name="actionConfig"></param>
    /// <returns></returns>
    public static IServiceCollection AddTransientTcpDmtpClientV4<TClient, [DynamicallyAccessedMembers(AOT.Container)] TImpClient>(this IServiceCollection services, Action<TouchSocketConfigV4> actionConfig)
        where TClient : class, ITcpDmtpClientV4
        where TImpClient : class, TClient
    {
        return services.AddTransientSetupConfigObjectV4<TClient, TImpClient>(actionConfig);
    }

    /// <summary>
    /// 添加瞬态TcpDmtpClient服务。并使用<see cref="ITcpDmtpClientV4"/>注册服务。
    /// </summary>
    /// <param name="services"></param>
    /// <param name="actionConfig"></param>
    /// <returns></returns>
    public static IServiceCollection AddTransientTcpDmtpClientV4(this IServiceCollection services, Action<TouchSocketConfigV4> actionConfig)
    {
        return services.AddTransientTcpDmtpClientV4<ITcpDmtpClientV4, TcpDmtpClientV4>(actionConfig);
    }

    #endregion TcpDmtpClient

    #region HttpService

    /// <summary>
    /// 添加HttpService服务。
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <typeparam name="TImpService"></typeparam>
    /// <param name="services"></param>
    /// <param name="actionConfig"></param>
    /// <returns></returns>
    public static IServiceCollection AddHttpServiceV4<TService, [DynamicallyAccessedMembers(AOT.Container)] TImpService>(this IServiceCollection services, Action<TouchSocketConfigV4> actionConfig)
        where TService : class, IHttpServiceBase
        where TImpService : class, TService
    {
        return services.AddTcpServiceV4<TService, TImpService>(actionConfig);
    }

    /// <summary>
    /// 添加HttpService服务。并使用<see cref="IHttpServiceV4"/>注册服务。
    /// </summary>
    /// <param name="services"></param>
    /// <param name="actionConfig"></param>
    /// <returns></returns>
    public static IServiceCollection AddHttpServiceV4(this IServiceCollection services, Action<TouchSocketConfigV4> actionConfig)
    {
        return services.AddHttpServiceV4<IHttpServiceV4, HttpServiceV4>(actionConfig);
    }

    #endregion HttpService

    #region HttpClient

    /// <summary>
    /// 添加Scoped HttpClient服务。
    /// </summary>
    /// <typeparam name="TClient"></typeparam>
    /// <typeparam name="TImpClient"></typeparam>
    /// <param name="services"></param>
    /// <param name="actionConfig"></param>
    /// <returns></returns>
    public static IServiceCollection AddScopedHttpClientV4<TClient, [DynamicallyAccessedMembers(AOT.Container)] TImpClient>(this IServiceCollection services, Action<TouchSocketConfigV4> actionConfig)
        where TClient : class, IHttpClientV4
        where TImpClient : class, TClient
    {
        return services.AddScopedSetupConfigObjectV4<TClient, TImpClient>(actionConfig);
    }

    /// <summary>
    /// 添加Scoped HttpClient服务。并使用<see cref="IHttpClientV4"/>注册服务。
    /// </summary>
    /// <param name="services"></param>
    /// <param name="actionConfig"></param>
    /// <returns></returns>
    public static IServiceCollection AddScopedHttpClientV4(this IServiceCollection services, Action<TouchSocketConfigV4> actionConfig)
    {
        return services.AddScopedHttpClientV4<IHttpClientV4, HttpClientV4>(actionConfig);
    }

    /// <summary>
    /// 添加单例HttpClient服务。
    /// </summary>
    /// <typeparam name="TClient"></typeparam>
    /// <typeparam name="TImpClient"></typeparam>
    /// <param name="services"></param>
    /// <param name="actionConfig"></param>
    /// <returns></returns>
    public static IServiceCollection AddSingletonHttpClientV4<TClient, [DynamicallyAccessedMembers(AOT.Container)] TImpClient>(this IServiceCollection services, Action<TouchSocketConfigV4> actionConfig)
        where TClient : class, IHttpClientV4
        where TImpClient : class, TClient
    {
        return services.AddSingletonSetupConfigObjectV4<TClient, TImpClient>(actionConfig);
    }

    /// <summary>
    /// 添加单例HttpClient服务。并使用<see cref="IHttpClientV4"/>注册服务。
    /// </summary>
    /// <param name="services"></param>
    /// <param name="actionConfig"></param>
    /// <returns></returns>
    public static IServiceCollection AddSingletonHttpClient(this IServiceCollection services, Action<TouchSocketConfigV4> actionConfig)
    {
        return services.AddSingletonHttpClientV4<IHttpClientV4, HttpClientV4>(actionConfig);
    }

    /// <summary>
    /// 添加瞬态HttpClient服务。
    /// </summary>
    /// <typeparam name="TClient"></typeparam>
    /// <typeparam name="TImpClient"></typeparam>
    /// <param name="services"></param>
    /// <param name="actionConfig"></param>
    /// <returns></returns>
    public static IServiceCollection AddTransientHttpClient<TClient, [DynamicallyAccessedMembers(AOT.Container)] TImpClient>(this IServiceCollection services, Action<TouchSocketConfigV4> actionConfig)
        where TClient : class, IHttpClientV4
        where TImpClient : class, TClient
    {
        return services.AddTransientSetupConfigObjectV4<TClient, TImpClient>(actionConfig);
    }

    /// <summary>
    /// 添加瞬态HttpClient服务。并使用<see cref="IHttpClientV4"/>注册服务。
    /// </summary>
    /// <param name="services"></param>
    /// <param name="actionConfig"></param>
    /// <returns></returns>
    public static IServiceCollection AddTransientHttpClient(this IServiceCollection services, Action<TouchSocketConfigV4> actionConfig)
    {
        return services.AddTransientHttpClient<IHttpClientV4, HttpClientV4>(actionConfig);
    }

    #endregion HttpClient

    #region HttpDmtpService

    /// <summary>
    /// 添加HttpDmtpService服务。
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <typeparam name="TImpService"></typeparam>
    /// <param name="services"></param>
    /// <param name="actionConfig"></param>
    /// <returns></returns>
    public static IServiceCollection AddHttpDmtpService<TService, [DynamicallyAccessedMembers(AOT.Container)] TImpService>(this IServiceCollection services, Action<TouchSocketConfigV4> actionConfig)
        where TService : class, IHttpDmtpServiceBase
        where TImpService : class, TService
    {
        return services.AddTcpServiceV4<TService, TImpService>(actionConfig);
    }

    /// <summary>
    /// 添加HttpDmtpService服务。并使用<see cref="IHttpDmtpServiceV4"/>注册服务。
    /// </summary>
    /// <param name="services"></param>
    /// <param name="actionConfig"></param>
    /// <returns></returns>
    public static IServiceCollection AddHttpDmtpServiceV4(this IServiceCollection services, Action<TouchSocketConfigV4> actionConfig)
    {
        return services.AddHttpDmtpService<IHttpDmtpServiceV4, HttpDmtpServiceV4>(actionConfig);
    }

    #endregion HttpDmtpService

    #region HttpDmtpClient

    /// <summary>
    /// 添加Scoped HttpDmtpClient服务。
    /// </summary>
    /// <typeparam name="TClient"></typeparam>
    /// <typeparam name="TImpClient"></typeparam>
    /// <param name="services"></param>
    /// <param name="actionConfig"></param>
    /// <returns></returns>
    public static IServiceCollection AddScopedHttpDmtpClientV4<TClient, [DynamicallyAccessedMembers(AOT.Container)] TImpClient>(this IServiceCollection services, Action<TouchSocketConfigV4> actionConfig)
        where TClient : class, IHttpDmtpClientV4
        where TImpClient : class, TClient
    {
        return services.AddScopedSetupConfigObjectV4<TClient, TImpClient>(actionConfig);
    }

    /// <summary>
    /// 添加Scoped HttpDmtpClient服务。并使用<see cref="IHttpDmtpClientV4"/>注册服务。
    /// </summary>
    /// <param name="services"></param>
    /// <param name="actionConfig"></param>
    /// <returns></returns>
    public static IServiceCollection AddScopedHttpDmtpClient(this IServiceCollection services, Action<TouchSocketConfigV4> actionConfig)
    {
        return services.AddScopedHttpDmtpClientV4<IHttpDmtpClientV4, HttpDmtpClientV4>(actionConfig);
    }

    /// <summary>
    /// 添加单例HttpDmtpClient服务。
    /// </summary>
    /// <typeparam name="TClient"></typeparam>
    /// <typeparam name="TImpClient"></typeparam>
    /// <param name="services"></param>
    /// <param name="actionConfig"></param>
    /// <returns></returns>
    public static IServiceCollection AddSingletonHttpDmtpClient<TClient, [DynamicallyAccessedMembers(AOT.Container)] TImpClient>(this IServiceCollection services, Action<TouchSocketConfigV4> actionConfig)
        where TClient : class, IHttpDmtpClientV4
        where TImpClient : class, TClient
    {
        return services.AddSingletonSetupConfigObjectV4<TClient, TImpClient>(actionConfig);
    }

    /// <summary>
    /// 添加单例HttpDmtpClient服务。并使用<see cref="IHttpDmtpClientV4"/>注册服务。
    /// </summary>
    /// <param name="services"></param>
    /// <param name="actionConfig"></param>
    /// <returns></returns>
    public static IServiceCollection AddSingletonHttpDmtpClientV4(this IServiceCollection services, Action<TouchSocketConfigV4> actionConfig)
    {
        return services.AddSingletonHttpDmtpClient<IHttpDmtpClientV4, HttpDmtpClientV4>(actionConfig);
    }

    /// <summary>
    /// 添加瞬态HttpDmtpClient服务。
    /// </summary>
    /// <typeparam name="TClient"></typeparam>
    /// <typeparam name="TImpClient"></typeparam>
    /// <param name="services"></param>
    /// <param name="actionConfig"></param>
    /// <returns></returns>
    public static IServiceCollection AddTransientHttpDmtpClient<TClient, [DynamicallyAccessedMembers(AOT.Container)] TImpClient>(this IServiceCollection services, Action<TouchSocketConfigV4> actionConfig)
        where TClient : class, IHttpDmtpClientV4
        where TImpClient : class, TClient
    {
        return services.AddTransientSetupConfigObjectV4<TClient, TImpClient>(actionConfig);
    }

    /// <summary>
    /// 添加瞬态HttpDmtpClient服务。并使用<see cref="IHttpDmtpClientV4"/>注册服务。
    /// </summary>
    /// <param name="services"></param>
    /// <param name="actionConfig"></param>
    /// <returns></returns>
    public static IServiceCollection AddTransientHttpDmtpClientV4(this IServiceCollection services, Action<TouchSocketConfigV4> actionConfig)
    {
        return services.AddTransientHttpDmtpClient<IHttpDmtpClientV4, HttpDmtpClientV4>(actionConfig);
    }

    #endregion HttpDmtpClient
}
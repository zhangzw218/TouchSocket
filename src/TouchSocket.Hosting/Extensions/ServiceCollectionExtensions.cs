//------------------------------------------------------------------------------
//  此代码版权(除特别声明或在XREF结尾的命名空间的代码)归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议,若本仓库没有设置,则按MIT开源协议授权
//  CSDN博客:https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频:https://space.bilibili.com/94253567
//  Gitee源代码仓库:https://gitee.com/RRQM_Home
//  Github源代码仓库:https://github.com/RRQM
//  API首页:https://touchsocket.net/
//  交流QQ群:234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using TouchSocket.Core.AspNetCore;
using TouchSocket.Hosting;
using TouchSocket.Hosting.Sockets.HostService;
using TouchSocket.Sockets;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 服务集合扩展类
/// </summary>
public static class ServiceCollectionExtensions
{
    #region SetupConfig

    /// <summary>
    /// 添加单例生命周期的配置对象服务
    /// </summary>
    /// <typeparam name="TObjectService">服务接口类型</typeparam>
    /// <typeparam name="TObjectImpService">服务实现类型</typeparam>
    /// <param name="services">服务集合</param>
    /// <param name="actionConfig">配置操作委托</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddSingletonSetupConfigObjectV4<TObjectService, [DynamicallyAccessedMembers(AOT.Container)] TObjectImpService>(this IServiceCollection services, Action<TouchSocketConfigV4> actionConfig)
           where TObjectService : class, ISetupConfigObjectV4
   where TObjectImpService : class, TObjectService
    {
        var aspNetCoreContainer = new AspNetCoreContainer(services);
        var config = new TouchSocketConfigV4();
        config.SetRegistrator(aspNetCoreContainer);
        actionConfig.Invoke(config);
        if (config.GetValue(TouchSocketCoreConfigExtension.ConfigureContainerProperty) is Action<IRegistrator> actionContainer)
        {
            actionContainer.Invoke(aspNetCoreContainer);
        }

        services.AddSingleton<TObjectService>(privoder =>
        {
            var imp = privoder.ResolveWithoutRoot<TObjectImpService>();
            config.RemoveValue(TouchSocketCoreConfigExtension.ConfigureContainerProperty);
            aspNetCoreContainer.BuildResolver(privoder);
            config.SetResolver(aspNetCoreContainer);
            imp.SetupAsync(config).GetFalseAwaitResult();
            return imp;
        });
        return services;
    }

    /// <summary>
    /// 添加瞬态生命周期的配置对象服务
    /// </summary>
    /// <typeparam name="TObjectService">服务接口类型</typeparam>
    /// <typeparam name="TObjectImpService">服务实现类型</typeparam>
    /// <param name="services">服务集合</param>
    /// <param name="actionConfig">配置操作委托</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddTransientSetupConfigObjectV4<TObjectService, [DynamicallyAccessedMembers(AOT.Container)] TObjectImpService>(this IServiceCollection services, Action<TouchSocketConfigV4> actionConfig)
             where TObjectService : class, ISetupConfigObjectV4
     where TObjectImpService : class, TObjectService
    {
        var aspNetCoreContainer = new AspNetCoreContainer(services);
        var config = new TouchSocketConfigV4();
        config.SetRegistrator(aspNetCoreContainer);
        actionConfig.Invoke(config);
        if (config.GetValue(TouchSocketCoreConfigExtension.ConfigureContainerProperty) is Action<IRegistrator> actionContainer)
        {
            actionContainer.Invoke(aspNetCoreContainer);
        }

        services.AddTransient<TObjectService>(privoder =>
        {
            var imp = privoder.ResolveWithoutRoot<TObjectImpService>();
            config.RemoveValue(TouchSocketCoreConfigExtension.ConfigureContainerProperty);
            aspNetCoreContainer.BuildResolver(privoder);
            config.SetResolver(aspNetCoreContainer);
            imp.SetupAsync(config.Clone()).GetFalseAwaitResult();
            return imp;
        });
        return services;
    }

    /// <summary>
    /// 添加作用域生命周期的配置对象服务
    /// </summary>
    /// <typeparam name="TObjectService">服务接口类型</typeparam>
    /// <typeparam name="TObjectImpService">服务实现类型</typeparam>
    /// <param name="services">服务集合</param>
    /// <param name="actionConfig">配置操作委托</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddScopedSetupConfigObjectV4<TObjectService, [DynamicallyAccessedMembers(AOT.Container)] TObjectImpService>(this IServiceCollection services, Action<TouchSocketConfigV4> actionConfig)
             where TObjectService : class, ISetupConfigObjectV4
     where TObjectImpService : class, TObjectService
    {
        var aspNetCoreContainer = new AspNetCoreContainer(services);
        var config = new TouchSocketConfigV4();
        config.SetRegistrator(aspNetCoreContainer);
        actionConfig.Invoke(config);
        if (config.GetValue(TouchSocketCoreConfigExtension.ConfigureContainerProperty) is Action<IRegistrator> actionContainer)
        {
            actionContainer.Invoke(aspNetCoreContainer);
        }

        services.AddScoped<TObjectService>(privoder =>
        {
            var imp = privoder.ResolveWithoutRoot<TObjectImpService>();
            config.RemoveValue(TouchSocketCoreConfigExtension.ConfigureContainerProperty);
            aspNetCoreContainer.BuildResolver(privoder);
            config.SetResolver(aspNetCoreContainer);
            imp.SetupAsync(config.Clone()).GetFalseAwaitResult();
            return imp;
        });
        return services;
    }

    #endregion SetupConfig

    #region HostedService

    /// <summary>
    /// 添加托管服务,该服务必须实现<see cref="ISetupConfigObjectV4"/>和<see cref="IServiceBase"/>接口
    /// </summary>
    /// <typeparam name="TObjectService">服务接口类型</typeparam>
    /// <typeparam name="TObjectImpService">服务实现类型</typeparam>
    /// <param name="services">服务集合</param>
    /// <param name="actionConfig">配置操作委托</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddServiceHostedServiceV4<TObjectService, [DynamicallyAccessedMembers(AOT.Container)] TObjectImpService>(this IServiceCollection services, Action<TouchSocketConfigV4> actionConfig)
             where TObjectService : class, ISetupConfigObjectV4, IServiceBase
     where TObjectImpService : class, TObjectService
    {
        return AddSetupConfigObjectHostedServiceV4<ServiceHost<TObjectService>, TObjectService, TObjectImpService>(services, actionConfig);
    }

    /// <summary>
    /// 添加配置对象托管服务
    /// </summary>
    /// <typeparam name="THostService">托管服务类型</typeparam>
    /// <typeparam name="TObjectService">服务接口类型</typeparam>
    /// <typeparam name="TObjectImpService">服务实现类型</typeparam>
    /// <param name="services">服务集合</param>
    /// <param name="actionConfig">配置操作委托</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddSetupConfigObjectHostedServiceV4<THostService, TObjectService, [DynamicallyAccessedMembers(AOT.Container)] TObjectImpService>(this IServiceCollection services, Action<TouchSocketConfigV4> actionConfig)
             where THostService : SetupConfigObjectHostedService<TObjectService>, new()
     where TObjectService : class, ISetupConfigObjectV4
     where TObjectImpService : class, TObjectService
    {
        var aspNetCoreContainer = new AspNetCoreContainer(services);
        var config = new TouchSocketConfigV4();
        config.SetRegistrator(aspNetCoreContainer);
        actionConfig.Invoke(config);
        if (config.GetValue(TouchSocketCoreConfigExtension.ConfigureContainerProperty) is Action<IRegistrator> actionContainer)
        {
            actionContainer.Invoke(aspNetCoreContainer);
        }

        services.AddSingleton<TObjectService, TObjectImpService>();
        services.AddHostedService(privoder =>
        {
            var serviceHost = new THostService();
            serviceHost.SetObject(privoder.GetService<TObjectService>());
            serviceHost.SetConfig(config);
            serviceHost.SetProvider(privoder, aspNetCoreContainer);
            return serviceHost;
        });
        return services;
    }

    #endregion HostedService

    #region TcpService

    /// <summary>
    /// 添加TCP服务
    /// </summary>
    /// <typeparam name="TService">服务接口类型</typeparam>
    /// <typeparam name="TImpService">服务实现类型</typeparam>
    /// <param name="services">服务集合</param>
    /// <param name="actionConfig">配置操作委托</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddTcpServiceV4<TService, [DynamicallyAccessedMembers(AOT.Container)] TImpService>(this IServiceCollection services, Action<TouchSocketConfigV4> actionConfig)
             where TService : class, ITcpServiceBase
     where TImpService : class, TService
    {
        return AddServiceHostedServiceV4<TService, TImpService>(services, actionConfig);
    }

    /// <summary>
    /// 添加TCP服务,使用<see cref="ITcpService"/>作为服务接口注册
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="actionConfig">配置操作委托</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddTcpServiceV4(this IServiceCollection services, Action<TouchSocketConfigV4> actionConfig)
    {
        return services.AddTcpServiceV4<ITcpService, TcpService>(actionConfig);
    }

    #endregion TcpService

    #region UdpSession

    /// <summary>
    /// 添加UDP会话服务
    /// </summary>
    /// <typeparam name="TService">服务接口类型</typeparam>
    /// <typeparam name="TImpService">服务实现类型</typeparam>
    /// <param name="services">服务集合</param>
    /// <param name="actionConfig">配置操作委托</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddUdpSessionV4<TService, [DynamicallyAccessedMembers(AOT.Container)] TImpService>(this IServiceCollection services, Action<TouchSocketConfigV4> actionConfig)
            where TService : class, IUdpSession
    where TImpService : class, TService
    {
        return AddServiceHostedServiceV4<TService, TImpService>(services, actionConfig);
    }

    /// <summary>
    /// 添加UDP会话服务,使用<see cref="IUdpSession"/>作为服务接口注册
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="actionConfig">配置操作委托</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddUdpSessionV4(this IServiceCollection services, Action<TouchSocketConfigV4> actionConfig)
    {
        return services.AddUdpSessionV4<IUdpSession, UdpSession>(actionConfig);
    }

    #endregion UdpSession

    #region TcpClient

    /// <summary>
    /// 添加单例生命周期的TCP客户端服务
    /// </summary>
    /// <typeparam name="TClient">客户端接口类型</typeparam>
    /// <typeparam name="TImpClient">客户端实现类型</typeparam>
    /// <param name="services">服务集合</param>
    /// <param name="actionConfig">配置操作委托</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddSingletonTcpClientV4<TClient, [DynamicallyAccessedMembers(AOT.Container)] TImpClient>(this IServiceCollection services, Action<TouchSocketConfigV4> actionConfig)
           where TClient : class, ITcpClient
    where TImpClient : class, TClient
    {
        return AddSingletonSetupConfigObjectV4<TClient, TImpClient>(services, actionConfig);
    }

    /// <summary>
    /// 添加单例生命周期的TCP客户端服务,使用<see cref="ITcpClient"/>作为服务接口注册
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="actionConfig">配置操作委托</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddSingletonTcpClientV4(this IServiceCollection services, Action<TouchSocketConfigV4> actionConfig)
    {
        return services.AddSingletonTcpClientV4<ITcpClient, TcpClient>(actionConfig);
    }

    /// <summary>
    /// 添加瞬态生命周期的TCP客户端服务
    /// </summary>
    /// <typeparam name="TClient">客户端接口类型</typeparam>
    /// <typeparam name="TImpClient">客户端实现类型</typeparam>
    /// <param name="services">服务集合</param>
    /// <param name="actionConfig">配置操作委托</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddTransientTcpClientV4<TClient, [DynamicallyAccessedMembers(AOT.Container)] TImpClient>(this IServiceCollection services, Action<TouchSocketConfigV4> actionConfig)
             where TClient : class, ITcpClient
     where TImpClient : class, TClient
    {
        return AddTransientSetupConfigObjectV4<TClient, TImpClient>(services, actionConfig);
    }

    /// <summary>
    /// 添加瞬态生命周期的TCP客户端服务,使用<see cref="ITcpClient"/>作为服务接口注册
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="actionConfig">配置操作委托</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddTransientTcpClientV4(this IServiceCollection services, Action<TouchSocketConfigV4> actionConfig)
    {
        return services.AddTransientTcpClientV4<ITcpClient, TcpClient>(actionConfig);
    }

    /// <summary>
    /// 添加作用域生命周期的TCP客户端服务
    /// </summary>
    /// <typeparam name="TClient">客户端接口类型</typeparam>
    /// <typeparam name="TImpClient">客户端实现类型</typeparam>
    /// <param name="services">服务集合</param>
    /// <param name="actionConfig">配置操作委托</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddScopedTcpClientV4<TClient, [DynamicallyAccessedMembers(AOT.Container)] TImpClient>(this IServiceCollection services, Action<TouchSocketConfigV4> actionConfig) where TClient : class, ITcpClient
     where TImpClient : class, TClient
    {
        return AddScopedSetupConfigObjectV4<TClient, TImpClient>(services, actionConfig);
    }

    /// <summary>
    /// 添加作用域生命周期的TCP客户端服务,使用<see cref="ITcpClient"/>作为服务接口注册
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="actionConfig">配置操作委托</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddScopedTcpClientV4(this IServiceCollection services, Action<TouchSocketConfigV4> actionConfig)
    {
        return services.AddScopedTcpClientV4<ITcpClient, TcpClient>(actionConfig);
    }

    #endregion TcpClient
}
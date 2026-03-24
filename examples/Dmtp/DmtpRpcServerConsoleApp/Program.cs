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

using System.ComponentModel;
using TouchSocket.Core;
using TouchSocket.Dmtp;
using TouchSocket.Dmtp.Rpc;
using TouchSocket.Rpc;
using TouchSocket.Sockets;

[assembly: GeneratorRpcServerRegister]//生成注册

namespace ConsoleApp2;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var service = new TcpDmtpService();
        var config = new TouchSocketConfig()//配置
               .SetListenIPHosts(7789)
               .ConfigureContainer(a =>
               {
                   a.AddRpcCallContextAccessor();

                   a.AddDmtpRouteService();
                   a.AddConsoleLogger();

                   a.AddRpcStore(store =>
                   {
                       store.RegisterServer<MyRpcServer>();
#if DEBUG
                       File.WriteAllText("../../../RpcProxy.cs", store.GetProxyCodes("RpcProxy", new Type[] { typeof(DmtpRpcAttribute) }));
                       ConsoleLogger.Default.Info("成功生成代理");
#endif
                   });
               })
               .ConfigurePlugins(a =>
               {
                   a.UseDmtpRpc();

                   a.Add<MyRpcPlugin>();
               })
               .SetDmtpOption(options =>
               {
                   options.VerifyToken = "Dmtp";//设定连接口令，作用类似账号密码
                   options.Id = "ServiceB";
               });

        await service.SetupAsync(config);
        await service.StartAsync();

        service.Logger.Info($"{service.GetType().Name}已启动");

        service.Logger.Info($"输入客户端Id，空格输入消息，将通知客户端方法");
        while (true)
        {
            var str = Console.ReadLine();
            if (service.TryGetClient(str.Split(' ')[0], out var socketClient))
            {
                var result = await socketClient.GetDmtpRpcActor().InvokeTAsync<bool>("Notice", DmtpInvokeOption.WaitInvoke, str.Split(' ')[1]);

                service.Logger.Info($"调用结果{result}");
            }
        }
    }

}


public partial class MyRpcServer : SingletonRpcServer
{
    private readonly ILog m_logger;
    private readonly IRpcCallContextAccessor m_rpcCallContextAccessor;

    public MyRpcServer(ILog logger, IRpcCallContextAccessor rpcCallContextAccessor)
    {
        this.m_logger = logger;
        this.m_rpcCallContextAccessor = rpcCallContextAccessor;
    }

}

#region 通过调用上下文获取调用客户端
public partial class MyRpcServer : SingletonRpcServer
{
    [Description("测试反向Rpc")]
    [DmtpRpc(MethodInvoke = true)]
    public async Task CallClientNotice(ICallContext callContext)
    {
        if (callContext.Caller is ITcpDmtpSessionClient sessionClient)
        {
            await sessionClient.GetDmtpRpcActor().InvokeTAsync<string>("Notice", InvokeOption.WaitInvoke, "Hello");
        }
    }
}
#endregion

#region DmtpRpc同意转发路由数据
internal class MyRpcPlugin : PluginBase, IDmtpRoutingPlugin
{
    public async Task OnDmtpRouting(IDmtpActorObject client, PackageRouterEventArgs e)
    {
        if (e.RouterType == RouteType.Rpc)
        {
            e.IsPermitOperation = true;
            return;
        }

        await e.InvokeNext();
    }
}
#endregion

#region 声明DmtpRpc服务
public partial class MyRpcServer : SingletonRpcServer
{
    /// <summary>
    /// 将两个数相加
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    [DmtpRpc(MethodInvoke = true)]//使用函数名直接调用，服务注册的函数键，此处为显式指定。默认不传参的时候，为该函数类全名+方法名的全小写。
    [Description("将两个数相加")]//服务描述，在生成代理时，会变成注释。
    public int Add(int a, int b)
    {
        var callerId = m_rpcCallContextAccessor.CallContext.Caller is IIdClient idClient ? idClient.Id : null;
        this.m_logger.Info($"调用方名称 {callerId} 调用Add");
        var sum = a + b;
        return sum;
    }

}
#endregion

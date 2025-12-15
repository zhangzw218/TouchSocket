// ------------------------------------------------------------------------------
// 此代码版权（除特别声明或在XREF结尾的命名空间的代码）归作者本人若汝棋茗所有
// 源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
// CSDN博客：https://blog.csdn.net/qq_40374647
// 哔哩哔哩视频：https://space.bilibili.com/94253567
// Gitee源代码仓库：https://gitee.com/RRQM_Home
// Github源代码仓库：https://github.com/RRQM
// API首页：https://touchsocket.net/
// 交流QQ群：234762506
// 感谢您的下载和使用
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Resources;
using TouchSocket.Sockets;

namespace DmtpRpcClientConsoleApp;
public static class ReconnectionOptionExtension
{
    /// <summary>
    /// 尝试连接的委托
    /// </summary>
    public static Func<TClient, CancellationToken, Task> DefaultConnectAction<TClient>(this ReconnectionOption<TClient> reconnectionOption)
        where TClient : IConnectableClient, IOnlineClient, IDependencyClient
    {
        return async (client, cancellationToken) =>
        {
            var attempts = 0;
            var currentInterval = reconnectionOption.BaseInterval;

            while (reconnectionOption.MaxRetryCount < 0 || attempts < reconnectionOption.MaxRetryCount)
            {
                if (client.GetPauseReconnection())
                {
                    if (reconnectionOption.LogReconnection)
                    {
                        client.Logger?.Debug(reconnectionOption, TouchSocketResource.PauseReconnection);
                    }
                    continue;
                }

                attempts++;

                try
                {
                    if (client.Online)
                    {
                        reconnectionOption.OnSuccessed?.Invoke(client);
                        return;
                    }

                    Console.WriteLine("开始连接");
                    //var cts = new CancellationTokenSource(5000);
                    //var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
                    //await client.ConnectAsync(linkedCts.Token).ConfigureAwait(EasyTask.ContinueOnCapturedContext);
                    await client.ConnectAsync(cancellationToken).ConfigureAwait(EasyTask.ContinueOnCapturedContext);
                    Console.WriteLine("连接结束");
                    reconnectionOption.OnSuccessed?.Invoke(client);

                    if (reconnectionOption.LogReconnection)
                    {
                        client.Logger?.Info(reconnectionOption, $"重连成功，尝试次数: {attempts}");
                    }
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("连接失败");
                    reconnectionOption.OnFailed?.Invoke(client, attempts, ex);

                    if (reconnectionOption.LogReconnection)
                    {
                        client.Logger?.Warning(reconnectionOption, $"重连失败，尝试次数: {attempts}，错误: {ex.Message}");
                    }

                    if (reconnectionOption.MaxRetryCount > 0 && attempts >= reconnectionOption.MaxRetryCount)
                    {
                        reconnectionOption.OnGiveUp?.Invoke(client, attempts);
                        if (reconnectionOption.LogReconnection)
                        {
                            client.Logger?.Error(reconnectionOption, $"达到最大重连次数 {reconnectionOption.MaxRetryCount}，放弃重连");
                        }
                        return;
                    }

                    await Task.Delay(currentInterval, CancellationToken.None).ConfigureAwait(EasyTask.ContinueOnCapturedContext);
                }
            }
        };
    }
}

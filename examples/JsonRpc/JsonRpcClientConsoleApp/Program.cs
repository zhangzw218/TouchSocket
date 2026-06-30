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

using JsonRpcProxy;
using System.Text;
using TouchSocket.Core;
using TouchSocket.Http.WebSockets;
using TouchSocket.JsonRpc;
using TouchSocket.Rpc;
using TouchSocket.Sockets;

namespace JsonRpcClientConsoleApp;

internal class Program
{
    private static void ConsoleAction_OnException(Exception obj)
    {
        ConsoleLogger.Default.Exception(obj);
    }

    private static async Task<IJsonRpcClient> CreateWebSocketJsonRpcClient()
    {
        #region 创建WebSocketJsonRpc客户端
        var jsonRpcClient = new WebSocketJsonRpcClient();
        await jsonRpcClient.SetupAsync(new TouchSocketConfig()
            .ConfigurePlugins(plug =>
            {
                plug.Add<AAWebSocketConnectedPlugin>();
            })
             .SetRemoteIPHost("ws://127.0.0.1:7707/ws"));//此url就是能连接到websocket的路径。
        var result = await jsonRpcClient.TryConnectAsync();
        ConsoleLogger.Default.Info($"JsonRpc连接结果 {result.Message}");
        return jsonRpcClient;
        #endregion
    }


    private static async Task JsonRpcClientInvokeByWebSocket()
    {
        var jsonRpcClient = await CreateWebSocketJsonRpcClient();

        Console.WriteLine("连接成功");
        var result = await jsonRpcClient.TestJsonRpcAsync("RRQM");
        Console.WriteLine($"WebSocket返回结果:{result}");

        result = await jsonRpcClient.TestJsonRpcAsync("RRQM");
        Console.WriteLine($"WebSocket返回结果:{result}");

        result = await jsonRpcClient.TestGetContextAsync("RRQM");
        Console.WriteLine($"WebSocket返回结果:{result}");
    }

    private static async Task Main(string[] args)
    {
        var consoleAction = new ConsoleAction();
        consoleAction.OnException += ConsoleAction_OnException;
        consoleAction.Add("3", "WebSocket调用", JsonRpcClientInvokeByWebSocket);

        consoleAction.ShowAll();

        await consoleAction.RunCommandLineAsync();
    }
}
/*
此代码由Rpc工具直接生成，非必要请不要修改此处代码
*/
#pragma warning disable
using System;
using TouchSocket.Core;
using TouchSocket.Sockets;
using TouchSocket.Rpc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
namespace RpcProxy
{
public interface IMyRpcServer:TouchSocket.Rpc.IRemoteServer
{
///<summary>
///测试反向Rpc
///</summary>
/// <exception cref="System.TimeoutException">调用超时</exception>
/// <exception cref="TouchSocket.Rpc.RpcInvokeException">Rpc调用异常</exception>
/// <exception cref="System.Exception">其他异常</exception>
Task CallClientNoticeAsync(InvokeOption invokeOption = default);

///<summary>
///将两个数相加
///</summary>
/// <exception cref="System.TimeoutException">调用超时</exception>
/// <exception cref="TouchSocket.Rpc.RpcInvokeException">Rpc调用异常</exception>
/// <exception cref="System.Exception">其他异常</exception>
Task<System.Int32> AddAsync(System.Int32 a,System.Int32 b,InvokeOption invokeOption = default);

}
public class MyRpcServer :IMyRpcServer
{
public MyRpcServer(IRpcClient client)
{
this.Client=client;
}
public IRpcClient Client{get;private set; }
///<summary>
///测试反向Rpc
///</summary>
public Task CallClientNoticeAsync(InvokeOption invokeOption = default)
{
if(this.Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
return this.Client.InvokeAsync("CallClientNotice",null,invokeOption, null);

}

///<summary>
///将两个数相加
///</summary>
public async Task<System.Int32> AddAsync(System.Int32 a,System.Int32 b,InvokeOption invokeOption = default)
{
if(this.Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{a,b};
return (System.Int32) await this.Client.InvokeAsync("Add",typeof(System.Int32),invokeOption, parameters);

}

}
public static class MyRpcServerExtensions
{
///<summary>
///测试反向Rpc
///</summary>
public static Task CallClientNoticeAsync<TClient>(this TClient client,InvokeOption invokeOption = default) where TClient:
TouchSocket.Dmtp.Rpc.IDmtpRpcActor{
return client.InvokeAsync("CallClientNotice",null,invokeOption, null);

}

///<summary>
///将两个数相加
///</summary>
public static async Task<System.Int32> AddAsync<TClient>(this TClient client,System.Int32 a,System.Int32 b,InvokeOption invokeOption = default) where TClient:
TouchSocket.Dmtp.Rpc.IDmtpRpcActor{
object[] parameters = new object[]{a,b};
return (System.Int32) await client.InvokeAsync("Add",typeof(System.Int32),invokeOption, parameters);

}

}
}

namespace camProject.Comm
{
    // 이식 대상: copyApp/Conf/TcpComm.cs — UI 비의존 계층, 1:1 포팅 가능
    //
    // 필요한 멤버:
    //   - public static TcpComm Instance { get; }
    //   - bool IsConnected { get; }
    //   - bool IsServerRunning { get; }
    //   - event EventHandler<string> DataReceived
    //   - event EventHandler<string> LogOccurred
    //   - event EventHandler Connected / DisconnectedEvent
    //   - bool ServerConnect(string host, int port)
    //   - Task<bool> Connect(string ip, int port)
    //   - void Disconnect() / void AllDisconnect()
    //   - void SendFramed(string payload)        // STX + payload + ETX
    //   - void Dispose()
    //
    // 포팅 시 주의:
    //   - using System.Net.Sockets; (net8 기본 포함, 별도 패키지 불필요)
    //   - ReceiveLoop 안에서 DataReceived?.Invoke(this, msg)를 두 번 호출하지 않도록 주의
    //     (이전 세션에서 발견된 버그 — 같은 프레임이 중복 처리되어 상태머신이 꼬이는 원인이었음)
    public sealed class TcpComm
    {
    }
}

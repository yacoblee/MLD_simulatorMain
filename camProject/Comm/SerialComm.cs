namespace camProject.Comm
{
    // 이식 대상: copyApp/Conf/SerialComm.cs — UI 비의존 계층이라 로직 거의 1:1 포팅 가능
    //
    // 필요한 멤버:
    //   - public static SerialComm Instance { get; }   (싱글톤, private 생성자)
    //   - public bool IsOpen { get; }
    //   - event EventHandler<string> DataReceived
    //   - event EventHandler<string> LogOccurred
    //   - event EventHandler Connected
    //   - bool Connect(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
    //   - void Disconnect()
    //   - void SendFramed(string payload)        // STX + payload + CR LF
    //   - void CheckTemp()                       // SendFramed("01TEMP")
    //   - bool VerifyConnection(int timeoutMs = 1000)
    //   - void Dispose()  (IDisposable)
    //
    // 포팅 시 주의:
    //   - using System.IO.Ports; 필요 → camProject.csproj에 System.IO.Ports 패키지 이미 추가되어 있음
    //   - 그 외 프레임워크 차이 없음 — 원본 로직 그대로 옮겨도 됨
    public sealed class SerialComm
    {
    }
}

namespace camProject.Model
{
    // 이식 대상: copyApp/Model/MainModel.cs — Serial 쪽 상태/온도 파싱/레시피 관리
    //
    // 필요한 멤버:
    //   - event EventHandler<string> LogOccurred / DataReceived
    //   - event EventHandler Connected
    //   - event EventHandler<int[]> TempReceived
    //   - bool IsConnected { get; }
    //   - static string RecipeDir  (ProtocolFile과 동일하게 BaseDirectory 기준으로 교체)
    //   - bool Connect(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits, string protocolName)
    //   - void Disconnect()
    //   - void PollingTemp()
    //   - Task<int[]> WaitFirstTempAsync(int timeoutMs)
    //   - void SaveSv(int[] sv, string path)
    //   - List<string> GetFileList()
    //   - int[] GetSelectedValue(string fileName)
    //   - void SendFramed(string payload)
    //   - private void TryParseTemp(string msg)  (내부 파싱, "01TEMP,OK,25,27,26,28" 형태)
    //
    // 포팅 시 주의:
    //   - Comm.SerialComm.Instance 구독(DataReceived/LogOccurred/Connected) 그대로 이동
    //   - RecipeDir의 Application.StartupPath → AppContext.BaseDirectory 교체 (ProtocolFile과 동일 이슈)
    //   - 이전 세션에서 확인된 버그: 연결 해제 분기에서 IsConnected를 false로 되돌리지 않던 문제 —
    //     포팅하며 Disconnect 분기에서 IsConnected = false; 를 반드시 넣을 것
    public class MainModel
    {
    }
}

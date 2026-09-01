namespace camProject.Model
{
    // 이식 대상: copyApp/Model/TcpModel.cs — TCP T1 → T2 → T3 상태머신
    //
    // 필요한 멤버:
    //   - event EventHandler<string> DataReceived / LogOccurred
    //   - bool IsConnected { get; } / bool ServerConnected { get; }
    //   - void UpdatePv(int[] pv) / void UpdateSv(int[] sv)
    //   - Task Connect(string ip, int port, int timeOutCnt, int retryCnt)
    //   - void ServerConn(string ipAddress, int port)
    //   - bool Disconnect()
    //   - void StartT1Process(string barcode, string recipe, string eqpId, string side)
    //   - void StartT2Process() / void StartT3Process()
    //   - private void OnData(object sender, string text)   — 상태머신 핵심 (수신 프레임 파싱 + 단계 전이)
    //
    // 포팅 시 함께 고칠 것 (이전 세션에서 실제로 찾은 버그들):
    //   1. WaitingT2Response 케이스에서 StartT3Process() 호출이 빠지지 않았는지 확인
    //      (T1 케이스가 StartT2Process()를 호출하는 것과 대칭이어야 함)
    //   2. StartT3Process 내부에서 상태를 WaitingT3Response로 정확히 설정 (WaitingT2Response로 잘못 쓰는
    //      복붙 오타가 원본에 있었음)
    //   3. 연결 해제 흐름에서 재연결이 유발되지 않는지 확인 (View 쪽 PreviousConn 이중 호출 버그와 유사한 패턴 주의)
    //
    // 권장 (선택 사항): 상태머신을 그대로 옮기지 말고, 이전 대화에서 제안한 타임아웃 기반
    // async 구조(SendAndWaitAsync + TaskCompletionSource)로 재작성하면 위 버그들이 구조적으로 사라짐.
    public class TcpModel
    {


    }
}

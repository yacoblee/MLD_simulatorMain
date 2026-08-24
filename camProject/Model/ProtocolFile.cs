namespace camProject.Model
{
    // 이식 대상: copyApp/Conf/ProtocolFIle.cs — Serial 연결 설정(포트/속도/패리티 등) 저장/로드
    //
    // 필요한 멤버:
    //   - PortName, BaudRateIndex, ParityIndex, BitIndex, LengthIndex, StopBitsIndex, ProtocolIndex 프로퍼티
    //   - void Save()
    //   - static ProtocolFile Load()
    //
    // 포팅 시 반드시 고칠 부분 (WinForms → net8 차이):
    //   - 원본이 Application.StartupPath(System.Windows.Forms) 사용 → WPF/net8엔 없음
    //     AppDomain.CurrentDomain.BaseDirectory 또는 AppContext.BaseDirectory로 교체
    public class ProtocolFile
    {
    }
}

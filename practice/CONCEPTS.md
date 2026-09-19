# 개념서 — HMI / MES 개발을 위한 WinForms 핵심 개념 (11/8 개념 리뷰용)

`PLAN.md` 4절의 15문항을 설명할 수 있게 만드는 것이 목표다. 각 장은 **핵심 → 코드 → 이 프로젝트에서 어디 → 흔한 실수 → 자기 점검** 순서다.
코드 예시는 C# 7.3 / .NET Framework 4.8 기준이다. 읽은 뒤 6장의 문항을 소리 내어 설명해 본다.

## 0. 전체 그림: 데이터가 흐르는 길

```
장비/Arduino ──시리얼──▶ [SerialPort 스레드] ──이벤트──▶ Model(파싱, 상태) ──이벤트──▶ View
                                 (ThreadPool)                                       │
MES 서버 ──────TCP────▶ [TcpComm.ReceiveLoop 스레드] ──이벤트──▶ TcpModel(상태머신)    ▼
                                                                              Invoke로 UI 스레드에서
                                                                              컨트롤 갱신
[타이머 스레드(System.Timers)]: 온도 폴링, 재연결, 워치독
```

- **수신은 항상 UI가 아닌 다른 스레드**에서 일어난다. 그래서 이벤트, 스레드, UI 전환, 잠금이 한 묶음으로 따라온다.
- 이 프로젝트의 계층: `copyApp\Conf\SerialComm.cs`, `TcpComm.cs`(통신) → `Model\MainModel.cs`, `TcpModel.cs`(파싱, 상태) → `View\MainView.cs`(화면).
  MES 쪽(`sendProject\Form1.cs`)은 수신 처리와 화면이 한 클래스에 섞여 있고, 시뮬레이터는 `Form1.Simulator.cs`에 있다.

---

## 1. 이벤트와 델리게이트 (문항 ①②③)

### 핵심
- **델리게이트**: 메서드를 담는 변수 타입. **이벤트**: 델리게이트에 "밖에서는 `+=`/`-=`만 가능, 발행은 선언한 클래스만"이라는 제한을 건 것.
- 발행하는 쪽은 누가 듣는지 모른다. 그래서 계층 분리(통신 → 모델 → 화면)가 가능하다.
- 표준 형태: `event EventHandler<T>`. 첫 인자 `sender`, 둘째 인자 데이터.

### 코드
```csharp
public class SerialComm
{
    public event EventHandler<string> DataReceived;          // 선언

    private void OnRx(string text)
    {
        DataReceived?.Invoke(this, text);                    // 발행: 구독자가 없으면 null이므로 ?. 필수
    }
}

// 구독 / 해제
comm.DataReceived += Model_OnData;
comm.DataReceived -= Model_OnData;
```

### 이 프로젝트에서
- `SerialComm`: `DataReceived`, `LogOccurred`, `Connected` 선언 (`copyApp\Conf\SerialComm.cs:17-19`), 수신 스레드에서 `DataReceived?.Invoke(this, text)` 발행(39행).
- `MainModel` 생성자가 `SerialComm.Instance.DataReceived += OnData`로 구독(45행). 그 모델이 다시 자기 이벤트(`TempReceived`, `LogOccurred`)를 발행하고, `MainView`가 이를 구독한다(`MainView.cs:59-69`).
- `SerialComm.Dispose`는 `DataReceived -= OnSerialDataReceived`로 해제한다(116행).

### 흔한 실수
- 발행 코드에 `?.`를 빼서 구독자가 없을 때 `NullReferenceException`.
- **구독 해제 누락**: 발행자(특히 `static Instance`처럼 오래 사는 객체)가 구독자를 계속 참조하므로 폼이 닫혀도 메모리와 이벤트가 남는다. 그 결과 닫힌 폼의 핸들러가 계속 호출되어 예외가 난다. (`TcpModel.Detach()`가 `DataReceived -= OnData`를 하는 이유)
- 람다(`(s, e) => ...`)로 구독하면 같은 람다를 다시 만들 수 없어 `-=`로 해제하기 어렵다. 해제가 필요하면 이름 있는 메서드를 쓰거나 람다를 필드에 보관한다.
- **이벤트 핸들러는 발행한 스레드에서 실행된다.** 수신 스레드가 발행하면 핸들러도 수신 스레드에서 돈다. UI를 만지는 핸들러라면 2장의 전환이 필요하다.

### 자기 점검
→ 문항 ①②③ (6장)

---

## 2. 스레드와 UI 스레드 (문항 ④⑤⑥⑦)

### 핵심
- WinForms 컨트롤은 **만든 스레드(UI 스레드)에서만** 접근할 수 있다. 다른 스레드에서 만지면 `InvalidOperationException`(크로스 스레드 작업)이 난다.
- 다른 스레드에서 UI를 갱신하려면 UI 스레드의 메시지 큐에 작업을 넣는다.

| | `Invoke` | `BeginInvoke` |
|---|---|---|
| 동작 | 작업을 큐에 넣고 **끝날 때까지 기다림** | 큐에 넣고 **즉시 반환** |
| 호출자 | UI 작업이 끝나야 다음 줄로 | 바로 계속 진행 |
| 위험 | **교착(deadlock)** | 폼이 이미 닫힌 뒤 실행되면 예외, 순서 보장은 큐 순서 |
| 용도 | 결과가 즉시 필요할 때 | 로그, 상태 표시처럼 기다릴 필요 없을 때 (대부분) |

- `InvokeRequired`: "지금 스레드가 UI 스레드가 아니면 true". 같은 스레드에서 불필요한 전환을 피하는 패턴에 쓴다.

### 코드
```csharp
// 패턴 1: 재귀로 자기 자신을 다시 부름 (copyApp\View\MainView.cs:185-197 의 WriteLog 방식)
public void WriteLog(string message)
{
    if (InvokeRequired)
    {
        Invoke(new Action(() => WriteLog(message)));
        return;
    }
    if (IsDisposed) return;
    textLog.AppendText(message + "\r\n");
}

// 패턴 2: 기다릴 필요 없는 로그는 BeginInvoke (sendProject\Form1.Simulator.cs 의 LogUi)
private void LogUi(string text)
{
    if (IsDisposed || !IsHandleCreated) return;
    BeginInvoke(new Action(() =>
    {
        if (!IsDisposed) txtBox.AppendText(text + "\r\n");
    }));
}
```

### 교착이 생기는 전형적인 상황 (⑤)
1. UI 스레드가 `serialPort.Close()`를 호출한다. `Close()`는 **`DataReceived` 핸들러가 끝나기를 기다린다.**
2. 그 순간 수신 스레드의 핸들러가 `Invoke(...)`로 UI 스레드에게 일을 시키고 기다린다.
3. 서로 상대를 기다려서 영원히 멈춘다.
- 대응: 수신 핸들러 안에서는 `BeginInvoke`를 쓰고, `Close()` 전에 수신 처리에서 UI를 기다리지 않게 한다.
- 이 프로젝트의 `copyApp\View\MainView.cs`는 `Invoke`를 쓰므로 이 패턴을 눈으로 확인해 볼 수 있다. 실제로 멈추는지는 11/9 실습에서 재현한다.

### `SerialPort.DataReceived`는 어느 스레드인가 (⑥)
- UI 스레드가 아니라 **ThreadPool의 보조 스레드**다. 그래서 `OnSerialDataReceived`에서 라벨을 직접 바꾸면 예외가 난다.
- 같은 이유로 이 이벤트를 받는 모든 하위 이벤트(Model → View)가 같은 스레드에서 실행된다.

### 폼 종료 중 BeginInvoke 예외 (⑦)
- 폼이 닫히는 도중 또는 이후에 도착한 수신 데이터가 `BeginInvoke`를 호출하면 `ObjectDisposedException` 또는 `InvalidOperationException`(핸들 없음)이 날 수 있다.
- 방어: `IsDisposed`, `IsHandleCreated` 확인 + `FormClosing`에서 통신과 타이머를 먼저 멈춘다. `Form1.Simulator.cs`의 `LogUi`가 이 방어를 한다.

### 흔한 실수
- 백그라운드에서 `textBox.Text += ...` 직접 접근.
- 무조건 `Invoke`를 써서 교착 위험을 만든다.
- `async/await` 이후에는 UI 스레드로 돌아오는 것이 기본이지만(`SynchronizationContext`), `Task.Run` 안쪽이나 이벤트 핸들러 안쪽에서는 아니다. "이 코드가 지금 어느 스레드인가"를 항상 묻는 습관이 필요하다.

---

## 3. 타이머와 잠금 (문항 ⑧⑨⑩)

### 타이머 3종 (⑧)
| 타이머 | 콜백 이벤트 | 실행 스레드 | 특징 |
|---|---|---|---|
| `System.Windows.Forms.Timer` | `Tick` | **UI 스레드** | UI를 직접 만질 수 있음. 핸들러가 느리면 화면이 멈춤. 정밀도 낮음(수십 ms) |
| `System.Timers.Timer` | `Elapsed` | **ThreadPool** (기본) | `AutoReset`으로 반복/1회 선택. UI 접근 시 `Invoke` 필요. **.NET Framework에서는 `Elapsed` 핸들러의 예외가 조용히 삼켜짐** → 내부에 `try/catch` 필수 |
| `System.Threading.Timer` | 콜백 | ThreadPool | 가장 가벼움. 이벤트 모델이 아니라 콜백 |

### 이 프로젝트에서
- `MainModel._tempTimer`: `System.Timers.Timer(30000)` 온도 폴링(`MainModel.cs:76-85`). `Elapsed`는 풀 스레드에서 돌고, 거기서 `SerialComm.CheckTemp()`를 부른다.
- `TcpModel._retryTimer`: `AutoReset = false` 1회용으로 재연결을 예약한다(`TcpModel.cs:197-218`). 실패하면 다시 `Start`해서 재시도 횟수를 센다.
- `sendProject\Form1.Simulator.cs`의 `_sweepTimer`: `Forms.Timer`. `Tick`이 UI 스레드에서 돌아서 각도 라벨을 바로 갱신한다.

### 워치독(⑨)
- 요청을 보내고 응답을 기다리는 동안 "N초 안에 응답 없으면 실패 처리"를 하는 장치.
- **`System.Timers.Timer` + `AutoReset = false`가 맞다**: 한 번만 울리면 되고, 응답이 오면 `Stop()`으로 취소한다. 상태를 바꾸는 코드가 UI 스레드가 아니어도 되기 때문이다.
- 주의: 응답 도착과 `Elapsed`가 거의 동시에 일어나면 경합이 생긴다. 상태 확인 + 변경을 `lock`으로 묶거나, `Elapsed` 안에서 "아직 같은 요청을 기다리는 중인지" 한 번 더 확인한다.
- 참고: copyApp의 `TcpModel`에는 T1~T3 응답 워치독이 **없다**. 서버가 응답하지 않으면 `WaitingT1Response`에서 영원히 머문다. 이것이 camProject에서 보강한 이유이고, 11/16 과제에서 직접 구현한다.

### lock (⑩)
- **여러 스레드가 같은 가변 상태를 동시에 읽고 쓸 때** 쓴다. 예: 시리얼 수신 스레드 + 타이머 스레드 + UI 스레드가 모두 `_state`나 같은 `SerialPort.Write`를 만지는 경우.

```csharp
private readonly object _lock = new object();   // private readonly 객체에만 lock

lock (_lock)
{
    if (_state != State.Idle) return;
    _state = State.Stage1;
}
```
- 규칙: ① 잠금 구간은 짧게 ② **잠금 안에서 이벤트 발행/`Invoke`/긴 I/O 금지**(교착, 재진입 위험) ③ 잠금 대상은 `this`나 문자열이 아닌 전용 객체.
- 이 프로젝트: `sendProject`의 `_serialLock`은 수신 스레드의 응답 전송과 UI 타이머의 `DONE` 전송이 같은 시리얼 포트에 동시에 `Write`하지 않도록 한다.

### 흔한 실수
- 필드를 여러 스레드가 쓰면서 잠금이 없어 값이 어긋난다(경쟁 조건).
- `lock`을 걸고 그 안에서 `Invoke`로 UI를 기다린다 → 교착.
- `Elapsed` 안의 예외가 삼켜져서 "타이머가 그냥 안 도는" 것처럼 보인다.

---

## 4. 통신 (문항 ⑪⑫⑬⑭)

### 바이트 스트림과 프레임 (⑪)
- 시리얼과 TCP는 **바이트의 흐름**일 뿐 "메시지 단위"가 없다. 보낸 쪽이 한 번에 `Write`한 것이 받는 쪽에서 한 번에 `Read`된다는 보장이 없다.
  - 쪼개져 올 수 있다: `"01TE"` / `"MP,OK,25..."`
  - 붙어서 올 수 있다: `"01CHECK,OK\r\n01TEMP,OK,..."`
- 그래서 **수신 버퍼에 계속 누적하고, 끝 표시가 나오면 한 건씩 잘라낸다.**

```csharp
private string _rx = "";

private void OnChunk(string chunk)
{
    _rx += chunk;                                   // 1) 누적
    while (true)
    {
        int end = _rx.IndexOf('\n');                // 2) 끝 표시 탐색
        if (end < 0) break;                         //    아직 안 왔으면 다음 조각을 기다림
        string frame = _rx.Substring(0, end + 1);   // 3) 한 건 분리
        _rx = _rx.Substring(end + 1);               // 4) 나머지는 버퍼에 남김
        Handle(frame);
    }
}
```

### 프레임 형식 (⑫)
| 구간 | 형식 | 이유 |
|---|---|---|
| 시리얼 | `STX(0x02) + 본문 + CR LF` | 시작과 끝 표시로 잡음, 중간에 붙은 쓰레기를 구분 |
| TCP | `STX(0x02) + 본문 + ETX(0x03)` | 본문에 줄바꿈이 있어도 안전 |
| TCP 본문 | `T1/바코드/레시피/설비/면` (`/` 구분) | 필드 구분 |

- 이 프로젝트에서 가장 교과서적인 구현은 `TcpComm.ReceiveLoop`(`copyApp\Conf\TcpComm.cs:160-202`): `_rxBuffer += text` → `STX`, `ETX` 위치 탐색 → 잘라서 `DataReceived` 발행 → 나머지는 버퍼에 보관.
- 개선 포인트 읽기 연습: `MainModel.OnData`(`MainModel.cs:50-71`)는 버퍼에 `TEMP`가 보일 때만 줄 단위 처리를 한다. `TEMP`가 한 번도 오지 않으면(`CHECK`만 오는 경우 등) 버퍼가 비워지지 않고 계속 쌓인다.

### 인코딩 (⑬)
- `Encoding.ASCII`는 0~127만 표현한다. 그 밖의 문자(한글)는 **`?`(0x3F)로 바뀐다.** 이 프로젝트에서 레시피 이름 `레시피1`이 MES에 `???1`로 도착한 것이 그 사례다 (`TcpComm.SendFramed`, `SerialComm.SendFramed`가 ASCII 사용).
- 장비 프로토콜은 대개 ASCII로 정의한다. 한글이 필요하면 양쪽이 UTF-8(또는 정해진 코드페이지)로 합의해야 하고, 합의가 안 되면 입력 단계에서 영문/숫자로 제한하는 편이 안전하다.
- 바이트 단위로 쪼개 받은 UTF-8 한글을 조각마다 `GetString`하면 깨진다. 한글을 쓴다면 프레임을 다 모은 뒤에 디코딩한다.

### 시리얼의 Write / Close가 멈추는 경우 (⑭)
- `SerialPort.WriteTimeout` 기본값은 `InfiniteTimeout`이다. 상대가 읽지 않아 버퍼가 차면(예: 가상 포트 상대가 열려 있지 않음) 동기 `Write`가 **무한 대기**하고, 그 상태에서 `Close()`도 끝나지 않아 프로그램이 멈춘다. 이 프로젝트에서 com0com으로 재현해 확인한 사실이다.
- 대응: `WriteTimeout`(예: 1000ms)을 설정하면 동기 `Write`는 `TimeoutException`으로 끝난다. `BaseStream.WriteAsync`는 이 타임아웃을 따르지 않으므로 대안이 되지 못했다.
- `Close()`는 `DataReceived` 핸들러가 끝나길 기다리므로, 2장의 교착 패턴과도 연결된다.

### 하트비트
- 일정 주기로 `CHECK`를 보내고 `CHECK,OK` 응답이 N회 연속 없으면 "통신 끊김"으로 판단하는 방식. 케이블이 빠져도 `IsOpen`은 `true`일 수 있어서 **응답 여부로 판단해야 한다.** 이 프로젝트: 5초 주기 + 3회 재시도.

### TCP 연결 상태 판단 주의
- `TcpClient.Connected`는 **마지막 I/O 시점의 상태**를 반영한다. 상대가 끊어도 바로 `false`가 되지 않는다. `ReceiveLoop`에서 `Read`가 0을 반환하는 순간이 "상대가 끊었다"는 신호다 (`TcpComm.cs:167-168`, `finally`에서 `DisconnectedEvent` 발행).

---

## 5. 상태머신과 도메인 (문항 ⑮)

### 상태머신
- **상태(State)** + **이벤트(입력)** + **전이(Transition)** + **동작(Action)**으로 시스템 흐름을 표로 정리하는 방식. 장비 SW의 기본 사고방식이다.
- 규칙: ① 모든 상태에서 "허용되지 않는 이벤트가 오면 어떻게 하는가"를 정한다 ② **모든 대기 상태에는 타임아웃**을 둔다 ③ 실패/NG/끊김/취소 경로에서 반드시 `Idle`로 돌아오고 정리 동작(타이머 정지, 장비 정지 명령)을 한다.

`copyApp\Model\TcpModel.cs`의 T1~T3 (`ProcessState`):

| 현재 상태 | 이벤트 | 동작 | 다음 상태 |
|---|---|---|---|
| Idle | `StartT1Process` | `T1/…` 전송 | WaitingT1Response |
| WaitingT1Response | `T1/…/OK` 수신 | `T2/…` 전송 | WaitingT2Response |
| WaitingT2Response | `T2/…/OK` 수신 | `T3/…` 전송 | WaitingT3Response |
| WaitingT3Response | `T3/…/OK` 수신 | `ProcessCompleted` 발행 | Idle |
| 어느 대기 상태든 | `…/NG` 수신 | 로그 | Idle |
| 어느 대기 상태든 | **응답 없음(타임아웃)** | *copyApp에는 없음* | *(없음 → 영구 대기)* |

- 같은 `enum + switch(_currentState)` 모양으로 장비 쪽 사이클(`Idle / Stage1 / Stage2`)도 표현할 수 있다.

### 이 프로젝트의 3자 구조
```
HMI(copyApp/camProject) ──시리얼──▶ 장비(Arduino/시뮬레이터)   : 온도 PV 폴링, 구동 명령
        │
        └────────TCP────────▶ MES(sendProject)               : 공정 단계 보고/승인
```
| 용어 | 의미 (이 프로젝트에서) |
|---|---|
| PV / SV | 현재값(Process Value) / 설정값(Set Value). 예: 존별 현재 온도 / 목표 온도 |
| 레시피 | SV 묶음을 이름 붙여 저장한 것(JSON). 공정 조건표 |
| `CHECK` | 하트비트(통신 감시) |
| `T1 / T2 / T3` | 이 프로젝트에서의 해석: T1 = 시작 요청(바코드, 레시피, 설비, 면), T2 = 진행 데이터(PV, SV 포함), T3 = 완료 데이터. MES는 각 단계마다 OK/NG로 응답 |
| `STX/ETX/CRLF` | 프레임 시작/끝 |
| 바코드 | 제품(기판) 식별자. MES 이력의 키 |

### SMEMA와의 관계 (면접 대비)
- SMEMA는 장비 사이를 **건접점 신호 2개(Board Available, Machine Ready)**로 잇고, 둘이 동시에 ON일 때만 기판을 이송하는 규격이다. 이 프로젝트는 그 개념을 LED 2개와 소프트웨어 AND 조건으로 **모사**한 것이지 구현이 아니다.

---

## 6. 자기 점검 15문항과 모범 답안
소리 내어 1분씩 설명한 뒤 답을 확인한다. **12문항 이상 막힘 없이 설명하면 통과.** 막힌 문항은 `STUCK.md`에 번호와 이유를 적는다.

| # | 질문 | 모범 답안(요지) |
|---|---|---|
| ① | `event EventHandler<T>`의 선언, 구독, 발행을 설명하라 | `public event EventHandler<string> X;` 선언 → 밖에서 `obj.X += 핸들러;` 구독 → 선언한 클래스 안에서 `X?.Invoke(this, 값)` 발행. `?.`는 구독자가 없을 때 null이라서 |
| ② | 구독을 해제하지 않으면 무슨 문제가 생기나 | 발행자가 구독자 참조를 계속 들고 있어 GC되지 않고(메모리 누수), 닫힌 폼의 핸들러가 계속 호출되어 예외/중복 처리가 난다. 특히 `static Instance`처럼 오래 사는 발행자일 때 |
| ③ | 람다 구독은 왜 해제가 어렵나 | `-=`는 같은 델리게이트 인스턴스를 요구하는데 람다는 매번 새로 만들어지는 익명 메서드라 같은 것을 다시 만들 수 없음. 이름 있는 메서드를 쓰거나 람다를 변수에 보관 |
| ④ | 컨트롤을 만든 스레드에서만 접근해야 하는 이유 | 컨트롤은 윈도우 핸들과 메시지 루프에 묶여 있고 스레드 안전하지 않다. 다른 스레드가 건드리면 상태가 깨질 수 있어 .NET이 크로스 스레드 예외로 막는다 |
| ⑤ | `Invoke`와 `BeginInvoke` 차이, 교착이 생기는 경우 | `Invoke`는 UI 스레드가 끝낼 때까지 기다리고 `BeginInvoke`는 큐에만 넣고 반환. UI 스레드가 수신 스레드를 기다리는 중(예: `SerialPort.Close()`가 핸들러 종료를 대기)에 수신 스레드가 `Invoke`로 UI를 기다리면 서로 영원히 대기 |
| ⑥ | `DataReceived`는 어느 스레드에서 오나 | UI 스레드가 아니라 ThreadPool 보조 스레드. 그 핸들러에서 UI를 만지려면 `Invoke/BeginInvoke` 필요 |
| ⑦ | 폼이 닫히는 중 `BeginInvoke`가 예외를 내는 경우 | 폼이 dispose되었거나 핸들이 아직 없을 때 호출하면 `ObjectDisposedException` / `InvalidOperationException`. `IsDisposed`/`IsHandleCreated` 확인, `FormClosing`에서 통신·타이머를 먼저 정지 |
| ⑧ | 세 타이머의 실행 스레드 차이 | `Forms.Timer`: UI 스레드의 `Tick` / `System.Timers.Timer`: ThreadPool의 `Elapsed`(.NET Framework는 예외를 삼킴) / `Threading.Timer`: ThreadPool 콜백 |
| ⑨ | 워치독에 어느 타이머가 맞고 왜 | `System.Timers.Timer`, `AutoReset=false`. 한 번만 울리면 되고 응답 도착 시 `Stop`으로 취소. UI 스레드가 필요 없고 UI 블로킹에 영향받지 않는다. 응답과 `Elapsed` 경합은 `lock`/상태 재확인으로 처리 |
| ⑩ | `lock`이 필요한 상황 | 서로 다른 스레드(수신, 타이머, UI)가 같은 가변 상태나 같은 포트 쓰기를 동시에 접근할 때. private readonly 객체로 잠그고, 구간을 짧게, 안에서 `Invoke`/이벤트 발행 금지 |
| ⑪ | 시리얼/TCP에서 메시지 경계가 없는 이유와 버퍼 누적 | 스트림이라 한 번의 쓰기가 한 번의 읽기와 대응하지 않는다(쪼개짐/붙음). 수신마다 버퍼에 이어 붙이고 끝 표시를 찾아 한 건씩 자르며 남는 조각은 보관 |
| ⑫ | STX/ETX/CRLF로 프레임을 나누는 방법 | 시작 표시(STX)와 끝 표시(ETX 또는 CRLF) 사이만 한 메시지로 취급. 버퍼에서 끝 표시 위치를 찾고, 그 앞의 시작 표시 뒤부터 잘라 발행한 뒤 나머지를 버퍼에 남김 |
| ⑬ | ASCII로 보내면 한글이 `?`가 되는 이유 | ASCII는 0~127만 표현. 범위 밖 문자는 대체 문자 `?`(0x3F)로 인코딩된다(`레시피1` → `???1`). UTF-8 합의 또는 영문/숫자 제한이 필요 |
| ⑭ | 시리얼 `Write`/`Close`가 멈추는 경우와 `WriteTimeout` | `WriteTimeout` 기본이 무한이라 상대가 읽지 않아 버퍼가 차면 동기 `Write`가 영원히 블록되고 `Close`도 멈춘다. `WriteTimeout`을 설정하면 `TimeoutException`으로 끝난다 |
| ⑮ | HMI–MES–장비 구조와 상태머신 | HMI가 시리얼로 장비(PV 폴링, 구동 명령), TCP로 MES(T1~T3 보고)와 통신. 상태머신은 상태/이벤트/전이로 정의하고, 모든 대기 상태에 타임아웃, 실패 경로에서 Idle 복귀와 정리 동작을 둔다 |

---

## 7. 오늘의 학습 순서 (제안)
| 순서 | 내용 | 방법 |
|---|---|---|
| 1 | 0장 + 1장 | 읽고, `SerialComm.cs`/`MainModel.cs`/`MainView.cs`를 열어 이벤트 연결 줄(`+=`)을 따라가 보기 |
| 2 | 2장 | `MainView.cs`의 `WriteLog`, `SetPvRow`와 `Form1.Simulator.cs`의 `LogUi`를 비교 |
| 3 | 3장 | `MainModel.PollingTemp`, `TcpModel.StartRetryTimer`, `Form1.Simulator.cs`의 `_serialLock` 확인 |
| 4 | 4장 | `TcpComm.ReceiveLoop`를 한 줄씩 손으로 따라가기 (예: `STX AB ETX STX C` 다음 `D ETX`가 올 때 버퍼 변화를 종이에 적기) |
| 5 | 5장 | `TcpModel.ProcessState`를 표로 직접 다시 그리기. 타임아웃이 없는 칸을 찾기 |
| 6 | 6장 | 15문항을 소리 내어 설명, 막힌 문항은 `STUCK.md`에 기록 |

**목표는 외우기가 아니라 "코드를 보지 않고도 흐름을 말로 설명할 수 있는 상태"다.** 설명이 막히는 곳이 11/9부터 직접 짜면서 가장 많이 막힐 곳이다.

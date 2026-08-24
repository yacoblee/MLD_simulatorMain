using System;
using System.Windows.Input;

namespace camProject.ViewModel
{
    // copyApp에는 대응 파일 없음 — WPF MVVM에서 새로 필요한 ICommand 표준 구현체
    //
    // 필요한 멤버:
    //   - 생성자: RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
    //   - Execute(object? parameter) → execute 호출
    //   - CanExecute(object? parameter) → canExecute?.Invoke(parameter) ?? true
    //   - event EventHandler? CanExecuteChanged
    //     → add/remove를 CommandManager.RequerySuggested에 위임하면 버튼 활성/비활성이 자동 갱신됨
    //
    // 비동기 커맨드가 필요하면(TcpModel.Connect 같은 Task 메서드 연결 시)
    // 같은 폴더에 AsyncRelayCommand를 별도로 추가하는 걸 권장 (Execute 안에서 async void 대신
    // Task를 저장해두고 중복 실행 방지하는 형태)
    public class RelayCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => throw new NotImplementedException();

        public void Execute(object? parameter) => throw new NotImplementedException();
    }
}

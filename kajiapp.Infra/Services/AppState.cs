using System;
using System.Threading.Tasks;

public class AppState
{
    public event Action? OnChange;

    // 他のコンポーネントに通知を送信
    public void NotifyStateChanged() => OnChange?.Invoke();
}
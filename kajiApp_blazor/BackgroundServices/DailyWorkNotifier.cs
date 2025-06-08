using kajiapp.Infra.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

public class DailyWorkNotifier : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DailyWorkNotifier> _logger;
    private readonly bool _testMode;

    public DailyWorkNotifier(
        IServiceProvider serviceProvider,
        ILogger<DailyWorkNotifier> logger,
        IConfiguration config)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _testMode = config.GetValue<bool>("Notification:TestMode");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        ///テストモード
        if (_testMode)
        {
            _logger.LogInformation("🧪 [TESTモード] 即時実行します");
            await NotifyAsync();
        }
        ///本番モード
        else
        {
            _logger.LogInformation("⏰ [PRODモード] スケジューラ開始（JST 7:00）");

            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    var now = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Tokyo Standard Time");
            //    var nextRun = now.Date.AddDays(now.Hour >= 7 ? 1 : 0).AddHours(7);
            //    var delay = nextRun - now;

            //    _logger.LogInformation($"次の実行は JST {nextRun}、{delay.TotalMinutes:F0} 分後");

            //    await Task.Delay(delay, stoppingToken);

            //    await NotifyAsync();

            //    await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            //}

            /// 7:00 JST に毎日実行するためのループに修正
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Tokyo Standard Time");
                var nextRun = now.Date.AddDays(now.Hour >= 7 ? 1 : 0).AddHours(7);
                var delay = nextRun - now;

                _logger.LogInformation($"次の実行は JST {nextRun}、{delay.TotalMinutes:F0} 分後");

                await Task.Delay(delay, stoppingToken); // JST 7:00 まで待機

                await NotifyAsync();

                // 次ループで再度「次のJST 7:00」を計算し直すので、ズレない
            }
        }
    }

    private async Task NotifyAsync()
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IWorkRepository>();
            var notifier = scope.ServiceProvider.GetRequiredService<DiscordNotifier>();

            var works = await repo.GetWorksOfPreviousDayAsync();
            var msg = works.Count > 0
                ? string.Join("\n", works.Select(w => $"{w.Day}: {w.Name} - {w.Work1}"))
                : "前日のデータが存在しません。";

            var finalMessage = $"📢 前日の家事実績通知\n```\n{msg}\n```";
            await notifier.SendAsync(finalMessage);
            _logger.LogInformation("✅ 通知送信しました");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ 通知処理でエラー発生");
        }
    }
}

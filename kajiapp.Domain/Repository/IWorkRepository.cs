using kajiApp_blazor.Domain.Entity;

public interface IWorkRepository
{
    /// <summary>
    /// 前日の家事実績を通知する
    /// </summary>
    /// <returns></returns>
    Task<List<Work>> GetWorksOfPreviousDayAsync();
}
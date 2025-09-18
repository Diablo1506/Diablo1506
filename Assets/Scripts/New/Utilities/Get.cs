using New.Managers;
using New.SO;

public static class Get
{
    public static CameraManager CameraManager => PersistentManager.Instance.CameraManager;
    public static UIManager UIManager => PersistentManager.Instance.UIManager;
    public static GameManager GameManager => PersistentManager.Instance.GameManager;
    public static PunchDataCollection PunchDataCollection => PersistentManager.Instance.PunchDataCollection;
    public static GlobalDataCollection GlobalDataCollection => PersistentManager.Instance.GlobalDataCollection;
    public static DifficultyDatabase DifficultyDatabase => PersistentManager.Instance.DifficultyDatabase;
}

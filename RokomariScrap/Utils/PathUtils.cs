namespace RokomariScrap.Utils;

public static class PathUtils
{
    public static string? TopLevelPath(string currentDir)
    {
        var dir = new DirectoryInfo(currentDir);
        return dir.Parent?.Parent?.Parent?.FullName;
    }

    public static string? TopLevelSqlitePath(string currentDir, string dbName = "sqlite.db")
    {
        var path = TopLevelPath(currentDir);
        return Path.Combine(path ?? AppDomain.CurrentDomain.BaseDirectory, dbName);
    }
}
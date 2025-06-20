public class DBBackup
{
    public string AdminId { get; }
    public string RestoreCode { get; }
    public string DbPath { get; }

    public DBBackup(string adminId, string restoreCode, string DbPath)
    {
        this.AdminId = adminId;
        this.RestoreCode = restoreCode;
        this.DbPath = DbPath;
    }
}
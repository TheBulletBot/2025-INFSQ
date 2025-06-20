using System.Text.Json;
using System.Text.Json.Nodes;

static class Logging
{
    private static string projectRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../");
    public static void Log(string username, string action, string description, bool isSuspicious)
    {
        var today = DateTime.Now;
        var todayDateString = today.ToString("d");
        var todayTimeString = today.ToString("T");
        var logFilePath = Path.Combine(projectRoot, $"Logs/{today:dd-MM-yyyy}.log");
        if (!File.Exists(logFilePath))
        {
            CreateLogFile(logFilePath,todayDateString, todayTimeString, username);
        }
        var encryptedJString = File.ReadAllText(logFilePath);
        var jString = CryptographyHelper.Decrypt(encryptedJString);
        List<LogItem> logs = JsonSerializer.Deserialize<List<LogItem>>(jString);
        var nextIdInLine = logs.Last().Id + 1;

        LogItem newLogItem = new(nextIdInLine, todayDateString, todayTimeString, username, action, description, isSuspicious);
        logs.Append(newLogItem);
        var writableJString = JsonSerializer.Serialize(logs);
        var encryptedWritableString = CryptographyHelper.Encrypt(writableJString);

        File.WriteAllText(logFilePath,encryptedWritableString);
    }
    private static void CreateLogFile(string filePath, string todayDateString,string todayTimeString, string username)
    {
        LogItem topRow = new(0, todayDateString, todayTimeString, username, "Log File Creation", "Created New log file for today", false);
        List<LogItem> logFile = [topRow];
        var jString = JsonSerializer.Serialize<List<LogItem>>(logFile);
        var encryptedJString = CryptographyHelper.Encrypt(jString);
        //"|Id|Date|Time|Username|Action|Description|Suspicious|\n";
        File.WriteAllText(filePath, encryptedJString);
    }
    public class LogItem {
        public int Id { get; }
        public string Date { get; }
        public string Time { get; }
        public string Username { get; }
        public string Action { get; }
        public string Description { get; }
        public bool Suspicious { get; }
        public LogItem(int id, string date, string time, string username, string action, string description, bool Suspicious)
        {
            this.Id = id;
            this.Action = action;
            this.Date = date;
            this.Time = time;
            this.Username = username;
            this.Description = description;
            this.Suspicious = Suspicious;
        }
    }
}


using System.Data.Entity.Core.Common.CommandTrees;

class Program
{
    public static void Main(string[] args)
    {
        DBSetup.CreateDBFile();
        DBSetup.SetupDB();
    }
}
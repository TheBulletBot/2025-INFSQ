// See https://aka.ms/new-console-template for more information
using System.Runtime.InteropServices;
//DBSetup.PopulateScooterTable();
var the = DatabaseHelper.Query<Scooter>("SELECT * FROM Scooter");
System.Console.WriteLine(the[0]);
using System;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using ScooterBackend;


public class SupermADFunc
{
    private readonly byte[] _aesKey = Encoding.UTF8.GetBytes("1234567890ABCDEF");
    private readonly byte[] _aesIV = Encoding.UTF8.GetBytes("FEDCBA0987654321");
    private readonly string connection = @"Data Source=C:\Users\rensg\OneDrive\Documenten\GitHub\2025-INFSQ-2\db\db\INFSQScooterBackend.db";
}
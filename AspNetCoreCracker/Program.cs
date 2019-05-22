using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Timers;
using CommandLine;
using ThreadGun;

namespace AspNetCoreCracker
{
    internal static class Program
    {
        public static int HashPerMin;
        public static int CheckPerSec;
        public static int CheckedHash;
        public static int CrackedHash;
        public static string CurrentHash;

        private static bool _completed;
        private static int _tempHashPerMin;
        private static int _tempCheckPerSec;
        private static ThreadGun<string> _threadGun;
        private static readonly List<string> _result = new List<string>();

        private static void Main(string[] args)
        {
            try
            {
                Parser.Default.ParseArguments<Options>(args).WithParsed(Crack);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            while (!_completed)
            {
                var key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.P:
                        _threadGun.Pause();
                        Console.WriteLine("\b[!] Paused");
                        break;
                    case ConsoleKey.R:
                        _threadGun.Resume();
                        Console.WriteLine("\b[!] Resume");
                        break;
                    case ConsoleKey.S:
                        File.WriteAllLines("result_" + DateTime.Now.ToString("yyyyMMdd_hh_mm") + ".txt", _result);
                        Console.WriteLine("\b[!] Saved");
                        break;
                }
            }
        }

        private static void Crack(Options options)
        {
            var hashes = File.ReadAllLines(options.HashFile);
            var stopwatch = new Stopwatch();
            var passwords = File.ReadAllLines(options.PasswordFile);

            stopwatch.Start();

            Timer hashSpeedTimer;
            (hashSpeedTimer = new Timer(60 * 1000)).Elapsed += (sender, args) =>
            {
                HashPerMin = _tempHashPerMin;
                _tempHashPerMin = 0;
            };
            hashSpeedTimer.Start();
            Timer checkSpeedTimer;
            (checkSpeedTimer = new Timer(1000)).Elapsed += (sender, args) =>
            {
                CheckPerSec = _tempCheckPerSec;
                _tempCheckPerSec = 0;
            };
            checkSpeedTimer.Start();
            Timer printTimer;
            (printTimer = new Timer(2000)).Elapsed += (sender, args) =>
            {
                Console.Clear();
                Console.Write($@"[+] Cracked Hash: {CrackedHash}
[+] Checked Hash: {CheckedHash}
[+] Current Hash: {CurrentHash}
[+] Hash/Min: {HashPerMin}
[+] Check/Sec: {CheckPerSec}
[+] Progress: {CheckedHash}/{hashes.Length} {Math.Round((double) CheckedHash / hashes.Length * 100)}%
[+] Time: {stopwatch.Elapsed.TimeSpanToString()}

[P] Pause - [R] Resume - [S] Save
");
            };
            printTimer.Start();

            _threadGun = new ThreadGun<string>(hash =>
                {
                    _tempHashPerMin++;
                    CheckedHash++;
                    CurrentHash = hash;
                    Parallel.ForEach(passwords, (password, state) =>
                    {
                        _tempCheckPerSec++;
                        if (!Crypto.VerifyHashedPassword(hash, password)) return;
                        _result.Add(hash + ":" + password);
                        CrackedHash++;
                        state.Break();
                    });
                }, hashes, int.Parse(options.ThreadCount)
                , inputs =>
                {
                    File.WriteAllLines("result.txt", _result);
                    stopwatch.Stop();
                    printTimer.Stop();
                    hashSpeedTimer.Stop();
                    checkSpeedTimer.Stop();
                    Console.WriteLine("[+] Finished");
                    _completed = true;
                }).FillingMagazine().Start();
        }

        public static string TimeSpanToString(this TimeSpan Ts)
        {
            if (Ts.TotalDays > 1d)
                return Ts.ToString("d'd 'h'h 'm'm 's's'");

            if (Ts.TotalHours > 1d)
                return Ts.ToString("h'h 'm'm 's's'");

            if (Ts.TotalMinutes > 1d)
                return Ts.ToString("m'm 's's'");

            if (Ts.TotalSeconds > 1d)
                return Ts.ToString("s's'");

            if (Ts.TotalMilliseconds > 1d)
                return Ts.ToString("fffffff'ms'");

            return Ts.ToString();
        }
    }
}
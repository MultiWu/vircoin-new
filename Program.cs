using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace vircoin_new
{
    class Program
    {

        public static int CurrentTime => (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        public static int poprawne = 0;
        public static int wszystkie = 0;
        public static int poprawneOgolem = 0;
        public static int wszystkieOgolem = 0;
        public static int s1Poprawne = 0;
        public static int s1Wszystkie = 0;
        public static int s60Poprawne = 0;
        public static int s60Wszystkie = 0;
        static void menu()
        {
            do
            {
                while (Console.ReadKey().Key == ConsoleKey.Enter)
                {
                    Console.Clear();
                    if (s1Wszystkie > 999)
                    {
                        int s1WszystkieKH = s1Wszystkie / 1000;
                        Console.WriteLine("1s: " + s1WszystkieKH + "Kh/s");
                    }
                    else
                    {
                        Console.WriteLine("1s: " + s1Wszystkie + "h/s");
                    }
                    if (s60Wszystkie > 999)
                    {
                        int s60WszystkieKH = s60Wszystkie / 1000;
                        Console.WriteLine("60s: " + s60WszystkieKH + "Kh/s");
                    }
                    else
                    {
                        Console.WriteLine("60s: " + s60Wszystkie + "h/s");
                    }
                }
            } while (1 == 1);
        }
        static void licznik()
        {
            int s = 0;

            do
            {
                Thread.Sleep(1000);
                s++;
                s1Poprawne = poprawne;
                s1Wszystkie = wszystkie;
                poprawne = 0;
                wszystkie = 0;
                if (s >= 60)
                {
                    s = 0;
                    s1Poprawne = s60Poprawne;
                    s1Wszystkie = s60Wszystkie;
                }
            } while (1 == 1);
        }
        static void Main(string[] args)
        {
            Console.Title = "VIRCOIN Node";
            Thread thr1 = new Thread(licznik);
            Thread thr2 = new Thread(menu);
            Console.WriteLine("VIRCOIN");
            // case number one (difficulty = 2)
            int difficulty = 5;
            int finishTime = CurrentTime + 15;
            thr1.Start();
            thr2.Start();
            do
            {
                ChallengeResult result = SolveTheChallenge(RandomStringGenerator.GenerateByLen(10), difficulty);
                poprawne++;
            } while (1 == 1);
            //} while (CurrentTime < finishTime);
            //Console.WriteLine($"Total number of solutions found for (difficulty = 2): {numberOfSolutionsFound}");
            //ChallengeResult result = SolveTheChallenge(RandomStringGenerator.GenerateByLen(10), difficulty);
            //Console.WriteLine(result.SolveHash);
            //Console.WriteLine(result.SolveString);

            //Console.ReadKey();
        }
        private static ChallengeResult SolveTheChallenge(string challengeString, int difficulty)
        {
            var challengeResult = new ChallengeResult();
            string expectedResult = new string('0', difficulty);  // if the difficulty = 4 the expectedResult will be = '0000'
            var stopwatch = new Stopwatch();
            stopwatch.Start(); // doing time measurement
            do
            {
                challengeResult.NumberOfIterations++; // counting the number of iterations
                challengeResult.SolveString = challengeString + RandomStringGenerator.GenerateByLen(10); // generating random string to test it
                challengeResult.SolveHash = Sha256Generator.GenerateSha256String(challengeResult.SolveString);  // generating sha-256 from the random string
                wszystkie++;
                wszystkieOgolem++;
            } while (!challengeResult.SolveHash.StartsWith(expectedResult)); // checking if we found a solution
            challengeResult.SolveSeconds = stopwatch.Elapsed.TotalSeconds;
            return challengeResult;
        }
    }
    public class ChallengeResult
    {
        // Contain result string 
        public string SolveString { get; set; }

        // Contain the number of seconds was spend to solve the challenge
        public double SolveSeconds { get; set; }

        // Contain hash of 'SolveString'
        public string SolveHash { get; set; }

        // Contain the number of iterations done to solve the challenge
        public int NumberOfIterations { get; set; }
    }
    public class RandomStringGenerator
    {
        private static readonly Random Random = new Random();
        /// <summary>
        /// Use RNGCryptoServiceProvider to generate truly random byte arrays
        /// </summary>
        public static string GenerateByLen(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
    }
    public class Sha256Generator
    {
        public static string GenerateSha256String(string inputString)
        {
            var sha256 = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha256.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            foreach (byte t in hash)
                result.Append(t.ToString("X2"));
            return result.ToString();
        }
    }
}

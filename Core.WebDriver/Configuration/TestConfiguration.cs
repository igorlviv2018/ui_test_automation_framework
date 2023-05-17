using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Taf.UI.Core.Configuration
{
    //public class TestConfiguration
    //{
    //    public IConfigurationRoot Config { get; }

    //    public TestConfiguration()
    //    {
    //        Config = new ConfigurationBuilder()
    //                        .AddJsonFile("appsettings.json")
    //                        //.AddJsonStream(jsonStream)
    //                        //.AddJsonStream(jsonDbConnectStr)
    //                        //.AddJsonStream(jsonTestEnv)
    //                        // Commented out to be able to run tests using Github Actions workflow (under Linux)
    //                        .AddUserSecrets("0bebd5d6-7c39-4e62-9f1c-6866e6aff9c6") //comment out for Github Actions test run
    //                        .AddEnvironmentVariables()
    //                        .Build();
    //    }
    //}

    public class TestConfiguration
    {
        public IConfigurationRoot ConfigRoot { get; }

        public TestConfiguration()
        {
            string json = Environment.GetEnvironmentVariable("UI_TEST_SECRET"); // read Github Actions secrets
            string env = Environment.GetEnvironmentVariable("TEST_ENV"); // read Github Actions secrets
            //string dbConnectString = Environment.GetEnvironmentVariable($"{env.ToUpper()}_DB_CONNECT_STR");

            Stream jsonStream = GenerateStreamFromString(json);
            //Stream jsonDbConnectStr = GenerateStreamFromString($"{{\"DBConnectStr\": \"{dbConnectString}\"}}");
            Stream jsonTestEnv = GenerateStreamFromString($"{{\"TestEnv\": \"{env}\"}}");

            ConfigRoot = new ConfigurationBuilder()
                         .AddJsonFile("appsettings.json")
                         .AddJsonStream(jsonStream)
                         //.AddJsonStream(jsonDbConnectStr)
                         .AddJsonStream(jsonTestEnv)
                         .AddEnvironmentVariables()
                         // Commented out to be able to run tests using Github Actions workflow (under Linux)
                         //.AddUserSecrets("0bebd5d6-7c39-4e62-9f1c-6866e6aff9c6") //comment out for Github Actions test run
                         //.AddEnvironmentVariables()
                         .Build();
        }

        public Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;

            return stream;
        }
    }

}

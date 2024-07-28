using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ExcelDataReader;
using Newtonsoft.Json;
using Serilog;
using Microsoft.Extensions.Configuration;

namespace PostRequestApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var apiUrl = configuration["ApiUrl"];
            var excelPath = configuration["ExcelPath"];
            var logPath = configuration["LogPath"];

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(logPath)
                .CreateLogger();

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            var requests = ReadRequestsFromExcel(excelPath);

            foreach (var request in requests)
            {
                var response = await PostRequest(apiUrl, request);
                Log.Information($"Request: {request.Body}, Response: {response}");
            }

            Log.CloseAndFlush();
        }

        private static List<ApiRequest> ReadRequestsFromExcel(string excelPath)
        {
            var requests = new List<ApiRequest>();

            using (var stream = File.Open(excelPath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var dataSet = reader.AsDataSet();
                    var table = dataSet.Tables[0];
                    var headerKeys = new List<string>();
                    foreach (DataRow row in table.Rows)
                    {
                        // skip header row and values of Body column to construct headers. Take value from row zero for header keys and use row values as header values
                        if (headerKeys.Count == 0)
                        {
                            foreach (DataColumn column in table.Columns)
                            {
                                headerKeys.Add(row[column].ToString());
                            }
                        }
                        else
                        {
                            var headers = new Dictionary<string, string>();
                            var body = string.Empty;
                            for (int i = 0; i < table.Columns.Count; i++)
                            {
                                if (headerKeys[i].Equals("Body"))
                                {
                                    body = row[i].ToString();
                                }
                                else
                                {
                                    headers.Add(headerKeys[i], row[i].ToString());
                                }
                            }

                            requests.Add(new ApiRequest { Headers = headers, Body = body });
                        }

                           // requests.Add(new ApiRequest { Headers = headers, Body = body });
                        
                    }
                }
            }

            return requests;
        }

        private static async Task<string> PostRequest(string apiUrl, ApiRequest request)
        {
            using (var client = new HttpClient())
            {
                foreach (var header in request.Headers)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }

                var content = new StringContent(request.Body, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(apiUrl, content);
                var responseBody = await response.Content.ReadAsStringAsync();

                return responseBody;
            }
        }

        class ApiRequest
        {
            public Dictionary<string, string> Headers { get; set; }
            public string Body { get; set; }
        }
    }
}

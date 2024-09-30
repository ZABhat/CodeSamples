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
using CsvHelper;
using System.Globalization;

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
            var csvPath = configuration["CsvPath"];
            var logPath = configuration["LogPath"];
            var clientId = configuration["client_id"];
            var clientSecret = configuration["client_secret"];

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(logPath)
                .CreateLogger();
            await Console.Out.WriteLineAsync($"{DateTime.Now} \t Application started ... ");
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            await Console.Out.WriteLineAsync($"{DateTime.Now} \t going to read csv file ");
            var requests = ReadRequestsFromCsv(csvPath,clientId,clientSecret);
            await Console.Out.WriteLineAsync($"{DateTime.Now} \t payload created for {requests.Count} requests  ");
            foreach (var request in requests)
            {
                //var response = await PostRequest(apiUrl, request);
                // Log.Information($"Request: {request.Body}, Response: {response}");
                Log.Information($"Request: {request.Body}, Response: dummy ");
            }

            Log.CloseAndFlush();
        }
        private static List<ApiRequest> ReadRequestsFromCsv(string csvPath,string clientId,string clientSecret)
        {
            var requests = new List<ApiRequest>();

            using (var reader = new StreamReader(csvPath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read(); // Read the first row (header)
                csv.ReadHeader(); // Interpret the first row as headers
                // Assuming the CSV file has headers (Header, Body)
                while (csv.Read())
                {
                    
                    var message = csv.GetField<string>("message");
                    message = message.Remove(0, message.IndexOf("{\"PartLines\":"));
                    message = message.Remove(message.Length-1, 1);
                    var partLines = JsonConvert.DeserializeObject<Payload>(message);
                    partLines.Campaign[0].customerReason = partLines.Campaign?.First()?.customerReason.RemoveSpecialCharacters();
                    var payload = JsonConvert.SerializeObject(partLines);
                    var headersDict = new Dictionary<string, string> ();
                    headersDict.Add("client_id", clientId);
                    headersDict.Add("client_secret", clientSecret);

                    requests.Add(new ApiRequest { Headers = headersDict, Body = payload });
                }
            }

            return requests;
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
                                if (headerKeys[i].Equals("message"))
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
            try
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
            catch(Exception ex) {
                Log.Information($"Request: {request.Body}, Response: Exception \n ExceptionMessage : {ex.Message} \n ExceptionStackTrace :  {ex.StackTrace}");
                return string.Empty;
            }
        }

        class ApiRequest
        {
            public Dictionary<string, string> Headers { get; set; }
            public string Body { get; set; }
        }
    }
}

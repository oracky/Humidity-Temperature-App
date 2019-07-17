using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Google.Apis.Sheets.v4;
using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using System.Reflection;

namespace HumidityAndTempApp.Models
{
    public abstract class Credentials
    {
        public static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        public static readonly string ApplicationName = "Humidity and Temperature";
        public static SheetsService service;
        public static void GetCredentials()
        {
            GoogleCredential credential;

            string _exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Replace(@"file:\", "");
            var credPath = _exePath + @"\client_secret.json";


            using (var stream = new FileStream(credPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
            }

            service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });
        }
    }
}
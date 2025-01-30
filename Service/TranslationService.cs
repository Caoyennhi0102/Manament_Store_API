using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Manament_Store_API.Models;
namespace Manament_Store_API.Service
{
    public class TranslationService
    {
        private readonly SqlConnectionServer _sqlConnectionServer;

        public TranslationService()
        {
            _sqlConnectionServer = new SqlConnectionServer();
        }
        public string GetTranslation(string key, string languageCode)
        {
            var translation = _sqlConnectionServer.BanDiches.FirstOrDefault(t => t.KhoaDich == key
             && t.NgonNgu.MaNgonNgu == languageCode);
            return translation?.KhoaDich ?? key;
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.ThirdParty.GoogleGemini
{
    public interface IGeminiService
    {
        public Task<string> QueryAsync(string content);
        public Task<string> ChatAsync(List<Content> contents);
        public Task ExtractChatDataAsync(List<Content> contents);
    }
}
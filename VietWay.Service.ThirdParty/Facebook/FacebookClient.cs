﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VietWay.Service.ThirdParty.Facebook
{
    public class FacebookClient(string pageId, string pageToken)
    {
        private readonly string _pageId = pageId;
        private readonly string _pageToken = pageToken;
        private const string BASEURL = "https://graph.facebook.com/v21.0";

        public async Task<string> PublishPostAsync(string content, string? url)
        {
            using HttpClient httpClient = new();
            HttpResponseMessage response = await httpClient.PostAsJsonAsync($"{BASEURL}/{_pageId}/feed?access_token={_pageToken}", new
            {
                message = content,
                link = url,
                published = true
            });
            response.EnsureSuccessStatusCode();
            JsonDocument jsonDocument = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
            JsonElement root = jsonDocument.RootElement;
            return root.GetProperty("id").GetString();
        }
        public async Task<int> GetPublishedPostReactionAsync(string facebookPostId)
        {
            using HttpClient httpClient = new();
            HttpResponseMessage response = await httpClient.GetAsync($"{BASEURL}/{facebookPostId}?fields=reactions.summary(total_count)&access_token={_pageToken}");
            response.EnsureSuccessStatusCode();
            JsonDocument jsonDocument = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
            JsonElement root = jsonDocument.RootElement;
            try
            {
                return root.GetProperty("reactions").GetProperty("summary").GetProperty("total_count").GetInt32();
            }
            catch
            {
                return 0;
            }
        }
    }
}
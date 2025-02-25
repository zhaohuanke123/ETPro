using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ET
{
	public class HttpService
	{
		private static readonly HttpClient _client = new HttpClient();

		// 异步GET请求
		public static async ETTask<string> GetAsync(string url)
		{
			try
			{
				var response = await _client.GetAsync(url);
				response.EnsureSuccessStatusCode();
				return await response.Content.ReadAsStringAsync();
			}
			catch (HttpRequestException ex)
			{
				throw new ApplicationException($"HTTP请求失败: {ex.Message}");
			}
		}

		// 同步GET请求
		public static string Get(string url)
		{
			return GetAsync(url).GetAwaiter().GetResult();
		}

		// 带参数的POST请求
		public static async Task<string> PostAsync(string url, Dictionary<string, string> data)
		{
			try
			{
				var content = new FormUrlEncodedContent(data);
				var response = await _client.PostAsync(url, content);
				response.EnsureSuccessStatusCode();
				return await response.Content.ReadAsStringAsync();
			}
			catch (HttpRequestException ex)
			{
				throw new ApplicationException($"HTTP请求失败: {ex.Message}");
			}
		}
	}
}

using System;
using System.Collections.Generic;

namespace ET
{
	using System;

	public class ResponseData
	{
		public int Money { get; private set; }
		public int StatusCode { get; private set; }
		public bool IsSuccess => StatusCode == 0;

		// 私有构造函数强制使用工厂方法
		private ResponseData()
		{
		}

		public static ResponseData Create(Dictionary<string, int> values)
		{
			return new ResponseData
			{
				Money = values.TryGetValue("money", out int m)? m : -1, // -1表示字段不存在
				StatusCode = values.TryGetValue("echo", out int e)? e : -1
			};
		}

		public override string ToString()
		{
			return $"{Money.ToString()} :: {StatusCode.ToString()}";

		}
	}

	public static class StringResponseParser
	{
		public static ResponseData Parse(string input)
		{
			var values = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
			input = input.Trim().Trim('{', '}');

			foreach (var segment in input.Split(','))
			{
				var kv = segment.Split(new[] { ':' }, 2);
				if (kv.Length != 2) continue;

				var key = kv[0].Trim().Trim('"', '\'');
				var value = kv[1].Trim().Trim('"', '\'');

				if (int.TryParse(value, out int num))
				{
					values[key] = num;
				}
			}

			ValidateRequiredFields(values);
			return ResponseData.Create(values);
		}

		private static void ValidateRequiredFields(Dictionary<string, int> values)
		{
			if (!values.ContainsKey("echo"))
			{
				throw new ArgumentException("响应数据必须包含echo字段");
			}
		}
	}
}

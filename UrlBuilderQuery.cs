using System;
using System.Collections.Specialized;
using System.Linq;

namespace UrlHandling
{
	public class UrlBuilderQuery
	{
		public UrlBuilderQuery()
		{
			Values = new OrderedDictionary();
		}

		public UrlBuilderQuery(string query)
			: this()
		{
			if (query.StartsWith("?"))
			{
				query = query.Substring(1);
			}
			var pairs = query.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

			foreach (var pair in pairs)
			{
				if (pair.StartsWith("="))
				{
					continue;
				}

				if (pair.EndsWith("="))
				{
					AddOrUpdate(pair.Substring(0, pair.Length - 1), String.Empty);
					continue;
				}

				var keyValue = pair.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
				if (keyValue.Length != 2)
				{
					continue;
				}

				AddOrUpdate(keyValue[0], Uri.UnescapeDataString(keyValue[1]));
			}
		}

		public OrderedDictionary Values { get; private set; }

		public bool AddOrUpdate(string key, string value)
		{
			if (Values.Contains(key))
			{
				Values[key] = value;
				return true;
			}

			Values.Add(key, value);
			return false;
		}

		public string GetValue(string key)
		{
			var value = Values[key];
			return value == null ? null : value.ToString();
		}

		public override string ToString()
		{
			if (Values.Count == 0)
				return String.Empty;

			var pairs = Values.Keys
				.Cast<string>()
				.Select(
					k => "{0}={1}"
						.With(k, Uri.EscapeDataString(Convert.ToString(Values[k]))))
				.ToArray();

			return "?" + String.Join("&", pairs);
		}
	}
}

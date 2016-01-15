namespace UrlHandling
{
	public static class UrlBuilderExtensions
	{
		public static UrlBuilder ToUrlBuilder(this string url)
		{
			return new UrlBuilder(url);
		}

		public static UrlBuilder WithQueryParam(this UrlBuilder urlBuilder, string key, string value)
		{
			urlBuilder.Query.AddOrUpdate(key, value);
			return urlBuilder;
		}

		public static UrlBuilder Secured(this UrlBuilder urlBuilder)
		{
			urlBuilder.Protocol = "https";
			return urlBuilder;
		}

		public static UrlBuilder WithQueryParamsFromAnotherUrl(this UrlBuilder target, string url)
		{
			if (url == null || url == string.Empty)
			{
				return target;
			}

			var otherUrlBuilder = new UrlBuilder(url);
			foreach (string key in otherUrlBuilder.Query.Values.Keys)
			{
				var value = otherUrlBuilder.Query.GetValue(key);
				target.Query.AddOrUpdate(key, value);
			}

			return target;
		}
	}
}

// Licensed to Elasticsearch B.V under one or more agreements.
// Elasticsearch B.V licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information

using Elastic.Xunit.XunitPlumbing;
using Nest;
using System.ComponentModel;

namespace Examples.Search.Request
{
	public class SearchTypePage : ExampleBase
	{
		[U(Skip = "Example not implemented")]
		[Description("search/request/search-type.asciidoc:54")]
		public void Line54()
		{
			// tag::de3c9fe00efc5647ad4b695524cbe8a0[]
			var response0 = new SearchResponse<object>();
			// end::de3c9fe00efc5647ad4b695524cbe8a0[]

			response0.MatchesExample(@"GET twitter/_search?search_type=query_then_fetch");
		}

		[U(Skip = "Example not implemented")]
		[Description("search/request/search-type.asciidoc:72")]
		public void Line72()
		{
			// tag::be0d2fbf861842eef2c98d5e5bf6e406[]
			var response0 = new SearchResponse<object>();
			// end::be0d2fbf861842eef2c98d5e5bf6e406[]

			response0.MatchesExample(@"GET twitter/_search?search_type=dfs_query_then_fetch");
		}
	}
}
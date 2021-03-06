// Licensed to Elasticsearch B.V under one or more agreements.
// Elasticsearch B.V licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information

using Elastic.Xunit.XunitPlumbing;
using Nest;
using System.ComponentModel;

namespace Examples.XPack.Docs.En.Security.Authorization
{
	public class MappingRolesPage : ExampleBase
	{
		[U(Skip = "Example not implemented")]
		[Description("../../x-pack/docs/en/security/authorization/mapping-roles.asciidoc:119")]
		public void Line119()
		{
			// tag::f2359acfb6eaa919125463cc1d3a7cd1[]
			var response0 = new SearchResponse<object>();
			// end::f2359acfb6eaa919125463cc1d3a7cd1[]

			response0.MatchesExample(@"PUT /_security/role_mapping/admins
			{
			  ""roles"" : [ ""monitoring"", ""user"" ],
			  ""rules"" : { ""field"" : { ""groups"" : ""cn=admins,dc=example,dc=com"" } },
			  ""enabled"": true
			}");
		}

		[U(Skip = "Example not implemented")]
		[Description("../../x-pack/docs/en/security/authorization/mapping-roles.asciidoc:129")]
		public void Line129()
		{
			// tag::3ea33023474e77d73ac0540e3a02b0b2[]
			var response0 = new SearchResponse<object>();
			// end::3ea33023474e77d73ac0540e3a02b0b2[]

			response0.MatchesExample(@"PUT /_security/role_mapping/basic_users
			{
			  ""roles"" : [ ""user"" ],
			  ""rules"" : { ""any"" : [
			      { ""field"" : { ""dn"" : ""cn=John Doe,cn=contractors,dc=example,dc=com"" } },
			      { ""field"" : { ""groups"" : ""cn=users,dc=example,dc=com"" } }
			  ] },
			  ""enabled"": true
			}");
		}

		[U(Skip = "Example not implemented")]
		[Description("../../x-pack/docs/en/security/authorization/mapping-roles.asciidoc:161")]
		public void Line161()
		{
			// tag::81ee2ad368208c4c78098292547b0577[]
			var response0 = new SearchResponse<object>();
			// end::81ee2ad368208c4c78098292547b0577[]

			response0.MatchesExample(@"PUT /_security/role_mapping/admin_user
			{
			  ""roles"" : [ ""monitoring"" ],
			  ""rules"" : { ""field"" : { ""dn"" : ""cn=Admin,ou=example,o=com"" } },
			  ""enabled"": true
			}");
		}

		[U(Skip = "Example not implemented")]
		[Description("../../x-pack/docs/en/security/authorization/mapping-roles.asciidoc:171")]
		public void Line171()
		{
			// tag::87846c3ddacab1da4af626ae8099e4be[]
			var response0 = new SearchResponse<object>();
			// end::87846c3ddacab1da4af626ae8099e4be[]

			response0.MatchesExample(@"PUT /_security/role_mapping/basic_user
			{
			  ""roles"" : [ ""user"" ],
			  ""rules"" : { ""field"" : { ""dn"" : ""cn=John Doe,ou=example,o=com"" } },
			  ""enabled"": true
			}");
		}
	}
}
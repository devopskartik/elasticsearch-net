using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Elasticsearch.Net;
using Nest;
using Tests.Benchmarking.Framework;
using Tests.Core.Client;
using Tests.Domain;

namespace Tests.Benchmarking
{
	[BenchmarkConfig(5)]
	public class BulkBenchmarkLowLevelTests
	{
		private static readonly IList<Project> Projects = Project.Generator.Clone().Generate(10000);

		private static readonly byte[] Response = TestClient.DefaultInMemoryClient.ConnectionSettings.RequestResponseSerializer.SerializeToBytes(ReturnBulkResponse(Projects));

		private static readonly IElasticClient Client =
			new ElasticClient(new ConnectionSettings(new InMemoryConnection(Response, 200, null, null))
				.DefaultIndex("index")
				.EnableHttpCompression(false)
			);

		private static readonly IElasticLowLevelClient ClientLowLevel = Client.LowLevel;

		[GlobalSetup]
		public void Setup() { }

		[Benchmark(Description = "ListOfObjects")]
		public BulkResponse ListOfObjects()
		{
			var postData = Projects.SelectMany(p => new object[] { new { index = new { } }, p });
			var lowLevel = ClientLowLevel.Bulk<BulkResponse>(PostData.MultiJson(postData));
			return lowLevel;
		}

		private static readonly object[] StaticArrayOfObjects =
			Projects.SelectMany(p => new object[] { new { index = new { } }, p }).ToArray();

		[Benchmark(Description = "StaticListOfObjects")]
		public BulkResponse StaticListOfObjects()
		{
			var lowLevel = ClientLowLevel.Bulk<BulkResponse>(PostData.MultiJson(StaticArrayOfObjects));
			return lowLevel;
		}

		private static readonly PostData StaticPostData = PostData.MultiJson(StaticArrayOfObjects);
		[Benchmark(Description = "PrecomputedPostDataOfObjects")]
		public BulkResponse PrecomputedPostDataOfObjects()
		{
			var lowLevel = ClientLowLevel.Bulk<BulkResponse>(StaticPostData);
			return lowLevel;
		}

		private static readonly string _header = @"{""index"":{}}";
		private static readonly string[] StaticListString =
			Projects.SelectMany(p => new string[] { _header, Client.SourceSerializer.SerializeToString(p) }).ToArray();
		private static readonly PostData StaticPostDataListString = PostData.MultiJson(StaticListString);

		[Benchmark(Description = "StaticListOfStrings")]
		public BulkResponse StaticListOfStrings()
		{
			var lowLevel = ClientLowLevel.Bulk<BulkResponse>(PostData.MultiJson(StaticListString));
			return lowLevel;
		}

		[Benchmark(Description = "PrecomputedPostDataOfStrings")]
		public BulkResponse PrecomputedPostDataOfStrings()
		{
			var lowLevel = ClientLowLevel.Bulk<BulkResponse>(StaticPostDataListString);
			return lowLevel;
		}

		private static readonly string StaticString = string.Join("\n", StaticListString);
		[Benchmark(Description = "PostDataString")]
		public BulkResponse PostDataString()
		{
			var lowLevel = ClientLowLevel.Bulk<BulkResponse>(PostData.String(StaticString));
			return lowLevel;
		}

		private static readonly PostData PrecomputedStaticString = PostData.String(StaticString);
		[Benchmark(Description = "PrecomputedPostDataString")]
		public BulkResponse PrecomputedPostDataString()
		{
			var lowLevel = ClientLowLevel.Bulk<BulkResponse>(PrecomputedStaticString);
			return lowLevel;
		}

		private static readonly byte[] StaticBytes = Encoding.UTF8.GetBytes(StaticString);
		[Benchmark(Description = "PostDataByteArray")]
		public BulkResponse PostDataByteArray()
		{
			var lowLevel = ClientLowLevel.Bulk<BulkResponse>(PostData.Bytes(StaticBytes));
			return lowLevel;
		}

		private static readonly ReadOnlyMemory<byte> StaticMemory = StaticBytes.AsMemory();

		[Benchmark(Description = "PostDataReadOnlyMemory")]
		public BulkResponse PostDataReadOnlyMemory()
		{
			var lowLevel = ClientLowLevel.Bulk<BulkResponse>(PostData.ReadOnlyMemory(StaticMemory));
			return lowLevel;
		}

		[Benchmark(Description = "PostDataStreamHandler")]
		public BulkResponse PostDataStreamHandler()
		{
			var lowLevel = ClientLowLevel.Bulk<BulkResponse>(PostData.StreamHandler(StaticBytes,
				(bytes, s) => s.Write(bytes.AsSpan()),
				async (bytes, s, ctx) => await s.WriteAsync(bytes.AsMemory(), ctx)));
			return lowLevel;
		}

		[GlobalCleanup]
		private static void X()
		{
			var bytes = Encoding.UTF8.GetByteCount(StaticString);
			Console.WriteLine($"=====> {((bytes/1024f)/1024f)}");
		}

		private static object BulkItemResponse(Project project) => new
		{
			index = new
			{
				_index = "nest-52cfd7aa",
				_id = project.Name,
				_version = 1,
				_shards = new
				{
					total = 2,
					successful = 1,
					failed = 0
				},
				created = true,
				status = 201
			}
		};

		private static object ReturnBulkResponse(IList<Project> projects) => new
		{
			took = 276,
			errors = false,
			items = projects
				.Select(p => BulkItemResponse(p))
				.ToArray()
		};
	}
}
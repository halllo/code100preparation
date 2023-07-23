using QuickGraph;
using QuickGraph.Algorithms.Observers;
using QuickGraph.Algorithms.Search;

namespace Tests
{
    [TestClass]
    public class Road
    {
        [TestMethod]
        public async Task Dfs()
        {
            using var http = new HttpClient();
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("apikey", "myak1");
            using var response = await http.GetAsync("http://localhost:5109/road");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<RoadItem[]>();
            Assert.AreEqual(6, result!.Length);

            var graph = new BidirectionalGraph<RoadItem, IEdge<RoadItem>>();
			graph.AddVertexRange(result);
			graph.AddEdgeRange(result
                .Where(r => r.Parent is not null)
                .Select(r => new Edge<RoadItem>(source: result.Single(p => r.Parent == p.Name), target: r))
            );

            var root = result.Where(r => r.Parent is null).Single();

			var dfs = new DepthFirstSearchAlgorithm<RoadItem, IEdge<RoadItem>>(
				host: null,
				visitedGraph: graph,
				colors: new Dictionary<RoadItem, GraphColor>(),
				outEdgeEnumerator: outEdges => outEdges.OrderBy(e => e.Target.Score));
			var vertexRecorder = new VertexRecorderObserver<RoadItem, IEdge<RoadItem>>();
			using (vertexRecorder.Attach(dfs))
			{
				dfs.Compute(root);
			}

			var routes = vertexRecorder.Vertices.ToList();
            Assert.AreEqual("A", routes[0].Name);
            Assert.AreEqual("F", routes[1].Name);
            Assert.AreEqual("B", routes[2].Name);
            Assert.AreEqual("C", routes[3].Name);
            Assert.AreEqual("D", routes[4].Name);
            Assert.AreEqual("E", routes[5].Name);
        }

        public record RoadItem(string Name, int Score, string? Parent);
    }
}
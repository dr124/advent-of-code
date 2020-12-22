using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent.Core.Graphs
{
    /// <summary>
    /// https://www.geeksforgeeks.org/ford-fulkerson-algorithm-for-maximum-flow-problem/
    /// </summary>
    public class MaxFlow
    {
        static readonly int V = 6; //Number of vertices in graph 

        /* Returns true if there is a path 
        from source 's' to sink 't' in residual 
        graph. Also fills parent[] to store the 
        path */
        public bool Bfs(int[,] rGraph, int s, int t, int[] parent)
        {
            // Create a visited array and mark  
            // all vertices as not visited 
            var visited = new bool[V];
            for (var i = 0; i < V; ++i)
                visited[i] = false;

            // Create a queue, enqueue source vertex and mark 
            // source vertex as visited 
            var queue = new List<int>();
            queue.Add(s);
            visited[s] = true;
            parent[s] = -1;

            // Standard BFS Loop 
            while (queue.Count != 0)
            {
                var u = queue[0];
                queue.RemoveAt(0);

                for (var v = 0; v < V; v++)
                {
                    if (visited[v] == false && rGraph[u, v] > 0)
                    {
                        queue.Add(v);
                        parent[v] = u;
                        visited[v] = true;
                    }
                }
            }

            // If we reached sink in BFS  
            // starting from source, then 
            // return true, else false 
            return (visited[t] == true);
        }

        // Returns tne maximum flow 
        // from s to t in the given graph 
        public int FordFulkerson(int[,] graph, int s, int t)
        {
            int u, v;

            // Create a residual graph and fill  
            // the residual graph with given  
            // capacities in the original graph as 
            // residual capacities in residual graph 

            // Residual graph where rGraph[i,j]  
            // indicates residual capacity of  
            // edge from i to j (if there is an  
            // edge. If rGraph[i,j] is 0, then  
            // there is not) 
            var rGraph = new int[V, V];

            for (u = 0; u < V; u++)
                for (v = 0; v < V; v++)
                    rGraph[u, v] = graph[u, v];

            // This array is filled by BFS and to store path 
            var parent = new int[V];

            var max_flow = 0; // There is no flow initially 

            // Augment the flow while tere is path from source 
            // to sink 
            while (Bfs(rGraph, s, t, parent))
            {
                // Find minimum residual capacity of the edhes 
                // along the path filled by BFS. Or we can say 
                // find the maximum flow through the path found. 
                var path_flow = int.MaxValue;
                for (v = t; v != s; v = parent[v])
                {
                    u = parent[v];
                    path_flow = Math.Min(path_flow, rGraph[u, v]);
                }

                // update residual capacities of the edges and 
                // reverse edges along the path 
                for (v = t; v != s; v = parent[v])
                {
                    u = parent[v];
                    rGraph[u, v] -= path_flow;
                    rGraph[v, u] += path_flow;
                }

                // Add path flow to overall flow 
                max_flow += path_flow;
            }

            // Return the overall flow 
            return max_flow;
        }
    }

    /// <summary>
    /// Maximum bipartite matching using flow network and maximum-flow
    /// https://www.geeksforgeeks.org/maximum-bipartite-matching/
    /// </summary>
    public class Matching
    {
        // M is number of applicants  
        // and N is number of jobs 
        int M = 6;
        int N = 6;

        // A DFS based recursive function  
        // that returns true if a matching  
        // for vertex u is possible 
        private bool IsMatching(int[,] graph, int u, Span<int> seen, Span<int> matchR)
        {
            // Try every job one by one 
            for (var v = 0; v < N; v++)
            {
                // If applicant u is interested  
                // in job v and v is not visited 
                if (graph[u, v] > 0 && seen[v] == 0)
                {
                    // Mark v as visited 
                    seen[v] = 1;

                    // If job 'v' is not assigned to 
                    // an applicant OR previously assigned  
                    // applicant for job v (which is matchR[v]) 
                    // has an alternate job available. 
                    // Since v is marked as visited in the above  
                    // line, matchR[v] in the following recursive  
                    // call will not get job 'v' again 
                    if (matchR[v] < 0 || IsMatching(graph, matchR[v],
                                             seen, matchR))
                    {
                        matchR[v] = u;
                        return true;
                    }
                }
            }
            return false;
        }

        // Returns maximum number of  
        // matching from M to N 
        public int MaxFlow(int[,] graph)
        {
            M = graph.GetLength(0);
            N = graph.GetLength(1);
            // An array to keep track of the  
            // applicants assigned to jobs.  
            // The value of matchR[i] is the  
            // applicant number assigned to job i,  
            // the value -1 indicates nobody is assigned. 
            // -1 = Initially all jobs are available 
            Assigned = new int[N].Populate(-1);

            // Count of jobs assigned to applicants 
            var result = 0;
            for (var u = 0; u < M; u++)
            {
                // Mark all jobs as not 
                // seen for next applicant. 
                var seen = new int[N].Populate(0);

                // Find if the applicant  
                // 'u' can get a job 
                if (IsMatching(graph, u, seen, Assigned))
                    result++;
            }
            return result;
        }

        
        public int[] Assigned { get; private set; }
    }
}

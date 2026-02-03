using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PerformanceBenchmark
{
    // Mocks simulating Navisworks API structure
    public class Document
    {
        private readonly Dictionary<string, object> _plugins = new Dictionary<string, object>();

        public Document()
        {
            // Simulate that the plugin is already loaded
            _plugins["Clash"] = new ClashPlugin();
        }

        public ClashPlugin GetClash()
        {
            // Simulate lookup overhead (e.g., retrieving a plugin from a collection)
            if (_plugins.TryGetValue("Clash", out var plugin))
            {
                return (ClashPlugin)plugin;
            }
            return null;
        }
    }

    public class ClashPlugin
    {
        // Property access might have backing field, relatively cheap but not free if repeatedly accessed via deeper path
        public TestsData TestsData { get; } = new TestsData();
    }

    public class TestsData
    {
        public void TestsRunTest(object test)
        {
            // Simulate some minimal work for the test run
            // We keep it light so the method call overhead is visible relative to the lookup overhead
            // In reality, TestsRunTest might take significant time, which would dilute the % improvement,
            // but the absolute time saved (CPU cycles) remains the same.
            // However, inside a tight loop (e.g. simulation step), avoiding allocations/lookups is always good practice.
            int x = 0;
            for(int i=0; i<10; i++) x++;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var doc = new Document();
            var test = new object();
            int iterations = 1000000; // High iteration count to make the overhead measurable

            Console.WriteLine($"Running benchmark with {iterations} iterations...");

            // Warmup
            for (int i = 0; i < 1000; i++)
            {
                 doc.GetClash().TestsData.TestsRunTest(test);
            }

            // Baseline: Lookup inside loop
            var stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                doc.GetClash().TestsData.TestsRunTest(test);
            }
            stopwatch.Stop();
            long baselineMs = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Baseline (repeated lookup): {baselineMs} ms");

            // Optimized: Lookup outside loop
            stopwatch.Restart();
            var cachedTestsData = doc.GetClash().TestsData;
            for (int i = 0; i < iterations; i++)
            {
                cachedTestsData.TestsRunTest(test);
            }
            stopwatch.Stop();
            long optimizedMs = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Optimized (cached): {optimizedMs} ms");

            if (baselineMs > 0)
            {
                double improvement = (double)(baselineMs - optimizedMs) / baselineMs * 100;
                Console.WriteLine($"Improvement: {improvement:F2}%");
            }
            else
            {
                Console.WriteLine("Execution time too short to measure difference.");
            }
        }
    }
}

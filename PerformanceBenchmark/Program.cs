using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PerformanceBenchmark
{
    // Mock Enums and Classes
    public enum ClashResultStatus
    {
        New,
        Active,
        Resolved,
        Approved
    }

    public class ClashResult
    {
        public ClashResultStatus Status { get; set; }
    }

    public class ClashTest
    {
        public List<object> Children { get; set; } = new List<object>();
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Performance Benchmark: Missing Break in Clash Result Loop");

            // Configuration
            int totalItems = 100000;
            int iterations = 1000;

            Console.WriteLine($"Total Items per Test: {totalItems}");
            Console.WriteLine($"Iterations: {iterations}");

            // Scenarios
            // 1. First Item is Collision (Best Case for Optimization)
            // 2. Middle Item is Collision (Average Case)
            // 3. Last Item is Collision (Worst Case - same performance expected)
            // 4. No Collision (Worst Case - same performance expected)

            RunScenario("Collision at Start", totalItems, iterations, 0);
            RunScenario("Collision in Middle", totalItems, iterations, totalItems / 2);
            RunScenario("Collision at End", totalItems, iterations, totalItems - 1);
        }

        static void RunScenario(string name, int count, int iterations, int collisionIndex)
        {
            Console.WriteLine($"\n--- Scenario: {name} ---");

            // Setup Data
            var test = new ClashTest();
            // Fill with safe items
            for (int i = 0; i < count; i++)
            {
                test.Children.Add(new ClashResult { Status = ClashResultStatus.Resolved });
            }

            // Insert collision at specific index
            if (collisionIndex >= 0 && collisionIndex < count)
            {
                ((ClashResult)test.Children[collisionIndex]).Status = ClashResultStatus.New;
            }

            // Measure Baseline (No Break)
            long baselineTicks = 0;
            Stopwatch sw = new Stopwatch();

            sw.Start();
            for (int i = 0; i < iterations; i++)
            {
                ProcessNoBreak(test);
            }
            sw.Stop();
            baselineTicks = sw.ElapsedTicks;
            double baselineMs = sw.Elapsed.TotalMilliseconds;

            // Measure Optimized (With Break)
            long optimizedTicks = 0;
            sw.Reset();
            sw.Start();
            for (int i = 0; i < iterations; i++)
            {
                ProcessWithBreak(test);
            }
            sw.Stop();
            optimizedTicks = sw.ElapsedTicks;
            double optimizedMs = sw.Elapsed.TotalMilliseconds;

            Console.WriteLine($"Baseline (No Break): {baselineMs:F2} ms");
            Console.WriteLine($"Optimized (Break):   {optimizedMs:F2} ms");

            if (optimizedMs < baselineMs)
            {
                double speedup = baselineMs / optimizedMs;
                Console.WriteLine($"Improvement: {speedup:F2}x faster");
            }
            else
            {
                Console.WriteLine("No significant improvement (expected for worst case).");
            }
        }

        static bool ProcessNoBreak(ClashTest test)
        {
            bool collisionDetected = false;
            foreach (var result in test.Children)
            {
                ClashResult cr = result as ClashResult;
                if (cr != null && (cr.Status == ClashResultStatus.New || cr.Status == ClashResultStatus.Active))
                {
                    collisionDetected = true;
                    // Missing break
                }
            }
            return collisionDetected;
        }

        static bool ProcessWithBreak(ClashTest test)
        {
            bool collisionDetected = false;
            foreach (var result in test.Children)
            {
                ClashResult cr = result as ClashResult;
                if (cr != null && (cr.Status == ClashResultStatus.New || cr.Status == ClashResultStatus.Active))
                {
                    collisionDetected = true;
                    break; // Optimization
                }
            }
            return collisionDetected;
        }
    }
}

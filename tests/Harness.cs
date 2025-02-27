//===--- Harness.cs -------------------------------------------------------===//
//
// Copyright (c) 2015 Joe Duffy. All rights reserved.
//
// This file is distributed under the MIT License. See LICENSE.md for details.
//
//===----------------------------------------------------------------------===//

using System;
using System.Diagnostics;
using System.Reflection;

class Program
{
    public static void Main()
    {
        Console.WriteLine("TESTING...");
        Console.WriteLine("==========");
        var sw = Stopwatch.StartNew();
        var t = new Tester();
        if (t.RunTests(new Tests())) {
            Console.WriteLine("Success! ({0})", sw.Elapsed);
        }
        else {
            Console.WriteLine("{0} Failures ({1})", t.Failures, sw.Elapsed);
        }
    }
}

class Tester
{
    int m_failures;

    public int Failures
    {
        get { return m_failures; }
    }

    public bool Success
    {
        get { return (m_failures == 0); }
    }

    public void Assert(bool condition, string msg = "")
    {
        if (!condition) {
            m_failures++;
            Console.WriteLine("    # assertion failed [{0}]", msg);
        }
    }

    public void AssertEqual<T>(T x, T y)
        where T : IEquatable<T>
    {
        Assert(x.Equals(y), String.Format("{0} != {1}", x, y));
    }

    public bool RunTests(object tests)
    {
        // Run all methods that match the pattern 'bool Test*(Tester t)'
        var sw = new Stopwatch();
        var testMethods =
            tests.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);
        foreach (var testMethod in testMethods) {
            if (testMethod.Name.StartsWith("Test") &&
                testMethod.ReturnType == typeof(bool) &&
                testMethod.GetParameters().Length == 1 &&
                testMethod.GetParameters()[0].ParameterType == typeof(Tester)) {

                Console.WriteLine("Run {0}...", testMethod.Name);
                sw.Start();
                int failures = m_failures;
                bool success = (bool)testMethod.Invoke(tests, new object[] { this });
                sw.Stop();
                if (success && m_failures == failures) {
                    Console.WriteLine("    - success ({0})", sw.Elapsed);
                }
                else {
                    m_failures++;
                    Console.WriteLine("    - failure ({0})", sw.Elapsed);
                }
                sw.Reset();
            }

        }
        return Success;
    }
}


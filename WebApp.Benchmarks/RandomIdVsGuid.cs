using BenchmarkDotNet.Attributes;
using WebApp.Common.Helpers;

namespace WebApp.Benchmarks;

public class RandomIdVsGuid
{
    [Benchmark]
    public string NewRandomId() => IdHelper.NewRandomId();

    [Benchmark]
    public string NewGuid() => IdHelper.NewGuid().ToBase64();
}

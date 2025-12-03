using System.Diagnostics;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Shouldly;
using Xunit;
using Xunit.Sdk;
using Xunit.v3;

[assembly: CaptureConsole]

BenchmarkRunner.Run<AocBenchmark>();

[SimpleJob, MemoryDiagnoser]
public class AocBenchmark
{
	const int Day = 2;
	private readonly string _className = $"Advent._2025.Day{Day:D2}";
	private readonly string _inputFile = $"Day{Day:D2}.txt";
	private string[] _input = null!;
	private Day? _instance;

	[GlobalSetup]
	public void Setup()
	{
		_input = File.ReadAllLines(_inputFile);
		var t = Type.GetType(_className) ?? throw new InvalidOperationException("Invalid Day type");
		_instance = (Day)Activator.CreateInstance(t)!;
	}

	[Benchmark(Baseline = true)]
	public void Part()
	{
		if (_instance is null)
		{
			throw new InvalidOperationException("Invalid Day instance");
		}

		_instance?.Run(_input);
	}
}

/// <summary>
/// Declares expected AoC inputs and results for a day. Multiple instances allowed.
/// </summary>
/// <param name="path">Relative path to the input file.</param>
/// <param name="part1">Expected Part 1 result (optional).</param>
/// <param name="part2">Expected Part 2 result (optional).</param>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class AocDataAttribute(string path, object? part1 = null, object? part2 = null) : Attribute
{
	/// <summary>Relative input file path.</summary>
	public string Path { get; } = path;

	/// <summary>Expected result for Part 1 (nullable).</summary>
	public object? Part1 { get; } = part1;

	/// <summary>Expected result for Part 2 (nullable).</summary>
	public object? Part2 { get; } = part2;
}

/// <summary>
/// Base AoC day with a single run entry and an inherited xUnit theory verifying datasets.
/// </summary>
public abstract class Day
{
	/// <summary>
	/// Executes the puzzle and returns Part A and Part B results.
	/// </summary>
	public abstract (object? PartA, object? PartB) Run(string[] lines);

	/// <summary>
	/// xUnit theory: for each <see cref="AocDataAttribute"/> on the derived day,
	/// loads input and asserts expected Part 1/Part 2 if provided.
	/// </summary>
	/// <param name="path">Relative input file path.</param>
	/// <param name="expectedPart1">Expected Part 1 (nullable).</param>
	/// <param name="expectedPart2">Expected Part 2 (nullable).</param>
	[Theory(DisplayName = "AoC Dataset")]
	[AocDataset]
	public void VerifiesDataset(string path, object? expectedPart1, object? expectedPart2)
	{
		if (!File.Exists(path))
			throw new FileNotFoundException($"Input file not found: {path}", path);

		var lines = File.ReadAllLines(path);
		var instance = (Day)Activator.CreateInstance(GetType())!;

		var sw = Stopwatch.StartNew();
		var (actualPart1, actualPart2) = instance.Run(lines);
		sw.Stop();

		Console.WriteLine($"{GetType().Name} | {path} | Part1={actualPart1} | Part2={actualPart2} | Elapsed={sw.ElapsedMilliseconds}ms");

		if (expectedPart1 is not null)
			actualPart1.ShouldBe(expectedPart1);

		if (expectedPart2 is not null)
		{
			actualPart2.ShouldBe(expectedPart2);
		}
	}
}

/// <summary>
/// Supplies theory rows by reading <see cref="AocDataAttribute"/> from the derived day type.
/// </summary>
internal sealed class AocDatasetAttribute : DataAttribute
{
	/// <inheritdoc/>
	public override ValueTask<IReadOnlyCollection<ITheoryDataRow>> GetData(MethodInfo testMethod, DisposalTracker disposalTracker)
	{
		var dayType = testMethod.ReflectedType!;
		var dataAttributes = dayType.GetCustomAttributes<AocDataAttribute>();
		var dataRows = new List<ITheoryDataRow>();
		foreach (var dataAttr in dataAttributes)
		{
			var dataRow = new TheoryDataRow(dataAttr.Path, dataAttr.Part1, dataAttr.Part2);
			dataRows.Add(dataRow);
		}
		return new ValueTask<IReadOnlyCollection<ITheoryDataRow>>(dataRows);
	}

	/// <inheritdoc/>
	public override bool SupportsDiscoveryEnumeration() => true;
}
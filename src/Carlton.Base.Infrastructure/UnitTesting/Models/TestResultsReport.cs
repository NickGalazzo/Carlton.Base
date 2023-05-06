﻿namespace Carlton.Base.Infrastructure.UnitTesting;

public record TestResultsReport
{
    public IEnumerable<TestResult> TestResults { get; init; }
    public TestResultsSummary Summary { get; init; }

    public TestResultsReport(IEnumerable<TestResult> testResults)
    {
        TestResults = testResults;

        var totalCount = 0;
        var totalPassedCount = 0;
        var totalFailureCount = 0;
        var totalDuration = 0.0;

        TestResults.ToList().ForEach(_ => CalculateSummary(_, ref totalCount, ref totalPassedCount, ref totalFailureCount, ref totalDuration));
        Summary = new TestResultsSummary(totalCount, totalPassedCount, totalFailureCount, totalDuration);
    }

    private static void CalculateSummary(TestResult testResult, ref int totalCount, ref int totalPassedCount, ref int totalFailureCount, ref double totalDuration)
    {
        totalCount++;
        totalDuration += testResult.TestDuration;

        if(testResult.TestResultOutcome == TestResultOutcomes.Passed)
            totalPassedCount++;
        if(testResult.TestResultOutcome == TestResultOutcomes.Failed)
            totalFailureCount++;
    }
}

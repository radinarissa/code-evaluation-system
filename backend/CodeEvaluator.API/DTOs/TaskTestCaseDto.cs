namespace CodeEvaluator.API.DTOs
{
    /// <summary>
    /// Represents a single test case for a programming task.
    /// </summary>
    public class TaskTestCaseDto
    {
        /// <summary>
        /// Human-readable name of the test case.
        /// </summary>
        public string Name { get; set; } = default!;

        /// <summary>
        /// Path or identifier of the input file used in the sandbox.
        /// </summary>
        public string InputFilePath { get; set; } = default!;

        /// <summary>
        /// Expected output file path or identifier (optional if reference solution is used).
        /// </summary>
        public string? OutputFilePath { get; set; }

        /// <summary>
        /// Additional read-only files required for the test case.
        /// </summary>
        public List<string> ReadonlyFilePaths { get; set; } = new();
    }
}

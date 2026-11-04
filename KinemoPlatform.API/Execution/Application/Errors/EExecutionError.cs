namespace KinemoPlatform.API.Execution.Application.Errors;

/// <summary>Expected failures of the Execution command service.</summary>
public enum EExecutionError
{
    /// <summary>The execution does not exist (HTTP 404).</summary>
    ExecutionNotFound,

    /// <summary>The show already has an active execution (HTTP 409).</summary>
    ActiveExecutionExists,

    /// <summary>The execution state does not allow the operation (HTTP 409).</summary>
    InvalidExecutionState
}
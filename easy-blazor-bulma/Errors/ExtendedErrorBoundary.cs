using Microsoft.AspNetCore.Components.Web;
using System.Net;

namespace easy_blazor_bulma;

/// <summary>
/// Extends the default error boundary to provide access to the current error details.
/// </summary>
public class ExtendedErrorBoundary : ErrorBoundary
{
	/// <summary>
	/// Returns the current exception if present.
	/// </summary>
	public Exception? GetCurrentException()
	{
		return CurrentException;
	}

	/// <summary>
	/// Returns the current error message.
	/// </summary>
	public string? GetErrorMessage()
	{
		return CurrentException?.Message;
	}

	/// <summary>
	/// Returns the type of the current error.
	/// </summary>
	public Type? GetErrorType()
	{
		return CurrentException?.GetType();
	}

	/// <summary>
	/// Returns the HTTP status code as an integer when present.
	/// </summary>
	public HttpStatusCode? GetHttpStatusCode()
	{
		if (CurrentException != null && CurrentException is HttpRequestException exception)
			return exception.StatusCode;

		return null;
	}
}

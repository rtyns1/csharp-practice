# Async Data Aggregator

## What this is

A console app that fetches user data from JSONPlaceholder and prayer times from Aladhan API at the same time. If a request fails, it retries 3 times with exponential backoff (2s, 4s, 8s). If both APIs take longer than 5 seconds total, it cancels everything. Errors are logged to `error.log` with timestamps.

## Dependencies

None. This uses .NET built-in libraries only:

- `System.Net.Http` - HttpClient for API calls
- `System.Text.Json` - JSON deserialization
- `System.Threading` - CancellationToken
- `System.IO` - File logging

## APIs Used

| API | Purpose | Endpoint |
|-----|---------|----------|
| JSONPlaceholder | Get list of fake users | `https://jsonplaceholder.typicode.com/users` |
| Aladhan | Get today's prayer times for Nairobi | `http://api.aladhan.com/v1/timingsByCity?city=Nairobi&country=Kenya&method=2` |

## How to Run

```bash
dotnet run

## Project Flow (Detailed)

### Step 1: Program.cs starts
- Creates a 5-second cancellation token
- Calls `ApiService.GetUsersAsync()` and `ApiService.GetPrayerTimesAsync()` in parallel using `Task.WhenAll()`

### Step 2: ApiService calls RetryHandler
- Each API method wraps its HTTP call inside `RetryHandler.ExecuteWithRetryAsync()`
- RetryHandler executes the delegate (the API call) up to 3 times
- On failure: waits `2^retryCount` seconds, then retries

### Step 3: HTTP request and deserialization
- `HttpClient.GetStringAsync()` fetches raw JSON
- `JsonSerializer.Deserialize<T>()` converts JSON to C# objects
- If deserialization fails, error is logged and re-thrown

### Step 4: Error logging
- `FileLogger.LogErrorAsync()` appends timestamped message to `error.log`
- File is created in the same folder as the `.exe`

### Step 5: Display results
- `Program.cs` receives `List<User>` and `Timings`
- Loops through each user, prints user name and prayer times
- If any error occurs, prints error message to console

---

## File Structure

AsyncDataAggregator/
├── Program.cs # Orchestration
├── Models/
│ ├── User.cs # JSONPlaceholder response
│ ├── PrayerTimingResponse.cs # Aladhan response (nested)
│ └── AggregatedData.cs # Final output format
├── Services/
│ ├── ApiService.cs # Makes HTTP calls
│ └── RetryHandler.cs # Exponential backoff retry
├── Helpers/
│ └── FileLogger.cs # Async file logging
├── error.log # Created at runtime
├── README.md
└── LEARNING_SUMMARY.md

## What I Learned

- Async/await patterns with HttpClient
- Exponential backoff retry logic
- CancellationToken for timeouts
- JSON deserialization with System.Text.Json
- File logging without external libraries
- Debugging deserialization failures (printed raw JSON, found typo: "timing" vs "timings")

---

## Status

Complete. Working as of 19th April 2026.
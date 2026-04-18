Learning Summary: AsyncDataAggregator Project
Project Overview
AsyncDataAggregator — A console app that fetches user data and prayer times from two different websites simultaneously, with automatic retries, timeout cancellation, and file logging.

Core Problem Being Solved
Real networks fail, APIs go down, servers get slow. This project teaches how to handle these situations gracefully in real-world applications. When a request fails, the app should:

Retry the request a few times

Give up after a reasonable time

Log errors to a file for later analysis

Give the user feedback about what's happening

Key Concept: Async Aggregator
An async aggregator collects multiple data items over time and processes them as a group later, without blocking the main application flow while waiting for aggregation to complete.

Unlike a synchronous aggregator (which blocks the main thread until all data is collected), an async aggregator allows the application to continue running and responding to user input while aggregation happens in the background.

Characteristics (in my own words):
Non-blocking — Doesn't block the main thread, self-explanatory

Time-based triggers — Can process data at specific intervals or after a set amount of time

Event-driven — Can process data as soon as it arrives

Buffered processing — Items stored temporarily until ready to process together

Error handling — Designed to handle failures gracefully

Scalability — Can handle increasing data volumes

Listener/callback mechanism — Notifies other parts of the app when data is available or processing is complete

Real-world uses (I researched these):
High-volume event processing

Federated learning

Real-time analytics (streaming from social media feeds, financial markets)

API rate limiting/batching

Project Requirements (Restated in my words)
Two API calls — Fetch users from JSONPlaceholder, prayer times from Aladhan

Parallel execution — Both run at the same time

Retry logic — If an API fails: try again, wait 2s, then 4s, then 8s, then stop

Cancellation — If both APIs take longer than 5 seconds total, abort everything

File logging — Every error gets written to error.log with a timestamp

Merging — Combine user list + prayer times into one result

Display — Print the merged result to console in a readable format

File Structure (With Rationale)
Separation of concerns is essential. Each file has one responsibility:

text
AsyncDataAggregator/
├── Program.cs              # Orchestration only — calls services, displays results, no business logic
├── Models/
│   ├── User.cs             # Data model for user data (matches JSONPlaceholder response)
│   ├── PrayerTimingResponse.cs  # Nested JSON structure for Aladhan API
│   └── AggregatedData.cs   # Final combined output structure
├── Services/
│   ├── ApiService.cs       # Makes HTTP calls, knows WHAT to fetch but not HOW to retry
│   └── RetryHandler.cs     # Exponential backoff logic only, knows HOW to retry but not WHAT
├── Helpers/
│   └── FileLogger.cs       # Appends timestamped messages to error.log
├── error.log               # Created at runtime, gitignored
└── LEARNING_LOG.md         # This document
How They Call Each Other:
text
Program.cs
    ├── calls → ApiService.GetUsersAsync()
    │               └── calls → RetryHandler.ExecuteWithRetryAsync()
    └── calls → ApiService.GetPrayerTimesAsync()
                    └── calls → RetryHandler.ExecuteWithRetryAsync()

When RetryHandler fails after all retries:
    └── calls → FileLogger.LogError()
Key Technical Concepts I Learned
Static Classes (FileLogger.cs)
A static class cannot be instantiated and can only contain static members. You call its methods directly without creating an object.

Why FileLogger is static: It has no state (no fields that change per instance). You don't need multiple instances of a logger — just one set of methods that can be called from anywhere. Console.WriteLine() is a static method that outputs to the console, same pattern.

Static vs non-static is about whether you need multiple instances with different states. A user session class should be non-static (each user has different data). A logger can be static.

What an API Is
An API (Application Programming Interface) is a set of rules that allows different software applications to communicate. It defines how requests should be made, how data should be formatted, and what responses can be expected.

The two APIs I used:

JSONPlaceholder — Fake REST API for testing. Returns user data with id, name, username, email, address, phone, website, company

Aladhan — Provides prayer times based on location and date. Returns nested JSON: { "code": 200, "status": "ok", "data": { "timings": { "Fajr": "05:00"... } } }

What ApiService.cs Does (In Plain English)
Imagine you're a manager with two employees:

RetryHandler — Keeps trying a task until it works, or gives up after 3 tries

FileLogger — Writes down problems in a notebook with timestamps

ApiService is the manager who uses these employees to get real data from the internet. Two specific jobs (methods):

GetUsersAsync() — Goes to JSONPlaceholder, asks for the list of users, returns it as C# objects (List<User>)

GetPrayerTimesAsync() — Goes to Aladhan, asks for today's prayer times in Nairobi, returns just the timings

GetUsersAsync() flow:

Tell RetryHandler to fetch users (if fails, retry up to 3 times with exponential backoff)

RetryHandler calls HttpClient.GetStringAsync() — the actual internet request

If successful, JSON string comes back

Convert/deserialize JSON string into a List<User> object

Return that list to whoever called GetUsersAsync() (Program.cs)

If any step fails, RetryHandler logs the error via FileLogger and retries

HttpClient singleton — only one instance exists for the entire program.

The Bug I Debugged (And Fixed Myself)
Error I was getting:

text
ERROR: Deserialization failed for prayer times: Deserialized prayer timing is null.
ERROR: Deserialized prayer timing response has null timing.
What I did: Printed the raw JSON response from the API and compared it to my C# properties.

The bug: A typo — "timing" instead of "timings" in my model class.

The fix: Changed the property name from "timing" to "timings" to match the JSON field.

"I might be him." — Successfully debugged on my own.

What's Next
This challenge is finished as of 15th April 2026. But there's still quite a lot to do, so I have 4 more questions based on this one. This way, I can cement patterns and explore these topics to a greater extent.

My Learning Philosophy (Stated in the log)
"These notes are a combination of things I googled and read about. I read then formulate my own thoughts. This is an essential process to learning and it will help me to be accountable."

"Accountability is the goal."

"If the project is yours, you should be able to do both" — meaning bridging the simple explanation with the complex, deep technical explanations.

Status: Core challenge complete. Awaiting the 4 follow-up challenges.

i would prefer if its written in MD nad try to mirror how i speak more
Here's the summary written in markdown, keeping your voice and speaking style intact.

AsyncDataAggregator - Learning Log Summary
What I'm Building
A console app that fetches user data and prayer times from two different websites at the same time. Automatically retries if they fail, stops if it takes too long, and logs errors to a file.

Why? Real networks fail, APIs go down, servers get slow. This project is meant to teach me how to handle these situations gracefully in a real world application. I'm about to make apps and services soon and they will depend on external data. So if a request fails, my app should not just crash. It should:

retry the request a few times

give up after a reasonable time

log any errors to a file for later analysis

give the user feedback about what is going on

What Even Is An Async Aggregator?
Okay so I googled this and here's my understanding:

An async aggregator collects multiple individual data items over time and processes them as a group later, without blocking the main application flow while waiting.

Unlike a synchronous aggregator which would block the main thread (kinda the whole point why we use async in the first place), an async aggregator lets the main application keep running and responding to user input while the aggregation happens in the background.

Characteristics I noted down:

Non-blocking - self explanatory, doesn't block main thread

Time-based triggers - can process data at specific intervals or after a certain amount of time passes

Event driven - can process data as soon as it arrives

Buffered processing - items stored in a temporary container until they are ready to be processed together

Error handling - can be designed to handle errors gracefully

Scalability - yeah

Listener/callback mechanism - allows other parts of the app to be notified when new data is available or when processing is complete

Some uses I found: high volume event processing, federated learning, real time analytics like streaming data from social media feeds and financial markets, API rate limiting/batching (no idea what that means honestly, copied from google).

The Actual Requirements For This Project
Building 2 API calls, parallel execution:

if an API fails → try again, wait 2s then try, then 4s then 8s, then stop

if both APIs take longer than 5 seconds total → abort everything

every error gets written to error.log with a timestamp

combine user list + prayer times into one result

print the merged result to the console in a readable format

How I Structured The Project
I had no idea how to structure this one. But I've seen a lot of structuring in software before, and I managed to see how a huge codebase is arranged when I was doing my 'internship' - Barakallahu fiik Adam.

The rule: Separation of concerns and responsibilities is essential when deciding on a file structure.

Here's what I landed on (got help coming up with this structure but I understand why each file exists):

text
AsyncDataAggregator/
│
├── Program.cs
│
├── Models/
│   ├── User.cs
│   ├── PrayerTimingResponse.cs
│   └── AggregatedData.cs
│
├── Services/
│   ├── ApiService.cs           (makes HTTP calls, uses RetryHandler)
│   └── RetryHandler.cs         (exponential backoff logic only)
│
├── Helpers/
│   └── FileLogger.cs           (error logging to file)
│
├── docs/
│
├── README.md
├── LEARNING_LOG.md
├── LEARNING_SUMMARY.md
│
└── error.log                   (created at runtime, gitignored)
Why each file:

Program.cs → orchestration only, calls services, displays results, no business logic

User.cs → data model for user data

PrayerTimingResponse.cs → defines structure of Aladhan API response (nested JSON)

AggregatedData.cs → defines final combined output structure

ApiService.cs → makes HTTP requests using HttpClient. Knows WHAT to fetch but not HOW to retry

RetryHandler.cs → contains retry logic with exponential backoff. Knows HOW to retry but not WHAT to fetch

FileLogger.cs → appends timestamped messages to error.log. Knows HOW to log but not WHAT to log

As you can see, you need to understand how these call each other because it can easily grow overwhelming.

The call flow:

text
Program.cs
    │
    ├── calls → ApiService.GetUsersAsync()
    │               │
    │               └── calls → RetryHandler.ExecuteWithRetryAsync()
    │
    └── calls → ApiService.GetPrayerTimesAsync()
                    │
                    └── calls → RetryHandler.ExecuteWithRetryAsync()

When RetryHandler fails after all retries:
    └── calls → FileLogger.LogError()
My Approach: Write In Order, Not All At Once
I googled and got recommended to do it in this order. Smart move honestly.

Step 1: FileLogger.cs (no dependencies)
Step 2: Models (plain C# classes)
Step 3: RetryHandler.cs (depends on nothing)
Step 4: ApiService.cs (depends on RetryHandler + Models)
Step 5: Program.cs (brings it all together)

What I Learned About Static Classes (From FileLogger)
A static class cannot be instantiated and can only contain static members. When you write a static class, you expect to call its methods directly without creating an object. Usually for helper and utility functions that don't require any state to be maintained between calls.

If a class has no state (no fields that change per instance), it can be static.

Static classes do produce an output. FileLogger outputs to a file. Console.WriteLine() is a static method that outputs to the console.

Static vs non-static is about whether you need multiple instances of the class with different states. States here mean data stored in fields that can change per instance. For example, if you had a class representing a user session, you'd want it non-static because each user would have different session data. But for a logger, you don't need multiple instances, you just need one set of methods that can be called from anywhere. So it makes sense to make it static.

What The Models Do
I need to understand what an API returns before I can create models that represent that data in my code.

JSONPlaceholder returns a list of users, each user has id, name, username, email, address, phone, website and company. My User.cs only uses id, name, username and email. Ignoring the rest.

Aladhan API returns a nested JSON response. Nested structure means the JSON contains objects within objects. Like { "code": 200, "status": "ok", "data": { "timings": {"Fajr": "05:00"... } } }. You can see the "data" field has another object inside it called "timings" which contains the actual prayer times.

So:

User.cs → represents ONE user from JSONPlaceholder response

PrayerTimingResponse.cs → represents the ENTIRE Aladhan response, which contains timings inside it

AggregatedData.cs → represents the FINAL combined data I will display

What ApiService.cs Actually Does (In Plain English)
Imagine you are a manager and you have 2 employees:

RetryHandler → keeps trying a task until it works, or gives up after 3 tries

FileLogger → writes down problems in a notebook with timestamps

ApiService is the manager who uses these employees to get real data from the internet.

Two specific jobs (methods):

GetUsersAsync() → Goes to JSONPlaceholder website, asks for the list of users, and returns it as C# objects (List<User>)

GetPrayerTimesAsync() → Goes to Aladhan website, asks for today's prayer times in Nairobi, and returns just the timings

GetUsersAsync() step by step:

Tell RetryHandler to please fetch users. If it fails, retry up to 3 times with exponential backoff

RetryHandler calls HttpClient.GetStringAsync() - the actual internet request

If successful, the JSON string comes back

Convert/deserialize that JSON string into a List<User> object

Return that list to whoever called GetUsersAsync() (Program.cs)

If any step fails, RetryHandler logs the error via FileLogger and retries

HttpClient singleton means only one instance exists for the entire program.

The Bug I Fixed (And I'm Proud Of It)
The errors I was seeing:

text
2026-04-19 01:21:05 - ERROR: Deserialization failed for prayer times: Deserialized prayer timing is null.
2026-04-19 01:21:22 - ERROR: Deserialized prayer timing response has null timing.
2026-04-19 01:21:22 - ERROR: Deserialization failed for prayer times: Deserialized prayer timing is null.
2026-04-19 01:21:22 - ERROR: Program execution failed: Operation failed after 5
What I did: I printed the raw JSON response from the API and compared it to my C# properties.

The problem: A typo. I had written "timing" instead of "timings" in my model class.

The fix: Changed the property name from "timing" to "timings" to match the JSON field.

I SUCCESFULLY DEBUGGED ON MY OWN. I MIGHT BE HIM.

Where I'm At Now
This challenge is finished as of 15th April 2026. But there's still quite a lot to do, so I have 4 more questions based off of this one. This way I can cement patterns and explore these topics to a greater extent.

A note on my learning process: These notes are a combination of things I googled and read about. I read then formulate my own thoughts. This is an essential process to learning and it will help me to be accountable. Accountability is the goal.

If the project is yours, you should be able to explain it simply AND deeply. I'm trying to learn how to do both.
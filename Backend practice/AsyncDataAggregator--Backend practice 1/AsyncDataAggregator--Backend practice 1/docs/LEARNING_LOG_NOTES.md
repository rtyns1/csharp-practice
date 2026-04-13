**12 - 04 - 2026, Sunday 1600 hours login**

### BASIC DETAILS
 -**Project Name**: AsyncDataAggregator

 **Project Description**: A console app that fetches user data and prayer tmes form two different websites at the same time,
 automatically retries if they fail, stops if it takes too long, and logs any erros to a file.

 Real networks fail, APIs go down and servers get slow.
 This project is meant to teach me how to handle these situations gracefully in a real world application.
 I am to make apps anr services soon and they will depend on external data, so if a request fails, my app shoukd not just crash it should:
 -retry the request a few times.
 -Give up after a reasonable time.
 -Log any errors to a file for later analysis.
 -Give the user feedback about what is going on.
 
 An async aggregator is a software pattern or module that collects multiple individual data items, events, or messages over time and pprocesses them 
 as a group later, without blocking the main application flpw while waiting for the aggregation to complete.

 Unlike a synchronous aggregator , which would block the main thread(kinfd of the whole point why we use async in the first place) until all data is colected,
 an async aggregator allows the main application to continue running and responding to user input or other events while the agregation is happening in the background.

 It processes data as it arrives, relying on time based triggers, or asynchronous event notifications to initiate processing.
 This means that the application can be more responsive and efficient, especially when dealing with large volumes of data or when waiting for external resources.

 --these notes are a combination of things i googled and read about, so i read then formulate my own thoughts. This is an essential process to learning and it will help me to be accountable.

 Some of the characteristics of an async aggregator include::
 -Non blocking- self explanatory, doesn tblock main thread.

 -Time based triggers- it can be set to process data at specific intervals or after a certain amount of time has passed.
 This is useful in scenarios where data is expected to arrive at irregular intervals, or when you want to batch process data for efficeincy.
 For example, process data every 5 minutes, or aftr 100 data items have been collected. 
 The data processing here means taking the colllected data and perfoming some kind of operation on it, such as filtering, tansforming ,or agregating it in some way.

 - Event driven- it can be designed to process data as soon as it arrives, without waiting for a specific time trigger. This is useful in scenarios where data is expected to arrive continously or when you want to process data in real time .
   For example, process data as soon as it is received from an API or when user submits a form.

 -Buffered processing- items stored in a temporary container until they are ready to be processed together.

 -Error handling- can be dsinged to handle errors gracefully.
 
 -Scalability

 -Listener/callback mechanism: allows other parts of the application to be notified when new data is available or when processing is complete. This can be implemented using event emitters, callbacks, or other pub/sub mechanisms.

 some uses:
 -High volume event processing.
 - Federated learning.
 - Real time analytics like streaming data from social media feeds, financial markets etc.
 -API rate limiting/batching (no idea what this means, copied from google)-Combining multiple small API requests into a single, large request to reduce network overhead

 Okay, back to our problem.
 Building 2 API calls, parallel execution, retry logic=> if an API fails, try again, wait 2s and try, then 4 then 8, then stop
 cancellation=> if both APIs take longer than 5 seconds total aboirt everything
 file logging=> ever erro gets written to a error.log with a timestamp, KEY
 mergin=> combine user list + prayer times into one result
 display=> printthe merdged result to the console in a readable format.

 ### Folder and file structure 

 okay, now the next thing to decide os how to structure the project.
 I have no idea how to structure this one,
 But i have seen a lot of structuring in sofware before, and i managed to see how a huge codebase is arranged when i was doing my 'internship'=>Barakallahu fiik Adam.

 We will start with a simple file structure:

 --Separation of concerns, and responsibilities, is essential when deciding on a file strucuture.
 i need a filelogging file, so fileLogger.cs, async and await are the methods, all methonds should be asynchronous/
 i need a models folder, for data models, 
 i need a program.cs-- main flow, calls APIs, displays result
 i need APiService.cs-- HttpClient, retry logic and logging,
 i need a folder for helpers even if i wont use it immediate, and a filelogger.cs for error logging, i need a 
 i need an exponential backoff file RetryHnadler.cs

 AsyncDataAggregator--Backend practice 1/
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
│   └── (your documentation files)
│
├── README.md
├── LEARNING_LOG.md
├── LEARNING_SUMMARY.md
│
└── error.log                   (created at runtime, gitignored)

now, this structure is not my own, because i have no knowledge on the topic, i Hhad help coming up with this.
But i do understand why we need each file, and all the code inside will be made by me, and there will be an explanation on each design choice, and any other thing.
Accountability is the goal.

program.cs==> orchestration only- calls services, displays results, no business logic
user.cs==> data models for user data
PrayerTimingResponse.cs==> define structure of Aladhan API resposnse(nested JSON
AggregateData.cs==> defines final combines output structure with user data and prayer times
APIService.cs==> makes HTTP requests using HttpClient.Knows WHAT to fetch, but not HOW to retry
RetryHandler.cs==> contains retry logic with exponsential backoff. Knows How to retry but not WHAT to fetch
FileLogger.cs==> appends timestamped messages to error.log. knows HOW to log but not WHAT to log
Now, as you can see, you need to understand how these call each other because it can easily grow overwhelming.


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
  
  So yeah, now lets get to writing actual code
  now, there will be always the problem of decidning the starting point of writing your prject,
  even moresoe concidering i havent done anything like this before, so i have googled and i am rcommended to go with the following: i will simply compy paste the whole thing;:;

 ## Do not write everything at once. Write in this order, testing each piece:

Step 1: FileLogger.cs (no dependencies)
Write LogError(string message) method

Test it from Program.cs with a simple message

Verify error.log appears

Step 2: Models (plain C# classes)
User.cs — match JSONPlaceholder response structure

PrayerTimingResponse.cs — match Aladhan response structure (nested)

AggregatedData.cs — what you will display

Step 3: RetryHandler.cs (depends on nothing)
Write ExecuteWithRetryAsync<T> method

Test with a fake failing action to verify retry logic

Step 4: ApiService.cs (depends on RetryHandler + Models)
Write GetUsersAsync() — makes real HTTP call to JSONPlaceholder

Write GetPrayerTimesAsync() — makes real HTTP call to Aladhan

Both should use RetryHandler for retries

Both should call FileLogger on failures

Step 5: Program.cs (brings it all together)
Create instance of ApiService

Call both methods (parallel or sequential)

Combine results into AggregatedData

Display to console

The very first line of code you write
In Program.cs, write this:

csharp
using AsyncDataAggregator--Backend practice 1.Helpers;

FileLogger.LogError("Testing logger - delete this later");
Run it. Check if error.log appears.

If yes → delete that test line and move to Step 2.

Your action now
Open Program.cs. Write the test line above. Run:

bash
dotnet run
Then report back:

Did error.log appear?

What did it contain?

## FILELOGGER.cs

This class will be responsible for logging errors to a file.
So essentially waht it does is that it has a method that takes a string message and appends it to a file called error.log with timestamp
 Now, the first question i had is why does it need to be a static class( i got a  hint that it should eb a static method, and copied a soimple code fomr someone in reddit)
 **Short notes on static classes and methods**

 -A static class is a class that cannot be instantiated and can only contain static members.
 - When you write a static class, you expect to call its methods directly, without creating an object. Usually for helper and utility functions that dont require any state to be maintained btwn calls.
 
 **IF a class has no state(no fields that change per instance), it can be static.

 static classes do produce an output, FileLogger outputs to a file. Console.WriteLine() is a static method that outputs to the console.

 Static vs non static is about whether you need multiple instances of the class with different stats.
 States in this case mean data that is stored in fields and can change per instance, for example, if you had a class that represented a user session, you woud want it to be non static because each user would have different
 session data, But for a logger, you dont need multiple istances of the logger , you just need one set of methods that can be called from anywhere, so it makes sense to make it static.

 Now, how do we test this filelogger? 
 - using it in program.cs . I have no idea how to do this so i will google.
 - 
  **13 - 04 - 2026,Monday 9AM login**
 
 Left it of at testin the filelogger.
 So, first i have to ensure that a majortiy of the methods is in Async in this project since that is the basis of the whole thing.
 Now, i cant explain the code here, this will only be a spot for general explanationa and showing what ive learnt,if u need to see the actual code go to the codefile.
 There i would have comments explaining the deeper stuff, this is a reflection notes space.

 When you go to the code file, you will see the notes in the code itself, here its general.

 ## MODELS 

 Step 2: Models (plain C# classes)
User.cs — match JSONPlaceholder response structure

PrayerTimingResponse.cs — match Aladhan response structure (nested)

AggregatedData.cs — what you will display
I need to understand what an API is. Then understand what it is returning and how we will display it. Then i can create the models that will represent the datain my code.
 **What is an API?**
 An API, or Application Programming Interface, is a set of rules and protocols that allows different software applications to communicate with each other. It defines how requests should be made, how data should be formatted, and what responses can be expected. APIs are used to enable the integration of different systems and allow them to work together.
 In our case, we will be using two APIs:
 - JSONPlaceholder: A fake online REST API for testing and prototyping. It provides endpoints for users, posts, comments, etc.
 - Aladhan: An API that provides prayer times based on location and date.
 We will need to understand the structure of the data returned by these APIs in order to create our models. The models will be C# classes that represent the data we receive from the APIs, making it easier to work with that data in our code

 Jsonplaceholde returns a list of users, each user has an id, name , username, email, address, phone, website and company, So our User.cs model will have properties that match these fields .
 We will use the fields that are relevant to our project. We will use only name, email, id and username , we will ignore the rest.
 https://jsonplaceholder.typicode.com/users

 The Aladhan API returns a nested JSON response that contains prayer times for a specific location and date. The structure of the response is more complex, so we will need to create a model that can represent this nested strucutre.
 Nested structure means that the JSON response contains objects within objects, an example would be like { "code": 200, "status":"ok", "data": { "timings": {"Fajr" : "05:00"....}}}
 You can see that the " data" field contains has another object inside it called "timings" which contains the actual prayer times. So we will need to create a model that can represent this nested structure. It is already defined.
 http://api.aladhan.com/v1/timingsByCity?city=Nairobi&country=Kenya&method=2

 so:: User.cs==> represents ONE user from the JSON placeholder response
      PrayerTimingResponse.cs==> represents the ENTIRE Aladhan response, which contains timings inside it
      AgregateData.cs==> repesents the FINAL combines data you will display.

 for user.cs-- we will take id, Name, username and email
 for PrayerTimingResponse.cs-- we will take code, status, data.timings
 for AggregateData.cs-- we will take a list of users and the prayer timings, this is what we will display to the user at the end.

 user.cs is blueprint for one user, then we will have a list<User> in our code to represent the list of users we get form the API.


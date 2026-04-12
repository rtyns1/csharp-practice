**12 - 04 - 2026, Sunday 1600 hours login**

## BASIC DETAILS
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



 

  


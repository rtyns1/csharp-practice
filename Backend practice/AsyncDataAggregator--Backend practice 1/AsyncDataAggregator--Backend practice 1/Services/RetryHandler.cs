using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AsyncDataAggregator__Backend_practice_1.Services
{
    public class RetryHandler
    {
        public static async Task<T> ExecuteWithRetry<T>(Func<Task<T>> action, int maxRetries = 5, int delaymilliseconds = 1000)
        {
            // i have no idea what im doing, 
            //loop from retrycount = 0 to maxRetries.
            // inside loop, try to call action() and return the result if succesful
            //if it fails, catch block/catch exeception- check if retryCount == maxRetries  if yes, throw
            // if not final retry: calculate delay = Math.Pow(2, retryCount) not milisseconds, seconds
            // await Task.Delay(delayInMilliseconds) and continue the loop to next retry
            // if retrycount starts at 0 then delay seconds = Mth.Pow(2, retrycount + 1) to avoid 0 seconds delay on the first retry
            // the APiservice.cs relies on this retryhandler to handle retries for the API calls, so it will call this method with the appropriate action and parameters when making API calls that may fail and need to be retried.
            for (int retryCount = 0; retryCount < maxRetries; retryCount++)
            {
                try
                {
                    T result = await action();
                    return result;
                }
                catch (Exception e)
                {
                    if (retryCount == maxRetries - 1)
                    {
                        throw new Exception($"Operation failed after {maxRetries}", e);
                    }
                    else
                    {
                        int delayInSeconds = (int)Math.Pow(2, retryCount + 1);
                        int delayInMilliseconds = delayInSeconds * 1000;
                        await Task.Delay(delayInMilliseconds);

                    }
                }
            }
            throw new Exception($"Operation failed after {maxRetries} retries.");
        }
    }
}

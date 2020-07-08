using System;
using System.Threading.Tasks;
using Hangfire;
using NetCoreSample.Tools;

namespace NetCoreSample.BatchJobs
{
    public static class BatchJob
    {
        public static Task RunSomeJobs()
        {
            var task = new Task<string>(() =>
            {
                for (var i = 1; i < 10000; i++)
                {
                    var str = StringTool.GenerateString(5);
                    BackgroundJob.Enqueue(() => Console.WriteLine($"Hello {str}"));
                    // RecurringJob.AddOrUpdate("流程A", () => Console.Write($"Hello {str}"), Cron.Minutely);
                }
                return "";
            });
            return task;
        }
    }
}
using System;
using System.ComponentModel;
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
                var userId = "Ya";
                var jobName = "寫些東西到console";
                for (var i = 1; i < 10000; i++)
                {
                    var str = StringTool.GenerateString(5);
                    BackgroundJob.Enqueue(() => ExecuteJob($"{userId} 建立於 {DateTime.Now:MM-dd HH:mm:ss}", jobName, str));
                    // RecurringJob.AddOrUpdate("流程A", () => Console.Write($"Hello {str}"), Cron.Minutely);
                }
                return "";
            });
            return task;
        }

        [DisplayName("排程作業: {0} - {1}")]
        public static void ExecuteJob(string creattor, string jobName, string str)
        {
            Console.WriteLine($"Hello {str}");
            return;
        }
    }
}
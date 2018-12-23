using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Executors
{
    public class ProcessExecutor
    {
        public ProcessExecutor()
        {

        }


        public ResultProcessExecutor Execute(string fileName, string inputData, int timeLimit, int memoryLimit, IEnumerable<string> executionArguments = null)
        {
            var result = new ResultProcessExecutor();

            try
            {

                var workingDirectory = new FileInfo(fileName).DirectoryName;

                using (var restrictedProcess = new ProcessAdvanced(fileName, workingDirectory, executionArguments, Math.Max(4096, (inputData.Length * 2) + 4)))
                {
                    try
                    {

                        restrictedProcess.StandardInput.WriteLineAsync(inputData).ContinueWith(
                            delegate
                            {

                                try
                                {
                                // ReSharper disable once AccessToDisposedClosure
                                if (!restrictedProcess.IsDisposed)
                                    {
                                    // ReSharper disable once AccessToDisposedClosure
                                    restrictedProcess.StandardInput.FlushAsync().ContinueWith(
                                            delegate
                                            {
                                            try
                                            {
                                                restrictedProcess.StandardInput.Close();

                                            }
                                            catch (Exception ex)
                                            {
                                                Debug.WriteLine(ex.Message);
                                            }
                                        });
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine(ex.Message);
                                }

                            });
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }

                    // Read standard output using another thread to prevent process locking (waiting us to empty the output buffer)
                    var processOutputTask = restrictedProcess.StandardOutput.ReadToEndAsync().ContinueWith(
                        x =>
                        {
                            result.Output = x.Result;
                        });

                    // Read standard error using another thread
                    var errorOutputTask = restrictedProcess.StandardError.ReadToEndAsync().ContinueWith(
                        x =>
                        {
                            result.Error = x.Result;
                        });

                    // Read memory consumption every few milliseconds to determine the peak memory usage of the process
                    const int TimeIntervalBetweenTwoMemoryConsumptionRequests = 45;
                    var memoryTaskCancellationToken = new CancellationTokenSource();
                    var memoryTask = Task.Run(
                        () =>
                        {
                            try
                            {
                                while (true)
                                {
                                // ReSharper disable once AccessToDisposedClosure
                                var peakWorkingSetSize = restrictedProcess.PeakWorkingSetSize;

                                    result.MemoryUsed = Math.Max(result.MemoryUsed, peakWorkingSetSize);

                                    if (memoryTaskCancellationToken.IsCancellationRequested)
                                    {
                                        return;
                                    }

                                    Thread.Sleep(TimeIntervalBetweenTwoMemoryConsumptionRequests);
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                            }

                        },
                        memoryTaskCancellationToken.Token);

                    // Start the process
                    int kqxuli =  restrictedProcess.Start(timeLimit, memoryLimit);

                    if (kqxuli== 0) //  RE
                    {
                        result.ExitCode = restrictedProcess.ExitCode;
                        result.TimeWorked = restrictedProcess.ExitTime - restrictedProcess.StartTime;
                        result.PrivilegedProcessorTime = restrictedProcess.PrivilegedProcessorTime;
                        result.UserProcessorTime = restrictedProcess.UserProcessorTime;
                        result.TrangThai = StatusProcessExecutor.RE;
                        return result;
                    }

                    // Wait the process to complete. Kill it after (timeLimit * 1.5) milliseconds if not completed.
                    // We are waiting the process for more than defined time and after this we compare the process time with the real time limit.
                    var exited = restrictedProcess.WaitForExit((int)(timeLimit * 1));
                    if (!exited)
                    {
                        restrictedProcess.Kill();
                        result.TrangThai = StatusProcessExecutor.TLE;
                    }

                    // Close the memory consumption check thread
                    memoryTaskCancellationToken.Cancel();
                    try
                    {
                        // To be sure that memory consumption will be evaluated correctly
                        memoryTask.Wait(TimeIntervalBetweenTwoMemoryConsumptionRequests);
                    }
                    catch (AggregateException ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }

                    // Close the task that gets the process error output
                    try
                    {
                        errorOutputTask.Wait(100);
                    }
                    catch (AggregateException ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }

                    // Close the task that gets the process output
                    try
                    {
                        processOutputTask.Wait(100);
                    }
                    catch (AggregateException ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }

                    Debug.Assert(restrictedProcess.HasExited, "Restricted process didn't exit!");

                    // Report exit code and total process working time
                    result.ExitCode = restrictedProcess.ExitCode;
                    result.TimeWorked = restrictedProcess.ExitTime - restrictedProcess.StartTime;
                    result.PrivilegedProcessorTime = restrictedProcess.PrivilegedProcessorTime;
                    result.UserProcessorTime = restrictedProcess.UserProcessorTime;
                }

                if (result.TotalProcessorTime.TotalMilliseconds > timeLimit)
                {
                    result.TrangThai = StatusProcessExecutor.TLE;
                }

                if (!string.IsNullOrEmpty(result.Error))
                {
                    result.TrangThai = StatusProcessExecutor.RE;
                }

                if (result.MemoryUsed > memoryLimit)
                {
                    result.TrangThai = StatusProcessExecutor.MemoryLimit;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }


            return result;
        }






    }
}

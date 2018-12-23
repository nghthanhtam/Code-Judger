using Executors;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Utility;


namespace Judge3T
{
    class XuLi
    {

        #region xử lí biên dịch
        static public Result biendichcpp(string dircpp, string direxe)
        {
            try
            {
                string filegcc = DuongDan.fgcc + "\\bin\\g++.exe";
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.UseShellExecute = false;
                startInfo.FileName = filegcc;
                startInfo.WorkingDirectory = DuongDan.fgcc + "\\bin";
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.CreateNoWindow = true;
                startInfo.Arguments = "  -std=c++11 -o\"" + direxe + "\" \"" + dircpp + "\"  -pipe -O2 -s -static -Wl,--stack,66060288 -lm -x c++";

                using (Process exeProcess = Process.Start(startInfo))
                {
                    bool xong = exeProcess.WaitForExit(10000);
                    if (!xong)
                    {
                        exeProcess.Kill();
                        return new Result(false, "hết time biên dịch");
                    }
                }
                if (File.Exists(direxe) == false)
                {
                    throw new Exception("Lỗi biên dịch, not found exe");
                }
            }
            catch (Exception ex)
            {
                return new Result(false, "Biên dịch file thất bại", ex.Message);
            }
            return new Result(true, "Biên dịch thành công!");
        }

        static public Result biendichc(string dirc, string direxe)
        {
            try
            {
                string filegcc = DuongDan.fgcc + "\\bin\\gcc.exe";
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.UseShellExecute = false;
                startInfo.FileName = filegcc;
                startInfo.WorkingDirectory = DuongDan.fgcc + "\\bin";
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.CreateNoWindow = true;
                startInfo.Arguments = " -std=c11 \""+dirc+"\" -pipe -O2 -s -static -lm -x c -o\""+direxe+"\" -Wl,--stack,66060288";

                
                using (Process exeProcess = Process.Start(startInfo))
                {
                    bool xong = exeProcess.WaitForExit(10000);
                    if (!xong)
                    {
                        exeProcess.Kill();
                        return new Result(false, "hết time biên dịch");
                    }
                }
                if (File.Exists(direxe) == false)
                {
                    throw new Exception("Lỗi biên dịch, not found exe");
                }
            }
            catch (Exception ex)
            {
                return new Result(false, "Biên dịch file thất bại", ex.Message);
            }
            return new Result(true, "Biên dịch thành công!");
        }

        static public Result biendichpas(string dirpc, string direxe)
        {
            try
            {
                string filepc = DuongDan.fpc + "\\bin\\i386-win32\\fpc.exe";
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.UseShellExecute = false;
                startInfo.FileName = filepc;
                startInfo.WorkingDirectory = DuongDan.fpc + "\\bin\\i386-win32";
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.CreateNoWindow = true;

                startInfo.Arguments = " -o\"" + direxe + "\" \"" + dirpc + "\" -O2 -XS -Sg -Cs66060288 ";

                using (Process exeProcess = Process.Start(startInfo))
                {
                    bool xong = exeProcess.WaitForExit(30000);
                    if (!xong)
                    {
                        exeProcess.Kill();
                        return new Result(false, "hết time biên dịch");
                    }
                }
                if (File.Exists(direxe) == false)
                {
                    throw new Exception("Lỗi biên dịch, not found exe");
                }
            }
            catch (Exception ex)
            {
                return new Result(false, "Biên dịch file thất bại", ex.Message);
            }

            try
            {
                if (File.Exists(System.IO.Path.GetDirectoryName(direxe) + "\\" + System.IO.Path.GetFileNameWithoutExtension(dirpc) + ".o") == true)
                {
                    File.Delete(System.IO.Path.GetDirectoryName(direxe) + "\\" + System.IO.Path.GetFileNameWithoutExtension(dirpc) + ".o");
                }

            }
            catch
            {

            }
            return new Result(true, "Biên dịch thành công!");
        }
        #endregion


        /// <summary>
        /// Trả về true nếu 2 text giống nhau
        /// </summary>
        /// <param name="code"></param>
        /// <param name="kq"></param>
        /// <returns></returns>
        static public bool SoSanhOutput(string[] code, string[] kq)
        {
            int soDongCode = code.Length;
            int soDongkq = kq.Length;
            int i = 0;
            if (soDongCode != soDongkq)
                return false;
            else
            {
                do
                {
                    if (code[i].Length != kq[i].Length) //kt số lương kí tự của 1 dòng
                    {
                        return false;
                    }
                    int j = 0;
                    for (int k = 0; k < code[i].Length; k++) //kt các kí tự trên 1 dòng có                                                      
                    {                                        //giống nhau ko
                        if (code[i][k] == kq[i][j])
                            j++;
                        else
                            return false; // sai đáp án
                    }
                    if (i == soDongkq - 1) //kt đã hết dòng chưa
                        return true;
                    else i++;
                } while (true);
            }
        }



        static public KetQuaChamTest chambai(string fileName, string dirTest, string TenBai, int timeLimit, int MemoryLimit) // chấm bài với từng test
        {
            ResultProcessExecutor res = new ResultProcessExecutor();
            KetQuaChamTest ketQuaChamTest = new KetQuaChamTest(System.IO.Path.GetFileNameWithoutExtension(dirTest));

            try
            {
                ProcessExecutor proc = new ProcessExecutor();

                StreamReader sr = new StreamReader(dirTest + "\\" + TenBai + ".INP");  //Đưa vào input vào từ file
                string inputData = sr.ReadToEnd();
                sr.Close();

                res = proc.Execute(fileName, inputData, timeLimit, MemoryLimit * (int)Math.Pow(2, 20));

                ketQuaChamTest.TimeUsed = res.TimeWorked;
                ketQuaChamTest.MemoryUsed = res.MemoryUsed;

                Debug.WriteLine("");
                Debug.WriteLine("Bo test: ", dirTest);
                Debug.WriteLine("{0}, thoi gian {1}", res.TrangThai, res.TimeWorked);
                Debug.WriteLine("output:" + res.Output);
                Debug.WriteLine("MemUsed:" + (double)Math.Round((double)res.MemoryUsed / Math.Pow(2, 20), 2));


            }
            catch (Exception ex)
            {
                return new KetQuaChamTest(System.IO.Path.GetFileNameWithoutExtension(dirTest), StatusResult.RE, res.TimeWorked, res.MemoryUsed);
            }

            switch (res.TrangThai)
            {
                case StatusProcessExecutor.MemoryLimit:
                    ketQuaChamTest.Status = StatusResult.MemoryLimit;
                    return ketQuaChamTest;
                    break;
                case StatusProcessExecutor.RE:
                    ketQuaChamTest.Status = StatusResult.RE;
                    return ketQuaChamTest;
                    break;
                case StatusProcessExecutor.TLE:
                    ketQuaChamTest.Status = StatusResult.TLE;
                    return ketQuaChamTest;
                    break;
            }

            #region Xử lí output đề và test
            //so sánh output bài với output đề 
            string strOutputCode = res.Output;
            string[] OutputCode = strOutputCode.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            //lấy output đề
            StreamReader srout = new StreamReader(dirTest + "\\" + TenBai + ".out");
            string strOutputTest = srout.ReadToEnd();
            string[] OutputTest = strOutputTest.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            //lấy output đề


            //khử \n ở cuối cho test
            for (int i = OutputTest.Count() - 1; i >= 0; i--)
                if (OutputTest[i] == "")
                {
                    Array.Resize(ref OutputTest, OutputTest.Length - 1); 
                }
                else
                    break;

            //khử \n ở cuối cho bài làm thí sinh
            for (int i = OutputCode.Count() - 1; i >= 0; i--)
                if (OutputCode[i] == "")
                {
                    Array.Resize(ref OutputCode, OutputCode.Length - 1);
                }
                else
                    break;



            // Chuẩn hóa Ouput bộ test (dành cho bộ test sinh tự động
            for (int i = 0; i < OutputTest.Count(); i++)
            {
                int ViTriR = OutputTest[i].LastIndexOf('\n');
                if (ViTriR!=-1 && ViTriR == OutputTest[i].Length-1)
                {
                    OutputTest[i] = OutputTest[i].Remove(ViTriR, 1);
                }

                // khử space ở cuối cho trường hợp:  Khi thừa khoảng trắng ở cuối dòng thì vẫn coi như hợp lệ
                do
                {
                    int ViTriSpace = OutputTest[i].LastIndexOf(' ');
                    if (ViTriSpace != -1 && ViTriSpace == OutputTest[i].Length - 1)                    
                        OutputTest[i] = OutputTest[i].Remove(ViTriSpace, 1);                    
                    else
                        break;
                }
                while (true);


            }

            //int dem = 0, j = 1;
            // Chuẩn hóa Ouput code
            for (int i = 0; i < OutputCode.Count(); i++)
            {
                // khử space ở cuối cho trường hợp:  Khi thừa khoảng trắng ở cuối dòng thì vẫn coi như hợp lệ
                do
                {
                    #region Cài đặt số lượng space cho phép
                    //while (dem < 3)
                    //{
                    //    int x = OutputCode[i].IndexOf(" ", OutputCode[i].Length - j);//lấy index của khoảng trắng theo vị trí mình muốn  
                    //    if (x != -1) //nếu index có tồn tại                                           
                    //    {
                    //        dem++;
                    //        j++;
                    //    }
                    //    else break;
                    //}
                    #endregion

                    int ViTriSpace = OutputCode[i].LastIndexOf(' ');
                    if (ViTriSpace != -1 && ViTriSpace == OutputCode[i].Length - 1)
                    {
                        OutputCode[i] = OutputCode[i].Remove(ViTriSpace, 1);
                    }
                    else
                        break;
                }
                while (true);

            }
            #endregion


            //using (StreamWriter sw = new StreamWriter(@"C:\Users\Miticc06\Desktop\DataDemo\BoTest\SORT25\10\SORT251.OUT"))
            //{
            //    foreach (string s in OutputCode)
            //    {
            //        sw.WriteLine(s);
            //    }
            //}

            if (XuLi.SoSanhOutput(OutputCode, OutputTest) == true)
            {
                ketQuaChamTest.Status = StatusResult.AC;
                return ketQuaChamTest;
            }
            else
            {
                ketQuaChamTest.Status = StatusResult.WA;
                return ketQuaChamTest;
            }

        }

        #region CODE CŨ
        /*

        /// <summary>
        /// 0 là sai, 1 là đúng, 2 là quá thời gian
        /// </summary>
        /// <param name="duongdanexe"></param>
        /// <param name="dirTest"></param>
        /// <param name="TenBai"></param>
        /// <param name="timeLimit"></param>
        /// <returns></returns>
        static public int chambai1(string fileName, string dirTest, string TenBai, int timeLimit) // chấm bài với từng test
        {

            long memoryLimit = 3145728;
            //    static public int chambai(string duongdanexe, string dirTest, string TenBai, int timeLimit) // chấm bài với từng test
            KetQuaCham1 res = new KetQuaCham1();
            try
            {
                string stringOutput = "";

                StreamReader sr = new StreamReader(dirTest + "\\" + TenBai + ".inp");  //Đưa vào input vào từ file
                string inputData = sr.ReadToEnd();
                sr.Close();


                var workingDirectory = new FileInfo(fileName).DirectoryName;

                var processStartInfo = new ProcessStartInfo(fileName)
                {
                    Arguments = "",
                    //Arguments = executionArguments == null ? string.Empty : string.Join(" ", executionArguments),
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    ErrorDialog = false,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    WorkingDirectory = workingDirectory
                };

                using (var process = System.Diagnostics.Process.Start(processStartInfo))
                {
                    process.MaxWorkingSet = new IntPtr(100000);
                    if (process == null)
                    {
                        throw new Exception($"Could not start process: {fileName}!");
                    }

                    process.PriorityClass = ProcessPriorityClass.High;

                    process.StandardInput.WriteLineAsync(inputData).ContinueWith(
                        delegate
                        {
                            try
                            {
                                process.StandardInput.FlushAsync().ContinueWith(
                                delegate
                                {
                                    try
                                    {
                                        process.StandardInput.Close();

                                    }
                                    catch
                                    {

                                    }
                                });
                            }
                            catch
                            {

                            }

                        });

                    var processOutputTask = process.StandardOutput.ReadToEndAsync().ContinueWith(
                       x =>
                       {
                           res.ReceivedOutput = x.Result; // nhận ouput code sau khi thực thi
                        });

                    var errorOutputTask = process.StandardError.ReadToEndAsync().ContinueWith(
                       x =>
                       {
                           res.ErrorOutput = x.Result;
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
                                    if (process.HasExited)
                                    {
                                        return;
                                    }

                                    var peakWorkingSetSize = process.PeakWorkingSet64;

                                    // res.MemoryUsed = Math.Max(res.MemoryUsed, peakWorkingSetSize);

                                    res.MemoryUsed = Math.Max(res.MemoryUsed, peakWorkingSetSize);


                                    if (memoryTaskCancellationToken.IsCancellationRequested)
                                    {
                                        return;
                                    }

                                    Thread.Sleep(TimeIntervalBetweenTwoMemoryConsumptionRequests);
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("Loi gia ram + " + ex.Message);
                            }

                        },
                        memoryTaskCancellationToken.Token);



                    var exited = process.WaitForExit((int)(timeLimit * 1.5));
                    if (!exited)
                    {
                        // Double check if the process has exited before killing it
                        if (!process.HasExited)
                        {
                            process.Kill();
                            process.WaitForExit(5000);
                        }
                        res.TrangThai = TrangThaiChamBai.TimeLimit;
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
                        //logger.Warn("AggregateException caught.", ex.InnerException);
                    }

                    // Close the task that gets the process error output
                    try
                    {
                        errorOutputTask.Wait(100);
                    }
                    catch (AggregateException ex)
                    {
                        // logger.Warn("AggregateException caught.", ex.InnerException);
                    }

                    // Close the task that gets the process output
                    try
                    {
                        processOutputTask.Wait(100);
                    }
                    catch (AggregateException ex)
                    {
                    }

                    res.ExitCode = process.ExitCode;
                    res.TimeWorked = process.ExitTime - process.StartTime;

                }



                if (res.TotalProcessorTime.TotalMilliseconds > timeLimit)
                {
                    res.TrangThai = TrangThaiChamBai.TimeLimit;
                }
                else
                {
                    if (!string.IsNullOrEmpty(res.ErrorOutput))
                    {
                        res.TrangThai = TrangThaiChamBai.RunTimeError;
                    }

                    if (res.MemoryUsed > memoryLimit)
                    {
                        res.TrangThai = TrangThaiChamBai.memoryLimit;
                    }
                }




                Debug.WriteLine("");
                Debug.WriteLine("{0}, thoi gian {1}", res.TrangThai, res.TimeWorked.TotalMilliseconds);
                Debug.WriteLine("output:" + res.ReceivedOutput);
                Debug.WriteLine("MemUsed:" + res.MemoryUsed);



                //so sánh output bài với output đề


                stringOutput = res.ReceivedOutput;

                string[] code = stringOutput.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                //lấy output đề
                StreamReader srout = new StreamReader(dirTest + "\\" + TenBai + ".out");
                string[] kq = srout.ReadToEnd().Split(new string[] { "\n" }, StringSplitOptions.None);
                //lấy output đề


                //List<string> listOutputCode = code.ToList<>
                for (int i = kq.Count() - 1; i >= 0; i--)
                    if (kq[i] == "")
                    {
                        Array.Resize(ref kq, kq.Length - 1);

                    }

                for (int i = code.Count() - 1; i >= 0; i--)
                    if (code[i] == "")
                    {
                        Array.Resize(ref code, code.Length - 1);

                    }

                if (XuLi.SoSanhOutput(code, kq) == true)
                    return 1;
                else
                    return 0;




            }
            catch (Exception ex)
            {
                return 3; // RE;
            }




            return 1;
        }

            */



        /*
        using (System.Diagnostics.Process pProcess = new System.Diagnostics.Process())
        {
            pProcess.StartInfo.FileName = duongdanexe;
            pProcess.StartInfo.Arguments = ""; // lệnh 
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.RedirectStandardOutput = true; // lấy output
            pProcess.StartInfo.RedirectStandardInput = true;  // nhập input
            pProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            pProcess.StartInfo.CreateNoWindow = true;

          //  pProcess.PriorityClass = ProcessPriorityClass.High;

            try
            {
                Stopwatch sw = Stopwatch.StartNew(); // bắt đầu tính time
                string stringOutput = String.Empty;

               // Task t = Task.Run(() =>
               //{
               //    try
               //    {
               //        pProcess.Start();
               //        StreamReader sr = new StreamReader(dirTest + "\\" + TenBai + ".inp");  //Đưa vào input vào từ file
               //         pProcess.StandardInput.WriteAsync(sr.ReadToEnd() + "\n" + (char)4); // \n để enter, char ascii 4 để gửi lệnh dừng truyền
               //         sr.Close();
               //        StreamReader reader = pProcess.StandardOutput;
               //        stringOutput = reader.ReadToEnd();
               //        pProcess.Close();
               //    }
               //    catch (Exception ex)
               //    {
               //        Debug.WriteLine("Lỗi RE: " + ex.Message);
               //         //       return 3; // lỗi biên dịch, runtime 
               //     }
               //});
                var t = new Task<long>(() =>
                {

                    try
                    {
                        pProcess.Start();
                        StreamReader sr = new StreamReader(dirTest + "\\" + TenBai + ".inp");  //Đưa vào input vào từ file
                        pProcess.StandardInput.WriteAsync(sr.ReadToEnd() + "\n" + (char)4); // \n để enter, char ascii 4 để gửi lệnh dừng truyền
                        sr.Close();
                        StreamReader reader = pProcess.StandardOutput;
                        stringOutput = reader.ReadToEnd();
                        pProcess.Close();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Lỗi RE: " + ex.Message);
                        return 3; // lỗi biên dịch, runtime 
                    }
                    return 4;
                });
                t.RunSynchronously();
                TimeSpan ts = TimeSpan.FromMilliseconds(timeLimit + 1600);


                ////Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
                //// long memoryUsage = currentProcess.WorkingSet64;

                //// Debug.Write(memoryUsage);


                //Process[] localByName = Process.GetProcessesByName(Path.GetFileName(duongdanexe));

                //Debug.WriteLine("kill " + Path.GetFileName(duongdanexe));

                //// localByName[0].Kill();
                //if (localByName == null)
                //    Debug.WriteLine("null " + Path.GetFileName(duongdanexe));

                //Debug.WriteLine("Done kill " + Path.GetFileName(duongdanexe));


                if (!t.Wait(ts))
                {
                    Debug.WriteLine("Qua thoi gian !");
                    pProcess.Kill();
                    sw.Stop();
                    return 2; // quá thời gian
                }  
                sw.Stop();// ngưng tính time


                Debug.WriteLine(sw.ElapsedMilliseconds);


                //so sánh output bài với output đề

                string[] sepCode = new string[] { "\r\n" };
                string[] code = stringOutput.Split(sepCode, StringSplitOptions.None);

                //lấy output đề
                string[] sepTest = new string[] { "\n" };
                StreamReader srout = new StreamReader(dirTest + "\\" + TenBai + ".out");
                string[] kq = srout.ReadToEnd().Split(sepTest, StringSplitOptions.None);
                //lấy output đề


                //List<string> listOutputCode = code.ToList<>
                for (int i=kq.Count()-1; i>=0; i--)
                    if (kq[i]=="")
                    {
                        Array.Resize(ref kq, kq.Length - 1);

                    }

                for (int i = code.Count() - 1; i >= 0; i--)
                    if (code[i] == "")
                    {
                        Array.Resize(ref code, code.Length - 1);

                    }

                if (XuLi.SoSanhOutput(code, kq) == true)
                    return 1;
                else
                    return 0;

            }
            catch (Exception ex)
            {
                Debug.WriteLine("loi " + ex.Message);

                // MessageBox.Show("Exception EXPORT:  Lỗi biên dịch hoặc runtime!");
                return 3; // lỗi biên dịch, runtime
            }

        #endregion

    }
    */



        /*


        public static int Execute12(string fileName, string inputData, int timeLimit, int memoryLimit, IEnumerable<string> executionArguments = null)
        {
            var workingDirectory = new FileInfo(fileName).DirectoryName;

            Debug.WriteLine("file :" + fileName);

            var processStartInfo = new ProcessStartInfo(fileName)
            {
                Arguments = executionArguments == null ? string.Empty : string.Join(" ", executionArguments),
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                ErrorDialog = false,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                WorkingDirectory = workingDirectory
            };

            using (var process = System.Diagnostics.Process.Start(processStartInfo))
            {
                if (process == null)
                {
                    throw new Exception($"Could not start process: {fileName}!");
                }

                process.PriorityClass = ProcessPriorityClass.High;

                // Write to standard input using another thread
                process.StandardInput.WriteLineAsync(inputData).ContinueWith(
                    delegate
                    {
                        // ReSharper disable once AccessToDisposedClosure
                        process.StandardInput.FlushAsync().ContinueWith(
                            delegate
                            {
                                process.StandardInput.Close();
                            });
                    });

                // Read standard output using another thread to prevent process locking (waiting us to empty the output buffer)
                var processOutputTask = process.StandardOutput.ReadToEndAsync().ContinueWith(
                    x =>
                    {
                        Debug.WriteLine("output code: "+ x.Result);
                    });

                // Read standard error using another thread
                var errorOutputTask = process.StandardError.ReadToEndAsync().ContinueWith(
                    x =>
                    {
                        Debug.WriteLine(x.Result);
                    });

                // Read memory consumption every few milliseconds to determine the peak memory usage of the process
                const int TimeIntervalBetweenTwoMemoryConsumptionRequests = 45;
                var memoryTaskCancellationToken = new CancellationTokenSource();
                var memoryTask = Task.Run(
                    () =>
                    {
                        while (true)
                        {
                            // ReSharper disable once AccessToDisposedClosure
                            if (process.HasExited)
                            {
                                return;
                            }

                            // ReSharper disable once AccessToDisposedClosure
                            var peakWorkingSetSize = process.PeakWorkingSet64;

                            Debug.WriteLine( Math.Max(0, peakWorkingSetSize));

                            if (memoryTaskCancellationToken.IsCancellationRequested)
                            {
                                return;
                            }

                            Thread.Sleep(TimeIntervalBetweenTwoMemoryConsumptionRequests);
                        }
                    },
                    memoryTaskCancellationToken.Token);

                // Wait the process to complete. Kill it after (timeLimit * 1.5) milliseconds if not completed.
                // We are waiting the process for more than defined time and after this we compare the process time with the real time limit.
                var exited = process.WaitForExit((int)(timeLimit * 1.5));
                if (!exited)
                {
                    // Double check if the process has exited before killing it
                    if (!process.HasExited)
                    {
                        process.Kill();

                        // Approach: https://msdn.microsoft.com/en-us/library/system.diagnostics.process.kill(v=vs.110).aspx#Anchor_2
                        process.WaitForExit(5000);
                    }

                    Debug.WriteLine("ProcessExecutionResultType.TimeLimit");
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
                    //logger.Warn("AggregateException caught.", ex.InnerException);
                }

                // Close the task that gets the process error output
                try
                {
                    errorOutputTask.Wait(100);
                }
                catch (AggregateException ex)
                {
                   // logger.Warn("AggregateException caught.", ex.InnerException);
                }

                // Close the task that gets the process output
                try
                {
                    processOutputTask.Wait(100);
                }
                catch (AggregateException ex)
                {
                   // logger.Warn("AggregateException caught.", ex.InnerException);
                }

                Debug.Assert(process.HasExited, "Standard process didn't exit!");

                // Report exit code and total process working time
                Debug.WriteLine(process.ExitCode);
                Debug.WriteLine(process.ExitTime - process.StartTime);
                Debug.WriteLine(process.PrivilegedProcessorTime);
                Debug.WriteLine(process.UserProcessorTime);
            }

            //if (result.TotalProcessorTime.TotalMilliseconds > timeLimit)
            //{
            //    result.Type = ProcessExecutionResultType.TimeLimit;
            //}

            //if (!string.IsNullOrEmpty(result.ErrorOutput))
            //{
            //    result.Type = ProcessExecutionResultType.RunTimeError;
            //}

            //if (result.MemoryUsed > memoryLimit)
            //{
            //    result.Type = ProcessExecutionResultType.memoryLimit;
            //}

            //return result;
            return 1;
        }

 */
        #endregion

        #region Các kiểu chấm bài
        static public KetQuaChamBai ChamDiemOI(string duongdanexe, ClassBoTest BoTest)
        {
            //int dem = 0;
            KetQuaChamBai kqChamBai = new KetQuaChamBai()
            {
                Type = BoTest.kieuCham,
                Details = new List<KetQuaChamTest>()
            };
            kqChamBai.Details.Clear();

            var arrTest = Directory.GetDirectories(BoTest.dirBoTest); //lấy từng test của 1 bài
            float diem = 0;
            foreach (string element in arrTest)
            {           
                KetQuaChamTest kqChamTest = chambai(duongdanexe + ".exe", element, BoTest.tenBoTest, BoTest.timeLimit, BoTest.memoryLimit);
                kqChamBai.Details.Add(kqChamTest);

                if (kqChamTest.Status == StatusResult.AC)
                {
                    diem = diem + (float)100 / (arrTest.Count());
                }
                //else 
                //{
                //    dem++;
                //    kqChamBai.Status = kqChamTest.Status;
                //    if (dem == 2)
                //    {
                //        kqChamBai.Scores = 0;
                //        return kqChamBai;
                //    }
                //}
            }

            kqChamBai.Scores = (float)Math.Round(diem, 2);

            if (kqChamBai.Scores == 100)
                kqChamBai.Status = StatusResult.AC;
            else
                kqChamBai.Status = StatusResult.WA;


            return kqChamBai;
        }

        static public KetQuaChamBai ChamDiemACM(string duongdanexe, ClassBoTest BoTest)
        {
            KetQuaChamBai kqChamBai = new KetQuaChamBai()
            {
                Type = BoTest.kieuCham,
                Details = new List<KetQuaChamTest>(),
                Status = StatusResult.AC,
                Scores = 100
            };
            kqChamBai.Details.Clear();

            var arr = Directory.GetDirectories(BoTest.dirBoTest);
            foreach (string element in arr)
            {
                KetQuaChamTest kqChamTest = chambai(duongdanexe + ".exe", element, BoTest.tenBoTest, BoTest.timeLimit, BoTest.memoryLimit);
                kqChamBai.Details.Add(kqChamTest);

                if (kqChamTest.Status != StatusResult.AC) // nếu sai
                {
                    kqChamBai.Status = kqChamTest.Status;
                    kqChamBai.Scores = 0;
                    return kqChamBai;
                }
            }
            return kqChamBai;
        }


        static public KetQuaChamBai ChamDiemChallenge(string duongdanexe, ClassBoTest BoTest, int length)
        {
            KetQuaChamBai kqChamBai = new KetQuaChamBai()
            {
                Type = BoTest.kieuCham,
                Details = new List<KetQuaChamTest>(),
                Status = StatusResult.AC,
                Scores = length // điểm bằng độ dài code 
            };
            kqChamBai.Details.Clear();

            var arr = Directory.GetDirectories(BoTest.dirBoTest);
            foreach (string element in arr)
            {
                KetQuaChamTest kqChamTest = chambai(duongdanexe + ".exe", element, BoTest.tenBoTest, BoTest.timeLimit, BoTest.memoryLimit);
                kqChamBai.Details.Add(kqChamTest);

                if (kqChamTest.Status != StatusResult.AC) // nếu sai
                {
                    kqChamBai.Status = kqChamTest.Status;
                    kqChamBai.Scores = 0;
                    return kqChamBai;
                }
            }
            return kqChamBai;
        }
        #endregion

        /// <summary>
        /// Trả về true nếu có tồn tại từ khóa trong file
        /// </summary>
        /// <param name="pathfilecode">Đường dẫn file cần kiểm tra</param>
        /// <param name="TuKhoa">Từ khóa cần kiểm tra</param>
        /// <returns></returns>
        static public bool KiemTraTuKhoa(string pathfilecode, string TuKhoa)
        {
            string code = File.ReadAllText(pathfilecode);

            string[] sep = new string[] { "\r\n" };
            string[] ListTuKhoa = TuKhoa.Split(sep, StringSplitOptions.None);

            foreach (string tu in ListTuKhoa)
            {
                int a = code.IndexOf(tu);
                if (a != (-1))
                {
                    return true;// Co chuoi system trong file
                }
            }
            return false;

        }


    }
}
﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Spreads.LMDB;

namespace DockerAspTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (File.Exists($"/globaldata/error_{Process.GetCurrentProcess().Id}.txt"))
            {
                return;
            }
            var errf = File.OpenWrite($"/globaldata/error_{Process.GetCurrentProcess().Id}.txt");
            var fileWriter = new StreamWriter(errf);
            var count = 33000;
            FileStream[] fss = new FileStream[count];
            int i;
            var sw = new Stopwatch();
            sw.Start();
            try
            {
                for (i = 0; i < count; i++)
                {
                    var fs = new FileStream($"/globaldata/{i}.txt", FileMode.OpenOrCreate,
                        FileAccess.ReadWrite, FileShare.ReadWrite, 4096,
                        FileOptions.RandomAccess);
                    fss[i] = fs;
                    fs.WriteByte((byte)(i % 255));
                    fs.Flush();
                }
                sw.Stop();
                File.WriteAllText($"/globaldata/ok_{Process.GetCurrentProcess().Id}.txt", sw.ElapsedMilliseconds.ToString());
            }
            catch (Exception ex)
            {
                sw.Stop();
                fileWriter.WriteLine(sw.ElapsedMilliseconds + " - " + ex.ToString());
                fileWriter.Flush();
                errf.Flush();
                // File.WriteAllText($"/globaldata/error_{Process.GetCurrentProcess().Id}.txt", sw.ElapsedMilliseconds + " - " + ex.ToString());
            }

            //try
            //{
            //    //if (Directory.Exists("/localdata/shared_lmdb"))
            //    //{
            //    //    Directory.Delete("/localdata/shared_lmdb", true);
            //    //}
            //    var env = new LMDBEnvironment("/localdata/shared_lmdb", DbEnvironmentFlags.NoSync | DbEnvironmentFlags.WriteMap);
            //    env.MapSize = 300L * 1024 * 1024 * 1024;
            //    env.Open();

            //    // write 1M values and then read them

            //    var db = env.OpenDatabase("test", new DatabaseConfig(DbFlags.Create | DbFlags.IntegerKey)).Result;

            //    var count = 100_000_000;
            //    var lastValue = 0;
            //    var myupdates = 0;
            //    env.Write(txn => { db.Truncate(txn); txn.Commit(); });

            //    // Thread.Sleep(5000);

            //    var sw = new Stopwatch();
            //    sw.Start();

            //    while (lastValue < count)
            //    {
            //        env.Write(txn =>
            //        {
            //            using (var c = db.OpenCursor(txn))
            //            {
            //                int key = default(int);

            //                if (c.TryGet(ref key, ref lastValue, CursorGetOption.Last))
            //                {
            //                    key++;
            //                    if (key - 1 == lastValue)
            //                    {
            //                        myupdates++;
            //                    }

            //                    lastValue = key;
            //                }

            //                c.Put(ref key, ref lastValue, CursorPutOptions.NoOverwrite);
            //                txn.Commit();
            //            }
            //        });
            //    }

            //    sw.Stop();
            //    env.Close().Wait();
            //    var elapsed = sw.ElapsedMilliseconds;
            //    File.WriteAllText($"/localdata/stat_{Process.GetCurrentProcess().Id}.txt", $"{myupdates} - {elapsed}");
            //}
            //catch (Exception ex)
            //{
            //    File.WriteAllText($"/localdata/error_{Process.GetCurrentProcess().Id}.txt", ex.ToString());

            //    Console.WriteLine(ex);
            //}

            //CreateWebHostBuilder(args).Build().StartAsync();
            //var tcs = new TaskCompletionSource<bool>();
            //tcs.Task.Wait();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}

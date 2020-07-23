// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Threading;
using TCLite.Framework.Constraints;

#if TASK_PARALLEL_LIBRARY_API
using System.Threading.Tasks;
#endif

#if NET40
using Task = System.Threading.Tasks.TaskEx;
#endif

namespace TCLite.Framework
{
    public class WarningFixture
    {
        #region Helper Methods

        protected int ReturnsFour()
        {
            return 4;
        }

        protected int ReturnsFive()
        {
            return 5;
        }

#if TASK_PARALLEL_LIBRARY_API
        private static Task<int> One()
        {
            return Task.Run(() => 1);
        }
#endif

        #endregion

        #region Stack filter tests
#if WIP
        [TestCase]
        public static void WarningSynchronous()
        {
            Assert.Warn("(Warning message)");
        }

        [TestCase]
        public static void WarningInBeginInvoke()
        {
            using (var finished = new ManualResetEvent(false))
            {
                new Action(() =>
                {
                    try
                    {
                        Assert.Warn("(Warning message)");
                    }
                    finally
                    {
                        finished.Set();
                    }
                }).BeginInvoke(ar => { }, null);

                if (!finished.WaitOne(10_000)) Assert.Fail("Timeout while waiting for BeginInvoke to execute.");
            }
        }

        [TestCase]
        public static void WarningInThreadStart()
        {
            using (var finished = new ManualResetEvent(false))
            {
                new Thread(() =>
                {
                    try
                    {
                        Assert.Warn("(Warning message)");
                    }
                    finally
                    {
                        finished.Set();
                    }
                }).Start();

                if (!finished.WaitOne(10_000))
                    Assert.Fail("Timeout while waiting for threadstart to execute.");
            }
        }

        [TestCase]
        public static void WarningInThreadPoolQueueUserWorkItem()
        {
            using (var finished = new ManualResetEvent(false))
            {
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    try
                    {
                        Assert.Warn("(Warning message)");
                    }
                    finally
                    {
                        finished.Set();
                    }
                });

                if (!finished.WaitOne(10_000))
                    Assert.Fail("Timeout while waiting for Threadpool.QueueUserWorkItem to execute.");
            }
        }

#if TASK_PARALLEL_LIBRARY_API
        [TestCase]
        public static void WarningInTaskRun()
        {
            if (!Task.Run(() => Assert.Warn("(Warning message)")).Wait(10_000))
                Assert.Fail("Timeout while waiting for Task.Run to execute.");
        }

        [TestCase]
        public static async System.Threading.Tasks.Task WarningAfterAwaitTaskDelay()
        {
            await Task.Delay(1);
            Assert.Warn("(Warning message)");
        }
#endif
#endif
        #endregion
    }

#if WIP
    public abstract class WarningInSetUpOrTearDownFixture
    {
        [TestCase]
        public void WarningPassesInTest()
        {
            Warn.Unless(2 + 2, Is.EqualTo(4));
        }

        [TestCase]
        public void WarningFailsInTest()
        {
            Warn.Unless(2 + 2, Is.EqualTo(5));
        }

        [TestCase]
        public void ThreeWarningsFailInTest()
        {
            Warn.Unless(2 + 2, Is.EqualTo(5));
            Warn.Unless(2 + 2, Is.EqualTo(22));
            Warn.Unless(2 + 2, Is.EqualTo(42));
        }
    }

    public class WarningInSetUpPasses : WarningInSetUpOrTearDownFixture
    {
        public WarningInSetUpPasses()
        {
            Warn.Unless(2 + 2, Is.EqualTo(4));
        }
    }

    public class WarningInSetUpFails : WarningInSetUpOrTearDownFixture
    {
        public WarningInSetUpFails()
        {
            Warn.Unless(2 + 2, Is.EqualTo(5));
        }
    }

#if NYI
    public class WarningInTearDownPasses : WarningInSetUpOrTearDownFixture
    {
        [TearDown]
        public void TearDown()
        {
            Warn.Unless(2 + 2, Is.EqualTo(4));
        }
    }

    public class WarningInTearDownFails : WarningInSetUpOrTearDownFixture
    {
        [TearDown]
        public void TearDown()
        {
            Warn.Unless(2 + 2, Is.EqualTo(5));
        }
#endif
    }
#endif // WIP
}
// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Threading;

namespace TCLite.Framework.Internal
{
    /// <summary>
    /// The ThreadUtility class encapsulates several static methods
    /// useful when working with threads.
    /// </summary>
    public class ThreadUtility
    {
        /// <summary>
        /// Do our best to Kill a thread
        /// </summary>
        /// <param name="thread">The thread to kill</param>
        public static void Kill(Thread thread)
        {
            Kill(thread, null);
        }

        /// <summary>
        /// Do our best to kill a thread, passing state info
        /// </summary>
        /// <param name="thread">The thread to kill</param>
        /// <param name="stateInfo">Info for the ThreadAbortException handler</param>
        public static void Kill(Thread thread, object stateInfo)
        {
            try
            {
                if (stateInfo == null)
                    thread.Abort();
                else
                    thread.Abort(stateInfo);
            }
            catch (ThreadStateException)
            {
                // Although obsolete, this use of Resume() takes care of
                // the odd case where a ThreadStateException is received
                // so we continue to use it.
                thread.Resume();
            }

            if ( (thread.ThreadState & ThreadState.WaitSleepJoin) != 0 )
                thread.Interrupt();
        }

        private ThreadUtility() { }
    }
}

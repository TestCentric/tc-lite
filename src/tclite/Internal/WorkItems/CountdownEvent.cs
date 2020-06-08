// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Threading;

namespace TCLite.Framework.Internal.WorkItems
{
    /// <summary>
    /// A simplified implementation of .NET 4 CountdownEvent
    /// for use in earlier versions of .NET. Only the methods
    /// used by NUnit are implemented.
    /// </summary>
    public class CountdownEvent
    {
        int _initialCount;
        int _remainingCount;
        object _lock = new object();
        ManualResetEvent _event = new ManualResetEvent(false);

        /// <summary>
        /// Construct a CountdownEvent
        /// </summary>
        /// <param name="initialCount">The initial count</param>
        public CountdownEvent(int initialCount)
        {
            _initialCount = _remainingCount = initialCount;
        }

        /// <summary>
        /// Gets the initial count established for the CountdownEvent
        /// </summary>
        public int InitialCount
        {
            get { return _initialCount; }
        }

        /// <summary>
        /// Gets the current count remaining for the CountdownEvent
        /// </summary>
        public int CurrentCount
        {
            get { return _remainingCount; }
        }

        /// <summary>
        /// Decrement the count by one
        /// </summary>
        public void Signal()
        {
            lock (_lock)
            {
                if (--_remainingCount == 0)
                    _event.Set();
            }
        }

        /// <summary>
        /// Block the thread until the count reaches zero
        /// </summary>
        public void Wait()
        {
            _event.WaitOne();
        }
    }
}

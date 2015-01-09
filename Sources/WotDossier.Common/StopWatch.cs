using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;

namespace WotDossier.Common
{
    /// <summary>
    /// Represents a high-resolution stopwatch. It can be used to measure 
    /// very small intervals of time.
    /// </summary>
    public sealed class StopWatch
    {
        /// <summary>
        /// Holds the value of the StartTime property.
        /// </summary>
        private long _startValue;

        /// <summary>
        /// Initializes a new instance of the StopWatch class.
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// The system does not have a high-resolution 
        /// performance counter.
        /// </exception>
        public StopWatch()
        {
            Reset();
        }

        /// <summary>
        /// Resets the stopwatch. This method should be called 
        /// when you start measuring.
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// The system does not have a high-resolution 
        /// performance counter.
        /// </exception>
        public void Reset()
        {
            _startValue = GetValue();
        }

        /// <summary>
        /// Returns the time that has passed since the Reset() 
        /// method was called.
        /// </summary>
        /// <remarks>
        /// The time is returned in tenths-of-a-millisecond. 
        /// If the Peek method returns '10000', it means the interval 
        /// took exactely one second.
        /// </remarks>
        /// <returns>
        /// A long that contains the time that has passed 
        /// since the Reset() method was called.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// The system does not have a high-resolution performance counter.
        /// </exception>
        public long Peek()
        {
            long endValue = GetValue();
            long value = endValue - _startValue;
            return value;
        }

        /// <summary>
        /// Returns the time that has passed since the Reset() 
        /// method was called. In ms.
        /// </summary>
        /// <remarks>
        /// The time is returned in tenths-of-a-millisecond. 
        /// If the Peek method returns '10000', it means the interval 
        /// took exactely one second.
        /// </remarks>
        /// <returns>
        /// A long that contains the time that has passed 
        /// since the Reset() method was called.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// The system does not have a high-resolution performance counter.
        /// </exception>
        public double PeekMs()
        {
            long peek = Peek();
            return TimeSpan.FromMilliseconds(peek).TotalMilliseconds;
        }

        /// <summary>
        /// Retrieves the current value of the high-resolution 
        /// performance counter.
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// The system does not have a high-resolution 
        /// performance counter.
        /// </exception>
        /// <returns>
        /// A long that contains the current performance-counter 
        /// value, in counts.
        /// </returns>
        private static long GetValue()
        {
            return Environment.TickCount;
        }
    }

    public class HiPerfTimer
    {
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(
            out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(
            out long lpFrequency);

        private long startTime, stopTime;
        private long freq;

        // Constructor
        public HiPerfTimer()
        {
            startTime = 0;
            stopTime = 0;

            if (QueryPerformanceFrequency(out freq) == false)
            {
                // high-performance counter not supported
                throw new Win32Exception();
            }
        }

        // Start the timer
        public void Start()
        {
            // lets do the waiting threads there work
            Thread.Sleep(0);

            QueryPerformanceCounter(out startTime);
        }

        // Stop the timer
        public void Stop()
        {
            QueryPerformanceCounter(out stopTime);
        }

        // Returns the duration of the timer (in seconds)
        public double Duration
        {
            get
            {
                return (double)(stopTime - startTime) / (double)freq;
            }
        }
    }
}

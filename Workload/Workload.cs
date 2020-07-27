using System;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

namespace Workload
{
    /// <summary>
    /// Creates thread per processor, adds ability to start/stop calculation in each thread
    /// so switching processor utilization between 0% and 100%
    /// </summary>
    public class Workload
    {
        public Workload()
        {
            _cpuCount = Environment.ProcessorCount;
            _startHandles = new AutoResetEvent[_cpuCount];
            _finishFlags = new int[_cpuCount];
        }

        public void Start()
        {
            var process = Process.GetCurrentProcess();
            var initialThreadsCount = process.Threads.Count;
            
            for (var i = 0; i < _cpuCount; i++)
            {
                var startHandle = new AutoResetEvent(false);
                var finishFlagId = i;
                
                var thread = new Thread(() => Calculation(startHandle, ref _finishFlags[finishFlagId])) {IsBackground = true};
                thread.Start();
                
                _startHandles[i] = startHandle;
                _finishFlags[i] = 0;
            }
            
            process.Refresh();

            for (var i = 0; i < _cpuCount; i++)
            {
                process.Threads[initialThreadsCount + i].ProcessorAffinity = (IntPtr)(1 << i); 
            }
        }

        public void SetMask(IEnumerable<bool> mask)
        {
            var cpu = 0;
            foreach (var flag in mask)
            {
                if(flag)
                    StartCalculationsOn(cpu);
                else
                    StopCalculationOn(cpu);
                
                if (++cpu >= _cpuCount)
                    break;
            }
        }
        
        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////

        private void StartCalculationsOn(int processorId) 
        {
            Interlocked.Exchange(ref _finishFlags[processorId], 0);
            _startHandles[processorId].Set();
        }

        private void StopCalculationOn(int processorId)
        {
            _startHandles[processorId].Reset();
            Interlocked.Exchange(ref _finishFlags[processorId], 1);
        }
        
        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////

        private static void Calculation(WaitHandle startHandle, ref int finishFlag)
        {
            var rand = new Random();
            while (true)
            {
                startHandle.WaitOne();
                while (Interlocked.Exchange(ref finishFlag, 0) == 0)
                {
                    rand.NextDouble();
                }
            }
        }
 
        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////
        
        private readonly int _cpuCount;
        private readonly AutoResetEvent[] _startHandles;
        private readonly int[] _finishFlags;
    }
}
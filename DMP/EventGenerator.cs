/* Copyright (c) 2017-2018, Mayo Foundation for Medical Education and Research (MFMER), All rights reserved. 
Academic non-commercial use of this software is allowed with expressed permission of the developers. 
MFMER disclaims all implied warranties of merchantability and fitness for a particular purpose with 
respect to this software, its application, and any verbal or written statements regarding its use. 
The software may not be distributed to third parties without consent of MFMER. Use of this software 
constitutes acceptance of these terms. 
Contributors: Daniel Crepeau, Tal Pal Attia, Jan Cimbalnik, Hari Guragain, Mona Nasseri, Vaclav Kremen, 
Benjamin Brinkmann, Matt Stead, Gregory Worrell.  */

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DMP
{
    public class EventGenerator : IDisposable
    {
        public delegate void EventINSHandler();
        public delegate void EventCTMHandler();
        public delegate void EventTabHandler();
        public delegate void EventMessageHandler();

        public event EventINSHandler EventINS;
        public event EventCTMHandler EventCTM;
        public event EventTabHandler EventTab;
        public event EventMessageHandler EventMessage;

        private Thread m_eventThread = null;
        private bool m_bEndLoop = false;
        private Mutex m_endLoopMutex = null;

        private Mutex m_triggerEventINSMutex = null;
        private Mutex m_triggerEventCTMMutex = null;
        private Mutex m_triggerEventTabMutex = null;
        private Mutex m_triggerEventMessageMutex = null;

        private bool m_bTriggerEventCTM = false;
        private bool m_bTriggerEventINS = false;
        private bool m_bTriggerEventTab = false;
        private bool m_bTriggerEventMessage = false;

        public EventGenerator()
        {
            m_endLoopMutex = new Mutex();
            m_triggerEventINSMutex = new Mutex();
            m_triggerEventCTMMutex = new Mutex();
            m_triggerEventTabMutex = new Mutex();
            m_triggerEventMessageMutex = new Mutex();

            m_eventThread = new Thread(new ThreadStart(ThreadMethod));
            m_eventThread.Start();
        }

        public bool EndLoop
        {
            get
            {
                bool bRet = false;
                m_endLoopMutex.WaitOne();
                bRet = m_bEndLoop;
                m_endLoopMutex.ReleaseMutex();

                return bRet;
            }

            set
            {
                m_endLoopMutex.WaitOne();
                m_bEndLoop = value;
                m_endLoopMutex.ReleaseMutex();
            }
        }

        private void RaiseEventINS()
        {
            try
            {
                EventINS?.Invoke();
            }
            catch
            {
            }
        }
        public void TriggerEventINS()
        {
            m_triggerEventINSMutex.WaitOne();
            m_bTriggerEventINS = true;
            m_triggerEventINSMutex.ReleaseMutex();
        }

        private void RaiseEventCTM()
        {
            try
            {
                EventCTM?.Invoke();
            }
            catch
            {
            }
        }
        public void TriggerEventCTM()
        {
            m_triggerEventCTMMutex.WaitOne();
            m_bTriggerEventCTM = true;
            m_triggerEventCTMMutex.ReleaseMutex();
        }

        private void RaiseEventTab()
        {
            try
            {
                EventTab?.Invoke();
            }
            catch
            {
            }
        }
        public void TriggerEventTab()
        {
            m_triggerEventTabMutex.WaitOne();
            m_bTriggerEventTab = true;
            m_triggerEventTabMutex.ReleaseMutex();
        }

        private void RaiseEventMessage()
        {
            try
            {
                EventMessage?.Invoke();
            }
            catch
            {
            }
        }
        public void TriggerEventMessage()
        {
            m_triggerEventMessageMutex.WaitOne();
            m_bTriggerEventMessage = true;
            m_triggerEventMessageMutex.ReleaseMutex();
        }

        private void ThreadMethod()
        {
            while (!EndLoop)
            {
                m_triggerEventINSMutex.WaitOne();
                if (m_bTriggerEventINS)
                {
                    m_bTriggerEventINS = false;
                    RaiseEventINS();
                }
                m_triggerEventINSMutex.ReleaseMutex();

                m_triggerEventCTMMutex.WaitOne();
                if (m_bTriggerEventCTM)
                {
                    m_bTriggerEventCTM = false;
                    RaiseEventCTM();
                }
                m_triggerEventCTMMutex.ReleaseMutex();

                m_triggerEventTabMutex.WaitOne();
                if (m_bTriggerEventTab)
                {
                    m_bTriggerEventTab = false;
                    RaiseEventTab();
                }
                m_triggerEventTabMutex.ReleaseMutex();

                m_triggerEventMessageMutex.WaitOne();
                if (m_bTriggerEventMessage)
                {
                    m_bTriggerEventMessage = false;
                    RaiseEventMessage();
                }
                m_triggerEventMessageMutex.ReleaseMutex();
                Thread.Sleep(200);
            }
        }


        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                EndLoop = true;

                m_triggerEventTabMutex.Dispose();
                m_triggerEventMessageMutex.Dispose();
                m_triggerEventINSMutex.Dispose();
                m_triggerEventCTMMutex.Dispose();
                m_endLoopMutex.Dispose();

                // Let some time to stop
                Thread.Sleep(500);
            }
            // free native resources if there are any.
        }

        #endregion
    }
}

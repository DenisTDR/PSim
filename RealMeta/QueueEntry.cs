using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSim
{
    public class QueueEntry
    {
        public delegate bool theFunction(QueueEntry qe, EventArgs e);
        public event theFunction TheFunction;

        public int Period { get; set; }
        public int BackUpPeriod { get; set; }
        public bool Repeat { get; set; }
        public string UserData { get; set; }

        public bool CheckIfDone()
        {
            return TheFunction(this, null);
        }

        public QueueEntry()
        {
            Period = 1000;
            BackUpPeriod = Period;
            Repeat = false;
        }

    }
}

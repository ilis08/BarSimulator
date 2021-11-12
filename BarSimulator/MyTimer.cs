using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace BarSimulator
{
    public delegate void ActionEventHandler(object source, ElapsedEventArgs args);

    public class MyTimer
    {
        public event ActionEventHandler ActionEvent;

        public System.Timers.Timer timer = new();

        public void SetTimer(ActionEventHandler handler)
        {

            timer.Elapsed += new ElapsedEventHandler(handler);
            timer.Interval = 200000;
            timer.Enabled = true;
        }
    }
}

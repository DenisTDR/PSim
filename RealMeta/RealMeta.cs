using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSim
{
    public static class RealMeta
    {
        public static void initFunc1()
        {
            QueueEntry qe = new QueueEntry();
            qe.BackUpPeriod = qe.Period = 100;
            qe.Repeat = true;
            qe.TheFunction += func1;
            state1 = 0;
            ext.cmdQueue.Add(qe);
        }
        static int state1 = 0;
        static double rm1, tmp, leftOffset;
        public static bool func1(QueueEntry qe, EventArgs e)
        {
            switch (state1)
            {
                case 0:
                    state1 = 1;
                    RealFuncs.goFront(30, 200);
                    break;
                case 1:
                    if (funcs.getSensorValue(Sensor.FrontLeft) < 50)
                    {
                        RealFuncs.rotirePeLoc(30, 200, Engines.LeftEngines);
                        state1 = 2;
                        leftOffset = 80;
                        rm1 = 1000;
                    }
                    break;
                case 2:
                    tmp = funcs.getSensorValue(Sensor.SideRight);
                    funcs.Log("rm=" + rm1.ToString() + "   tmp=" + tmp.ToString());
                    if (tmp > leftOffset)
                        break;
                    if (tmp < rm1)
                    {
                        rm1 = tmp;
                    }
                    else
                    {
                        RealFuncs.StopEngines();
                        state1 = 3;
                        return true;
                    }
                    break;

                default:
                    break;
            }
            return false;
        }

    }
}

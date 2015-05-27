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
        static int cnt1;
        public static bool func1(QueueEntry qe, EventArgs e)
        {
            switch (state1)
            {
                case 0:
                    state1 = 1;
                    RealFuncs.goFront(30, 250);
                    break;
                case 1:
                    if (funcs.getSensorValue(Sensor.FrontLeft) < 50)
                    {
                        RealFuncs.rotirePeLoc(10, 200, Engines.LeftEngines);
                        state1 = 2;
                        leftOffset = 80;
                        rm1 = 1000;
                    }
                    break;
                case 2:
                    tmp = funcs.getSensorValue(Sensor.SideRight);
                    funcs.Log("st=" + state1.ToString() + "  rm=" + rm1.ToString() + "   tmp=" + tmp.ToString());
                    if (tmp > 150) break;
                    if (tmp > leftOffset) break;

                    if (tmp < rm1 + 0.05)
                        rm1 = tmp;
                    else
                    {
                        RealFuncs.rotirePeLoc(10, 200, Engines.RightEngines);
                        state1 = 3;
                        rm1=999;
                    }
                    break;
                case 3:
                    tmp = funcs.getSensorValue(Sensor.SideRight);
                    funcs.Log("st=" + state1.ToString() + "  rm=" + rm1.ToString() + "   tmp=" + tmp.ToString());
                    if (tmp > leftOffset)
                        break;
                    if (tmp < rm1+0.05)
                        rm1 = tmp;
                    else
                    {
                        RealFuncs.goFront(10, 250);
                        state1 = 4;
                        rm1 = 999;
                    }
                    break;
                case 4:
                    double fl, fr;
                    fl = funcs.getSensorValue(Sensor.FrontLeft);
                    fr = funcs.getSensorValue(Sensor.FrontRight);
                    funcs.Log("st=" + state1.ToString() + "   lf=" + fl.ToString() + "   fr=" + fr.ToString());
                    if (fl < 50 || fr < 50)
                    {
                        RealFuncs.rotirePeLoc(10, 200, Engines.RightEngines);
                        cnt1 = 0;
                        state1 = 5;
                        leftOffset = 80;
                        rm1 = 1000;
                    }
                    break;
                case 5:
                    tmp = funcs.getSensorValue(Sensor.SideLeft);
                    funcs.Log("st=" + state1.ToString() + "  rm=" + rm1.ToString() + "   tmp=" + tmp.ToString());
                    if (cnt1++ < 10) break;
                    if (tmp > leftOffset)
                        break;
                    if (tmp < rm1+0.05)
                        rm1 = tmp;
                    else
                    {
                        RealFuncs.rotirePeLoc(10, 200, Engines.LeftEngines);
                        state1 = 6;
                    }
                break;
                case 6:
                    tmp = funcs.getSensorValue(Sensor.SideLeft);
                    funcs.Log("st=" + state1.ToString() + "  rm=" + rm1.ToString() + "   tmp=" + tmp.ToString());
                    if (tmp > leftOffset)
                        break;
                    if (tmp < rm1)
                        rm1 = tmp;
                    else
                    {
                        RealFuncs.goFront(10, 250);
                        state1 = 4;
                        rm1 = 999;
                    }
                break;
                default:
                    break;
            }
            return false;
        }

        #region parcare laterala
        public static void initParcareLaterala()
        {
            QueueEntry qe = new QueueEntry();
            qe.BackUpPeriod = qe.Period = 100;
            qe.Repeat = true;
            qe.TheFunction += parcareLaterala;
            state2 = 0;
            ext.cmdQueue.Add(qe);
        }
        static int state2;
        static double rm2, tmp2;
        public static bool parcareLaterala(QueueEntry qe, EventArgs e)
        {
            funcs.Log("st=" + state2.ToString());

            switch (state2)
            {
                case 0:
                    RealFuncs.goFront(30, 200);
                    state2 = 1;
                    break;
                case 1:
                    tmp2 = funcs.getSensorValue(Sensor.SideRight);
                    if (tmp2 < 50)
                    {
                        state2 = 2;
                    }
                    break;
                case 2:
                    tmp2 = funcs.getSensorValue(Sensor.SideRight);
                    if (tmp2 > 50)
                    {
                        state2 = 3;
                        RealFuncs.goFrontRight(30, 200);
                    }
                    break;
                case 3:
                    tmp2 = funcs.getSensorValue(Sensor.FrontRight);
                    if (tmp2 < 55)
                    {
                        state2 = 4;
                        RealFuncs.goFrontLeft(30, 250);
                    }

                    break;

                case 4:
                    if (isRightParalel(3) == 0)
                    {
                        RealFuncs.goFront(30, 175);
                        state2 = 5;
                        return false;
                    }
                    break;
                case 5:
                    if (Math.Abs(funcs.getSensorValue(Sensor.FrontLeft) - funcs.getSensorValue(Sensor.FrontRight)) < 10)
                    {
                        RealFuncs.StopEngines();
                        RealFuncs.goBack(1, 150);
                        return true;
                    }
                    break;
                case 6:

                    break;
                default:
                    break;

            }

            return false;
        }
        #endregion

        #region Mers Paralel
        public static void initMersParalel(bool _rightParalel=true)
        {
            state3 = 0;
            QueueEntry qe = new QueueEntry();
            qe.BackUpPeriod = qe.Period = 100;
            qe.Repeat = true;
            if (_rightParalel)
                qe.TheFunction += mergiParalelPeDreapta;
            else
                qe.TheFunction += mergiParalelPeStanga;
            state2 = 0;
            ext.cmdQueue.Add(qe);
        }
        
        public static int state3;
        public static bool mergiParalelPeDreapta(QueueEntry qe, EventArgs e)
        {
            int pd;
            switch (state3)
            {
                case 0:
                    RealFuncs.goFront(30, 230);
                    state3 = 1;
                    break;
                case 1:
                     pd = isRightParalel();
                     if (pd == 0)
                     {
                         if (funcs.getSensorValue(Sensor.SideRight) < 12)
                         {
                             RealFuncs.goFrontLeft(30, 200);
                         }
                         break;
                     }
                     if (pd == 1)
                     {
                         RealFuncs.goFrontRight(30, 220);
                         state3 = 2;
                     }
                     else
                     {
                         RealFuncs.goFrontLeft(30, 220);
                         state3 = 2;
                     }
                    break;
                case 2:
                    pd = isRightParalel();
                    if (pd == 0)
                    {
                        RealFuncs.goFront(30, 230);
                        state3 = 1;
                    }
                    break;
            }

            return false;
        }
        public static bool mergiParalelPeStanga(QueueEntry qe, EventArgs e)
        {
            int pd;
            switch (state3)
            {
                case 0:
                    RealFuncs.goFront(30, 230);
                    state3 = 1;
                    break;
                case 1:
                    pd = isLeftParalel();
                    if (pd == 0)
                    {
                        if (funcs.getSensorValue(Sensor.SideRight) < 12)
                        {
                            RealFuncs.goFrontRight(30, 200);
                        }
                        break;
                    }
                    if (pd == 1)
                    {
                        RealFuncs.goFrontLeft(30, 220);
                        state3 = 2;
                    }
                    else
                    {
                        RealFuncs.goFrontRight(30, 220);
                        state3 = 2;
                    }
                    break;
                case 2:
                    pd = isLeftParalel();
                    if (pd == 0)
                    {
                        RealFuncs.goFront(30, 230);
                        state3 = 1;
                    }
                    break;
            }

            return false;
        }
        #endregion 

        public static void initP1()
        {
            QueueEntry qe = new QueueEntry();
            qe.BackUpPeriod = qe.Period = 100;
            qe.Repeat = true;
            qe.TheFunction += p1Entry;
            state4 = 0;
            ext.cmdQueue.Add(qe);
        }
        static int state4, ilp, irp;
        static double sfs, sfd, ssl, minL;
        static int cntDelayer;
        public static bool p1Entry(QueueEntry qe, EventArgs e)
        {
            funcs.Log("st=" + state4.ToString());
            switch(state4){
                case 0:
                    RealFuncs.goFront(30, 200);
                    state4 = 1;
                    break;
                case 1:
                    sfs = funcs.getSensorValue(Sensor.FrontLeft);
                    sfd = funcs.getSensorValue(Sensor.FrontRight);
                    if (sfs < 50 || sfd < 50)
                    {
                        RealFuncs.rotirePeLoc(30, 200, Engines.LeftEngines);
                        state4 = 2;
                        cntDelayer = 0;
                    }
                    break;
                case 2:
                    if (cntDelayer++ < 10)
                        break;
                    ssl = funcs.getSensorValue(Sensor.SideLeft);
                    funcs.Log("ssl=" + ssl.ToString() + "   minL=" + minL.ToString());
                    if (ssl < minL)
                        minL = ssl;
                    else
                        state4 = 3;
                    break;
                case 3:
                    ilp = isLeftParalel(3);
                    funcs.Log("ilp=" + ilp.ToString());
                    if (ilp == 0)
                    {
                        RealFuncs.goFront(30, 150);
                        state4 = 4;
                    }
                    else if (ilp == -1)
                    {
                        RealFuncs.rotirePeLoc(30, 200, Engines.LeftEngines);
                    }
                    else
                    {
                        RealFuncs.rotirePeLoc(30, 200, Engines.RightEngines);
                    }
                    break;
                case 4:
                    sfs = funcs.getSensorValue(Sensor.FrontLeft);
                    sfd = funcs.getSensorValue(Sensor.FrontRight);
                    if (sfs < 50 || sfd < 50)
                    {
                        RealFuncs.rotirePeLoc(30, 200, Engines.RightEngines);
                        state4 = 5;
                        cntDelayer = 0;
                    }
                    break;
                case 5:
                    irp = isRightParalel(3);
                    if (irp == 0)
                    {
                        //RealFuncs.goFront(30, 150);
                        RealFuncs.StopEngines();
                        return true;
                        state4 = 6;
                    }
                    else if (irp == -1)
                    {
                        RealFuncs.rotirePeLoc(30, 200, Engines.RightEngines);
                    }
                    else
                    {
                        RealFuncs.rotirePeLoc(30, 200, Engines.LeftEngines);
                    }
                    break;

            }
            return false;
        }

        public static void initTest1()
        {
            QueueEntry qe = new QueueEntry();
            qe.BackUpPeriod = qe.Period = 250;
            qe.Repeat = true;
            qe.TheFunction += test1Entry;
            state5 = 0;
            ext.cmdQueue.Add(qe);
        }
        static int state5;
        public static bool test1Entry(QueueEntry qe, EventArgs e)
        {
            funcs.Log("left: " + funcs.getSensorValue(Sensor.SideLeft));
            switch (state5)
            {
                case 0:
                    state5 = 1;
                    //RealFuncs.goFront(30, 200);
                    break;
                case 1:
                    
                    break;
            }
            return false;
        }

        
        public static double ParallelTollerance { get; set; }
        public static double ParallelWarningTollerance { get; set; }
        
        public static int isRightParalel(int epsilon = -1, int warningEpsilon = -1)
        {
            if (epsilon < 0) epsilon = (int)ParallelTollerance;
            if (warningEpsilon < 0) warningEpsilon = (int)ParallelWarningTollerance;
            double rightVal, frontRightVal;
            rightVal = funcs.getSensorValue(Sensor.SideRight);
            frontRightVal = funcs.getSensorValue(Sensor.FrontRight);
            return isParalel((int)rightVal, (int)frontRightVal, (int)(ext.TheCar.ActualWidth * 3 / 4), epsilon, warningEpsilon);
        }

        public static int isLeftParalel(int epsilon = -1, int warningEpsilon = -1)
        {
            if (epsilon < 0) epsilon = (int)ParallelTollerance;
            if (warningEpsilon < 0) warningEpsilon = (int)ParallelWarningTollerance;
            double leftVal, frontLeftVal;
            leftVal = funcs.getSensorValue(Sensor.SideLeft);
            frontLeftVal = funcs.getSensorValue(Sensor.FrontLeft);
            return isParalel((int)leftVal, (int)frontLeftVal, (int)(ext.TheCar.ActualWidth * 3 / 4), epsilon, warningEpsilon);
        }
        public static int isParalel(int sideValue, int frontValue, int sensorOffset, int epsilon, int warningEpsilon)
        {
            int rez = (frontValue - (sensorOffset * (int)Math.Pow(2, 9) / 362)) * 362 / (int)Math.Pow(2, 9) - sideValue;
            if (Math.Abs(rez) < warningEpsilon)
                if (Math.Abs(rez) < epsilon)
                    return 0;
                else
                    return rez > epsilon ? 1 : -1;
            else
                return rez > warningEpsilon ? 2 : -2;
            
        }
    }
}


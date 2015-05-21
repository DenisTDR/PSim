using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSim
{
    public static class RealFuncs
    {
        public static void StopEngines()
        {
            ext.ActionsList.Clear();
        }
        public static void goFront(byte timp, byte viteza)
        {
            SetEnginesSpeed(Engines.LeftEngines, Sens.SensFata, viteza);
            SetEnginesSpeed(Engines.RightEngines, Sens.SensFata, viteza);
            StopEnginesAfter(timp);
        }

        public static void goBack(byte timp, byte viteza)
        {
            SetEnginesSpeed(Engines.LeftEngines, Sens.SensSpate, viteza);
            SetEnginesSpeed(Engines.RightEngines, Sens.SensSpate, viteza);
            StopEnginesAfter(timp);
        }

        public static void goFrontLeft(byte timp, byte viteza)
        {
            SetEnginesSpeed(Engines.LeftEngines, Sens.SensFata, 50);
            SetEnginesSpeed(Engines.RightEngines, Sens.SensFata, viteza);
            StopEnginesAfter(timp);
        }

        public static void goFrontRight(byte timp, byte viteza)
        {
            SetEnginesSpeed(Engines.LeftEngines, Sens.SensFata, viteza);
            SetEnginesSpeed(Engines.RightEngines, Sens.SensFata, 50);
            StopEnginesAfter(timp);
        }
        public static void goBackLeft(byte timp, byte viteza)
        {
            SetEnginesSpeed(Engines.RightEngines, Sens.SensSpate, viteza);
            SetEnginesSpeed(Engines.LeftEngines, Sens.SensSpate, 50);
            StopEnginesAfter(timp);
        }
        public static void goBackRight(byte timp, byte viteza)
        {
            SetEnginesSpeed(Engines.RightEngines, Sens.SensSpate, 50);
            SetEnginesSpeed(Engines.LeftEngines, Sens.SensSpate, viteza);
            StopEnginesAfter(timp);
        }

        public static void rotirePeLoc(byte timp, byte viteza, Engines engines)
        {
            if (engines == Engines.LeftEngines)
            {
                SetEnginesSpeed(Engines.LeftEngines, Sens.SensSpate, viteza);
                SetEnginesSpeed(Engines.RightEngines, Sens.SensFata, viteza);
            }
            else
            {
                SetEnginesSpeed(Engines.LeftEngines, Sens.SensFata, viteza);
                SetEnginesSpeed(Engines.RightEngines, Sens.SensSpate, viteza);
            }
            StopEnginesAfter(timp);
        }

        public static void SetEnginesSpeed(Engines engine, Sens sens, byte viteza)
        {
            EnginesSetting es = new EnginesSetting();
            if (engine == Engines.LeftEngines)
            {
                es.LeftEnginesForce = viteza / 255.0;
                es.LeftEnginesSense = sens == Sens.SensFata ? 1 : -1;
                es.SetLeft = true;
            }
            else
            {
                es.RightEnginesForce = viteza / 255.0;
                es.RightEnginesSense = sens == Sens.SensFata ? 1 : -1;
                es.SetRight = true;
            }
            SetEngines(es);
        }

        public static void StopEnginesAfter(byte time)
        {
            QueueEntry qe = new QueueEntry();
            qe.BackUpPeriod = qe.Period = time * 1000;
            qe.Repeat = false;
            qe.TheFunction += (QueueEntry qee, EventArgs e) => { StopEngines(); return true; };
            ext.cmdQueue.Add(qe);
        }

        public static void SetEngines(EnginesSetting es)
        {
            if (es.SetLeft)
            {
                ext.TheCar.LeftEnginesForce = es.LeftEnginesForce;
                ext.TheCar.LeftEnginesSense = es.LeftEnginesSense;
            }
            else
            {
                ext.TheCar.RightEnginesForce = es.RightEnginesForce;
                ext.TheCar.RightEnginesSense = es.RightEnginesSense;
            }
            ext.ActionsList.Clear();
            ext.ActionsList.Add(new CarAction() { MoveAction = MoveAction.SmartMovement, Duration = 60/*double.Parse(this.timeTxt.Text)*/});
        }


    }


}

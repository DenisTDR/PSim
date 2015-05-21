using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSim
{
    public class EnginesSetting
    {
        public EnginesSetting()
        {
            SetLeft = SetRight = false;
        }

        double _leftEnginesForce, _rightEnginesForce, _maxEngineForce;
        int _leftEnginesSense, _rightEnginesSense;
        public double LeftEnginesForce
        {
            get { return _leftEnginesForce; }
            set { _leftEnginesForce = value;  }
        }
        public int LeftEnginesSense
        {
            get { return _leftEnginesSense; }
            set { _leftEnginesSense = value; }
        }
        public double RightEnginesForce
        {
            get { return _rightEnginesForce; }
            set { _rightEnginesForce = value; }
        }
        public int RightEnginesSense
        {
            get { return _rightEnginesSense; }
            set { _rightEnginesSense = value; }
        }
        public double MaxEngineForce
        {
            get { return _maxEngineForce; }
            set { _maxEngineForce = value; }
        }
        public bool SetLeft { get; set; }
        public bool SetRight { get; set; }

    }
}

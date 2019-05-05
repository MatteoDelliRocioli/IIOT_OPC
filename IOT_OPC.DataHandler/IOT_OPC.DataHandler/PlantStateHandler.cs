namespace IOT_OPC.DataHandler
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Handles plant state change and timing for each state
    /// </summary>
    public class PlantStateHandler
    {
        /// <summary>
        /// the current state of the plant
        /// </summary>
        private PlantState _currentState;
        private DateTime _lastChangeTime;
        Dictionary<PlantState, TimeSpan> _statesDuration;

        /// <summary>
        /// if a new object is called without arguments we aasume the plant is Off
        /// </summary>
        public PlantStateHandler(): this(PlantState.Off)
        {            
        }

        /// <summary>
        /// a new object is called providing a initial state
        /// </summary>
        /// <param name="currentState"></param>
        public PlantStateHandler(PlantState currentState)
        {
            _statesDuration = new Dictionary<PlantState, TimeSpan>();
            _currentState = currentState;
            _lastChangeTime = new DateTime();
            //_statesDuration.Add(PlantState.Off, 0);
            //_statesDuration.Add(PlantState.OnRunning, 0);
            //_statesDuration.Add(PlantState.OnStopped, 0);
        }

        /// <summary>
        /// when a new state is set, the current state duration is updated then
        /// the the lastTimeChange and the new currentState is set 
        /// </summary>
        public PlantState State
        {
            get => _currentState;
            set
            {
                // retrieve, the last duration for the current state
                // and remove the key from the dictionary
                if (_statesDuration.TryGetValue(_currentState, out TimeSpan lastStateDuration))
                {
                    _statesDuration.Remove(_currentState);
                }
                else
                {
                    lastStateDuration = new TimeSpan(0);
                }
                // calculate and update the duration for the current state
                DateTime currentTime = new DateTime();
                TimeSpan currentStateDuration = currentTime - _lastChangeTime;
                // store the value in the dictionary
                _statesDuration.Add(_currentState, lastStateDuration + currentStateDuration);

                // store new current state anc current time
                _lastChangeTime = currentTime;
                _currentState = value;
            }
        }
    }
    public enum PlantState
    {                     // plc internal bit state       On/Off  Running/stopped
        Off = 0x0,        // plant is Off                   0         0
        OnRunning = 0x3,  // plant is On  and Running       1         1
        OnStopped = 0x1   // Plant is On  and Stopped       1         0
     // OffRunning = 0x2  // Plant is Off BUT Running       0         1     Invalid state
    }
}

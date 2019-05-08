namespace IOT_OPC.DataHandler
{
    using IIOT_OPC.Shared.Models;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Handles plant state change and timing for each state
    /// </summary>
    public class PlantStateHandler
    {
        private DateTime _startTime;
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
        public PlantStateHandler(PlantState currentState): this(new DateTime(), currentState)
        {
        }

        public PlantStateHandler(DateTime startTime, PlantState currentState)
        {
            _statesDuration = new Dictionary<PlantState, TimeSpan>();
            _startTime = startTime;
            CurrentState = currentState;
            _statesDuration.Add(PlantState.Off, new TimeSpan(0));
            _statesDuration.Add(PlantState.OnRunning, new TimeSpan(0));
            _statesDuration.Add(PlantState.OnStopped, new TimeSpan(0));
        }

        /// <summary>
        /// when a new state is set, the current state duration is updated then
        /// the the lastTimeChange and the new currentState is set 
        /// </summary>
        public PlantState CurrentState { get; private set; }
        public PlantStateRowData PlantStateRowData
        {
            get
            {
                return new PlantStateRowData()
                {                
                    PlantState = CurrentState,
                    TimeStamp = _startTime
                };

            }
        }
        public void SetCurrentState(DateTime currentTime, PlantState value)
        {
            // retrieve, the last duration for the current state
            // and remove the key from the dictionary
            var lastStateDuration = _statesDuration[CurrentState];            
            // calculate and update the duration for the current state
            TimeSpan currentStateDuration = currentTime - _startTime;

            // ab edit: a scopo di debug _plantState.SetCurrentState  imposta a 0 lo span se è negativo, come accade
            // quando OnMidnight viene chiamata dal timer ogni X secondi anzichè a mezzanotte
            if ( currentStateDuration.CompareTo(new TimeSpan(0))<0)
            {
                Console.WriteLine($"    currentStateDuration: {currentStateDuration}"  );
                currentStateDuration = new TimeSpan(0);
            }
            // store the value in the dictionary
            _statesDuration[CurrentState] = lastStateDuration + currentStateDuration;

            // store new current state and current time
            _startTime = currentTime;
            CurrentState = value;
        }
        public PlantStateDuration PlantStateDuration
        {
            get
            {
                return new PlantStateDuration()
                {
                    
                    // ab edit: è necessario impostare il timeStamp 
                    TimeStamp = _startTime,
                    OffDuration = _statesDuration[PlantState.Off],
                    OnRunningDuration = _statesDuration[PlantState.OnRunning],
                    OnStoppedfDuration = _statesDuration[PlantState.OnStopped]
                };
            }
        }
    }
}

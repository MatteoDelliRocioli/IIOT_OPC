namespace IIOT_OPC
{
    using IIOT_OPC.DataAccess;
    using IIOT_OPC.Shared.Configuration;
    using IIOT_OPC.Shared.Extensions;
    using IIOT_OPC.Shared.Models;
    using IOT_OPC.DataHandler;
    using Kepware.ClientAce.OpcDaClient;
    using System;
    using System.Timers;

    public class DatasHandler
    {
        DaServerMgt daServerMgt = new DaServerMgt();
        ConnectInfo connectInfo = new ConnectInfo();
        PlantStateHandler _plantState; // todo: init this object with PlantState argument
        ConfigBuilder<AppConfig> _appsettings = new ConfigBuilder<AppConfig>(Utils.FileToProjectDirectory("..\\IIOT_OPCconfig.json"));
        AbstractDataAccess<PlantStateRowData> plantStateDataAccess;
        AbstractDataAccess<PlantStateDuration> plantStateDurationDataAccess;
        AbstractDataAccess<DailyProduction> dailyProductionDataAccess;
        Timer timerMidnight = new Timer();

        //prof, 50 euro per un 100, eh dai? :*
        //se vuoi anche uno sboccaponji
        public DatasHandler()
        {
            daServerMgt.DataChanged += DaServerMgt_DataChanged;
            var connectionString = _appsettings.Config.ConnectionString;
            plantStateDataAccess = new PlantStatesDataAccess()
            {
                ConnectionString = connectionString
            };

            plantStateDurationDataAccess = new PlantStateDurationDataAccess()
            {
                ConnectionString = connectionString
            };

            dailyProductionDataAccess = new DailyProductionDataAccess()
            {
                ConnectionString = connectionString
            };
        }

        public void DaServerMgt_DataChanged(int clientSubscription, bool allQualitiesGood, bool noErrors, ItemValueCallback[] itemValues)
        {
            try
            {
                foreach (ItemValueCallback itemValue in itemValues)
                {
                    if (itemValue.ResultID.Succeeded)
                    {
                        Console.WriteLine(itemValue.TimeStamp + ": " + itemValue.ClientHandle + " - " + itemValue.Value + "\n");
                        Enum.TryParse<PlantState>(itemValue.Value.ToString(), out PlantState plant);
                        if (_plantState == null)
                        {
                            _plantState = new PlantStateHandler(itemValue.TimeStamp, plant);
                        }
                        //plantStateDataAccess.Insert(new PlantStateRowData() { PlantState = (PlantState)plant, TimeStamp = itemValue.TimeStamp });

                        OnPlantStateChange(itemValue.TimeStamp, plant);
                    }
                    else
                    {
                        Console.WriteLine("Errore");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DataChanged exception. Reason: {0}", ex);
            }
        }

        public void Connect(/*object sender, EventArgs e*/)
        {
            connectInfo.LocalId = "en";
            connectInfo.KeepAliveTime = 5000;
            connectInfo.RetryAfterConnectionError = true;
            connectInfo.RetryInitialConnection = true;
            //connectInfo.ClientName = "Demo ClientAceC-Sharp DesktopApplication";
            bool connectFailed;
            string url = "opcda://127.0.0.1/Kepware.KEPServerEX.V6/{7BC0CC8E-482C-47CA-ABDC-0FE7F9C6E729}";
            int clientHandle = 1;

            // Connessione al server
            try
            {
                daServerMgt.Connect(url, clientHandle, ref connectInfo, out connectFailed);
                if (connectFailed)
                {
                    Console.WriteLine("Connect failed" + Environment.NewLine);
                }
                else
                {
                    Console.WriteLine("Connected to Server " + url + " Succeeded." + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                // ab edit: aggiungiamo ilcontesto per capire l'errore
                Console.WriteLine($"Connect exeception: {ex.ToString()}");
            }

            InitMidnightTimer();

            InitPlantState();

            // Tag a cui mi voglio sottoscrivere
            ItemIdentifier[] items = new ItemIdentifier[1];
            items[0] = new ItemIdentifier
            {
                ItemName = "its-iot-device.Device1.PlantStatus",
                ClientHandle = "PlantStatus"
            };
            /*items[1] = new ItemIdentifier
            {
                ItemName = "its-iot-device.Device1.PieceCounter",
                ClientHandle = "PieceCounter"
            };*/
            /*items[2] = new ItemIdentifier
            {
                ItemName = "Simulation Examples.Functions.Ramp1",
                ClientHandle = "Ramp1"
            };*/

            int serverSubscription;
            ReturnCode returnCode;

            // Parametri di sottoscrizione agli eventi
            int clientSubscription = 1;
            bool active = true;
            int updateRate = 1000;
            int revisedUpdateRate;
            float deadband = 0;

            try
            {
                // Sottoscrizione agli eventi change data
                returnCode = daServerMgt.Subscribe(clientSubscription,
                active,
                updateRate,
                out revisedUpdateRate,
                deadband,
                ref items,
                out serverSubscription);
            }
            catch (Exception ex)
            {
                // ab edit: aggiungiamo ilcontesto per capire l'errore
                Console.WriteLine($"Connect.subscribe exeception: {ex.ToString()}");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private void InitMidnightTimer()
        {
            //gets the interval
            timerMidnight.Interval = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59) - DateTime.Now).TotalMilliseconds;
            timerMidnight.Interval = 2000;
            timerMidnight.Elapsed += (sender, e) => OnMidnight();
            // ab edit: è necessario avviare il timer
            timerMidnight.Start();
        }

        private void InitPlantState()
        {
            // Aggiorno a mano i valori di tre tag

            int maxAge = 0;
            ItemIdentifier[] OPCItems = new ItemIdentifier[1];
            ItemValue[] OPCItemValues = null;

            OPCItems[0] = new ItemIdentifier
            {
                ItemName = "its-iot-device.Device1.PlantStatus",
                ClientHandle = 1
            };

            //OPCItems[1] = new ItemIdentifier();
            //OPCItems[1].ItemName = "its-iot-device.Device1.PieceCounter";
            //OPCItems[1].ClientHandle = 2;

            //OPCItems[2] = new ItemIdentifier();
            //OPCItems[2].ItemName = "its-iot-device.Device1.DefectedPiecesCounter";
            //OPCItems[2].ClientHandle = 3;

            Console.WriteLine(OPCItems[0].ItemName + "--STATUS \n");
            //Console.WriteLine(OPCItems[1].ItemName + "--PIECE COUNTER \n");
            //Console.WriteLine(OPCItems[2].ItemName + "--DEFECTED PIECES\n");

            try
            {
                daServerMgt.Read(maxAge, ref OPCItems, out OPCItemValues);

                /*if (OPCItems[0].ResultID.Succeeded & OPCItemValues[0].Quality.IsGood)
                {
                    Console.WriteLine(OPCItemValues[0].Value.ToString() + "\n");
                }
                else
                {
                    Console.WriteLine(OPCItems[0].ResultID.Description + "\n");
                }*/

                if (OPCItems[0].ResultID.Succeeded & OPCItemValues[0].Quality.IsGood)
                {
                    Console.WriteLine(OPCItemValues[0].Value.ToString() + "\n");

                    Enum.TryParse<PlantState>(OPCItemValues[0].Value.ToString(), out PlantState plant);
                    if (_plantState == null)
                    {
                        _plantState = new PlantStateHandler(OPCItemValues[0].TimeStamp, plant);
                    }
                    OnPlantStateChange(OPCItemValues[0].TimeStamp, plant);
                }
                else
                {
                    Console.WriteLine(OPCItems[0].ResultID.Description + "\n");
                }

                /*if (OPCItems[2].ResultID.Succeeded & OPCItemValues[1].Quality.IsGood)
                {
                    Console.WriteLine(OPCItemValues[2].Value.ToString() + "\n");
                }
                else
                {
                    Console.WriteLine(OPCItems[2].ResultID.Description + "\n");
                }*/
            }
            catch (Exception ex)
            {
                // ab edit: aggiungiamo ilcontesto per capire l'errore
                Console.WriteLine($"InitPlantState exeception: {ex.ToString()}");
            }

        }

        private void PollingCountPieces()
        {
            // Aggiorno a mano i valori di tre tag

            int maxAge = 0;
            ItemIdentifier[] OPCItems = new ItemIdentifier[2];
            ItemValue[] OPCItemValues = null;

            OPCItems[0] = new ItemIdentifier();
            OPCItems[0].ItemName = "its-iot-device.Device1.PieceCounter";
            OPCItems[0].ClientHandle = 1;

            OPCItems[1] = new ItemIdentifier();
            OPCItems[1].ItemName = "its-iot-device.Device1.DefectedPiecesCounter";
            OPCItems[1].ClientHandle = 2;

            // ab edit: corretto gli indici
            Console.WriteLine(OPCItems[0].ItemName + "--PIECE COUNTER");
            Console.WriteLine(OPCItems[1].ItemName + "--DEFECTED PIECES\n");

            try
            {
                daServerMgt.Read(maxAge, ref OPCItems, out OPCItemValues);
                if (OPCItems[0].ResultID.Succeeded && OPCItemValues[0].Quality.IsGood && OPCItems[1].ResultID.Succeeded && OPCItemValues[1].Quality.IsGood)
                {
                    int num;
                    int numDefected;
                    Console.WriteLine(OPCItemValues[0].Value.ToString() + "\n");
                    Console.WriteLine(OPCItemValues[1].Value.ToString() + "\n");

                    int.TryParse(OPCItemValues[0].Value.ToString(), out num);
                    int.TryParse(OPCItemValues[1].Value.ToString(), out numDefected);

                    DailyProduction dailyProduction = new DailyProduction()
                    {
                        // ab edit: mancava Timestamp
                        TimeStamp =DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59),
                        NumPieces = num,
                        NumPiecesRejected = numDefected
                    };

                    SavePlantDailyProductionToDb(dailyProduction);
                }
                else
                {
                    // ab edit: aggiungiamo ilcontesto per capire l'errore
                    Console.WriteLine($"ResultID 0: {OPCItems[0].ResultID.Description}\n");
                    Console.WriteLine($"ResultID 1: {OPCItems[1].ResultID.Description}\n");
                }
            }
            catch (Exception ex)
            {
                // ab edit: aggiungiamo ilcontesto per capire l'errore
                Console.WriteLine($"PollingCountPieces exeception: {ex.ToString()}");
            }
        }

        private void OnPlantStateChange(DateTime timeStamp, PlantState newState)
        {
            _plantState.SetCurrentState(timeStamp, newState);
            SavePlantStateToDb(_plantState.PlantStateRowData);
        }
        private void OnMidnight()
        {
            //timerMidnight.Interval = 860000399; //resets the timer
            timerMidnight.Interval = 5000; //resets the timer
                                                //the following updated the object
            var now = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59);
            // ab edit: a scopo di debug _plantState.SetCurrentState  imposta a 0 lo span se è negativo, come accade
            // quando OnMidnight viene chiamata dal timer ogni X secondi anzichè a mezzanotte
            _plantState.SetCurrentState(now, _plantState.CurrentState);
            SavePlantStateToDb(_plantState.PlantStateRowData);
            SavePlantDurationToDb(_plantState.PlantStateDuration);
            PollingCountPieces();
            now = now.AddSeconds(1);
            //missing polling operations (read and persistence);
            //get data from durations counters
            //_plantState.PlantStateDuration.OffDuration.
            //plantStateDataAccess
            //save them to the db

            _plantState = new PlantStateHandler(now, _plantState.CurrentState);
            SavePlantStateToDb(_plantState.PlantStateRowData);
        }

        private void SavePlantStateToDb(PlantStateRowData rowData)
        {
            plantStateDataAccess.Insert(rowData);
        }

        private void SavePlantDurationToDb(PlantStateDuration stateDuration)
        {
            plantStateDurationDataAccess.Insert(stateDuration);
        }

        private void SavePlantDailyProductionToDb(DailyProduction dailyProduction)
        {
            dailyProductionDataAccess.Insert(dailyProduction);
        }
    }
}
namespace IIOT_OPC
{
    using IOT_OPC.DataHandler;
    using Kepware.ClientAce.OpcDaClient;
    using System;
    public class DatasHandler
    {
        DaServerMgt daServerMgt = new Kepware.ClientAce.OpcDaClient.DaServerMgt();
        ConnectInfo connectInfo = new Kepware.ClientAce.OpcDaClient.ConnectInfo();
        PlantStateHandler _plantState = new PlantStateHandler(); // todo: init this object with PlantState argument
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
                Console.WriteLine(ex.ToString());
            }

            AggiornaDati();

            // Tag a cui mi voglio sottoscrivere
            ItemIdentifier[] items = new ItemIdentifier[2];
            /*items[0] = new ItemIdentifier
            {
                ItemName = "its-iot-device.Device1.PlantStatus",
                ClientHandle = "PlantStatus"
            };*/
            items[1] = new ItemIdentifier
            {
                ItemName = "its-iot-device.Device1.PieceCounter",
                ClientHandle = "PieceCounter"
            };
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
                Console.WriteLine(ex);
            }

        }

        private void AggiornaDati()
        {
            // Aggiorno a mano i valori di tre tag

            int maxAge = 0;
            Kepware.ClientAce.OpcDaClient.ItemIdentifier[] OPCItems = new Kepware.ClientAce.OpcDaClient.ItemIdentifier[2];
            Kepware.ClientAce.OpcDaClient.ItemValue[] OPCItemValues = null;

            /*OPCItems[0] = new Kepware.ClientAce.OpcDaClient.ItemIdentifier();
            OPCItems[0].ItemName = "its-iot-device.Device1.PlantStatus";
            OPCItems[0].ClientHandle = 1;*/

            OPCItems[1] = new Kepware.ClientAce.OpcDaClient.ItemIdentifier();
            OPCItems[1].ItemName = "its-iot-device.Device1.PieceCounter";
            OPCItems[1].ClientHandle = 1;

            /*OPCItems[2] = new Kepware.ClientAce.OpcDaClient.ItemIdentifier();
            OPCItems[2].ItemName = "Simulation Examples.Functions.Ramp1";
            OPCItems[2].ClientHandle = 3;*/

            /*Console.WriteLine(OPCItems[0].ItemName + "\n");*/
            Console.WriteLine(OPCItems[1].ItemName + "ciao !!!!!!!\n");
            /*Console.WriteLine(OPCItems[2].ItemName + "\n");*/

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

                if (OPCItems[1].ResultID.Succeeded & OPCItemValues[1].Quality.IsGood)
                {
                    Console.WriteLine(OPCItemValues[1].Value.ToString() + "\n");
                }
                else
                {
                    Console.WriteLine(OPCItems[1].ResultID.Description + "\n");
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
                Console.WriteLine(ex.ToString());
            }

        }

        private void OnPlantStateChange (PlantState newState)
        {
            _plantState.SetCurrentState(new DateTime(), newState);
            SavePlantStateToDb(_plantState.PlantStateRowData);
        }
        private void OnMidnight()
        {
            var now =new DateTime( DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23,59,59);
            _plantState.SetCurrentState(now, _plantState.CurrentState);
            SavePlantStateToDb(_plantState.PlantStateRowData);
            now = now.AddSeconds(1);
            _plantState = new PlantStateHandler(now,_plantState.CurrentState);
            SavePlantStateToDb(_plantState.PlantStateRowData);
        }
        private void SavePlantStateToDb(PlantStateRowData rowData)
        {

        }
    }
}

using System;
using System.Threading;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WirecastTallyBridge
{
     class ShotStatus {
         public string name;
         public int id;
         public uint layerIdx;
         public uint shotIdx;
    }
    // the class that is serialized and sent over the wire
    class WirecastStatus {
        public List<ShotStatus> shots = new List<ShotStatus> ();
        public List<int> previews = new List<int>();
        public List<int> lives = new List<int>();
        public Boolean isRecording = false;
        public Boolean isBroadcasting = false;
    }
    class Program
    {
        static TcpServer server = null;
        static void Main(string[] args)
        {
            server = new TcpServer("0.0.0.0", 1234);

            while (true) {
                try {
                    ConnectWirecast();
                }
                catch (Exception e) {
                    if (IsCausedByCOM(e)) {
                        Console.Error.WriteLine("Error in connection to Wirecast: {0}. Trying to reconnect.", e);
                    } else {
                        throw e;
                    }
                }
                Thread.Sleep(2000);
            }
        }

        private static Boolean IsCausedByCOM(Exception e) {
            while(e != null) {
                if(e is System.Runtime.InteropServices.COMException) {
                    return true;
                }
                e = e.InnerException;
            }
            return false;
        }

        private static void ConnectWirecast() {
            WirecastBinding _Wirecast = new WirecastBinding();
            _Wirecast.Initialize();
            
            Boolean valid = _Wirecast.SwitchDocument(1);
            while(! valid) {
                Console.WriteLine("The Wirecast Application exists, but no document has been opened yet.");
                Thread.Sleep(1000);
                valid = _Wirecast.SwitchDocument(1);
            }
            Console.WriteLine("Connected to Wirecast Application.");

            while(true) {
                ProcessWirecast(_Wirecast);
                Thread.Sleep(50);
            }
        }

        private static void ProcessWirecast(WirecastBinding _Wirecast) {
            WirecastStatus status = new WirecastStatus();

            for(uint i = 1; i <=_Wirecast.GetMasterLayerCount(); i++) {
                _Wirecast.SwitchLayer((int)i);
                HashSet<int> knownShotIds = new HashSet<int>();

                const uint minIdx = 2; // first index would be 1, but this is always the empty layer. We are not interested in this.
                for(uint j = minIdx; j <= _Wirecast.GetShotCount(); j++) {
                    ShotStatus shotStatus = new ShotStatus();
                    int shotId = _Wirecast.GetShotIDByIndex((int)j);
                    shotStatus.id = shotId;
                    shotStatus.name = _Wirecast.GetShotNameWithIndex((int)j);
                    shotStatus.layerIdx = i;
                    shotStatus.shotIdx = j;

                    status.shots.Add(shotStatus);
                    knownShotIds.Add(shotId);
                }

                if (_Wirecast.IsLayerVisible()) {
                    int previewId = _Wirecast.GetPreviewShotID();
                    if (knownShotIds.Contains(previewId)) {
                        // only add shotId if it is not the empty layer
                        status.previews.Add(previewId);
                    }
                }

                /* We MUST NOT assume that this shot is not visible because layer is not visible.
                    * 
                    * When hiding a layer in Wirecast, it will not modify the output in any way
                    * unless AutoLive is enabled. So this could be live even though the layer
                    * is hidden.
                    */
                if (_Wirecast.IsLayerVisible() || !_Wirecast.IsAutoLiveOn()) {
                    int programId = _Wirecast.GetLiveShotID();
                    if (knownShotIds.Contains(programId)) {
                        // only add shotId if it is not the empty layer
                        status.lives.Add(programId);
                    }
                }
            }

            status.isRecording = _Wirecast.IsRecording();
            status.isBroadcasting = _Wirecast.IsBroadcasting();

            string output = JsonConvert.SerializeObject(status);
            server.Broadcast(output);
        }
    }
}

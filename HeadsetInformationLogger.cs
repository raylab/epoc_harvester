
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emotiv;
using System.Threading;
using System.Web.Script.Serialization;


namespace Epoc_harvister
{
    public class Epoc_runner
    {
        public static int userID = -1;
        public static int WiFi = 0;
        public static int hsOn = 0;
        internal EmoEngine myEngine;
        public static IntPtr myState;
        public static IntPtr myEvent;
        public static Dictionary<string, object> outputBuffer = new Dictionary<string, object>();
        //public string myPath = System.IO.Path.Combine(System.IO.Path.GetFullPath(@"..\..\"), "Resources");
        public string myPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
        internal static string[] header = {"COUNTER", "INTERPOLATED", "RAW_CQ", "AF3", "F7", "F3", "FC5", "T7", "P7", "O1", "O2", "P8",
                "T8", "FC6", "F4", "F8", "AF4", "GYROX", "GYROY", "TIMESTAMP", "MARKER_HARDWARE", "ES_TIMESTAMP", "FUNC_ID", "FUNC_VALUE", "MARKER", "SYNC_SIGNAL" };


        internal Epoc_runner()
        {
            myEngine = EmoEngine.Instance;
            myEngine.UserAdded += new EmoEngine.UserAddedEventHandler(Engine_UserAdded_Event);
            myEngine.UserRemoved += new EmoEngine.UserRemovedEventHandler(Engine_UserRemoved_Event);
            myEngine.EmoStateUpdated += new EmoEngine.EmoStateUpdatedEventHandler(Engine_EmoStateUpdated);
            myEngine.PerformanceMetricEmoStateUpdated += new EmoEngine.PerformanceMetricEmoStateUpdatedEventHandler(PerformanceMetricEmoStateUpdated);
            myEvent = EdkDll.IEE_EmoEngineEventCreate();
            myState = EdkDll.IEE_EmoStateCreate();
            //myEvent = myEngine.hEvent;
            myEngine.Connect();
            My_Program.myForm.textBox2.Text = $"Started.";
        }

        static void Engine_UserAdded_Event(object sender, EmoEngineEventArgs e)
        {
            userID = (int)e.userId;
            outputBuffer["UserId"] = userID;
            My_Program.myForm.textBox3.Text = $"Add:{userID}";
        }
        static void Engine_UserRemoved_Event(object sender, EmoEngineEventArgs e)
        {
            userID = (int)e.userId;
            My_Program.myForm.textBox3.Text = $"Rem:{userID}";
            userID = -1;
            My_Program.myForm.textBox7.Text = $"{hsOn}";
        }

        static void PerformanceMetricEmoStateUpdated(object sender, EmoStateUpdatedEventArgs e)
        {
            EmoState eState = e.emoState;
            // userID = e.userId;
            string myMsg = "";
            Single timeFromStart = eState.GetTimeFromStart();
            myMsg += $"Time:{timeFromStart},";
            Double rawScore = 0;
            Double minScale = 0;
            Double maxScale = 0;
            Double scaledScore = 0;
            My_Program.myForm.textBox2.Text = $"PMetricUpdate.";
            Dictionary<string, object> myEmoState = new Dictionary<string, object>();

            //Getting Stress
            eState.PerformanceMetricGetStressModelParams(out rawScore, out minScale, out maxScale);
            if (minScale == maxScale)
            {
                scaledScore = 0;
            }
            else
            {
                CaculateScale(rawScore, maxScale, minScale, out scaledScore);
            }
            Dictionary<string, double> Stress = new Dictionary<string, double>();
            Stress.Add("Raw", rawScore);
            Stress.Add("Min", minScale);
            Stress.Add("Max", maxScale);
            Stress.Add("Scaled", scaledScore);
            //myMsg += $"Stress Raw:{rawScore}, Min:{minScale}, Max:{maxScale}, Scaled:{scaledScore}";

            //Getting Engagement
            eState.PerformanceMetricGetEngagementBoredomModelParams(out rawScore, out minScale, out maxScale);
            if (minScale == maxScale)
            {
                scaledScore = 0;
            }
            else
            {
                CaculateScale(rawScore, maxScale, minScale, out scaledScore);
            }
            Dictionary<string, double> Engagement = new Dictionary<string, double>();
            Engagement.Add("Raw", rawScore);
            Engagement.Add("Min", minScale);
            Engagement.Add("Max", maxScale);
            Engagement.Add("Scaled", scaledScore);
            //myMsg += $"Engagement Raw:{rawScore}, Min:{minScale}, Max:{maxScale}, Scaled:{scaledScore}";

            //Getting Relaxation
            eState.PerformanceMetricGetRelaxationModelParams(out rawScore, out minScale, out maxScale);
            if (minScale == maxScale)
            {
                scaledScore = 0;
            }
            else
            {
                CaculateScale(rawScore, maxScale, minScale, out scaledScore);
            }
            Dictionary<string, double> Relaxation = new Dictionary<string, double>();
            Relaxation.Add("Raw", rawScore);
            Relaxation.Add("Min", minScale);
            Relaxation.Add("Max", maxScale);
            Relaxation.Add("Scaled", scaledScore);
            //myMsg += $"Relaxation Raw:{rawScore}, Min:{minScale}, Max:{maxScale}, Scaled:{scaledScore}";

            //Getting Excitement
            eState.PerformanceMetricGetInstantaneousExcitementModelParams(out rawScore, out minScale, out maxScale);
            if (minScale == maxScale)
            {
                scaledScore = 0;
            }
            else
            {
                CaculateScale(rawScore, maxScale, minScale, out scaledScore);
            }
            Dictionary<string, double> Excitement = new Dictionary<string, double>();
            Excitement.Add("Raw", rawScore);
            Excitement.Add("Min", minScale);
            Excitement.Add("Max", maxScale);
            Excitement.Add("Scaled", scaledScore);
            //myMsg += $"Excitement Raw:{rawScore}, Min:{minScale}, Max:{maxScale}, Scaled:{scaledScore}";

            //Getting Interest
            eState.PerformanceMetricGetInterestModelParams(out rawScore, out minScale, out maxScale);
            if (minScale == maxScale)
            {
                scaledScore = 0;
            }
            else
            {
                CaculateScale(rawScore, maxScale, minScale, out scaledScore);
            }
            Dictionary<string, double> Interest = new Dictionary<string, double>();
            Interest.Add("Raw", rawScore);
            Interest.Add("Min", minScale);
            Interest.Add("Max", maxScale);
            Interest.Add("Scaled", scaledScore);
            //myMsg += $"Interest Raw:{rawScore}, Min:{minScale}, Max:{maxScale}, Scaled:{scaledScore}";

            //Output of the Performance Metric 5 parameters here.
            //My_Program.myForm.richTextBox1.Text = myMsg;
            myEmoState["Stress"] = Stress;
            myEmoState["Engagement"] = Engagement;
            myEmoState["Relaxation"] = Relaxation;
            myEmoState["Exitement"] = Excitement;
            myEmoState["Interest"] = Interest;

            //Harvest_EEG_Headset();
            //Harvest_Wavebands_Headset();

            outputBuffer["Emostate"] = myEmoState;

            //string json = new JavaScriptSerializer().Serialize(outputBuffer);
           // if (My_Program.SRV)
           // {
               // My_Program.myServer.Broadcast(json);
           // }
            //outputBuffer.Remove("Emostate");

        }

        static void Engine_EmoStateUpdated(object sender, EmoStateUpdatedEventArgs e)
        {

            EmoState es = e.emoState;

            hsOn = es.GetHeadsetOn();
            My_Program.myForm.textBox2.Text = $"ESUpdate.";


            My_Program.myForm.textBox7.Text = "";
            //If HS is swiched OFF during run, it does not changes, so it only reacts
            //on switching HS ON for the first time After UserAdded Event only. Useless.
            My_Program.myForm.textBox7.Text = $"{hsOn}";
            //Getting events is useless or impossible. None of the below doesn't work as promised!
            // es.GetHandle() returns the State of the event.
            EdkDll.IEE_Event_t thisEvent = EdkDll.IEE_EmoEngineEventGetType(es.GetHandle());

            //My_Program.myForm.textBox7.Text = $"{EdkDll.IEE_EmoEngineEventGetType(myEvent)}";
            //My_Program.myForm.textBox7.Text = $"{EdkDll.IEE_EngineGetNextEvent(myEvent)}";
            //My_Program.myForm.textBox7.Text = $"{EdkDll.IEE_EngineGetNextEvent(es.GetHandle())}";
            //EdkDll.IEE_Event_t eventType = EdkDll.IEE_EmoEngineEventGetType(hEvent);
            //My_Program.myForm.textBox1.Text = $"State:{myState};Now:{thisEvent.GetTypeCode()}";

            float timeFromStart = es.GetTimeFromStart();
            My_Program.myForm.textBox6.Text = $"{timeFromStart}";

            //When USB dongle plugged in, WiFi for some reason jumps to 2, even if HeadSet if OFF.
            //but if HS if off later, this doesn't change.
            if (hsOn != 0)
            {
                EdkDll.IEE_SignalStrength_t signalStrength = es.GetWirelessSignalStatus();
                WiFi = (int)signalStrength;
                //My_Program.myForm.textBox4.Text = $"{WiFi}";
                //The only way to reset headset ON switch if headset is OFF!!!
                if (WiFi == 0)
                {
                    hsOn = 0;
                    My_Program.myForm.textBox7.Text = $"{hsOn}";
                }
                else
                {
                    My_Program.myForm.textBox7.Text = $"{hsOn}";
                    Int32 chargeLevel = 0;
                    Int32 maxChargeLevel = 0;
                    es.GetBatteryChargeLevel(out chargeLevel, out maxChargeLevel);
                    My_Program.myForm.textBox5.Text = $"{chargeLevel}";
                    //string myStr = "";
                    //EdkDll.IEE_InputChannels_t myChannelsList = new EdkDll.IEE_InputChannels_t();
                    //Get EEG Electrode Contact Quality and change the electrode image on the Form.
                    Dictionary<string, int> electrodeQuality = new Dictionary<string, int>();
                    foreach (EdkDll.IEE_InputChannels_t chan in Enum.GetValues(typeof(EdkDll.IEE_InputChannels_t)))
                    {
                        electrodeQuality.Add($"{chan}", (int)es.GetContactQuality((int)chan));
                        foreach (var myElectrode in My_Program.myElectrodes)
                        {
                            // myStr += chan;
                            if (myElectrode.Name == $"{chan}")
                            {
                                string newImage = $@"{My_Program.myEpoc.myPath}";
                                if (myElectrode.Name == "IEE_CHAN_CMS" || myElectrode.Name == "IEE_CHAN_DRL")
                                {
                                    newImage += $@"\relectrode_q{(int)es.GetContactQuality((int)chan)}.png";
                                }
                                else
                                {
                                    newImage += $@"\electrode_q{(int)es.GetContactQuality((int)chan)}.png";
                                }

                                myElectrode.Image = System.Drawing.Image.FromFile(newImage);
                                //myStr += chan + "WILL BE:" + (int)es.GetContactQuality((int)chan) + " | ";
                                //myStr += newImage;
                            }
                        }
                        //myStr += $"{(int)chan}";
                        // myStr += chan + ":" + (int)es.GetContactQuality((int)chan) + " | ";
                    }
                    //Individual electrode signal quality output
                    //EdkDll.IEE_WindowingTypes myWType = new EdkDll.IEE_WindowingTypes();
                    //My_Program.myForm.textBox1.Text = $"{EdkDll.IEE_FFTGetWindowingType((uint)userID, myWType)}";
                    //My_Program.myEpoc.myEngine.IEE_FFTGetWindowingType((uint)userID, myWType);
                    //My_Program.myForm.textBox1.Text = $"{myWType}";
                    outputBuffer["Type"] = "EpocRawBuffer";
                    Harvest_EEG_Headset();
                    Harvest_Wavebands_Headset();
                    outputBuffer["EQ"] = electrodeQuality;

                    //string json = new JavaScriptSerializer().Serialize(outputBuffer);
                    //if (My_Program.SRV)
                    //{
                    //    My_Program.myServer.Broadcast(json);
                   // }
                }
            }
        }

        public void Run()
        {
            My_Program.myForm.textBox4.Text = $"{WiFi}";

            if (userID != -1 && WiFi > 0)
            {
                //If USB dongle is IN and Headset is On.

                //Get the headset settings.
                //uint EPOCmode = 0;
                //uint eegRate = 0;
                //uint eegRes = 0;
                //uint memsRate = 0;
                //uint memsRes = 0;
                //This get Headset Settings that mean nothing, maybe HS must be USB plugged, not via WiFi?
                //myEngine.GetHeadsetSettings((uint)userID, out EPOCmode, out eegRate, out eegRes, out memsRate, out memsRes);
                //reply: EPOC Mode:1;EEG Rate:1;EEG Res:1,Mems Rate:3,Mems Res:2
                //My_Program.myForm.textBox1.Text = $"EPOC Mode:{EPOCmode};EEG Rate:{eegRate};EEG Res:{eegRes},Mems Rate:{memsRate},Mems Res:{memsRes}";

                //Set the EEG data collection.
                bool dataGet = myEngine.IsDataAcquisitionEnabled((uint)userID);
                if (!dataGet)
                {
                    myEngine.DataAcquisitionEnable((uint)userID, true);
                    myEngine.DataSetBufferSizeInSec(1);
                }
            }
            else
            {
                //If USB dongle is in, but Headset is OFF
                My_Program.myForm.textBox7.Text = $"{hsOn}";
                myEngine.DataAcquisitionEnable((uint)userID, false);
                //My_Program.myForm.textBox8.Text = $"{myEngine.IsDataAcquisitionEnabled((uint)userID)}";
            }
            if(My_Program.myForm.textBox9.Text != "")
            {
                outputBuffer["RecordNumber"] = My_Program.myForm.textBox9.Text;
            }
            //Users are the USB dongles, not the headsets.
            //uint userNum = 0;
            //EdkDll.IEE_EngineGetNumUser(out userNum);
            //My_Program.myForm.textBox8.Text = $"{userNum}";
            //This falls over after few runs: 
            //KeyNotFoundException: 'The given key was not present in the dictionary.'
            //My_Program.myForm.textBox7.Text = $"{(EdkDll.IEE_Event_t)EdkDll.IEE_EngineGetNextEvent(myEvent)}";
            //Doesn't change at all.
            //My_Program.myForm.textBox7.Text = $"{EdkDll.IS_GetHeadsetOn(myState)}";
            myEngine.ProcessEvents();
        }

        static void Harvest_Wavebands_Headset()
        {
            //string header = "Theta, Alpha, Low_beta, High_beta, Gamma"; ;
            double[] alpha = new double[1];
            double[] low_beta = new double[1];
            double[] high_beta = new double[1];
            double[] gamma = new double[1];
            double[] theta = new double[1];
            string myMsg = "";

            EdkDll.IEE_DataChannel_t[] channelList = new EdkDll.IEE_DataChannel_t[14] { EdkDll.IEE_DataChannel_t.IED_AF3, EdkDll.IEE_DataChannel_t.IED_F7,
                                                                                        EdkDll.IEE_DataChannel_t.IED_F3, EdkDll.IEE_DataChannel_t.IED_FC5,
                                                                                        EdkDll.IEE_DataChannel_t.IED_T7, EdkDll.IEE_DataChannel_t.IED_P7,
                                                                                        EdkDll.IEE_DataChannel_t.IED_O1, EdkDll.IEE_DataChannel_t.IED_O2,
                                                                                        EdkDll.IEE_DataChannel_t.IED_P8, EdkDll.IEE_DataChannel_t.IED_T8,
                                                                                        EdkDll.IEE_DataChannel_t.IED_FC6, EdkDll.IEE_DataChannel_t.IED_F4,
                                                                                        EdkDll.IEE_DataChannel_t.IED_F8, EdkDll.IEE_DataChannel_t.IED_AF4 };
            Dictionary<string, object>[] bands = new Dictionary<string, object>[14];
            for (int i = 0; i < 14; i++)
            {
                Int32 result = My_Program.myEpoc.myEngine.IEE_GetAverageBandPowers((uint)userID, channelList[i], theta, alpha, low_beta, high_beta, gamma);
                Dictionary<string, double> waveBand = new Dictionary<string, double>();
                if (result == EdkDll.EDK_OK)
                {
                    //Dictionary<string, double> waves = new Dictionary<string, double>();
                    waveBand.Add("Theta", theta[0]);
                    waveBand.Add("Alpha", alpha[0]);
                    waveBand.Add("LBeta", low_beta[0]);
                    waveBand.Add("HBeta", high_beta[0]);
                    waveBand.Add("Gamma", gamma[0]);
                }
                Dictionary<string, object> band = new Dictionary<string, object> () ;
                band.Add($"{channelList[i]}", waveBand);
                bands[i] = band; 
            }

            outputBuffer["Bands"] = bands;// new JavaScriptSerializer().Serialize(bands);
            //myMsg = new JavaScriptSerializer().Serialize(bands);
            //if (My_Program.SRV)
            //{
            //    My_Program.myServer.Broadcast(myMsg);
            //}
            //Output for Average Band Powers
            //My_Program.myForm.richTextBox1.Text = myMsg;
            //outputBuffer["type"] = "epoc_wavebands";
        }

        static void  Harvest_EEG_Headset()
        {
            //Get Raw EEG data for given headset/user, make json out of it.
            Dictionary<EdkDll.IEE_DataChannel_t, double[]> data = My_Program.myEpoc.myEngine.GetData((uint)userID);
            if (data != null)
            {
                int bufferSize = data[EdkDll.IEE_DataChannel_t.IED_TIMESTAMP].Length;
                //Dictionary<string, object> message = new Dictionary<string, object>();
                Dictionary<string, double>[] frames = new Dictionary<string, double>[bufferSize];
                outputBuffer["Frames"] = frames;
                for (int i = 0; i < bufferSize; i++)
                {
                    Dictionary<string, double> frame = new Dictionary<string, double>();
                    int j = 0;
                    foreach (EdkDll.IEE_DataChannel_t channel in data.Keys)
                    {
                        double value = data[channel][i];
                        frame[header[j]] = value;
                        j++;
                    }
                    frames[i] = frame;
                }
                //return frames.ToString();
                //outputBuffer["type"] = "epoc_raw_buffer";
                //Console.WriteLine(My_Program.myPics.ElementAt(0));
                //Console.WriteLine(frame["MARKER"]);
                //Console.WriteLine(frame["MARKER_HARDWARE"]);   //string.Join("; ", frame));
                //Console.WriteLine(My_Program.myPics.Any());
                if (My_Program.myPics.Any() == true)
                {
                    outputBuffer["Stim"] = My_Program.myPics.ElementAt(0);
                    outputBuffer["Stim_time"] = My_Program.stimTime;
                }
                else
                {
                    outputBuffer.Remove("Stim");
                    outputBuffer.Remove("Stim_time");
                }
                string json = new JavaScriptSerializer().Serialize(outputBuffer);
                //Json output of the raw EEG harvister.
                //My_Program.myForm.richTextBox1.Text = json;
                if (My_Program.SRV)
                {
                    //My_Program.myServer.Broadcast(json);
                    My_Program.myWS.SendWS(json);
                }
            }
            My_Program.myForm.textBox2.Text = $"Harvisted.";
            return;
        }

        public void Change_My_FFT_Window(string arg)
        {
            //Nothing changing anyway, so all this useless.
            switch (arg)
            {
                case "HANNING":
                    myEngine.IEE_FFTSetWindowingType((uint)userID, EdkDll.IEE_WindowingTypes.IEE_HANNING);
                    break;
                case "HAMMING":
                    myEngine.IEE_FFTSetWindowingType((uint)userID, EdkDll.IEE_WindowingTypes.IEE_HAMMING);
                    break;
                case "HANN":
                    myEngine.IEE_FFTSetWindowingType((uint)userID, EdkDll.IEE_WindowingTypes.IEE_HANN);
                    break;
                case "BLACKMAN":
                    myEngine.IEE_FFTSetWindowingType((uint)userID, EdkDll.IEE_WindowingTypes.IEE_BLACKMAN);
                    break;
                case "RECTANGLE":
                    myEngine.IEE_FFTSetWindowingType((uint)userID, EdkDll.IEE_WindowingTypes.IEE_RECTANGLE);
                    break;
            }
        }

        static void CaculateScale(Double rawScore, Double maxScale, Double minScale, out Double scaledScore)
        {
            if (rawScore < minScale)
                scaledScore = 0;
            else if (rawScore > maxScale)
                scaledScore = 1;
            else
                scaledScore = (rawScore - minScale) / (maxScale - minScale);
        }

        public void Stop()
        {
            myEngine.Disconnect();
            My_Program.myForm.textBox1.Text = "";
            My_Program.myForm.textBox2.Text = $"Stoped.";
            My_Program.myForm.textBox3.Text = "";
            My_Program.myForm.textBox4.Text = "";
            My_Program.myForm.textBox5.Text = "";
            My_Program.myForm.textBox6.Text = "";
            My_Program.myForm.textBox7.Text = "";
            //My_Program.myForm.textBox8.Text = "";
        }
    }
}

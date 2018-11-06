using System;
using System.Text;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
//*****************************************************************************************
//                           LICENSE INFORMATION
//*****************************************************************************************
//   PCCom.SerialCommunication Version 1.0.0.0
//   Class file for managing serial port communication
//
//   Copyright (C) 2007  
//   Richard L. McCutchen 
//   Email: richard@psychocoder.net
//   Created: 20OCT07
//
//   This program is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or
//   (at your option) any later version.
//
//   This program is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details.
//
//   You should have received a copy of the GNU General Public License
//   along with this program.  If not, see <http://www.gnu.org/licenses/>.
//*****************************************************************************************
namespace eb_20_test2
{
    class CommunicationManager
    {
        byte[] recvBuf = new byte[2048];
        int readCnt;
        StringBuilder strRecv = new StringBuilder();
        delegate void DProcessPacket();
        #region Manager Enums
        /// <summary>
        /// enumeration to hold our transmission types
        /// </summary>
        public enum TransmissionType { Text, Hex }

        /// <summary>
        /// enumeration to hold our message types
        /// </summary>
        public enum MessageType { Incoming, Outgoing, Normal, Warning, Error };
        #endregion

        #region Manager Variables
        //property variables
        private string _baudRate = string.Empty;
        private string _parity = string.Empty;
        private string _stopBits = string.Empty;
        private string _dataBits = string.Empty;
        private string _portName = string.Empty;
        private TransmissionType _transType;
        private RichTextBox _displayWindow;
        public TextBox txt, txtRemote, txtTotal, txtReject, txtTime, txtAmt, txtB20, txtB50, txtB100, txtB500,txtB1000;
        public Boolean RDB_SO_A, RDB_SO_M, RDB_SO_OFF;
        //global manager variables
        private Color[] MessageColor = { Color.Blue, Color.Green, Color.Black, Color.Orange, Color.Red };
        public SerialPort comPort = new SerialPort();
        public int B20 = 20, B50 = 50, B100 = 100, B500 = 500, B1000 = 1000;
        #endregion

        #region Manager Properties
        /// <summary>
        /// Property to hold the BaudRate
        /// of our manager class
        /// </summary>
        public string BaudRate
        {
            get { return _baudRate; }
            set { _baudRate = value; }
        }

        /// <summary>
        /// property to hold the Parity
        /// of our manager class
        /// </summary>
        public string Parity
        {
            get { return _parity; }
            set { _parity = value; }
        }

        /// <summary>
        /// property to hold the StopBits
        /// of our manager class
        /// </summary>
        public string StopBits
        {
            get { return _stopBits; }
            set { _stopBits = value; }
        }

        /// <summary>
        /// property to hold the DataBits
        /// of our manager class
        /// </summary>
        public string DataBits
        {
            get { return _dataBits; }
            set { _dataBits = value; }
        }

        /// <summary>
        /// property to hold the PortName
        /// of our manager class
        /// </summary>
        public string PortName
        {
            get { return _portName; }
            set { _portName = value; }
        }

        /// <summary>
        /// property to hold our TransmissionType
        /// of our manager class
        /// </summary>
        public TransmissionType CurrentTransmissionType
        {
            get { return _transType; }
            set { _transType = value; }
        }

        /// <summary>
        /// property to hold our display window
        /// value
        /// </summary>
        public RichTextBox DisplayWindow
        {
            get { return _displayWindow; }
            set { _displayWindow = value; }
        }
        #endregion

        #region Manager Constructors
        /// <summary>
        /// Constructor to set the properties of our Manager Class
        /// </summary>
        /// <param name="baud">Desired BaudRate</param>
        /// <param name="par">Desired Parity</param>
        /// <param name="sBits">Desired StopBits</param>
        /// <param name="dBits">Desired DataBits</param>
        /// <param name="name">Desired PortName</param>
        public CommunicationManager(string baud, string par, string sBits, string dBits, string name, RichTextBox rtb)
        {
            _baudRate = baud;
            _parity = par;
            _stopBits = sBits;
            _dataBits = dBits;
            _portName = name;
            _displayWindow = rtb;
            //now add an event handler
            comPort.DataReceived += new SerialDataReceivedEventHandler(comPort_DataReceived);
        }

        /// <summary>
        /// Comstructor to set the properties of our
        /// serial port communicator to nothing
        /// </summary>
        public CommunicationManager()
        {
            _baudRate = string.Empty;
            _parity = string.Empty;
            _stopBits = string.Empty;
            _dataBits = string.Empty;
            _portName = "COM1";
            _displayWindow = null;
            //add event handler
            comPort.DataReceived += new SerialDataReceivedEventHandler(comPort_DataReceived);
        }
        #endregion

        #region WriteData
        public void WriteData(string msg)
        {
            switch (CurrentTransmissionType)
            {
                case TransmissionType.Text:
                    //first make sure the port is open
                    //if its not open then open it
                    if (!(comPort.IsOpen == true)) comPort.Open();
                    //send the message to the port
                    comPort.Write(msg);
                    //display the message
                    DisplayData(MessageType.Outgoing, msg + "\n");
                    break;
                case TransmissionType.Hex:
                    try
                    {
                        //convert the message to byte array
                        byte[] newMsg = HexToByte(msg);
                        //send the message to the port
                        comPort.Write(newMsg, 0, newMsg.Length);
                        //convert back to hex and display
                        DisplayData(MessageType.Outgoing, ByteToHex(newMsg) + "\n");
                    }
                    catch (FormatException ex)
                    {
                        //display error message
                        DisplayData(MessageType.Error, ex.Message);
                    }
                    finally
                    {
                        _displayWindow.SelectAll();
                    }
                    break;
                default:
                    //first make sure the port is open
                    //if its not open then open it
                    if (!(comPort.IsOpen == true)) comPort.Open();
                    //send the message to the port
                    comPort.Write(msg);
                    //display the message
                    DisplayData(MessageType.Outgoing, msg + "\n");
                    break;
                    break;
            }
        }
        #endregion

        #region HexToByte
        /// <summary>
        /// method to convert hex string into a byte array
        /// </summary>
        /// <param name="msg">string to convert</param>
        /// <returns>a byte array</returns>
        private byte[] HexToByte(string msg)
        {
            //remove any spaces from the string
            msg = msg.Replace(" ", "");
            //create a byte array the length of the
            //divided by 2 (Hex is 2 characters in length)
            byte[] comBuffer = new byte[msg.Length / 2];
            //loop through the length of the provided string
            for (int i = 0; i < msg.Length; i += 2)
                //convert each set of 2 characters to a byte
                //and add to the array
                comBuffer[i / 2] = (byte)Convert.ToByte(msg.Substring(i, 2), 16);
            //return the array
            return comBuffer;
        }
        #endregion

        #region ByteToHex
        /// <summary>
        /// method to convert a byte array into a hex string
        /// </summary>
        /// <param name="comByte">byte array to convert</param>
        /// <returns>a hex string</returns>
        private string ByteToHex(byte[] comByte)
        {
            //create a new StringBuilder object
            StringBuilder builder = new StringBuilder(comByte.Length * 3);
            //loop through each byte in the array
            foreach (byte data in comByte)
                //convert the byte to a string and add to the stringbuilder
                builder.Append(Convert.ToString(data, 16).PadLeft(2, '0').PadRight(3, ' '));
            //return the converted value
            return builder.ToString().ToUpper();
        }
        #endregion

        #region DisplayData
        /// <summary>
        /// method to display the data to & from the port
        /// on the screen
        /// </summary>
        /// <param name="type">MessageType of the message</param>
        /// <param name="msg">Message to display</param>
        [STAThread]
        private void DisplayData(MessageType type, string msg)
        {
            _displayWindow.Invoke(new EventHandler(delegate
        {
            _displayWindow.SelectedText = string.Empty;
            _displayWindow.SelectionFont = new Font(_displayWindow.SelectionFont, FontStyle.Bold);
            _displayWindow.SelectionColor = MessageColor[(int)type];
            _displayWindow.AppendText(msg);
            _displayWindow.ScrollToCaret();
        }));
        }
        #endregion
        [STAThread]
        private void txtStatus(String msg)
        {
            txt.Invoke(new EventHandler(delegate
            {
                txt.Text = msg;
            }));
        }
        [STAThread]
        private void txtStatusRemote(String msg)
        {
            txtRemote.Invoke(new EventHandler(delegate
            {
                txtRemote.Text = msg;
            }));
        }
        [STAThread]
        private void txtTotalCnt(String msg)
        {
            txtTotal.Invoke(new EventHandler(delegate {txtTotal.Text = msg;}));
        }
        [STAThread]
        private void txtRejectCnt(String msg)
        {
            txtReject.Invoke(new EventHandler(delegate { txtReject.Text = msg; }));
        }
        [STAThread]
        private void txtCurTime(String msg)
        {
            txtTime.Invoke(new EventHandler(delegate { txtTime.Text = msg; }));
        }
        [STAThread]
        private void txtAmount(String msg)
        {
            txtAmt.Invoke(new EventHandler(delegate { txtAmt.Text = msg; }));
        }
        [STAThread]
        private void txtb20(String msg)
        {
            txtB20.Invoke(new EventHandler(delegate { txtB20.Text = msg; }));
        }
        [STAThread]
        private void txtb50(String msg)
        {
            txtB50.Invoke(new EventHandler(delegate { txtB50.Text = msg; }));
        }
        [STAThread]
        private void txtb100(String msg)
        {
            txtB100.Invoke(new EventHandler(delegate { txtB100.Text = msg; }));
        }
        [STAThread]
        private void txtb500(String msg)
        {
            txtB500.Invoke(new EventHandler(delegate { txtB500.Text = msg; }));
        }
        [STAThread]
        private void txtb1000(String msg)
        {
            txtB1000.Invoke(new EventHandler(delegate { txtB1000.Text = msg; }));
        }
        #region OpenPort
        public bool OpenPort()
        {
            try
            {
                //first check if the port is already open
                //if its open then close it
                if (comPort.IsOpen == true) comPort.Close();

                //set the properties of our SerialPort Object
                comPort.BaudRate = int.Parse(_baudRate);    //BaudRate
                comPort.DataBits = int.Parse(_dataBits);    //DataBits
                comPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), _stopBits);    //StopBits
                comPort.Parity = (Parity)Enum.Parse(typeof(Parity), _parity);    //Parity
                comPort.PortName = _portName;   //PortName
                //now open the port
                comPort.Open();
                //display message
                DisplayData(MessageType.Normal, "Port opened at " + DateTime.Now + "\n");
                //return true
                return true;
            }
            catch (Exception ex)
            {
                DisplayData(MessageType.Error, ex.Message);
                return false;
            }
        }
        #endregion

        #region SetParityValues
        public void SetParityValues(object obj)
        {
            foreach (string str in Enum.GetNames(typeof(Parity)))
            {
                ((ComboBox)obj).Items.Add(str);
            }
        }
        #endregion

        #region SetStopBitValues
        public void SetStopBitValues(object obj)
        {
            foreach (string str in Enum.GetNames(typeof(StopBits)))
            {
                ((ComboBox)obj).Items.Add(str);
            }
        }
        #endregion

        #region SetPortNameValues
        public void SetPortNameValues(object obj)
        {

            foreach (string str in SerialPort.GetPortNames())
            {
                ((ComboBox)obj).Items.Add(str);
            }
        }
        #endregion

        #region comPort_DataReceived
        /// <summary>
        /// method that will be called when theres data waiting in the buffer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void comPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            txtStatus("");
            txtRejectCnt("");
            txtTotalCnt("");
            txtCurTime("");
            txtAmount("");
            txtb20("");
            txtb50("");
            txtb100("");
            txtb500("");
            txtb1000("");
            //if (comPort.BytesToRead > 0)
            //{
            //    readCnt = comPort.Read(recvBuf, 0, recvBuf.Length);
            //    for (int i = 0; i < readCnt; i++)
            //    {
            //        CheckPacket(recvBuf[i]);
            //    }
            //}
            //determine the mode the user selected (binary/string)
            switch (CurrentTransmissionType)
            {
                //user chose string
                case TransmissionType.Text:
                    //read data waiting in the buffer
                    string msg = comPort.ReadExisting();
                    string[] words = new string[30];
                    char[] delimiterChars = { ' ', '\n' };
                    words = (msg.ToString()).Split(delimiterChars, System.StringSplitOptions.RemoveEmptyEntries);
                    ////display the data to the user
                    DisplayData(MessageType.Incoming, msg + "\n");
                    if (words.Length == 0) return;
                    if (words[0] == "ack" || (words[words.Length - 1].Length > 4 && words[words.Length - 1].IndexOf("ack", (words[words.Length - 1].Length - 4)) >= 0))
                    {
                        txtStatus("OK");
                        if (RDB_SO_A)
                        {
                            txtStatusRemote("AUTO");
                        }
                        else if (RDB_SO_M)
                        {
                            txtStatusRemote("MANUAL");
                        }
                        else if (RDB_SO_OFF)
                        {
                            txtStatusRemote("OFF");
                        }
                    }
                    else if (words[0] == "cres")
                    {
                        uint iTotalCnt;
                        uint[] iDenomCnt = new uint[20];
                        //uint iTotalCnt;
                        uint iDenomTotal=0;
                        if (words.Length <= 5) return;
                        iTotalCnt = UInt32.Parse(words[3]);
                        for (int i = 0; ((i + 4) < words.Length); i++)
                        {
                            iDenomCnt[i] = UInt32.Parse(words[i + 4]);
                            iDenomTotal += iDenomCnt[i];
                            txtStatus("OK");
                        }
                        if (iDenomTotal != iTotalCnt)
                        {
                            //if (iComState > 0) iNak = 0;
                            comPort.Write("nak\r");
                        }
                        else
                        {
                            //iComState = 4;

                            //LSTB_CRES.Items.Insert(0, DateTime.Now.ToString("HH:mm:ss") + "-" + strRecv.ToString());
                            //TXTB_LD_TIME.Text = DateTime.Now.ToString("HH:mm:ss");
                            //TXTB_LD_STATUS.Text = words[1];
                            txtRejectCnt(words[2]);
                            txtTotalCnt(words[3]);
                            int chk = 0, amt=0;
                            if(int.TryParse(words[4], out chk))
                            {
                                amt += chk * B20;
                                txtb20(words[4]);
                            }
                            if(int.TryParse(words[5], out chk))
                            {
                                amt += chk * B50;
                                txtb50(words[5]);
                            }
                            if (int.TryParse(words[6], out chk))
                            {
                                amt += chk * B100;
                                txtb100(words[6]);
                            }
                            if (int.TryParse(words[7], out chk))
                            {
                                amt += chk * B500;
                                txtb500(words[7]);
                            }
                            if (int.TryParse(words[8], out chk))
                            {
                                amt += chk * B1000;
                                txtb1000(words[8]);
                            }
                            
                            txtAmount(amt.ToString());
                            txtCurTime(DateTime.Now.ToString("hh:MM:ss"));
                            
                        }
                    }
                    break;
                //user chose binary
                case TransmissionType.Hex:
                    //retrieve number of bytes in the buffer
                    int bytes = comPort.BytesToRead;
                    //create a byte array to hold the awaiting data
                    byte[] comBuffer = new byte[bytes];
                    //read the data and store it
                    comPort.Read(comBuffer, 0, bytes);
                    //display the data to the user
                    DisplayData(MessageType.Incoming, ByteToHex(comBuffer) + "\n");
                    break;
                default:
                    //read data waiting in the buffer
                    string str = comPort.ReadExisting();
                    //display the data to the user
                    DisplayData(MessageType.Incoming, str + "\n");
                    break;
            }
        }
        #endregion
        //public void CheckPacket(byte aByte)
        //{
        //    if (aByte == '\r') // Echo string, EB-20 send recieved character
        //    {
        //        strRecv.Remove(0, strRecv.Length);
        //    }
        //    else if (aByte == '\n' || (aByte >= 0x20))
        //    {
        //        strRecv.Append((char)aByte);
        //        if (aByte == '\n')
        //        {
        //            DProcessPacket PPacket = new DProcessPacket(ProcessPacket);
        //            //this.Invoke(PPacket);

        //            strRecv.Remove(0, strRecv.Length);
        //        }
        //    }
        //}
        //void ProcessPacket()
        //{
        //    string[] words = new string[30];
        //    char[] delimiterChars = { ' ', '\n' };

        //    words = (strRecv.ToString()).Split(delimiterChars, System.StringSplitOptions.RemoveEmptyEntries);

        //    if (words.Length == 0) return;

        //    if (words[0] == "ack" || (words[words.Length - 1].Length > 4 && words[words.Length - 1].IndexOf("ack", (words[words.Length - 1].Length - 4)) >= 0))
        //    {
        //        txtStatus("OK");
        //    }
        //    else if (words[0] == "req" && (words[1] == "go"))
        //    {
        //        iComState = 2;
        //        iReSend = 0;
        //        Send_Command();
        //    }
        //    else if (words[0] == "count")
        //    {
        //        iComState = 3;
        //        if (RDB_SO_M.Checked)
        //        {
        //            Recv_Ack();
        //        }
        //    }
        //    else if (words[0] == "cres")
        //    {
        //        iDenomTotal = 0;

        //        iTotalCnt = UInt32.Parse(words[3]);
        //        for (int i = 0; ((i + 4) < words.Length); i++)
        //        {
        //            iDenomCnt[i] = UInt32.Parse(words[i + 4]);
        //            iDenomTotal += iDenomCnt[i];
        //        }

        //        if (iDenomTotal != iTotalCnt)
        //        {
        //            if (iComState > 0) iNak = 0;
        //            Send_Nak();
        //        }
        //        else
        //        {
        //            iComState = 4;

        //            LSTB_CRES.Items.Insert(0, DateTime.Now.ToString("HH:mm:ss") + "-" + strRecv.ToString());
        //            TXTB_LD_TIME.Text = DateTime.Now.ToString("HH:mm:ss");
        //            TXTB_LD_STATUS.Text = words[1];
        //            TXTB_LD_REJECT.Text = words[2];
        //            TXTB_LD_TOTAL.Text = words[3];
        //        }
        //    }
        //    else if (words[0] == "info")
        //    {
        //        TXTB_INFO.Text = words[1];
        //    }
        //    else if (iRecvAck == 1)
        //    {
        //        if (iComState > 0) iNak = 0;
        //        Send_Nak();
        //    }
        //    if (iComState > 0) iNak = 0;
        //}
    }
}

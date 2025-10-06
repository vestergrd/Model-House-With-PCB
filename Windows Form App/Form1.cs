using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace HOUSE_REALLY_FIXED
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string value;
        private string indicator;
        
        private void lightUI_ON (Button light1, Button light2)
        {
            light1.Enabled = false;
            light2.Enabled = true;
        }
        private void lightUI_OFF(Button light1, Button light2)
        {
            light1.Enabled = true;
            light2.Enabled = false;

        }
        private void SendData(string data)
        {
            if (serialPort1.IsOpen)
            {
                try
                {
                    serialPort1.Write(data);
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
            }

        }
        private void Button_Status(bool state)
        {
            //Lights
            button_1turnON.Enabled = state;
            button_1turnOFF.Enabled = false;

            button_2turnON.Enabled = state;
            button_2turnOFF.Enabled = false;

            button_3turnON.Enabled = state;
            button_3turnOFF.Enabled = false;

            button_4turnON.Enabled = state;
            button_4turnOFF.Enabled = false;

            button_turnONALL.Enabled = state;
            button_turnOFFALL.Enabled = state;

            //Fan
            button_FMOff.Enabled = state;
            button_FMLow.Enabled = state;
            button_FMMed.Enabled = state;
            button_FMHigh.Enabled = state;

            button_FAOff.Enabled = false;
            button_FAOn.Enabled = state;

            //Alarm
            button_aON.Enabled = state;
            button_aOFF.Enabled = false;
            alarm_tb.Enabled = false;

            //Window
            button_WAOn.Enabled = state;
            button_WAOff.Enabled = false;
            button_WMClose.Enabled = state;
            button_WMHalf.Enabled = state;
            button_WMFull.Enabled = state;
            

            //Data
            temp1_tb.Enabled = false;
            humi1_tb.Enabled = false;

            temp2_tb.Enabled = false;
            humi2_tb.Enabled = false;

            //Label
            connected_label.Enabled = state;

            //Radiator
            button_RMOff.Enabled = state;
            button_RMLow.Enabled = state;
            button_RMMed.Enabled = state;
            button_RMHigh.Enabled = state;

            button_RAOn.Enabled = state;
            button_RAOff.Enabled = false;

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            button_Open.Enabled = true;
            button_Close.Enabled = false;
            disconnected_label.Enabled = true;
            connection_PB.Value = 0;
            comboBox_baudRate.Text = "9600";
            Button_Status(false);
        }

        private void comboBox_comPort_DropDown(object sender, EventArgs e)
        {
            string[] portLists = SerialPort.GetPortNames();
            comboBox_comPort.Items.Clear();
            comboBox_comPort.Items.AddRange(portLists);
        }

        private void button_Open_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.PortName = comboBox_comPort.Text;
                serialPort1.BaudRate = Convert.ToInt32(comboBox_baudRate.Text);
                serialPort1.Open();
                serialPort1.Write("#"); // clear data første gang man connecter til Arduino
                serialPort1.ReadExisting();
                temp1_tb.Text = "";

                button_Open.Enabled = false;
                button_Close.Enabled = true;
                disconnected_label.Enabled = false;
                connection_PB.Value = 100;
                Button_Status(true);

            }

            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void button_Close_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                try
                {
                    serialPort1.Close();

                    button_Open.Enabled = true;
                    button_Close.Enabled = false;
                    disconnected_label.Enabled = true;
                    connection_PB.Value = 0;
                    Button_Status(false);

                }

                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                try
                {
                    serialPort1.Close();
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
            }
        }

        //LIGHTS
        private void button_1turnON_Click(object sender, EventArgs e)
        {
            SendData("1ON#");
            L1_tb.BackColor = Color.Yellow;

            lightUI_ON(button_1turnON, button_1turnOFF);

        }

        private void button_1turnOFF_Click(object sender, EventArgs e)
        {
            SendData("1OFF#");
            L1_tb.BackColor = Color.White;

            lightUI_OFF(button_1turnON, button_1turnOFF);
        }

        private void button_2turnON_Click(object sender, EventArgs e)
        {
            SendData("2ON#");
            L2_tb.BackColor = Color.Yellow;

            lightUI_ON(button_2turnON, button_2turnOFF);

        }

        private void button_2turnOFF_Click(object sender, EventArgs e)
        {
            SendData("2OFF#");
            L2_tb.BackColor = Color.White;

            lightUI_OFF(button_2turnON, button_2turnOFF);
        }

        private void button_3turnON_Click(object sender, EventArgs e)
        {
            SendData("3ON#");
            L3_tb.BackColor = Color.Yellow;

            lightUI_ON(button_3turnON, button_3turnOFF);

        }

        private void button_3turnOFF_Click(object sender, EventArgs e)
        {
            SendData("3OFF#");
            L3_tb.BackColor = Color.White;

            lightUI_OFF(button_3turnON, button_3turnOFF);
        }

        private void button_4turnON_Click(object sender, EventArgs e)
        {
            SendData("4ON#");
            L4_tb.BackColor = Color.Yellow;

            lightUI_ON(button_4turnON, button_4turnOFF);

        }

        private void button_4turnOFF_Click(object sender, EventArgs e)
        {
            SendData("4OFF#");
            L4_tb.BackColor = Color.White;

            lightUI_OFF(button_4turnON, button_4turnOFF);

        }

        private void button_turnONALL_Click(object sender, EventArgs e)
        {
            SendData("allON#");
            L1_tb.BackColor = Color.Yellow;
            L2_tb.BackColor = Color.Yellow;
            L3_tb.BackColor = Color.Yellow;
            L4_tb.BackColor = Color.Yellow;

            lightUI_ON(button_1turnON, button_1turnOFF);
            lightUI_ON(button_2turnON, button_2turnOFF);
            lightUI_ON(button_3turnON, button_3turnOFF);
            lightUI_ON(button_4turnON, button_4turnOFF);
        }

        private void button_turnOFFALL_Click(object sender, EventArgs e)
        {
            SendData("allOFF#");
            L1_tb.BackColor = Color.White;
            L2_tb.BackColor = Color.White;
            L3_tb.BackColor = Color.White;
            L4_tb.BackColor = Color.White;

            lightUI_OFF(button_1turnON, button_1turnOFF);
            lightUI_OFF(button_2turnON, button_2turnOFF);
            lightUI_OFF(button_3turnON, button_3turnOFF);
            lightUI_OFF(button_4turnON, button_4turnOFF);
        }

        //FAN

        private void button_FMOff_Click(object sender, EventArgs e)
        {
            SendData("FMOff#");
        }

        private void button_FMLow_Click(object sender, EventArgs e)
        {
            SendData("FMLow#");
        }

        private void button_FMMed_Click(object sender, EventArgs e)
        {
            SendData("FMMed#");
        }

        private void button_FMHigh_Click(object sender, EventArgs e)
        {
            SendData("FMHigh#");
        }

        private void button_FAOn_Click(object sender, EventArgs e)
        {
            SendData("FAOn#");
            button_FMOff.Enabled = false;
            button_FMLow.Enabled = false;
            button_FMMed.Enabled = false;
            button_FMHigh.Enabled = false;

            button_FAOn.Enabled = false;
            button_FAOff.Enabled = true;
        }

        private void button_FAOff_Click(object sender, EventArgs e)
        {
            SendData("FAOff#");
            button_FMOff.Enabled = true;
            button_FMLow.Enabled = true;
            button_FMMed.Enabled = true;
            button_FMHigh.Enabled = true;

            button_FAOn.Enabled = true;
            button_FAOff.Enabled = false;
        }

        //ALARM

        private void button_aON_Click(object sender, EventArgs e)
        {
            SendData("aON#");
            button_aON.Enabled = false;
            button_aOFF.Enabled = true;

            alarm_tb.BackColor = Color.Green;
        }

        private void button_aOFF_Click(object sender, EventArgs e)
        {
            SendData("aOFF#");
            button_aON.Enabled = true;
            button_aOFF.Enabled = false;

            alarm_tb.BackColor = Color.Red;
        }

        //WINDOW

        private void button_WMClose_Click(object sender, EventArgs e)
        {
            SendData("WMClose#");
        }

        private void button_WMHalf_Click(object sender, EventArgs e)
        {
            SendData("WMHalf#");
        }

        private void button_WMFull_Click(object sender, EventArgs e)
        {
            SendData("WMFull#");
        }

        private void button_WAOn_Click(object sender, EventArgs e)
        {
            SendData("WAOn#");
            button_WAOn.Enabled = false;
            button_WAOff.Enabled = true;
            button_WMClose.Enabled = false;
            button_WMHalf.Enabled = false;
            button_WMFull.Enabled = false;
        }

        private void button_WAOff_Click(object sender, EventArgs e)
        {
            SendData("WAOff#");
            button_WAOn.Enabled = true;
            button_WAOff.Enabled = false;
            button_WMClose.Enabled = true;
            button_WMHalf.Enabled = true;
            button_WMFull.Enabled = true;
        }

        //DATA FRA ARDUINO
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
           string serialData = serialPort1.ReadLine();

            int divider = serialData.IndexOf('-');
            if(divider == -1)
            {
                return;
            }
           value = serialData.Substring(0, divider);
           indicator = serialData.Substring(divider + 1, 3);

            this.Invoke(new EventHandler(dataHandler_event));

        }

        private void dataHandler_event(object sender, EventArgs e)
        {
            //ALARM
            if (indicator == "A1#")
            {
                alarm_tb.BackColor = Color.Yellow;
            }

            //PROGRESSBARS
            if (indicator == "S1#")
            {
                fanSpeed_PB.Value = Convert.ToInt32(value);
            }
            if (indicator == "W1#")
            {
                windowOpen_PB.Value = Convert.ToInt32(value);
            }

            //DATA FOR ROOM
            if (indicator == "T1#")
            {
                temp1_tb.Text = value + "°C";
            }
            if (indicator == "H1#")
            {
                humi1_tb.Text = value + "%";
            }
            if (indicator == "R1#")
            {
                radiatorHeat_PB.Value = Convert.ToInt32(value);
            }
            //DATA FOR BATHROOM
            if (indicator == "T2#")
            {
                temp2_tb.Text = value + "°C";
            }
            if (indicator == "H2#")
            {
                humi2_tb.Text = value + "%";
            }
        }

        //RADIATOR
        private void button_RMOff_Click(object sender, EventArgs e)
        {
            SendData("RMOff#");
        }

        private void button_RMLow_Click(object sender, EventArgs e)
        {
            SendData("RMLow#");
        }

        private void button_RMMed_Click(object sender, EventArgs e)
        {
            SendData("RMMed#");
        }

        private void button_RMHigh_Click(object sender, EventArgs e)
        {
            SendData("RMHigh#");
        }

        private void button_RAOn_Click(object sender, EventArgs e)
        {
            SendData("RAOn#");

            button_RMOff.Enabled = false;
            button_RMLow.Enabled = false;
            button_RMMed.Enabled = false;
            button_RMHigh.Enabled = false;

            button_RAOn.Enabled = false;
            button_RAOff.Enabled = true;
        }

        private void button_RAOff_Click(object sender, EventArgs e)
        {
            SendData("RAOff#");

            button_RMOff.Enabled = true;
            button_RMLow.Enabled = true;
            button_RMMed.Enabled = true;
            button_RMHigh.Enabled = true;

            button_RAOn.Enabled = true;
            button_RAOff.Enabled = false;
        }
    }
}

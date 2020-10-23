using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace Autoclave
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            try
            {

                string[] ArrayComPortsNames = null;
                int index = -1;
                string ComPortName = null;
                //Com Ports
                ArrayComPortsNames = SerialPort.GetPortNames();
                do
                {
                    index += 1;
                    comboBox1.Items.Add(ArrayComPortsNames[index]);
                } while (!((ArrayComPortsNames[index] == ComPortName) || (index == ArrayComPortsNames.GetUpperBound(0))));
                Array.Sort(ArrayComPortsNames);

                //Baud Rate
                comboBox2.Items.Add(300);
                comboBox2.Items.Add(600);
                comboBox2.Items.Add(1200);
                comboBox2.Items.Add(2400);
                comboBox2.Items.Add(9600);
                comboBox2.Items.Add(14400);
                comboBox2.Items.Add(19200);
                comboBox2.Items.Add(38400);
                comboBox2.Items.Add(57600);
                comboBox2.Items.Add(115200);

                if (serialPort1.IsOpen == false)
                {
                    comboBox1.SelectedIndex = 0;
                    serialPort1.PortName = comboBox1.SelectedItem.ToString();
                    serialPort1.Open();
                    serialPort1.DiscardInBuffer();
                }

            }
            catch
            {
                MessageBox.Show("پورت مورد نظر در دسترس نیست به یکی از  دو دلیل زیر :\n 1-پورت توسط برنامه دیگر در حال استفاده است\n2- پورت در سیستم موجود نمی باشد", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        byte[] inChar = new byte[1];
        byte sizes = 0, checksum = 0;
        //byte[] inData = new byte[32];
        byte[] inputSerial = new byte[128];
        byte[] sendSerial = new byte[64];
        int CounterParser = 0;
        int count_Send_Serial = 20, Flag_recive_data;
        byte[] data_Send = new byte[32];
        int request_Send;
        private void button36_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1.IsOpen == false)
                {
                    serialPort1.PortName = Convert.ToString(comboBox1.Text);
                    serialPort1.BaudRate = Convert.ToInt32(comboBox2.Text);
                    serialPort1.Open();
                    request_Diconnect = 0;
                    button12.Enabled = true;
                    start_btn.Enabled = true;
                    door.Enabled = true;
                    reset_btn.Enabled = true;
                    //serialPort1.DiscardInBuffer();

                }
            }
            catch
            {
                MessageBox.Show("پورت مورد نظر در دسترس نیست به یکی از  دو دلیل زیر :\n 1-پورت توسط برنامه دیگر در حال استفاده است\n2- پورت در سیستم موجود نمی باشد", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        int request_Diconnect;
        private void button35_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1.IsOpen == true)
                {
                    serialPort1.Close();
                    request_Diconnect = 1;
                    button12.Enabled = false;
                    start_btn.Enabled = false;
                    door.Enabled = false;
                    reset_btn.Enabled = false;
                }
            }
            catch
            {
                MessageBox.Show("پورت مورد نظر در دسترس نیست به یکی از  دو دلیل زیر :\n 1-پورت توسط برنامه دیگر در حال استفاده است\n2- پورت در سیستم موجود نمی باشد", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timerReadData_Tick(object sender, EventArgs e)
        {
            Read_Data();
        }
        public void Read_Data()
        {
            try
            {
                if (serialPort1.IsOpen == true)
                {
                    button36.BackColor = Color.Green;
                    button35.BackColor = Color.Transparent;
                }
                else
                {
                    button36.BackColor = Color.Transparent;
                    button35.BackColor = Color.Red;
                    if(request_Diconnect==0)
                        serialPort1.Open();
                }
                if (CounterParser > 0)
                {
                    protocolParser();
                    serialPort1.Close();
                    for (int m = 0; m < 48; m++)
                        inputSerial[m] = 0;
                    Flag_recive_data = 1;
                    request_Send = 0;
                    CounterParser = 0;
                    serialPort1.Open();
                }
            }
            catch
            {

            }
        }
        public void protocolParser()
        {
           
            switch ((byte)inputSerial[2])
            {
                case 1:
                    textBox1.Text = inputSerial[4].ToString();
                    textBox2.Text = inputSerial[5].ToString();
                    textBox3.Text = inputSerial[6].ToString();
                    textBox4.Text = inputSerial[7].ToString();
                    textBox5.Text = inputSerial[8].ToString();
                    textBox6.Text = (inputSerial[9]*.1).ToString("N1");
                    break;
                case 2:
                    textBox19.Text = inputSerial[4].ToString();
                    textBox20.Text = inputSerial[5].ToString();
                    textBox21.Text = inputSerial[6].ToString();
                    textBox22.Text = inputSerial[7].ToString();
                    textBox23.Text = inputSerial[8].ToString();
                    textBox24.Text = (inputSerial[9] * .1).ToString("N1");
                    break;
                case 3:
                    textBox31.Text = inputSerial[4].ToString();
                    textBox32.Text = inputSerial[5].ToString();
                    textBox33.Text = inputSerial[6].ToString();
                    textBox34.Text = inputSerial[7].ToString();
                    textBox35.Text = inputSerial[8].ToString();
                    textBox36.Text = (inputSerial[9] * .1).ToString("N1");
                    break;
                case 4:
                    textBox43.Text = inputSerial[4].ToString();
                    textBox44.Text = inputSerial[5].ToString();
                    textBox45.Text = inputSerial[6].ToString();
                    textBox46.Text = inputSerial[7].ToString();
                    textBox47.Text = inputSerial[8].ToString();
                    textBox48.Text = (inputSerial[9] * .1).ToString("N1");
                    break;
                case 5:
                    textBox55.Text = inputSerial[4].ToString();
                    textBox56.Text = inputSerial[5].ToString();
                    textBox57.Text = inputSerial[6].ToString();
                    textBox58.Text = inputSerial[7].ToString();
                    textBox59.Text = inputSerial[8].ToString();
                    textBox60.Text = (inputSerial[9] * .1).ToString("N1");
                    break;
                case 6:
                    temp_label.Text = adc2temp(inputSerial[5]*256+inputSerial[4]).ToString("n3");
                    time_label.Text = inputSerial[6].ToString();
                    press_label.Text = adc2press(inputSerial[8] * 256+inputSerial[7]).ToString("n3");
                    if (inputSerial[9] == 1)
                        water_label.Text = "Enough";
                    else
                        water_label.Text = "Not Enough";
                    switch (inputSerial[10])
                    {
                        case 1:
                            mode_label.Text = "Quick B";
                            break;
                        case 2:
                            mode_label.Text = "Quick S";
                            break;
                        case 3:
                            mode_label.Text = "Universal PRY";
                            break;
                        case 4:
                            mode_label.Text = "Gentel";
                            break;
                        case 5:
                            mode_label.Text = "Prion";
                            break;
                    }
                    if (inputSerial[11] == 1)
                        status_label.Text = "Ready For Start";
                    else
                        status_label.Text = "Not Ready For Start";
                    if (inputSerial[12] == 1)
                    {
                        door_status_label.Text = "Close";
                        door.Text = "Open Door";
                    }
                    else
                    {
                        door.Text = "Close Door";
                        door_status_label.Text = "Open";
                    }
                    steem1_temp.Text = adc2temp(inputSerial[14] * 256 + inputSerial[13]).ToString("n3");
                    steem2_temp.Text = adc2temp(inputSerial[16] * 256 + inputSerial[15]).ToString("n3");
                    drier_temp.Text  = adc2temp(inputSerial[17]).ToString("n3");
                    Label[] rl = {rl1_status, rl2_status, rl3_status, rl4_status, rl5_status,
                    rl6_status,rl7_status,rl8_status,rl9_status,rl10_status,rl11_status,rl12_status,
                    rl13_status,rl14_status,rl15_status,};
                    int x = inputSerial[19]*256+ inputSerial[18];
                    for (int i = 0; i < 15;i++) {
                        rl[i].Text = (x % 2==1) ? "on" : "off";
                        rl[i].BackColor= (x % 2 == 1) ? Color.Green : Color.Red;
                        x = x / 2;
                    }

                    break;
            }
        }

        double adc2temp(int adc)
        {
            double x = (double)adc;
            return 0.0001991352 * x * x - 0.5090383200 * x + 258.6052355506-int.Parse(temp_offset.Text);
        }
        double adc2press(int adc)
        {
            double x = (double)adc;
            return ((((x / 4095 )* 3300 )/ 120) * 5 )/ 16 - 2.25; //120 ohm res and [-1,4] bar to [4,20] mA
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                Display_Results();
            }
            catch
            {

            }
        }
        int count, level, CounterSerial;
        private void Display_Results()
        {

            inputSerial[count] = (byte)serialPort1.ReadByte();
            if (inputSerial[0] == 0x5A && level == 0)
            {
                count++;
                if (count > 1)
                    level = 1;

            }
            else if (inputSerial[0] != 0x5A && level == 0)
            {
                count = 0;
                level = 0;
            }
            else if (level == 1 && inputSerial[1] == 0xA5)
            {
                count++;
                if (count > 2)
                    level = 2;

            }
            else if (level == 1 && inputSerial[1] != 0xA5)
            {
                count = 0;
                level = 0;
            }
            else if (level == 2)
            {
                sizes = (byte)(inputSerial[3] + 5);
                if (sizes < 48 && sizes > 0)
                {
                    count++;
                    if (count > 3)
                        level = 3;
                }
                else
                {
                    count = 0;
                    level = 0;
                    sizes = 0;
                }
            }
            else if (level == 3)
                count++;
            if (count == sizes && level == 3)
            {
                checksum = 0;
                for (int m = 2; m < (sizes - 1); m++)
                    checksum += inputSerial[m];

                if (checksum == inputSerial[sizes - 1])
                {
                    CounterParser++;
                }
                else
                {
                    count_Send_Serial = 20;
                    for (int m = 0; m < sizes; m++)
                        inputSerial[m] = 0;

                }
                count = 0;
                level = 0;
                sizes = 0;
            }
            //if (serialPort1.IsOpen == true)
            //{
            //    int in_S;
            //    in_S = serialPort1.BytesToRead;
            //    serialPort1.Read(inputSerial, 0, in_S);
            //    if (inputSerial[0] == 0x5A && inputSerial[1] == 0xA5)
            //    {
            //            sizes = (byte)((byte)inputSerial[2] + 4);
            //            checksum = 0;
            //            for (int m = 2; m < (sizes - 1); m++)
            //                checksum += (byte)inputSerial[m];
            //            if (checksum == (byte)inputSerial[sizes - 1])
            //            {
            //                CounterParser++;
            //            }

            //    }
            //}

        }
        void sendProtocol(byte command, byte[] inData, byte inSize)
        {
            int size;
            size = inSize + 5;
            byte[] send = new byte[32];
            send[0] = 0x5A;
            send[1] = 0xA5;
            send[2] = command;
            send[3] = (byte)(inSize);
            send[4 + inSize] = (byte)(send[2] + send[3]);
            for (int m = 0; m < inSize; m++)
            {
                send[4 + m] = inData[m];
                send[4 + inSize] += inData[m];
            }
            if (serialPort1.IsOpen == true)
                serialPort1.Write(send, 0, size);
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            request_Send = 1;
        }

        private void Send_Data_Tick(object sender, EventArgs e)
        {
           /* if (request_Send == 0)
            {
                int c = 0;
                DateTime now = DateTime.Now;
                data_Send[c++] = (byte)(now.Hour);
                data_Send[c++] = (byte)(now.Minute);
                data_Send[c++] = (byte)(now.Second);
                data_Send[c++] = (byte)(now.Year % 100);
                data_Send[c++] = (byte)(now.Month);
                data_Send[c++] = (byte)(now.Day);
                sendProtocol(11, data_Send,6);
            }*/

            if (request_Send!=0 && request_Send <= 6)
            {
                data_Send[0] = (byte)request_Send;
                sendProtocol(0, data_Send, 1);
            }
            else if (request_Send ==21)
            {
                data_Send[0] = byte.Parse(textBox7.Text);
                data_Send[1] = byte.Parse(textBox8.Text);
                data_Send[2] = byte.Parse(textBox9.Text);
                data_Send[3] = byte.Parse(textBox10.Text);
                data_Send[4] = byte.Parse(textBox11.Text);
                data_Send[5] =(byte)(double.Parse(textBox12.Text)*10);
                sendProtocol(2, data_Send, 6);
            }
            else if (request_Send == 31)
            {
                data_Send[0] = byte.Parse(textBox13.Text);
                data_Send[1] = byte.Parse(textBox14.Text);
                data_Send[2] = byte.Parse(textBox15.Text);
                data_Send[3] = byte.Parse(textBox16.Text);
                data_Send[4] = byte.Parse(textBox17.Text);
                data_Send[5] = (byte)(double.Parse(textBox18.Text) * 10);
                sendProtocol(3, data_Send, 6);
            }
            else if (request_Send == 41)
            {
                data_Send[0] = byte.Parse(textBox25.Text);
                data_Send[1] = byte.Parse(textBox26.Text);
                data_Send[2] = byte.Parse(textBox27.Text);
                data_Send[3] = byte.Parse(textBox28.Text);
                data_Send[4] = byte.Parse(textBox29.Text);
                data_Send[5] = (byte)(double.Parse(textBox30.Text) * 10);
                sendProtocol(4, data_Send, 6);
            }
            else if (request_Send == 51)
            {
                data_Send[0] = byte.Parse(textBox37.Text);
                data_Send[1] = byte.Parse(textBox38.Text);
                data_Send[2] = byte.Parse(textBox39.Text);
                data_Send[3] = byte.Parse(textBox40.Text);
                data_Send[4] = byte.Parse(textBox41.Text);
                data_Send[5] = (byte)(double.Parse(textBox42.Text) * 10);
                sendProtocol(5, data_Send, 6);
            }
            else if (request_Send == 61)
            {
                data_Send[0] = byte.Parse(textBox49.Text);
                data_Send[1] = byte.Parse(textBox50.Text);
                data_Send[2] = byte.Parse(textBox51.Text);
                data_Send[3] = byte.Parse(textBox52.Text);
                data_Send[4] = byte.Parse(textBox53.Text);
                data_Send[5] = (byte)(double.Parse(textBox54.Text) * 10);
                sendProtocol(6, data_Send, 6);
            }
            else if (request_Send == 11)
            {
 
                int mode =0;
                if (quick_b_rbtn.Checked)
                    mode = 1;
                else if (quick_s_rbtn.Checked)
                    mode = 2;
                else if (upry_rbtn.Checked)
                    mode = 3;
                else if (gentel_btn.Checked)
                    mode = 4;
                else if (prion_rbtn.Checked)
                    mode = 5;
                else
                {
                    request_Send = 0;
                    MessageBox.Show("please select one mode", "No mode selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (mode!=0) {
                    byte i = 0;
                    data_Send[i++] = (byte)mode;
                    data_Send[i++] = 1;
                    data_Send[i++] = (byte)(int.Parse(steem1_temp_setpoint.Text)%256);
                    data_Send[i++] = (byte)(int.Parse(steem1_temp_setpoint.Text) / 256);
                    data_Send[i++] = (byte)(int.Parse(steem2_temp_setpoint.Text) % 256);
                    data_Send[i++] = (byte)(int.Parse(steem2_temp_setpoint.Text) / 256);
                    data_Send[i++] = byte.Parse(drier_temp_setpoint.Text);
                    data_Send[i++] = byte.Parse(steem_on_time.Text);
                    data_Send[i++] = byte.Parse(steem_off_time.Text);
                    data_Send[i++] = byte.Parse(steem_shooting_time.Text);
                    data_Send[i++] = byte.Parse(temp_offset.Text);
                    data_Send[i++] = byte.Parse(door_opening_time.Text);
                    data_Send[i++] = byte.Parse(door_closing_time.Text);

                    sendProtocol(7, data_Send, i);
                    steem1_temp_setpoint.Enabled = false;
                    steem2_temp_setpoint.Enabled = false;
                    drier_temp_setpoint.Enabled = false;
                    steem_on_time.Enabled = false;
                    steem_off_time.Enabled = false;
                    steem_shooting_time.Enabled = false;
                    temp_offset.Enabled = false;
                    door_opening_time.Enabled = false;
                    door_closing_time.Enabled = false;
                }
            }
            else if (request_Send == 10)
            {
                request_Send = 0;
                if (!water_label.Text.Equals("Enough"))
                    MessageBox.Show("Water isn't Enough", "info",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if(!status_label.Text.Equals("Ready For Start"))
                    MessageBox.Show("Device isn't ready for start", "info",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if(!door.Text.Equals("Open Door"))
                    MessageBox.Show("The door is open.please close it by click on its button", "info",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    {
                        data_Send[0] = (byte)1;
                        sendProtocol(8, data_Send, 1);
                    }
            }
            else if (request_Send == 12)
            {
                request_Send = 0;
                data_Send[0] = (byte)(door.Text.Equals("close Door")?0:1);
                sendProtocol(9, data_Send, 1);
            }
            else if (request_Send == 13)
            {
                request_Send = 0;
                data_Send[0] = (byte)1;
                sendProtocol(10, data_Send, 1);
                steem1_temp_setpoint.Enabled = true;
                steem2_temp_setpoint.Enabled = true;
                drier_temp_setpoint.Enabled = true;
                steem_on_time.Enabled = true;
                steem_off_time.Enabled = true;
                steem_shooting_time.Enabled = true;
                temp_offset.Enabled = true;
                door_opening_time.Enabled = true;
                door_closing_time.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            request_Send = 21;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            request_Send = 2;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            request_Send = 31;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            request_Send = 3;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            request_Send = 4;
        }

        private void start_btn_Click(object sender, EventArgs e)
        {
            request_Send = 10;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            request_Send = 11;
        }

        private void label49_Click(object sender, EventArgs e)
        {

        }

        private void button12_Click_1(object sender, EventArgs e)
        {
            request_Send = 11;
        }

        private void door_Click(object sender, EventArgs e)
        {
            request_Send = 12;
        }

        private void reset_btn_Click(object sender, EventArgs e)
        {
            request_Send = 13;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            request_Send = 51;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            request_Send = 5;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            request_Send = 41;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            request_Send = 61;
        }
    }
}

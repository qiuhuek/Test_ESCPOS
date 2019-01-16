using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using InTheHand.Windows.Forms;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Test_ESCPOS
{
    public partial class Form1 : Form
    {
        BluetoothClient bc;
        BinaryWriter os;


        //Encoding gb2312 = Encoding.GetEncoding("GB2312");
        Encoding gb2312 = Encoding.GetEncoding("GBK");
        Guid bService = new Guid("00001101-0000-1000-8000-00805f9b34fb");

        byte[] LF = { 0x0a };

        //-- 复位打印机
        byte[] RESET = { 0x1b, 0x40 };

        //-- 左对齐
        byte[] ALIGN_LEFT = { 0x1b, 0x61, 0x00 };

        //-- 中间对齐
        byte[] ALIGN_CENTER = { 0x1b, 0x61, 0x01 };

        //-- 右对齐
        byte[] ALIGN_RIGHT = { 0x1b, 0x61, 0x02 };

        //-- Print Mode - Font
        byte[] FONT_A = { 0x1b, 0x21, 0x00 };
        byte[] FONT_B = { 0x1b, 0x21, 0x01 };
        
        //-- 选择加粗模式
        byte[] BOLD = { 0x1b, 0x45, 0x01 };

        //-- 取消加粗模式
        byte[] BOLD_CANCEL = { 0x1b, 0x45, 0x00 };

        //-- 宽高加倍
        byte[] DOUBLE_HEIGHT_WIDTH = { 0x1d, 0x21, 0x11 };

        //-- 宽加倍
        byte[] DOUBLE_WIDTH = { 0x1d, 0x21, 0x10 };

        //-- 高加倍
        byte[] DOUBLE_HEIGHT = { 0x1d, 0x21, 0x01 };

        //-- 字体不放大
        byte[] NORMAL = { 0x1d, 0x21, 0x00 };

        //-- 设置默认行间距
        byte[] LINE_SPACING_DEFAULT = { 0x1b, 0x32 };

        //-- Enable Font
        byte[] FONT_ALL = { 0x1b, 0x4d, 0x30 };
        byte[] FONT_ALL2 = { 0x1b, 0x4d, 0x48 };


        byte[] CODE_PAGE = { 0x1c, 0x7d, 0x26, 0xe4, 0x04 };

        byte[] CHAR_SIZE = { 0x1d, 0x21, 0x20 };


        byte[] PRINTER = { 0x1d, 0x49, 0x00 };

        byte[] LEFT_MARGIN = { 0x1d, 0x4c, 0x00, 0x00 };

        byte[] MOTION_UNIT = { 0x1d, 0x50, 0xcc, 0xcc };

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //BluetoothAddress ba = new BluetoothAddress(new byte[] { 240, 24, 0, 240, 34, 2, 0, 0 });
            BluetoothAddress ba = new BluetoothAddress(new byte[] { 17, 82, 2, 10, 2, 0, 0, 0 });
            //BluetoothEndPoint bep = new BluetoothEndPoint(ba, BluetoothService.McapDataChannelProtocol, 1);
            BluetoothEndPoint bep = new BluetoothEndPoint(ba, BluetoothService.SerialPort, 1);


            bc = new BluetoothClient();

            try
            {
                bc.Connect(bep);

                os = new BinaryWriter(bc.GetStream());
                
                os.Write(RESET);
            }
            catch (IOException ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            os.Close();

            if (bc.Connected)
                bc.Close();

            os.Dispose();
            bc.Dispose();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            String data = "ABCDEF123456";

            try
            {
                byte[] sContent = gb2312.GetBytes(data);

                os.Write(sContent, 0, sContent.Length);
                os.Write(LF);
                os.Write(LF);
                os.Flush();
            }
            catch (IOException ex)
            {
                MessageBox.Show(this, ex.Message);
            }

        }

        private void btnBarcode_Click(object sender, EventArgs e)
        {
            byte[] sContent;

            try
            {
                sContent = gb2312.GetBytes(new char[] { (char)0x1d, 'h', (char)40 });
                os.Write(sContent, 0, sContent.Length);
                os.Flush();

                sContent = gb2312.GetBytes(new char[] { (char)0x1d, 'w', (char)2 });
                os.Write(sContent, 0, sContent.Length);
                os.Flush();

                sContent = gb2312.GetBytes(new char[] { (char)0x1d, 'H', (char)2 });
                os.Write(sContent, 0, sContent.Length);
                os.Flush();

                sContent = gb2312.GetBytes(new char[] { (char)0x1d, 'f', (char)0 });
                os.Write(sContent, 0, sContent.Length);
                os.Flush();

                sContent = gb2312.GetBytes(new char[] { (char)0x1d, 'k', (char)4, '*', 'T', 'E', 'S', 'T', '8', '0', '5', '2','*', (char)0 });
                os.Write(sContent, 0, sContent.Length);
                os.Flush();

                os.Write(LF);
                os.Write(LF);
                os.Flush();
            }
            catch (IOException ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }
    }
}

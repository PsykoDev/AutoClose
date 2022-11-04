using PInvoke;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static AutoClose.Form1;

namespace AutoClose
{
    public partial class Form1 : Form
    {

        [DllImport("user32")]
        public static extern int SetCursorPos(int x, int y);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string strClassName, string strWindowName);


        public const int MOUSEEVENTF_LEFTDOWN = 0x0002; 
        public const int MOUSEEVENTF_LEFTUP = 0x0004;

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;  
            public int Top;   
            public int Right; 
            public int Bottom;
        }

        Rectangle myRect = new Rectangle();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                radioButton2.Checked = false;
            }
            else
            {
                radioButton2.Checked = true;
            }

            this.BeginInvoke((MethodInvoker)delegate {
                AskClose();
            });
        }

        private void AskClose()
        {
            RECT rct;

            if (!GetWindowRect(new HandleRef(this, FindWindow("AutoClose")), out rct))
            {
                MessageBox.Show(FindWindow("AutoClose").ToString());
                return;
            }

            myRect.X = rct.Left;
            myRect.Y = rct.Top;

            DialogResult dialogResult = MessageBox.Show("AutoClose", "Fermer l'app ?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                ObjetsClick(myRect.X + 140, myRect.Y + 120);
                ObjetsClick(myRect.X + 140, myRect.Y + 200);
            }
            else if (dialogResult == DialogResult.No)
            {
                ObjetsClick(myRect.X + 140, myRect.Y + 120);
                ObjetsClick(myRect.X + 140, myRect.Y + 200);
            }

        }

        public static IntPtr FindWindow(string windowName)
        {
            var hWnd = FindWindow(null, windowName);
            return hWnd;
        }

        public void ObjetsClick(int x, int y)
        {
            SetCursorPos(x, y);
            mouse_event(MOUSEEVENTF_LEFTDOWN, x, y, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, x, y, 0, 0);

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (radioButton1.Checked == true)
            {
                return;
            }
            else
            {
                Application.Exit();
            }
        }
    }
}
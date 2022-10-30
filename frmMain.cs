using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace GameDuoiHinhBatChu
{
    public partial class frmMain : Form
    {
        #region Report Thư viện
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        #endregion
        public static extern bool ReleaseCapture();
        private void PlayMusic()
        {

            SoundPlayer player = new SoundPlayer();
            player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + @"MUSIC\mscbackground.wav";
            player.PlayLooping();
        }
       
        
        int Soxu = 0, Sodiem = 0;
        List<Hinh> lstHinh = new List<Hinh>()
        {
            new Hinh(){Address="DUOIHINHBATCHU/"+"1."+"jpg",Answer="NHACCU"},
            new Hinh(){Address="DUOIHINHBATCHU/"+"2."+"jpg",Answer="NOITHAT"},
            new Hinh(){Address="DUOIHINHBATCHU/"+"3."+"jpg",Answer="KINHDO"},
            new Hinh(){Address="DUOIHINHBATCHU/"+"4."+"jpg",Answer="HUNGTHU"},
            new Hinh(){Address="DUOIHINHBATCHU/"+"5."+"jpg",Answer="BAIBAC"},
            new Hinh(){Address="DUOIHINHBATCHU/"+"6."+"jpg",Answer="CHIDIEM"},
            new Hinh(){Address="DUOIHINHBATCHU/"+"7."+"jpg",Answer="LUCLAC"},
            new Hinh(){Address="DUOIHINHBATCHU/"+"8."+"jpg",Answer="NHAHAT"},
            new Hinh(){Address="DUOIHINHBATCHU/"+"9."+"jpg",Answer="BACTINH"},
            new Hinh(){Address="DUOIHINHBATCHU/"+"10."+"jpg",Answer="XICHLO"},
        };
        List<int> lstvitri = new List<int>();
        Random rd = new Random();
        int vitri;
        int checkLose = 0;
        private int counter = 60;
        Timer timer1 = new Timer();
        
        public frmMain()
        {
            InitializeComponent();
        }
        #region TuychinhForm
        private void ptbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ptbMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        //Chỉnh thu phóng của Form
        private void ptbMaxMin_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else if (this.WindowState == FormWindowState.Maximized)
            {

                this.WindowState = FormWindowState.Normal;
            }
        }
        #endregion 

        private void frmMain_Load(object sender, EventArgs e)
        {
            //Bật Nhạc
            PlayMusic();
            timer1.Interval = 1000;
            timer1.Tick += new System.EventHandler(this.timer1_Tick);
            LoadHinh();
        }
        //Random vị trí của ảnh
        private int RandomHinh()
        {
            Random rd = new Random();
            return rd.Next(0, lstHinh.Count);

        }
        //Load hình ảnh lên picturebox
        private void LoadHinh()
        {

            Timer();
            vitri = RandomHinh();
            while (lstvitri.Contains(vitri))
            {
                vitri = RandomHinh();
                if (lstvitri.Count == lstHinh.Count)
                {
                    return;
                }
            }
            //Reset lại lstvitrigoiy
            lstvitrigoiy.Clear();
            string Diachi = lstHinh[vitri].Address;
            ptbpicture.BackgroundImage = Image.FromFile(Diachi);
            //MessageBox.Show(lstHinh[vitri].Answer);
            ptbpicture.BackgroundImageLayout = ImageLayout.Stretch;
            lstvitri.Add(vitri);
            CreateButton(vitri);
            CreateTextBox(vitri);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

           
            if (KtraDapAn(vitri))
            {
                counter = 60;
                Soxu += 10;
                Sodiem += 10;
                lblXu.Text = Soxu.ToString();
                lblSodiem.Text = Sodiem.ToString();
                RemoveButton();
                RemoveTextBox();
                LoadHinh();
            }
            else
            {
                if (checkLose == 3)
                {
                    //Thêm form xác nhận
                    timer1.Stop();
                    MessageBox.Show("Bạn thua cmnr!!!");
                    Application.Exit();
                }
                else
                {
                    foreach (PictureBox item in pnlHeart.Controls)
                    {
                        if (item is PictureBox)
                        {
                            ((PictureBox)item).Dispose();
                            checkLose++;
                            break;
                        }
                    }
                    MessageBox.Show("Bạn đã chọn sai");
                }
            }
        }

        private void CreateButton(int vt)
        {

            List<Button> lstbtn = new List<Button>();
            List<int> indexbtn = new List<int>();
            //Lưu lại answer của Hình
            string str = lstHinh[vt].Answer;
            //Tạo button
            for (int i = 0; i < (lstHinh[vt].Answer.Length) * 2; i++)
            {
                Button btn = new Button();
                btn.Name = "btn" + i.ToString();
                btn.Height = 25;
                btn.Width = 25;
                btn.BackColor = Color.Silver;
                btn.Click += new EventHandler(btnClick);
                btn.Cursor = Cursors.Hand;
                if (i == 0)
                {

                    lstbtn.Add(btn);
                    //btn.Location=new Point(pnlLuaChon.Location.X,(pnlLuaChon.Location.Y+pnlLuaChon.Height)/2);
                    pnlLuaChon.Controls.Add(btn);
                }
                else
                {
                    lstbtn.Add(btn);
                    btn.Location = new Point(lstbtn[i - 1].Location.X + 40, lstbtn[0].Location.Y);
                }
                pnlLuaChon.Controls.Add(btn);
            }
            //Gán chữ cái lên từng button

            for (int i = 0; i < str.Length; i++)
            {

                int vitribtn = Randomvitri(0, (str.Length) * 2);
                while (indexbtn.Contains(vitribtn))
                {
                    vitribtn = Randomvitri(0, (str.Length) * 2);
                }
                foreach (Button item in pnlLuaChon.Controls)
                {
                    string name = "btn" + vitribtn.ToString();
                    if (item.Name == name)
                    {
                        item.Text = str[i].ToString();

                    }

                }
                indexbtn.Add(vitribtn);



            }

            //Random các button còn lại
            List<string> lstbtnthieuchu = new List<string>();
            foreach (Button item in pnlLuaChon.Controls)
            {
                if (item.Text == "")
                {
                    lstbtnthieuchu.Add(item.Name);
                }
            }
            for (int i = 0; i < lstbtnthieuchu.Count; i++)
            {
                foreach (Button item in pnlLuaChon.Controls)
                {
                    char tmp = (char)Randomvitri(65, 91);
                    if (item.Name == lstbtnthieuchu[i])
                    {
                        //item.Text = "";
                        //MessageBox.Show(item.Name + lstbtnthieuchu[i]);                        
                        item.Text = tmp.ToString();
                    }

                }
            }
        }
        //Tạo textbox
        private void CreateTextBox(int vt)
        {
            List<TextBox> lsttxt = new List<TextBox>();
            for (int i = 0; i < lstHinh[vt].Answer.Length; i++)
            {
                TextBox txt = new TextBox();
                txt.Name = "txt" + i.ToString();
                txt.Height = 25;
                txt.Width = 25;
                txt.Click += btnClick;
                txt.TextAlign = HorizontalAlignment.Center;
                txt.ReadOnly = true;
                if (lsttxt.Count == 0)
                {
                    lsttxt.Add(txt);
                }
                else
                {
                    txt.Location = new Point(lsttxt[lsttxt.Count - 1].Location.X + 40, lsttxt[0].Location.Y);
                    lsttxt.Add(txt);
                }
                pnlDapAn.Controls.Add(txt);
            }
        }
        //Hàm xóa Button
        private void RemoveButton()
        {
            pnlLuaChon.Controls.Clear();
        }
        //Hàm xóa textbox
        private void RemoveTextBox()
        {
            pnlDapAn.Controls.Clear();
        }

        //Sự kiện Click
        private void btnClick(object sender, System.EventArgs e)
        {
           
            //Nếu sender là button
            if (sender is Button)
            {
                //MessageBox.Show(((Button)sender).Text);
                int cnt = 0;
                foreach (TextBox txt in pnlDapAn.Controls)
                {
                    if (txt.Text == "")
                    {
                        txt.Text = ((Button)sender).Text;
                        ((Button)sender).Visible = false;
                        break;
                    }
                    else cnt++;
                }
                lstvitrigoiy.Add(cnt);
                cnt = 0;
            }//Nếu là textbox
            else if (sender is TextBox)
            {
                //MessageBox.Show(((TextBox)sender).Text);

                string Nametxt = ((TextBox)sender).Name;
                for (int i = 0; i < lstvitrigoiy.Count; i++)
                {
                    if (int.Parse(Nametxt[Nametxt.Length - 1].ToString()) == lstvitrigoiy[i])
                    {
                        lstvitrigoiy.RemoveAt(i);
                        break;
                    }
                }
                //Sự kiện khi bấm vào textbox sẽ hiện lại button
                foreach (Button btn in pnlLuaChon.Controls)
                {
                    if (btn.Visible == false && btn.Text == ((TextBox)sender).Text)
                    {
                        btn.Visible = true;
                        ((TextBox)sender).Text = "";
                        break;
                    }
                }
            }
        }
        //Check Đáp án
        private bool KtraDapAn(int vt)
        {
            string str = "";
            foreach (TextBox txt in pnlDapAn.Controls)
            {
                str += txt.Text;
            }
            if (str == lstHinh[vt].Answer) return true;
            return false;

        }

        //Radom vị trí chữ cái 
        private int Randomvitri(int st, int ed)
        {

            return rd.Next(st, ed);
        }

        //Sự kiện kéo form mà không có border
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        //Tạo sự kiện cho nút gợi ý
        List<int> lstvitrigoiy = new List<int>();
        private void btnGoiY_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(lstHinh[vitri].Answer);
            if (Soxu >= 25)
            {
                int count = 0, check = 0;
                string Dapangoiy = lstHinh[vitri].Answer;

                int randomvitri = Randomvitri(0, lstHinh[vitri].Answer.Length);


                foreach (TextBox txt in pnlDapAn.Controls)
                {
                    if (txt.Text != "") check++;
                }
                if (check < Dapangoiy.Length)
                {
                    while (lstvitrigoiy.Contains(randomvitri))
                    {
                        randomvitri = Randomvitri(0, lstHinh[vitri].Answer.Length);
                    }
                    foreach (TextBox item in pnlDapAn.Controls)
                    {
                        if (count == randomvitri)
                        {
                            item.Text = Dapangoiy[randomvitri].ToString();
                        }
                        count++;
                    }
                    foreach (Button item in pnlLuaChon.Controls)
                    {
                        if (Dapangoiy[randomvitri].ToString() == item.Text && item.Visible == true)
                        {
                            item.Visible = false;
                            break;
                        }
                    }
                    lstvitrigoiy.Add(randomvitri);
                    Soxu -= 25;
                    lblXu.Text = Soxu.ToString();
                }
                else
                {
                    MessageBox.Show("Không còn ô nào để gợi ý");
                }
            }
            else
            {
                MessageBox.Show("Bạn không đủ xu");
            }


        }

        //Tạo thời gian
        private void Timer()
        {

            timer1.Stop();
            label4.Text = counter.ToString() + " s";
            timer1.Start();

        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            counter--;
            if (counter == -1)
            {
                
                timer1.Stop();
                MessageBox.Show("Bạn thua cmnr");
                Application.Exit();
            }
            label4.Text = counter.ToString() + " s";
        }
    }
    public class Hinh
    {
        public string Address { get; set; }
        public string Answer { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

//Inspiration code: https://www.youtube.com/watch?time_continue=9&v=GJbVEZkeImk

namespace AlarmClock
{
    public partial class Alarm : Form
    {
        System.Timers.Timer timer;
        SoundPlayer player = new SoundPlayer();

        //delegate label threads are used to prevent the system from going into an inconsistent state
        //it's not safe to call a control from a thread without using the invoke method
        delegate void UpdateLable(Label lbl, string value);
        void UpdateDataLable(Label lbl, string value)
        {
            lbl.Text = value;
        }

        public Alarm()
        {
            InitializeComponent();
        }

        private void Alarm_Load(object sender, EventArgs e)
        {
            //start the timer
            timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
        }

        //this function plays music from the time the timer starts till the time the user put
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //calculate how much time has passed since timer began
            DateTime currentTIme = DateTime.Now;
            DateTime userTime = dateTimePicker.Value;
            if(currentTIme.Hour == userTime.Hour && currentTIme.Minute == userTime.Minute && currentTIme.Second == userTime.Second)
            {
                timer.Stop();
                try
                {
                    UpdateLable upd = UpdateDataLable;
                    if(lblStatus.InvokeRequired)
                    {
                        Invoke(upd, lblStatus, "Running...");
                    }
                    
                    player.SoundLocation = @"C:\Windows\media\Alarm01.wav";
                    player.PlayLooping();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            timer.Start();
            lblStatus.Text = "Running...";
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            timer.Stop();
            lblStatus.Text = "Stop";
            player.Stop();
        }
        private void Snooze_Click(object sender, EventArgs e)
        {
            //stop current timer
            timer.Stop();
            player.Stop();
            
            try
            {
                System.Threading.Thread.Sleep(5000);
                timer.Start();

                player.SoundLocation = @"C:\Windows\media\Alarm01.wav";
                player.PlayLooping();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

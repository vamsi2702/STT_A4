using System;
using System.Drawing;
using System.Windows.Forms;

namespace AlarmWinFormsApp  // Changed from AlarmApp to match Designer.cs
{
    // Event arguments for alarm event
    public class AlarmEventArgs : EventArgs
    {
        public DateTime AlarmTime { get; }

        public AlarmEventArgs(DateTime alarmTime)
        {
            AlarmTime = alarmTime;
        }
    }

    // Publisher class
    public class AlarmClock
    {
        // Define delegate and event
        public delegate void AlarmEventHandler(object sender, AlarmEventArgs e);
        public event AlarmEventHandler RaiseAlarm;

        // Fully qualified Timer to avoid ambiguity
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        private TimeSpan targetTime;

        public AlarmClock()
        {
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
        }

        public void SetAlarm(TimeSpan time)
        {
            targetTime = time;
            timer.Start();
        }

        public void StopAlarm()
        {
            timer.Stop();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeSpan currentTime = DateTime.Now.TimeOfDay;

            if (currentTime.Hours == targetTime.Hours &&
                currentTime.Minutes == targetTime.Minutes &&
                currentTime.Seconds == targetTime.Seconds)
            {
                timer.Stop();
                OnRaiseAlarm(new AlarmEventArgs(DateTime.Now));
            }
        }

        // Method to raise the event
        protected virtual void OnRaiseAlarm(AlarmEventArgs e)
        {
            RaiseAlarm?.Invoke(this, e);
        }

        // Method to expose timer for subscribers
        public event EventHandler TimerTick
        {
            add { timer.Tick += value; }
            remove { timer.Tick -= value; }
        }
    }

    public partial class Form1 : Form
    {
        private AlarmClock alarmClock = new AlarmClock();
        private Random rand = new Random();

        public Form1()
        {
            InitializeComponent();
            SetupUI();

            // Subscribe to events
            alarmClock.RaiseAlarm += Ring_Alarm;
            alarmClock.TimerTick += ChangeBackgroundColor;
        }

        private void SetupUI()
        {
            // Create UI components if they don't exist yet
            if (Controls.Count == 0)
            {
                this.Text = "Alarm Application";
                this.Size = new Size(400, 200);

                Label lblTime = new Label
                {
                    Text = "Enter Time (HH:MM:SS):",
                    Location = new Point(20, 20),
                    AutoSize = true
                };
                Controls.Add(lblTime);

                TextBox txtTime = new TextBox
                {
                    Name = "txtTime",
                    Location = new Point(20, 50),
                    Size = new Size(150, 25)
                };
                Controls.Add(txtTime);

                Button btnStart = new Button
                {
                    Name = "btnStart",
                    Text = "Start Alarm",
                    Location = new Point(20, 90),
                    Size = new Size(100, 30)
                };
                btnStart.Click += btnStart_Click;
                Controls.Add(btnStart);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            TextBox txtTime = Controls.Find("txtTime", true)[0] as TextBox;

            if (TimeSpan.TryParse(txtTime.Text, out TimeSpan targetTime))
            {
                alarmClock.SetAlarm(targetTime);
            }
            else
            {
                MessageBox.Show("Invalid time format! Please use HH:MM:SS.");
            }
        }

        private void ChangeBackgroundColor(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
        }

        private void Ring_Alarm(object sender, AlarmEventArgs e)
        {
            MessageBox.Show($" Alarm Time Reached at {e.AlarmTime.ToLongTimeString()}!");
        }

        // Add this method to handle the Form1_Load event referenced in Designer.cs
        private void Form1_Load(object sender, EventArgs e)
        {
            // Any initialization code for when the form loads
        }
    }
}

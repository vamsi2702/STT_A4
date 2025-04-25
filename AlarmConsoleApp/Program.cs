using System;

namespace AlarmConsoleApp
{
    // Publisher class
    public class AlarmClock
    {
        // Define the delegate and event
        public delegate void AlarmEventHandler(object sender, EventArgs e);
        public event AlarmEventHandler RaiseAlarm;

        private readonly TimeSpan targetTime;

        public AlarmClock(TimeSpan targetTime)
        {
            this.targetTime = targetTime;
        }

        // Method to check time
        public void CheckTime()
        {
            Console.WriteLine("Alarm set. Monitoring time...");

            while (true)
            {
                TimeSpan currentTime = DateTime.Now.TimeOfDay;

                if (currentTime.Hours == targetTime.Hours &&
                    currentTime.Minutes == targetTime.Minutes &&
                    currentTime.Seconds == targetTime.Seconds)
                {
                    // Raise the event when the target time is reached
                    OnRaiseAlarm(EventArgs.Empty);
                    break;
                }

                // Wait for a second before checking again
                System.Threading.Thread.Sleep(1000);
            }
        }

        // Method to raise the event
        protected virtual void OnRaiseAlarm(EventArgs e)
        {
            RaiseAlarm?.Invoke(this, e);
        }
    }

    // Subscriber class
    public class AlarmListener
    {
        public void Ring_Alarm(object sender, EventArgs e)
        {
            Console.WriteLine("ALARM! The target time has been reached!");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            TimeSpan targetTime;
            bool validInput = false;

            // Get input from user and validate
            do
            {
                Console.Write("Enter alarm time (HH:MM:SS): ");
                string timeInput = Console.ReadLine();

                if (TimeSpan.TryParse(timeInput, out targetTime))
                {
                    validInput = true;
                }
                else
                {
                    Console.WriteLine("Invalid time format! Please use HH:MM:SS format.");
                }
            } while (!validInput);

            // Create publisher object
            AlarmClock alarmClock = new AlarmClock(targetTime);

            // Create subscriber object
            AlarmListener listener = new AlarmListener();

            // Subscribe to the event
            alarmClock.RaiseAlarm += listener.Ring_Alarm;

            // Start checking the time
            alarmClock.CheckTime();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}

using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stateless;

// state machine example

namespace StateMachine_Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StateMachine<State, Action> m = null;
            Task t1 = new Task(() =>
            {
                m = new StateMachine<State, Action>(State.Idle);
            });
            t1.ContinueWith(t =>
            {
                m.Configure(State.Idle)
                .Permit(Action.Initialize, State.Initializing)
                .OnEntry(() =>
                {

                })
                .OnExit(() =>
                {
                    MessageBox.Show("Exiting Idle...");
                });
            }).ContinueWith(t=>
            {
                m.Configure(State.Initializing)
                .Permit(Action.Start, State.Running)
                .OnEntry(() =>
                {
                    MessageBox.Show("Entering Initializing...");
                })
                .OnExit(() =>
                {
                    MessageBox.Show("Exiting Initializing...");
                });
            }).ContinueWith(t=>
            {
                m.Fire(Action.Initialize);
            });

            t1.Start();
            
        }
    }

    public enum State
    {
        Idle = 1,
        Initializing = 2,
        Running = 3,
        Finished = 4
    }

    public enum Action
    {
        Initialize = 1,
        Start = 2,
        Stop = 3
    }

}

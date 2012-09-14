using System;
using System.Collections.Generic;
using System.Text;
using DSTDControls;

namespace AnyCardGame2
{
    public class Debugger :Panel {

        public static event GrabVariable foobar;

        private List<GrabVariable> foobars = new List<GrabVariable>();
        private List<GrabVariableReturn> vars = new List<GrabVariableReturn>();

        public void AddFoobar(GrabVariable a) {
            foobars.Add(a);
        }
        public void PushVariable(GrabVariableReturn a) {
            vars.Add(a);
        }
        public Debugger() {
            id = "theDebugger";
            Init += new ControlInit(Debugger_Init);
            Load += new ControlLoad(Debugger_Load);
        }

        public override void Initialize() {
            Timer t = new Timer();
            t.Time = 900;
            t.Where = "theDebugger";
            t.id = "debuggerTimer";
            t.OnFire += new TriggeredEvent(t_OnFire);
            this.Children.Add(t);

        }

        void t_OnFire(Control sender) {
            Children.Clear();
            foreach (GrabVariable foobar in foobars) {
                GrabVariableReturn v = foobar(this);
                Children.Add(new Label(v.Name + "&nbsp;"));
                Children.Add(new Label(v.Value.ToString()));
                Children.Add(new BR());
            }
            Children.Add(new BR());
            Children.Add(new BR());
            Children.Add(new BR());
            foreach (GrabVariableReturn v in vars) {
                Children.Add(new Label(v.Name + "&nbsp;"));
                Children.Add(new Label(v.Value.ToString()));
                Children.Add(new BR());
            }

            Timer t = new Timer();
            t.Time = 900;
            t.Where = "theDebugger";
            t.id = "debuggerTimer";
            t.OnFire += new TriggeredEvent(t_OnFire);
            this.Children.Add(t);
        }

        void Debugger_Load(Control sender) {

        }
        void Debugger_Init(Control sender) {
            
        }
 
        public delegate GrabVariableReturn GrabVariable(Control sender);
    }

    public class GrabVariableReturn
    {
        public string Name;
        public object Value;
        
        public GrabVariableReturn(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using DSTDControls;

namespace Control_Namespace {
    public class Calculator :Page {
        public Calculator() {
            id = "theCalculators";

            Init += new ControlInit(Calculator_Init);
            Load += new ControlLoad(Calculator_Load);
        }
        enum sign {
            plus = 0,
            minus = 1,
            multipy = 2,
            equal = 3
        }

        void Calculator_Load(Control sender) {
            if (!Session.ContainsKey("CurrentValue"))
                Session.Add("CurrentValue", 0);
            if (!Session.ContainsKey("CurrentClick"))
                Session.Add("CurrentClick", 0);
            if (!Session.ContainsKey("LastSign"))
                Session.Add("LastSign", null);

        }

        void Calculator_Init(Control sender) {
       

        }


        public void Calc(Control sender) {

            int val = (int)Session["CurrentValue"];
            int val2 = (int)Session["CurrentClick"];

            sign s;

            if (((Button)sender).label == "+")
                s = sign.plus;
            else if (((Button)sender).label == "-")
                s = sign.minus;
            else if (((Button)sender).label == "*")
                s = sign.multipy;
            else if (((Button)sender).label == "=")
                s = sign.equal;
            else
                return;

            if (s!=sign.equal)
                Session["LastSign"] = s;
            switch (s) {
                case sign.plus:
                    val += val2;
                    break;
                case sign.minus:
                    val -= val2;
                    break;
                case sign.multipy:
                    if (((int)Session["CurrentValue"])==0)
                        val2 = 1;
                    val *= val2;

                    break;
                case sign.equal:
                    val2 = int.Parse(((TextBox)GetControlByID("result")).text);
                    if (Session["LastSign"]==null)
                        return;
                    switch ((sign)Session["LastSign"]) {
                        case sign.plus:
                            val += val2;
                            break;
                        case sign.minus:
                            val -= val2;
                            break;
                        case sign.multipy:
                            val *= val2;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    Session["CurrentClick"] = val;
                    Session["LastSign"] = null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Session["CurrentValue"] = val;
            Session["CurrentClick"] = 0;

            ((TextBox)GetControlByID("result")).text = val.ToString();

            if (val==5)
            {
                Request.TransferToPage("Home2");
    
            }
        }

        public void Compute(Control sender) {
            Session["CurrentClick"] = int.Parse(int.Parse(((Button)sender).label)+(((int)Session["CurrentClick"]) == 0 ? "" : Session["CurrentClick"].ToString()));
            ((TextBox)GetControlByID("result")).text = Session["CurrentClick"].ToString();
        }
        public void Clear(Control sender) {
            Session["CurrentClick"] = 0;
            Session["CurrentValue"] = 0;
            ((TextBox)GetControlByID("result")).text = Session["CurrentClick"].ToString();
        }

    }
}
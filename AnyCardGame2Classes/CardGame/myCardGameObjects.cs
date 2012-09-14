using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Xml;
using AnyCardGameClasses;

namespace AnyCardGameClasses
{/*
    public class myCardGameObject
    {

        #region ParseCode
        public void SetVarFromString(string from, object o, Variables v)
        {
            if (from.Contains("."))
                v.SetEvalObjectVariable(from.Substring(0, from.IndexOf('.')), from.Substring(from.IndexOf('.') + 1, from.Length - from.IndexOf('.') - 1), o);
            else if (from.Contains("|"))
                v.SetEvalObjectVariable(from, "", o);
            else
                v.SetVariable(from, o);
        }

        public static Type getTypeFromString(string value)
        {
            switch (value)
            {
                case "bool":
                    return typeof(bool);
                case "int":
                    return typeof(int);
                case "string":
                    return typeof(string);
                case "_Pile":
                    return typeof(_Pile);
                case "Pile":
                    return typeof(_Pile);
                case "_User":
                    return typeof(_User);
                case "User":
                    return typeof(_User);
                case "_Card":
                    return typeof(_Card);
                case "Card":
                    return typeof(_Card);
            }
            return null;
        }
        public object EvaluateVarFromString(string str, Variables v)
        {
            char[] donts = { '+', '-', '/', '*' };
            bool isSingleVar = true;

            foreach (char c in donts)
            {
                if (str.Contains(c.ToString()))
                {
                    isSingleVar = false;
                    break;
                }
            }

            if (isSingleVar)
                return v.EvalVariable(str);

            if (str.Contains(donts[0].ToString()))
            {
                string[] s = str.Split(donts[0]);
                object r = null;
                foreach (string s1 in s)
                {
                    int val;
                    string peice = EvaluateVarFromString(s1, v).ToString();
                    if (int.TryParse(peice, out val))
                    {
                        if (r == null)
                            r = 0;

                        r = (int.Parse(r.ToString()) + (val));
                    }
                    else
                    {
                        if (r == null)
                            r = "";
                        r = r.ToString() + peice;
                    }
                }
                return r;
            }
            if (str.Contains(donts[1].ToString()))
            {
                string[] s = str.Split(donts[1]);
                object r = null;
                foreach (string s1 in s)
                {
                    int val;
                    if (s1.Trim(' ') == "")
                    {
                        r = 0;
                    }
                    else
                    {
                        string peice = EvaluateVarFromString(s1, v).ToString();
                        if (int.TryParse(peice, out val))
                        {
                            if (r == null)
                                r = val;
                            else
                            r = (int.Parse(r.ToString()) - (val));
                        }
                        else
                        {
                            if (r == null)
                                r = peice;
                            else
                                r = r.ToString().Replace(peice, "");
                        }
                    }
                    if (s.Length==1)
                    {
                        r = 0 - (int)r;
                    }
                }
                return r;
            }
            if (str.Contains(donts[2].ToString()))
            {
                string[] s = str.Split(donts[2]);
                int r = 0;

                string peice = EvaluateVarFromString(s[0], v).ToString();
                if (int.TryParse(peice, out r))
                {

                }

                int val;
                peice = EvaluateVarFromString(s[1], v).ToString();
                if (int.TryParse(peice, out val))
                {
                    r = r / val;
                }
                return r;
            }
            if (str.Contains(donts[3].ToString()))
            {
                string[] s = str.Split(donts[3]);
                int r = 0;
                foreach (string s1 in s)
                {
                    int val;
                    string peice = EvaluateVarFromString(s1, v).ToString();
                    if (int.TryParse(peice, out val))
                    {
                        r = r * val;
                    }
                }
                return r;
            }
            return new object();
            // _Variables.GetVariable() 
        }

        public bool evaluateString(string str, Variables v)
        {
            try
            {
                string[] donts = { "==", "<=", ">=", "!=", ">", "<" };

                bool isSingleSide = true;

                foreach (string c in donts)
                {
                    if (str.Contains(c))
                    {
                        isSingleSide = false;
                        break;
                    }
                }
                if (isSingleSide)
                {
                    return false;
                }

                if (str.Contains(donts[0].ToString()))
                {
                    string[] temp = { donts[0] };
                    string[] s = str.Split(temp, StringSplitOptions.RemoveEmptyEntries);
                    object r = null;
                    foreach (string s1 in s)
                    {
                        object peice = EvaluateVarFromString(s1, v);
                        if (r == null)
                            r = peice;
                        else
                        {
                            int r_ = 0;
                            if (int.TryParse(r.ToString(), out r_))
                            {
                                return r_ == int.Parse(peice.ToString());
                            }
                            return r.Equals(peice);
                        }
                    }
                    return false;
                }
                else if (str.Contains(donts[1].ToString()))
                {
                    string[] temp = { donts[1] };
                    string[] s = str.Split(temp, StringSplitOptions.RemoveEmptyEntries);
                    int r = 0;
                    foreach (string s1 in s)
                    {
                        int val;
                        string peice = EvaluateVarFromString(s1, v).ToString();

                        if (int.TryParse(peice, out val))
                        {
                            if (r == 0)
                                r = val;
                            else
                                return r <= val;
                        }
                    }
                    return false;
                }
                else if (str.Contains(donts[2].ToString()))
                {
                    string[] temp = { donts[2] };
                    string[] s = str.Split(temp, StringSplitOptions.RemoveEmptyEntries);
                    int r = 0;
                    foreach (string s1 in s)
                    {
                        int val;
                        string peice = EvaluateVarFromString(s1, v).ToString();

                        if (int.TryParse(peice, out val))
                        {
                            if (r == 0)
                                r = val;
                            else
                                return r >= val;
                        }
                    }
                    return false;
                }
                else if (str.Contains(donts[3].ToString()))
                {
                    string[] temp = { donts[3] };
                    string[] s = str.Split(temp, StringSplitOptions.RemoveEmptyEntries);
                    object r = null;
                    foreach (string s1 in s)
                    {
                        object peice = EvaluateVarFromString(s1, v);
                        if (r == null)
                            r = peice;
                        else
                            return r.ToString() != peice.ToString();
                    }
                    return false;
                }
                else if (str.Contains(donts[4].ToString()))
                {
                    string[] temp = { donts[4] };
                    string[] s = str.Split(temp, StringSplitOptions.RemoveEmptyEntries);
                    int r = 0;
                    foreach (string s1 in s)
                    {
                        int val;
                        string peice = EvaluateVarFromString(s1, v).ToString();

                        if (int.TryParse(peice, out val))
                        {
                            if (r == 0)
                                r = val;
                            else
                                return r > val;
                        }
                    }
                    return false;
                }
                else if (str.Contains(donts[5].ToString()))
                {
                    string[] temp = { donts[5] };
                    string[] s = str.Split(temp, StringSplitOptions.RemoveEmptyEntries);
                    int r = 0;
                    foreach (string s1 in s)
                    {
                        int val;
                        string peice = EvaluateVarFromString(s1, v).ToString();

                        if (int.TryParse(peice, out val))
                        {
                            if (r == 0)
                                r = val;
                            else
                                return r < val;
                        }
                    }
                    return false;
                }

                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        #endregion


        private bool myEvalTop;
        private string myCurrent;
        public bool EvalTop
        {
            get
            {
                return myEvalTop;
            }
            set
            {
                myEvalTop = value;
            }
        }
        public string Current
        {
            get
            {
                return myCurrent;
            }
            set
            {
                myCurrent = value;
            }
        }

        private string myGoBack="";
        public string GoBack
        {
            get
            {
                return myGoBack;
            }
            set
            {
                myGoBack = value;
            }
        }



        private List<myCardGameObject> myChildren = new List<myCardGameObject>();
        public List<myCardGameObject> Children
        {
            get
            {
                return myChildren;
            }
            set
            {
                myChildren = value;
            }
        }

        private myCardGameObject myParent;
        public myCardGameObject Parent
        {
            get
            {
                return myParent;
            }
            set
            {
                myParent = value;
            }
        }
        private Variables myVariables;
        public Variables _Variables
        {
            get
            {
                return myVariables;
            }
            set
            {
                myVariables = value;
            }
        }
        private string myNodeName;
        public string NodeName
        {
            get
            {
                return myNodeName;
            }
            set
            {
                myNodeName = value;
            }
        }

        public string Break="false";


        public myCardGameObject(Variables v, bool e)
        {
            _Variables = v;
            EvalTop = e;
        }

        public myCardGameObject(XmlNode n)
        {
            foreach (XmlAttribute atr in n.Attributes)
            {
         //       if (!GameDebug.GetVariable(atr.Value))
         //           throw new Exception(atr.Value + " Doesnt exist");
            }

            NodeName = n.Name;
            if (n.Attributes["Break"] != null)
                Break = n.Attributes["Break"].Value;
        }

        private static int count = 0;

        public static myCardGameObject XmlToObject(string s)
        {
            myCardGameObject obj = new myCardGameObject();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(s);
            doc.GetElementsByTagName("GameInfo")[0].ParentNode.RemoveChild(doc.GetElementsByTagName("GameInfo")[0]);
            

            return ConsumeXML(doc.ChildNodes[0].ChildNodes[0], "0");
        }


        public static myCardGameObject ConsumeXML(XmlNode node, string current)
        {
            try
            {
                count++;
                int i = 0;

                myCardGameObject obj = (myCardGameObject) myHelper.InvokeConstructorFromChildObjectInNamespace(null, node.Name, "C_CardGameObjects");
                obj.Current = current;
                obj.NodeName = node.Name;
                foreach (XmlNode n in node.ChildNodes)
                {
                    if (n.Name == "Field")
                    {
                        if (n.Attributes["name"].Value == "Elapsed")
                        {
                            foreach (string s in n.Attributes["value"].Value.Split('|'))
                            {
                                long result;
                                if (long.TryParse(s, out result))
                                    obj.Elapsed.Add(new TimeSpan(result));
                            }
                        }
                        else
                            obj.GetType().GetField(n.Attributes["name"].Value).SetValue(obj, n.Attributes["value"].Value);
                    }
                    else
                    {
                        myCardGameObject o = ConsumeXML(n, current + i);
                        o.Parent = obj;
                        obj.Children.Add(o);
                        i++;
                    }
                }
                return obj;
            }
            catch (Exception ee)
            {
                return new myCardGameObject();
            }
        }

        private List<TimeSpan> myElapsed = new List<TimeSpan>();
        public List<TimeSpan> Elapsed
        {
            get
            {
                return myElapsed;
            }
            set { myElapsed = value; }
        }


        public myCardGameObject()
        {

        }

        internal bool EvalChildren()
        {
            Elapsed.Add(new TimeSpan(DateTime.Now.Ticks));
            foreach (myCardGameObject myChild in Children)
            {
                try
                {
                    if (myChild.Break == "true")
                        throw new Exception();
                }
                catch (Exception e)
                {
                }


                if (GoBack != "")
                {
                    if (GoBack == myChild.Current)
                    {
                        myChild.EvalTop = EvalTop;
                        myChild._Variables = _Variables;
                        if (!myChild.Run())
                        {
                            removeGoBack();
                            removEvalTop();

                            Elapsed[Elapsed.Count - 1] = new TimeSpan(DateTime.Now.Ticks - Elapsed[Elapsed.Count - 1].Ticks);
                            return false;
                        }
                        removeGoBack();
                        removEvalTop();
                    }
                    if (GoBack.StartsWith(myChild.Current))
                    {
                        myChild.EvalTop = EvalTop;
                        myChild.GoBack = GoBack;
                        myChild._Variables = _Variables;
                        if (!myChild.Run())
                        {
                            Elapsed[Elapsed.Count - 1] = new TimeSpan(DateTime.Now.Ticks - Elapsed[Elapsed.Count - 1].Ticks);
                            return false;
                        }
                    }
                }
                else
                {
                    myChild.EvalTop = EvalTop;
                    myChild._Variables = _Variables;
                    if (!myChild.Run())
                    {
                        Elapsed[Elapsed.Count - 1] = new TimeSpan(DateTime.Now.Ticks - Elapsed[Elapsed.Count - 1].Ticks);
                        return false;
                    }
                }
            }

            Elapsed[Elapsed.Count - 1] = new TimeSpan(DateTime.Now.Ticks - Elapsed[Elapsed.Count - 1].Ticks);
            return true;
        }

        private void removEvalTop()
        {
            EvalTop = true;
            if (Parent != null)
                Parent.removEvalTop();
        }

        private void removeGoBack()
        {
            GoBack = "";
            if (Parent != null)
                Parent.removeGoBack();
        }

        public virtual string Parse()
        {
            string str = "";

            foreach (myCardGameObject myChild in Children)
            {
                str += myHelper.CallMethodFromChildObjectInNamespace("Parse2", myChild, "C_CardGameObjects");
            }
            return str;
        }

        public string ToString(FieldInfo[] infos, string name)
        {
            string str = "";

            str += "<" + name + ">";
            foreach (FieldInfo myInfo in infos)
            {
                str += "<Field name=\"" + myInfo.Name + "\" value=\"" + myInfo.GetValue(this) + "\" />";
            }
            str += "<Field name=\"Elapsed\" value=\"" + myHelper.ListToString(Elapsed,"|","Ticks") + "\" />";
            str += Parse();
            str += "</" + name + ">";
            return str;
        }

        public virtual bool Run()
        {
            if (Children.Count == 0)
                return false;
            foreach (myCardGameObject myChild in Children)
            {
                myChild.EvalTop = EvalTop;
                if (!myChild.Run())
                    return false;
            }
            return true;
        }
        public void Run2()
        {
            Run();
        }

        public static Type[] AllObjects
        {
            get
            {
                return myHelper.GetAllClasses("C_CardGameObjects").ToArray();
            }
        }

            public myCardGameObject MoveToCurrent(string cur)
        {
            if (Current == cur)
            {
                return this;
            }
            else
            {
                if (Current == null)
                {
                    foreach (myCardGameObject myChild in myChildren)
                    {
                        myCardGameObject o = myChild.MoveToCurrent(cur);
                        if (o != null)
                            return o;
                    }
                }
                else if (cur.StartsWith(Current))
                {
                    foreach (myCardGameObject myChild in myChildren)
                    {
                        myCardGameObject o = myChild.MoveToCurrent(cur);
                        if (o != null)
                            return o;
                    }
                }
            }
            return null;
        }

        public void SetFromCurrent(string cur, myCardGameObject aCurrent)
        {
            if (Current == null)
            {
                int i = 0;
                foreach (myCardGameObject myChild in myChildren)
                {
                    if (cur == myChild.Current)
                        myChildren[i] = aCurrent;
                    else
                        myChild.SetFromCurrent(cur, aCurrent);
                    i++;
                }
            }
            else if (cur.StartsWith(Current))
            {
                int i = 0;
                foreach (myCardGameObject myChild in myChildren)
                {
                    if (cur == myChild.Current)
                        myChildren[i] = aCurrent;
                    else
                        myChild.SetFromCurrent(cur, aCurrent);
                    i++;
                }
            }
        }

        public static List<Type> buildProbableChildren(myCardGameObject p)
        {
            List<Type> l = new List<Type>();
            l.AddRange(((ICardGameObject)p).ProbableChildren());

            for (int myInt = l.Count - 1; myInt > 0; myInt--)
            {
                Type myType = l[myInt];
                if (p.IsParent(myType))
                {
                    l.Remove(myType);
                }
            }


            if (p.Parent != null)
            {
                l.AddRange(buildProbableChildren(p.Parent,p));
            }
            return l;
        }

        public static List<Type> buildProbableChildren(myCardGameObject p,myCardGameObject o)
        {
            List<Type> l = new List<Type>();
            l.AddRange(((ICardGameObject)p).ProbableChildren());

            for (int myInt = l.Count - 1; myInt > 0; myInt--)
            {
                Type myType = l[myInt];
                if (p.IsParent(myType))
                {
                    l.Remove(myType);
                }
            }


            if (p.Parent != null)
            {
                l.AddRange(buildProbableChildren(p.Parent,o));
            }
            return l;
        }

        public bool IsParent(Type t)
        {
            if(GetType() == t)
            {
                return true;
            }
            if (Parent != null)
                return Parent.IsParent(t);
            return false;
        }
    }
    public interface ICardGameObject
    {
        bool Run();
        string ToString();
        string Parse();
        Type[] PossibleChildren();
        Type[] ProbableChildren();
    }
}

namespace C_CardGameObjects
{
    public class C_LoopInputAnswer : myCardGameObject, ICardGameObject
    {
        public Type[] PossibleChildren()
        {
            return AllObjects;
        }
        public Type[] ProbableChildren()
        {
            return AllObjects;
        }

        public string myInput;
        public string Input
        {
            get
            {
                return _Variables.EvalVariableString(myInput, true);
            }
        }

        public C_LoopInputAnswer(XmlNode n)
            : base(n)
        {
            myInput = n.Attributes["Input"].Value;
        }

        public C_LoopInputAnswer()
        {
        }
        public override bool Run()
        {
            try
            {
                List<_User> al = (List<_User>)_Variables["AllUsers"];
                bool next = false;
                for (int bb = 0; bb < al.Count; bb++)
                {
                    _User user = al[bb];
                    // Variables tempVars = (Variables)_Variables.Clone();
                    if (EvalTop)
                    {
                        _Variables.CreateVariable("User", typeof(_User), user);
                        next = true;
                    }
                    else
                    {
                        if (((_User)_Variables["User"]).GameUserID == user.GameUserID)
                            next = true;
                    }
                    if (next)
                    {
                        if (EvalChildren())
                        {
                            return false;
                        }
                        al[bb] = ((_User)_Variables["User"]);
                        EvalTop = true;
                        _Variables["AllUsers"] = al;
                        _Variables.DestroyVariable("User");
                    }
                    next = false;
                }
                return true;
            }
            catch (MyException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new MyException(this, _Variables, e);
            }
        }
        public string Parse2()
        {
            return base.ToString(GetType().GetFields(), this.GetType().Name);
        }
    }

    public class C_CreateMethod : myCardGameObject, ICardGameObject
    {
        public Type[] PossibleChildren()
        {
            return AllObjects;
        }
        public Type[] ProbableChildren()
        {
            return new Type[] { typeof(C_MethodParameter), typeof(C_UpdateMethodParameter) };
        }
        public string myID;
        public string ID
        {
            get
            {
                return _Variables.EvalVariableString(myID, true);
            }
        }
        public C_CreateMethod(XmlNode n)
            : base(n)
        {
            myID = n.Attributes["ID"].Value;
      //      GameDebug.AddVariable(myID);
        }

        public C_CreateMethod()
        {
        }
        public override bool Run()
        {
            try
            {
                return true;
            }
            catch (MyException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new MyException(this, _Variables, e);
            }
        }
        public string Parse2()
        {
            return base.ToString(GetType().GetFields(), this.GetType().Name);
        }
    }/*
    public class C_AddReturnParameter : myCardGameObject, ICardGameObject
    {
        public C_AddReturnParameter(XmlNode n)
            : base(n)
        {
        }

        public bool Run()
        {
        }
    }
    public class C_ReturnParameter : myCardGameObject, ICardGameObject
    {
        public string Pile;
        public C_ReturnParameter(XmlNode n)
            : base(n)
        {
            Pile = n.Attributes["Pile"].Value;
        }

        public bool Run()
        {
        }
    }* /

    public class C_PossibleAnswer : myCardGameObject, ICardGameObject
    {
        public Type[] PossibleChildren()
        {
            return Type.EmptyTypes;
        }

        public Type[] ProbableChildren()
        {
            return Type.EmptyTypes;
        }
        public string myName;
        public string myValue;
        public string Name
        {
            get
            {
                return _Variables.EvalVariableString(myName, true);
            }
        }
        public string Value
        {
            get
            {
                return _Variables.EvalVariableString(myValue, true);
            }
        }
        public C_PossibleAnswer(XmlNode n)
            : base(n)
        {
            myName = n.Attributes["Name"].Value;
            myValue = n.Attributes["Value"].Value;
        }

        public C_PossibleAnswer()
        {
        }
        public override bool Run()
        {
            try
            {
                if (EvalTop)
                {
                    myPossibleAnswer posAns = new myPossibleAnswer();
                    posAns.PossibleAnswerContent = (Name);
                    if (_Variables["_UserID"].ToString() != "_UserID")
                        posAns.PossibleAnswerValue = (Value) + "|" + _Variables["_UserID"];
                    else
                        posAns.PossibleAnswerValue = (Value);
                    posAns.InsertData();
                    List<_PossibleAnswer> lst = ((List<_PossibleAnswer>)_Variables.GetVariable("PossibleAnswers"));
                    lst.Add(new _PossibleAnswer(posAns.PossibleAnswerID));
                    _Variables.SetVariable("PossibleAnswers", lst);
                }
                if (!EvalChildren())
                {
                    return false;
                }
                return true;
            }
            catch (MyException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new MyException(this, _Variables, e);
            }
        }
        public string Parse2()
        {
            return base.ToString(GetType().GetFields(), this.GetType().Name);
        }
    }
    public class C_CheckWinner : myCardGameObject, ICardGameObject
    {
        public Type[] PossibleChildren()
        {
            return AllObjects;
        }

        public Type[] ProbableChildren()
        {
            return AllObjects;
        }
        public string myID;
        public string ID
        {
            get
            {
                return _Variables.EvalVariableString(myID, true);
            }
        }
        public C_CheckWinner(XmlNode n)
            : base(n)
        {
            myID = n.Attributes["ID"].Value;
        }
        public C_CheckWinner()
        {
        }

        public override bool Run()
        {
            try
            {
                if (EvalTop)
                {
                }
                //   if (!EvaluateNodes(((XmlNode)_Variables.GetVariable("WinType" +ID)).ChildNodes, _Variables, Current))

                //    return false;


                return true;
            }
            catch (MyException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new MyException(this, _Variables, e);
            }
        }
        public string Parse2()
        {
            return base.ToString(GetType().GetFields(), this.GetType().Name);
        }
    }
    public class C_ShowInformation : myCardGameObject, ICardGameObject
    {

        public Type[] PossibleChildren()
        {
            return Type.EmptyTypes;
        }
        public Type[] ProbableChildren()
        {
            return Type.EmptyTypes;
        }
        public string myMessage;
        public string myUserID;
        public string Message
        {
            get
            {
                return _Variables.EvalVariableString(myMessage, true);
            }
        }
        public string UserID
        {
            get
            {
                return _Variables.EvalVariableString(myUserID, true);
            }
        }
        public C_ShowInformation(XmlNode n)
            : base(n)
        {
            myUserID = n.Attributes["UserID"].Value;
            myMessage = n.Attributes["Message"].Value;
        }

        public C_ShowInformation()
        {
        }
        public override bool Run()
        {
            try
            {

                if (EvalTop)
                {
                    myChatLine line = new myChatLine();
                    line.GameUserID = -1;
                    line.GameRoomID = (int)_Variables["GameID"];
                    line.TimePosted = DateTime.Now;
                    line.LineContent = "/sys " + (new myGameUser((int)EvaluateVarFromString(UserID, _Variables))).GameUserName + " " + Message;
                    line.InsertData();
                }
                return true;
            }
            catch (MyException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new MyException(this, _Variables, e);
            }
        }
        public string Parse2()
        {
            return base.ToString(GetType().GetFields(), this.GetType().Name);
        }
    }
    public class C_ShowAllInformation : myCardGameObject, ICardGameObject
    {
        public Type[] PossibleChildren()
        {
            return Type.EmptyTypes;
        }
        public Type[] ProbableChildren()
        {
            return Type.EmptyTypes;
        }
        public string myMessage;
        public string Message
        {
            get
            {
                return _Variables.EvalVariableString(myMessage, true);
            }
        }
        public C_ShowAllInformation(XmlNode n)
            : base(n)
        {
            myMessage = n.Attributes["Message"].Value;
        }

        public C_ShowAllInformation()
        {
        }
        public override bool Run()
        {
            try
            {
                if (EvalTop)
                {
                    myChatLine line = new myChatLine();
                    line.GameUserID = -1;
                    line.GameRoomID = (int)_Variables["GameID"];
                    line.TimePosted = DateTime.Now;
                    line.LineContent = (Message);
                    line.InsertData();
                }
                return true;
            }
            catch (MyException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new MyException(this, _Variables, e);
            }
        }
        public string Parse2()
        {
            return base.ToString(GetType().GetFields(), this.GetType().Name);
        }
    }



    public class C_Evaluate : myCardGameObject, ICardGameObject
    {
        public Type[] PossibleChildren()
        {
            return new Type[] { typeof(C_isFalse), typeof(C_isTrue) };
        }
        public Type[] ProbableChildren()
        {
            return new Type[] { typeof(C_isFalse), typeof(C_isTrue) };
        }
        public string myExpression;
        public string Expression
        {
            get
            {
                return _Variables.EvalVariableString(myExpression, true);
            }
        }
        public C_Evaluate(XmlNode n)
            : base(n)
        {
            myExpression = n.Attributes["Expression"].Value;
        }
        public C_Evaluate()
        {
        }

        public override bool Run()
        {
            try
            {
                if (EvalTop)
                {
                    _Variables.CreateVariable("EvaluatedExpression" + Current, typeof(bool), evaluateString((Expression), _Variables));
                }
                if (!EvalChildren())
                {
                    return false;
                }
                return true;
            }
            catch (MyException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new MyException(this, _Variables, e);
            }
        }
        public string Parse2()
        {
            return base.ToString(GetType().GetFields(), this.GetType().Name);
        }
    }
    public class C_isTrue : myCardGameObject, ICardGameObject
    {
        public Type[] PossibleChildren()
        {
            return AllObjects;
        }
        public Type[] ProbableChildren()
        {
            return AllObjects;
        }
        public C_isTrue(XmlNode n)
            : base(n)
        {
        }
        public C_isTrue()
        {
        }

        public override bool Run()
        {
            try
            {
                if (EvalTop)
                {
                    if (_Variables["EvaluatedExpression" + myHelper.RemoveLast(Current, 1)].ToString() == "EvaluatedExpression" + myHelper.RemoveLast(Current, 1))
                    {
                        if ((bool)_Variables.GetVariable("EvaluatedInPile" + myHelper.RemoveLast(Current, 1)))
                            if (!EvalChildren())
                            {
                                return false;
                            }
                    }
                    else if ((bool)_Variables.GetVariable("EvaluatedExpression" + myHelper.RemoveLast(Current, 1)))
                        if (!EvalChildren())
                        {
                            return false;
                        }
                }
                else
                {
                    if (!EvalChildren())
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (MyException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new MyException(this, _Variables, e);
            }
        }

        public string Parse2()
        {
            return base.ToString(GetType().GetFields(), this.GetType().Name);
        }
    }
    public class C_Bet : myCardGameObject, ICardGameObject
    {
        public Type[] PossibleChildren()
        {
            return Type.EmptyTypes;
        }
        public Type[] ProbableChildren()
        {
            return Type.EmptyTypes;
        }
        public C_Bet(XmlNode n)
            : base(n)
        {
        }
        public C_Bet()
        {
        }

        public override bool Run()
        {
            try
            {
                return true;
            }
            catch (MyException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new MyException(this, _Variables, e);
            }
        }
        public string Parse2()
        {
            return base.ToString(GetType().GetFields(), this.GetType().Name);
        }
    }
    public class C_isFalse : myCardGameObject, ICardGameObject
    {
        public Type[] PossibleChildren()
        {
            return AllObjects;
        }
        public Type[] ProbableChildren()
        {
            return AllObjects;
        }
        public C_isFalse(XmlNode n)
            : base(n)
        {
        }

        public C_isFalse()
        {
        }
        public override bool Run()
        {
            try
            {

                if (EvalTop)
                {
                    if (_Variables["EvaluatedExpression" + myHelper.RemoveLast(Current, 1)].ToString() == "EvaluatedExpression" + myHelper.RemoveLast(Current, 1))
                    {
                        if (!(bool)_Variables.GetVariable("EvaluatedInPile" + myHelper.RemoveLast(Current, 1)))
                            if (!EvalChildren())
                            {
                                return false;
                            }
                    }
                    else if (!(bool)_Variables.GetVariable("EvaluatedExpression" + myHelper.RemoveLast(Current, 1)))
                        if (!EvalChildren())
                        {
                            return false;
                        }
                }
                else
                {
                    if (!EvalChildren())
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (MyException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new MyException(this, _Variables, e);
            }
        }
        public string Parse2()
        {
            return base.ToString(GetType().GetFields(), this.GetType().Name);
        }
    }




    public class C_DealCards : myCardGameObject, ICardGameObject
    {
        public Type[] PossibleChildren()
        {
            return Type.EmptyTypes;
        }

        public Type[] ProbableChildren()
        {
            return Type.EmptyTypes;
        }
        public string myPile;
        public string myCards;
        public string myFace;
        public string myPeak;
        public string Peak
        {
            get
            {
                return _Variables.EvalVariableString(myPeak, true);
            }
        }
        public string Cards
        {
            get
            {
                return _Variables.EvalVariableString(myCards, true);
            }
        }
        public string Face
        {
            get
            {
                return _Variables.EvalVariableString(myFace, true);
            }
        }
        public string Pile
        {
            get
            {
                return _Variables.EvalVariableString(myPile, true);
            }
        }
        public C_DealCards(XmlNode n)
            : base(n)
        {
            myCards = n.Attributes["Cards"].Value;
            myFace = n.Attributes["Face"].Value;
            myPeak = n.Attributes["Peak"].Value;
            myPile = n.Attributes["Pile"].Value;
        }

        public C_DealCards()
        {
        }
        public override bool Run()
        {
            try
            {
                if (EvalTop)
                {
                    List<_Card> temp = new List<_Card>(_Variables.Piles[Pile].Cards);


                    int cardCount = (int.Parse((Cards))) * ((List<_User>)_Variables["AllUsers"]).Count;
                    cardCount = cardCount > temp.Count ? temp.Count : cardCount;

                    int user = 0;

                    List<_User> al = (List<_User>)_Variables["AllUsers"];

                    do
                    {
                        cardCount--;
                        _Card c = temp[cardCount];
                        if (Face != null)
                            c.Face = Face == "DOWN" ? false : true;
                        if (Peak != null)
                            c.Peak = bool.Parse(Peak);
                        al[user].Cards.Cards.Add(c);
                        temp.Remove(temp[cardCount]);

                        user++;
                        if (user == al.Count)
                            user = 0;
                    } while (cardCount != 0);

                    _Variables["AllUsers"] = al;
                    _Variables.Piles[Pile].Cards = temp;
                }
                return true;
            }
            catch (MyException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new MyException(this, _Variables, e);
            }
        }
        public string Parse2()
        {
            return base.ToString(GetType().GetFields(), this.GetType().Name);
        }
    }
    public class C_Round : myCardGameObject, ICardGameObject
    {
        public Type[] PossibleChildren()
        {
            return AllObjects;
        }
        public Type[] ProbableChildren()
        {
            return AllObjects;
        }
        public string myNumber;
        public string Number
        {
            get
            {
                return _Variables.EvalVariableString(myNumber, true);
            }
        }
        public C_Round(XmlNode n)
            : base(n)
        {
            myNumber = n.Attributes["Number"].Value;
        }
        public C_Round()
        {
        }

        public override bool Run()
        {
            try
            {
                int roundNumber = int.Parse(Number);
                if (roundNumber == -1)
                {
                    do
                    {
                        //   Variables tempVars = (Variables) _Variables.Clone();
                        if (EvalTop)
                        {
                            roundNumber++;
                            _Variables.CreateVariable("RoundNumber", typeof(int), roundNumber);
                        }
                        if (!EvalChildren())
                        {
                            return false;
                        }
                        _Variables.DestroyVariable("RoundNumber");
                        EvalTop = true;
                    } while (true);
                }
                else
                {
                    if (!EvalChildren())
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (MyException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new MyException(this, _Variables, e);
            }
        }
        public string Parse2()
        {
            return base.ToString(GetType().GetFields(), this.GetType().Name);
        }
    }
    public class C_Game : myCardGameObject, ICardGameObject
    {
        public Type[] PossibleChildren()
        {
            return new Type[] { typeof(C_CreateMethod), typeof(C_Round) };
        }
        public Type[] ProbableChildren()
        {
            return new Type[] { typeof(C_CreateMethod), typeof(C_Round) };
        }
        public C_Game(XmlNode n)
            : base(n)
        {
        }
        public C_Game()
        {
        }

        public override bool Run()
        {
            try
            {
                if (EvalTop)
                {
                    _Variables.CreateVariable("RoundNumber", typeof(int), 0);
                }

                if (!EvalChildren())
                {
                    return false;
                }
                _Variables.DestroyVariable("RoundNumber");

                return true;
            }
            catch (MyException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new MyException(this, _Variables, e);
            }

        }
        public string Parse2()
        {
            return base.ToString(GetType().GetFields(), this.GetType().Name);
        }
    }







    public class C_CreatePile : myCardGameObject, ICardGameObject
    {
        public Type[] PossibleChildren()
        {
            return Type.EmptyTypes;
        }
        public Type[] ProbableChildren()
        {
            return Type.EmptyTypes;
        }
        public string myID;
        public string mySort;
        public string myStack;
        public string myVisible;
        public string ID
        {
            get
            {
                return _Variables.EvalVariableString(myID, true);
            }
        }
        public string Stack
        {
            get
            {
                return _Variables.EvalVariableString(myStack, true);
            }
        }
        public string Sort
        {
            get
            {
                return _Variables.EvalVariableString(mySort, true);
            }
        }
        public string Visible
        {
            get
            {
                return _Variables.EvalVariableString(myVisible, true);
            }
        }
        public C_CreatePile(XmlNode n)
            : base(n)
        {
            //GameDebug.AddVariable(myID);

            myID = n.Attributes["ID"].Value;
            myStack = n.Attributes["Stack"].Value;
            if (n.Attributes["Visible"] != null)
                myVisible = n.Attributes["Visible"].Value;
            else
                myVisible = "false";

            if (n.Attributes["Sort"] != null)
                mySort = n.Attributes["Sort"].Value;
            else
                mySort = "";
        }

        public C_CreatePile()
        {
        }
        public override bool Run()
        {
            try
            {
                _Pile p = new _Pile();
                p.Stack = bool.Parse(Stack);
                p.Sort = mySort;
                p.Visible = bool.Parse(Visible);

                _Variables.Piles.Add((ID), p);

                return true;
            }
            catch (MyException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new MyException(this, _Variables, e);
            }
        }
        public string Parse2()
        {
            return base.ToString(GetType().GetFields(), this.GetType().Name);
        }
    }

    public class C_MethodParameter : myCardGameObject, ICardGameObject
    {
        public Type[] PossibleChildren()
        {
            return Type.EmptyTypes;
        }
        public Type[] ProbableChildren()
        {
            return Type.EmptyTypes;
        }
        public string myID;
        public string ID
        {
            get
            {
                return _Variables.EvalVariableString(myID, true);
            }
        }

        public C_MethodParameter(XmlNode n)
            : base(n)
        {
            myID = n.Attributes["ID"].Value;
        }

        public C_MethodParameter()
        {
        }
        public override bool Run()
        {
            try
            {
                if (EvalTop)
                {
                    string id = _Variables[myHelper.RemoveLast(Current, 1) + "||LastMethod"].ToString();
                    _Variables.CreateVariable(ID, _Variables.GetVariable("Method" + id + "Parameter" + ID).GetType(), _Variables.GetVariable("Method" + id + "Parameter" + ID));
                }
                return true;
            }
            catch (MyException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new MyException(this, _Variables, e);
            }
        }
        public string Parse2()
        {
            return base.ToString(GetType().GetFields(), this.GetType().Name);
        }
    }

    public class C_CreateVariable : myCardGameObject, ICardGameObject
    {
        public string myType;
        public Type[] PossibleChildren()
        {
            return System.Type.EmptyTypes;
        }
        public Type[] ProbableChildren()
        {
            return System.Type.EmptyTypes;
        }
        public string myValue;
        public string myVariable;

        public string Type
        {
            get
            {
                return _Variables.EvalVariableString(myType, true);
            }
        }
        public string Value
        {
            get
            {
                return _Variables.EvalVariableString(myValue, true);
            }
        }
        public string Variable
        {
            get
            {
                return _Variables.EvalVariableString(myVariable, true);
            }
        }
        public C_CreateVariable(XmlNode n)
            : base(n)
        {

            myType = n.Attributes["Type"].Value;
            myVariable = n.Attributes["Variable"].Value;
            //GameDebug.AddVariable(myVariable);
            myValue = n.Attributes["Value"].Value;
        }

        public C_CreateVariable()
        {
        }
        public override bool Run()
        {
            try
            {

                _Variables.CreateVariable(Variable, getTypeFromString(Type), myHelper.ReturnIClonable(_Variables.EvalVariable(Value)));

                return true;
            }
            catch (MyException e)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new MyException(this, _Variables, e);
            }
        }
        public string Parse2()
        {
            return base.ToString(GetType().GetFields(), this.GetType().Name);
        }
    }

    public class C_CreateListVariable : myCardGameObject, ICardGameObject
    {
        public Type[] PossibleChildren()
        {
            return Type.EmptyTypes;
        }

        public Type[] ProbableChildren()
        {
            return Type.EmptyTypes;
        }
        public string myVariable;
        public string Variable
        {
            get
            {
                return _Variables.EvalVariableString(myVariable, true);
            }
        }
        public C_CreateListVariable(XmlNode n) : base(n)
        {

            myVariable = n.Attributes["Variable"].Value;
          //  GameDebug.AddVariable(myVariable);

        }

        public C_CreateListVariable()
        {
        }
        public override bool Run()
        {
            try
            {
                _Variables.CreateVariable(Variable, new ArrayList().GetType(), new ArrayList());

                return true;
            }
            catch (MyException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new MyException(this, _Variables, e);
            }
        }
        public string Parse2()
        {
            return base.ToString(GetType().GetFields(), this.GetType().Name);
        }
    }
    public class C_AddToList : myCardGameObject, ICardGameObject
    {
        public Type[] PossibleChildren()
        {
            return Type.EmptyTypes;
        }
        public Type[] ProbableChildren()
        {
            return Type.EmptyTypes;
        }
        public string myValue;
        public string myVariable;
        public string Value
        {
            get
            {
                return _Variables.EvalVariableString(myValue, true);
            }
        }
        public string Variable
        {
            get
            {
                return _Variables.EvalVariableString(myVariable, true);
            }
        }
        public C_AddToList(XmlNode n)
            : base(n)
        {
            myValue = n.Attributes["Value"].Value;
            myVariable = n.Attributes["Variable"].Value;
        }

        public C_AddToList()
        {
        }
        public override bool Run()
        {
            try
            {
                ((ArrayList)_Variables[Variable]).Add(myHelper.ReturnIClonable(_Variables.EvalVariable(Value)));

                return true;
            }
            catch (MyException e)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new MyException(this, _Variables, e);
            }
        }
        public string Parse2()
        {
            return base.ToString(GetType().GetFields(), this.GetType().Name);
        }
    }

    public class C_ClearList : myCardGameObject, ICardGameObject
    {
        public Type[] PossibleChildren()
        {
            return Type.EmptyTypes;
        }
        public Type[] ProbableChildren()
        {
            return Type.EmptyTypes;
        }
        public string myVariable;

        public string Variable
        {
            get
            {
                return _Variables.EvalVariableString(myVariable, true);
            }
        }
        public C_ClearList(XmlNode n)
            : base(n)
        {
            myVariable = n.Attributes["Variable"].Value;
        }

        public C_ClearList()
        {
        }
        public override bool Run()
        {
            try
            {
                ((ArrayList)_Variables[Variable]).Clear();

                return true;
            }
            catch (MyException e)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new MyException(this, _Variables, e);
            }
        }
        public string Parse2()
        {
            return base.ToString(GetType().GetFields(), this.GetType().Name);
        }
    }

    public class C_RemoveFromList : myCardGameObject, ICardGameObject
    {
        public Type[] PossibleChildren()
        {
            return Type.EmptyTypes;
        }
        public Type[] ProbableChildren()
        {
            return Type.EmptyTypes;
        }
        public string myValue;
        public string myVariable;
        public string Value
        {
            get
            {
                return _Variables.EvalVariableString(myValue, true);
            }
        }
        public string Variable
        {
            get
            {
                return _Variables.EvalVariableString(myVariable, true);
            }
        }
        public C_RemoveFromList(XmlNode n)
            : base(n)
        {
            myValue = n.Attributes["Value"].Value;
            myVariable = n.Attributes["Variable"].Value;
        }

        public C_RemoveFromList()
        {
        }
        public override bool Run()
        {
            try
            {
                ((ArrayList)_Variables[Variable]).Remove(_Variables.EvalVariable(Value));

                return true;
            }
            catch (MyException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new MyException(this, _Variables, e);
            }
        }
        public string Parse2()
        {
            return base.ToString(GetType().GetFields(), this.GetType().Name);
        }
    }
    public class C_SetVariable : myCardGameObject, ICardGameObject
    {
        public Type[] PossibleChildren()
        {
            return Type.EmptyTypes;
        }
        public Type[] ProbableChildren()
        {
            return Type.EmptyTypes;
        }
        public string myValue;
        public string myVariable;
        public string Value
        {
            get
            {
                return _Variables.EvalVariableString(myValue, true);
            }
        }
        public string Variable
        {
            get
            {
                return _Variables.EvalVariableString(myVariable, true);
            }
        }
        public C_SetVariable(XmlNode n)
            : base(n)
        {
            myValue = n.Attributes["Value"].Value;
            myVariable = n.Attributes["Variable"].Value;
        }

        public C_SetVariable()
        {
        }
        public override bool Run()
        {
            try
            {
                if (_Variables.GetVariable(Variable) == null)
                    _Variables.SetVariable(Variable, Value);
                else
                {
                    if (_Variables.GetVariable((Variable)).ToString() == Variable || Variable.Contains("|"))
                        SetVarFromString((Variable), myHelper.ReturnIClonable(EvaluateVarFromString((Value), _Variables)), _Variables);
                    else
                        _Variables.SetVariable((Variable), myHelper.ReturnIClonable(EvaluateVarFromString((Value), _Variables)));
                }

                return true;
            }
            catch (MyException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new MyException(this, _Variables, e);
            }
        }
        public string Parse2()
        {
            return base.ToString(GetType().GetFields(), this.GetType().Name);
        }
    }
    public class C_Shuffle : myCardGameObject, ICardGameObject
    {
        public Type[] PossibleChildren()
        {
            return Type.EmptyTypes;
        }
        public Type[] ProbableChildren()
        {
            return Type.EmptyTypes;
        }
        public string myPile;
        public string Pile
        {
            get
            {
                return _Variables.EvalVariableString(myPile, true);
            }
        }
        public C_Shuffle(XmlNode n)
            : base(n)
        {
            myPile = n.Attributes["Pile"].Value;
        }

        public C_Shuffle()
        {
        }
        public override bool Run()
        {
            try
            {
                _Variables.Piles[Pile].shuffle();
                return true;
            }
            catch (MyException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new MyException(this, _Variables, e);
            }
        }
        public string Parse2()
        {
            return base.ToString(GetType().GetFields(), this.GetType().Name);
        }
    }

    public class C_Card : myCardGameObject, ICardGameObject
    {
        public Type[] PossibleChildren()
        {
            return Type.EmptyTypes;
        }
        public Type[] ProbableChildren()
        {
            return Type.EmptyTypes;
        }
        public string myFace;
        public string myPeak;
        public string Face
        {
            get
            {
                return _Variables.EvalVariableString(myFace, true);
            }
        }
        public string Peak
        {
            get
            {
                return _Variables.EvalVariableString(myPeak, true);
            }
        }
        public C_Card(XmlNode n)
            : base(n)
        {
            myFace = n.Attributes["Face"].Value;
            myPeak = n.Attributes["Peak"].Value;
        }

        public C_Card()
        {
        }
        public override bool Run()
        {
            try
            {

                _Card card = new _Card();
                string _fromspot = _Variables.GetVariable("FromSpot").ToString();
                string _tospot = _Variables.GetVariable("ToSpot").ToString();
                int outSpot;

                if (_fromspot == "" || _fromspot == "TakeLast")
                    card = ((_Pile)_Variables.GetVariable("FromLocation")).Cards[0];
                else if (_fromspot == "TakeFirst")
                    card = ((_Pile)_Variables.GetVariable("FromLocation")).Cards[((_Pile)_Variables.GetVariable("FromLocation")).Cards.Count - 1];
                else if (int.TryParse(_fromspot, out outSpot))
                    card = ((_Pile)_Variables.GetVariable("FromLocation")).Cards[outSpot];
                else
                {
                    foreach (_Card myCard in ((_Pile)_Variables.GetVariable("FromLocation")).Cards)
                    {
                        if (myCard.getCardName == _fromspot)
                        {
                            card = myCard;
                            break;
                        }
                    }
                }


                card.Face = Face == "DOWN" ? false : true;
                if (Peak != null)
                    card.Peak = bool.Parse(Peak);

                _Variables["LastCard"] = card;

                //DOIT

                if (_tospot == "" || _tospot == "First")
                    ((_Pile)_Variables.GetVariable("ToLocation")).Cards.Insert(0,card);
                else if (_fromspot == "Last")
                    ((_Pile)_Variables.GetVariable("ToLocation")).Cards.Add(card);
                else if (int.TryParse(_fromspot, out outSpot))
                    ((_Pile)_Variables.GetVariable("ToLocation")).Cards.Insert(outSpot,card);
                else
                {
                    foreach (_Card myCard in ((_Pile)_Variables.GetVariable("FromLocation")).Cards)
                    {
                        if (myCard.getCardName == _tospot)
                        {
                            ((_Pile) _Variables.GetVariable("ToLocation")).Cards.Insert(((_Pile) _Variables.GetVariable("ToLocation")).Cards.IndexOf(card) + 1, card);
                            break;
                        }
                    }
                }

                //DOIT


                if (_fromspot == "" || _fromspot == "TakeLast")
                    ((_Pile)_Variables.GetVariable("FromLocation")).Cards.RemoveAt(0);
                else if (_fromspot == "TakeFirst")
                    ((_Pile)_Variables.GetVariable("FromLocation")).Cards.RemoveAt(((_Pile)_Variables.GetVariable("FromLocation")).Cards.Count - 1);
                else if (int.TryParse(_fromspot, out outSpot))
                    ((_Pile)_Variables.GetVariable("FromLocation")).Cards.RemoveAt(outSpot);
                else
                {
                    foreach (_Card myCard in ((_Pile)_Variables.GetVariable("FromLocation")).Cards)
                    {
                        if (myCard.getCardName == _fromspot)
                        {
                            ((_Pile)_Variables.GetVariable("FromLocation")).Cards.Remove(myCard);
                            break;
                        }
                    }
                }

                return true;
            }
            catch (MyException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new MyException(this, _Variables, e);
            }
        }
        public string Parse2()
        {
            return base.ToString(GetType().GetFields(), this.GetType().Name);
        }
    }






    public class C_GetAllUserInput : myCardGameObject, ICardGameObject
    {
        public string myDefault;
        public Type[] PossibleChildren()
        {
            return AllObjects;
        }
        public Type[] ProbableChildren()
        {
            return new Type[] { typeof(C_PossibleAnswer) };
        }
        public string myID;
        public string myType;
        public string myQuestion;
        public string Default
        {
            get
            {
                return _Variables.EvalVariableString(myDefault, true);
            }
        }
        public string ID
        {
            get
            {
                return _Variables.EvalVariableString(myID, true);
            }
        }
        public string Type
        {
            get
            {
                return _Variables.EvalVariableString(myType, true);
            }
        }
        public string Question
        {
            get
            {
                return _Variables.EvalVariableString(myQuestion, true);
            }
        }
        public C_GetAllUserInput(XmlNode n)
            : base(n)
        {
            myID = n.Attributes["ID"].Value;
            myType = n.Attributes["Type"].Value;
            myDefault = n.Attributes["Default"].Value;
            myQuestion = n.Attributes["Question"].Value;
        }
        public C_GetAllUserInput()
        {
        }
        public override bool Run()
        {
            try
            {
                if (EvalTop)
                {
                    _Variables.CreateVariable(ID, getTypeFromString(Type), null);

                    _Variables.CreateVariable("PossibleAnswers", typeof(List<_PossibleAnswer>), new List<_PossibleAnswer>());


                    List<_User> al = (List<_User>)_Variables["AllUsers"];

                    foreach (_User myUser in al)
                    {
                        _Variables.CreateVariable("_UserID", typeof(int), myUser.GameUserID);
                        if (!EvalChildren())
                        {
                            return false;
                        }
                    }
                    _Variables.DestroyVariable("_UserID");
                    GetAllUserInput(Question, (List<_PossibleAnswer>)_Variables.GetVariable("PossibleAnswers"), _Variables, Current);

                    myQuestion q = new myQuestion((int)_Variables["QuestionID"]);
                    q.VariableInfo = _Variables.ToString();
                    q.UpdateData();

                    return false;
                }
                else
                {
                    myQuestion m = new myQuestion((int)_Variables.GetVariable("QuestionID"));
                    m.DeleteQuestion();

                    foreach (string answer_ in m.AnswerChosen.Split('_'))
                    {
                        if (answer_ != "")
                        {
                            _PossibleAnswer p = new _PossibleAnswer(int.Parse(answer_.Split('|')[0]));

                            if (p.PossibleAnswer == int.Parse(answer_.Split('|')[0]))
                            {
                                object str = EvaluateVarFromString(p.getPossibleAnswer().PossibleAnswerValue, _Variables);
                                _Variables.CreateVariable(ID + "|" + int.Parse(answer_.Split('|')[1]), _Variables.GetVariable(ID).GetType(), str.ToString().Split('|')[0]);
                            }
                        }
                        _Variables.DestroyVariable("PossibleAnswers");
                        _Variables.DestroyVariable("QuestionID");
                    }

                }
                return true;
            }
            catch (MyException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new MyException(this, _Variables, e);
            }
        }

        private void GetAllUserInput(string question, List<_PossibleAnswer> list, Variables _Variables, string cur)
        {
            myQuestion q = new myQuestion();
            q.QuestionName = cur + "|" + question;
            q.GameUserID = -1;
            q.AnswerChosen = "";
            q.GameRoomID = (int)_Variables["GameID"];
            list.ForEach(delegate(_PossibleAnswer p)
            {
                q.PossibleAnswers.Add(p.getPossibleAnswer());
            });
            q.InsertData();

            list.ForEach(delegate(_PossibleAnswer p)
            {
                myPossibleAnswer a = p.getPossibleAnswer();
                a.QuestionID = q.QuestionID;
                a.UpdateData();
            });

            _Variables.CreateVariable("QuestionID", typeof(int), q.QuestionID);
        }
        public string Parse2()
        {
            return base.ToString(GetType().GetFields(), this.GetType().Name);
        }

    }
    public class C_GetUserInput : myCardGameObject, ICardGameObject
    {
        public Type[] PossibleChildren()
        {
            return AllObjects;
        }
        public Type[] ProbableChildren()
        {
            return new Type[] { typeof(C_PossibleAnswer) };
        }
        public string myDefault;
        public string myID;
        public string myType;
        public string myQuestion;

        public string Default
        {
            get
            {
                return _Variables.EvalVariableString(myDefault, true);
            }
        }
        public string ID
        {
            get
            {
                return _Variables.EvalVariableString(myID, true);
            }
        }
        public string Type
        {
            get
            {
                return _Variables.EvalVariableString(myType, true);
            }
        }
        public string Question
        {
            get
            {
                return _Variables.EvalVariableString(myQuestion, true);
            }
        }
        public C_GetUserInput(XmlNode n)
            : base(n)
        {
            myID = n.Attributes["ID"].Value;
            myType = n.Attributes["Type"].Value;
            myDefault = n.Attributes["Default"].Value;
            myQuestion = n.Attributes["Question"].Value;
        }

        public C_GetUserInput()
        {
        }
        public override bool Run()
        {
            try
            {
                if (EvalTop)
                {
                    if (Default == null)

                        _Variables.CreateVariable(ID, getTypeFromString(Type), null);
                    else
                        _Variables.CreateVariable(ID, getTypeFromString(Type), EvaluateVarFromString((Default), _Variables));

                    _Variables.CreateVariable("PossibleAnswers", typeof(List<_PossibleAnswer>), new List<_PossibleAnswer>());
                    if (!EvalChildren())
                    {
                        return false;
                    }
                    GetUserInput((_User)_Variables.GetVariable("User"), Question,
                                 (List<_PossibleAnswer>)_Variables.GetVariable("PossibleAnswers"), _Variables, Current);

                    myQuestion q = new myQuestion((int)_Variables["QuestionID"]);
                    q.VariableInfo = _Variables.ToString();

                    q.UpdateData();


                    return false;
                }
                else
                {
                    myQuestion m = new myQuestion((int)_Variables.GetVariable("QuestionID"));
                    if (m.QuestionName.Split('|')[0] == Current)
                    {
                        m.DeleteQuestion();

                        foreach (_PossibleAnswer answer in (List<_PossibleAnswer>)_Variables.GetVariable("PossibleAnswers"))
                        {
                            if (answer.PossibleAnswer == int.Parse(m.AnswerChosen))
                                _Variables.SetVariable(ID, EvaluateVarFromString(answer.getPossibleAnswer().PossibleAnswerValue, _Variables));
                        }
                        _Variables.DestroyVariable("PossibleAnswers");
                        _Variables.DestroyVariable("QuestionID");
                    }
                }
                return true;
            }
            catch (MyException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new MyException(this, _Variables, e);
            }
        }
        private void GetUserInput(_User user, string question, List<_PossibleAnswer> list, Variables _Variables, string cur)
        {
            myQuestion q = new myQuestion();
            q.QuestionName = cur + "|" + question;
            q.GameUserID = user.GameUserID;
            q.AnswerChosen = "0";
            q.GameRoomID = (int)_Variables["GameID"];
            list.ForEach(delegate(_PossibleAnswer p)
            {
                q.PossibleAnswers.Add(p.getPossibleAnswer());
            });
            q.InsertData();

            list.ForEach(delegate(_PossibleAnswer p)
            {
                myPossibleAnswer a = p.getPossibleAnswer();
                a.QuestionID = q.QuestionID;
                a.UpdateData();
            });

            _Variables.CreateVariable("QuestionID", typeof(int), q.QuestionID);
        }

        public string Parse2()
        {
            return base.ToString(GetType().GetFields(), this.GetType().Name);
        }
    }
    public class C_MoveCards : myCardGameObject, ICardGameObject
    {
        public Type[] PossibleChildren()
        {
            return AllObjects;
        }
        public Type[] ProbableChildren()
        {
            return new Type[] { typeof(C_Card) };
        }
        public string myFromLocation;
        public string myToLocation;
        public string myFromSpot;
        public string myToSpot;

        public string FromLocation
        {
            get
            {
                return _Variables.EvalVariableString(myFromLocation, true);
            }
        }
        public string ToLocation
        {
            get
            {
                return _Variables.EvalVariableString(myToLocation, true);
            }
        }
        public string FromSpot
        {
            get
            {
                return _Variables.EvalVariableString(myFromSpot, true);
            }
        }
        public string ToSpot 
        {
            get
            {
                return _Variables.EvalVariableString(myToSpot, true);
            }
        }

        public C_MoveCards(XmlNode n) : base(n)
        {
            myFromLocation = n.Attributes["FromLocation"].Value;
            myToLocation = n.Attributes["ToLocation"].Value;
            if (n.Attributes["FromSpot"] != null)
                myFromSpot = n.Attributes["FromSpot"].Value;
            if (n.Attributes["ToSpot"] != null)
                myToSpot = n.Attributes["ToSpot"].Value;
        }

        public C_MoveCards()
        {
        }
        public override bool Run()
        {
            try
            {
                string from = (FromLocation);
                string to = (ToLocation);
                if (EvalTop)
                {
                    _Pile _to = new _Pile();
                    if (_Variables.EvalVariable(to) is _Pile)
                        _to = (_Pile)EvaluateVarFromString(to, _Variables);
                    else
                    {
                        _to.Cards = new List<_Card>(myHelper.ArrayListToList<_Card>(_Variables.EvalVariable<ArrayList>(to)));
                    }

                    _Variables.CreateVariable("ToLocation", typeof(_Pile), _to);


                    _Pile _from = new _Pile();
                    if (_Variables.EvalVariable(from) is _Pile)
                        _from = (_Pile)EvaluateVarFromString(from, _Variables);
                    else
                    {
                        _from.Cards = new List<_Card>(myHelper.ArrayListToList<_Card>(_Variables.EvalVariable<ArrayList>(from)));
                    }

                    _Variables.CreateVariable("FromLocation", typeof(_Pile), _from);


                    if (FromSpot != null)
                        _Variables.CreateVariable("FromSpot", typeof(string), EvaluateVarFromString(FromSpot, _Variables));


                    if (ToSpot != null)
                        _Variables.CreateVariable("ToSpot", typeof(string), EvaluateVarFromString(ToSpot, _Variables));

                    _Variables.CreateVariable("LastCard", typeof(_Card), new _Card());

                    if (!EvalChildren())
                    {
                        return false;
                    }
                    bottom(to, from);
                }
                else
                {
                    bottom(to, from);
                }
                return true;
            }
            catch (MyException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new MyException(this, _Variables, e);
            }
        }

        public void bottom(string to, string from)
        {
            _Variables.DestroyVariable("LastCard");

            if (_Variables.EvalVariable(to) is _Pile && !to.Contains("."))
                _Variables.Piles[to] = (_Pile)_Variables.GetVariable("ToLocation");
            else if (_Variables.EvalVariable(to) is ArrayList)
                SetVarFromString(to, myHelper.ListToArrayList(((_Pile)_Variables.GetVariable("ToLocation")).Cards), _Variables);
            else if (!_Variables.Piles.ContainsKey(to))
                SetVarFromString(to, (_Pile)_Variables.GetVariable("ToLocation"), _Variables);

            if (_Variables.EvalVariable(from) is _Pile && !from.Contains("."))
                _Variables.Piles[from] = (_Pile)_Variables.GetVariable("FromLocation");
            else if (_Variables.EvalVariable(from) is ArrayList)
                SetVarFromString(from, myHelper.ListToArrayList(((_Pile)_Variables.GetVariable("FromLocation")).Cards), _Variables);
            else if (!_Variables.Piles.ContainsKey(from))
                SetVarFromString(from, (_Pile)_Variables.GetVariable("FromLocation"), _Variables);


            _Variables.DestroyVariable("FromSpot");
            _Variables.DestroyVariable("ToSpot");
            _Variables.DestroyVariable("ToLocation");
            _Variables.DestroyVariable("FromLocation");
        }
        public string Parse2()
        {
            return base.ToString(GetType().GetFields(), this.GetType().Name);
        }
    }
    public class C_InPile : myCardGameObject, ICardGameObject
    {
        public Type[] PossibleChildren()
        {
            return AllObjects;
        }
        public Type[] ProbableChildren()
        {
            return new Type[] { typeof(C_isTrue), typeof(C_isFalse) };
        }

        public string myPile;
        public string myCard;

        public string Pile
        {
            get
            {
                return _Variables.EvalVariableString(myPile, true);
            }
        }
        public string Card
        {
            get
            {
                return _Variables.EvalVariableString(myCard, true);
            }
        }

        public C_InPile(XmlNode n)
            : base(n)
        {
            myPile = n.Attributes["Pile"].Value;
            myCard = n.Attributes["Card"].Value;

        }

        public C_InPile()
        {
        }
        public override bool Run()
        {
            try
            {
                if (EvalTop)
                {
                    _Pile pile = new _Pile();
                    if (_Variables.EvalVariable(Pile) is _Pile)
                        pile = (_Pile)EvaluateVarFromString(Pile, _Variables);
                    else
                        pile.Cards = new List<_Card>(myHelper.ArrayListToList<_Card>(_Variables.EvalVariable<ArrayList>(Pile)));

                    _Card card = _Variables.EvalVariable<_Card>(Card);
                    _Variables.CreateVariable("EvaluatedInPile" + Current, typeof(bool), false);

                    foreach (_Card my_Card in pile.Cards)
                    {
                        if (card.Value == my_Card.Value && card.Type == my_Card.Type)
                            _Variables.SetVariable("EvaluatedInPile" + Current, true);
                    }
                    if (!EvalChildren())
                    {
                        return false;
                    }
                    _Variables.DestroyVariable("EvaluatedInPile" + Current);
                }
                else
                {
                    if (!EvalChildren())
                    {
                        return false;
                    }
                    _Variables.DestroyVariable("EvaluatedInPile" + Current);
                }
                return true;
            }
            catch (MyException e)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new MyException(this, _Variables, e);
            }
        }

        public string Parse2()
        {
            return base.ToString(GetType().GetFields(), this.GetType().Name);
        }
    }


    public class C_Loop : myCardGameObject, ICardGameObject
    {
        public Type[] PossibleChildren()
        {
            return AllObjects;
        }
        public Type[] ProbableChildren()
        {
            return new Type[] { typeof(C_isTrue), typeof(C_isFalse) };
        }

        public string myVariable;
        public string myStart;
        public string myEnd;

        public string Variable
        {
            get
            {
                return _Variables.EvalVariableString(myVariable, true);
            }
        }
        public string Start
        {
            get
            {
                return _Variables.EvalVariableString(myStart, true);
            }
        }
        public string End
        {
            get
            {
                return _Variables.EvalVariableString(myEnd, true);
            }
        }

        public C_Loop(XmlNode n)
            : base(n)
        {
            myVariable = n.Attributes["Variable"].Value;
            myStart = n.Attributes["Start"].Value;
            myEnd = n.Attributes["End"].Value;

        }

        public C_Loop()
        {
        }
        public override bool Run()
        {
            try
            {
                if (EvalTop)
                {
                    int s = int.Parse(EvaluateVarFromString(Start, _Variables).ToString());
                    int e = int.Parse(EvaluateVarFromString(End, _Variables).ToString());
                    _Variables.CreateVariable("InLoopCounter" + Current, typeof(int), s);
                    _Variables.CreateVariable(Variable, typeof(int), _Variables["InLoopCounter" + Current]);
                    for (int i = int.Parse(Start); i <= e; i++)
                    {
                        _Variables.SetVariable("InLoopCounter" + Current, i);
                        _Variables.SetVariable(Variable, _Variables["InLoopCounter" + Current]);
                        if (!EvalChildren())
                        {
                            return false;
                        }
                    }
                    _Variables.DestroyVariable("InLoopCounter" + Current);
                    _Variables.DestroyVariable(Variable);
                }
                else
                {
                    int e = int.Parse(EvaluateVarFromString(End, _Variables).ToString());
                    for (int i = (int)_Variables["InLoopCounter" + Current]; i <= e; i++)
                    {
                        _Variables.SetVariable("InLoopCounter" + Current, i);
                        _Variables.SetVariable(Variable, _Variables["InLoopCounter" + Current]);
                        if (!EvalChildren())
                        {
                            return false;
                        }
                    }
                    _Variables.DestroyVariable("InLoopCounter" + Current);
                    _Variables.DestroyVariable(Variable);
                }
                return true;
            }
            catch (MyException e)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new MyException(this, _Variables, e);
            }
        }

        public string Parse2()
        {
            return base.ToString(GetType().GetFields(), this.GetType().Name);
        }
    }
  
     
    public class C_AddMethodParameter : myCardGameObject, ICardGameObject
    {
        public Type[] PossibleChildren()
        {
            return Type.EmptyTypes;
        }
        public Type[] ProbableChildren()
        {
            return Type.EmptyTypes;
        }
        public string myVariable;
        public string myID;
        public string Variable
        {
            get
            {
                return _Variables.EvalVariableString(myVariable, true);
            }
        }
        public string ID
        {
            get
            {
                return _Variables.EvalVariableString(myID, true);
            }
        }
        public C_AddMethodParameter(XmlNode n)
            : base(n)
        {
            myID = n.Attributes["ID"].Value;
           // GameDebug.AddVariable(myID);
            myVariable = n.Attributes["Variable"].Value;
        }
        public C_AddMethodParameter()
        {
        }

        public override bool Run()
        {
            try
            {
                if (EvalTop)
                {
                    _Variables.CreateVariable(ID + "_Variable", typeof(string), Variable);
                    _Variables.CreateVariable("Method" + ((C_RunMethod)Parent).ID + "Parameter" + ID, _Variables.GetVariable(Variable).GetType(), _Variables.GetVariable(Variable));
                }
                return true;
            }
            catch (MyException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new MyException(this, _Variables, e);
            }
        }
        public string Parse2()
        {
            return base.ToString(GetType().GetFields(), this.GetType().Name);
        }
    }
    public class C_UpdateMethodParameter : myCardGameObject, ICardGameObject
    {
        public Type[] PossibleChildren()
        {
            return Type.EmptyTypes;
        }
        public Type[] ProbableChildren()
        {
            return Type.EmptyTypes;
        }

        public string myID;
        public string ID
        {
            get
            {
                return _Variables.EvalVariableString(myID, true);
            }
        }

        public C_UpdateMethodParameter(XmlNode n)
            : base(n)
        {
            myID = n.Attributes["ID"].Value;
        }

        public C_UpdateMethodParameter()
        {
        }
        public override bool Run()
        {
            try
            {
                SetVarFromString(_Variables[ID + "_Variable"].ToString(), _Variables[ID], _Variables);
                return true;
            }
            catch (MyException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new MyException(this, _Variables, e);
            }
        }
        public string Parse2()
        {
            return base.ToString(GetType().GetFields(), this.GetType().Name);
        }
    }

    public class C_RunMethod : myCardGameObject, ICardGameObject
    {
        public Type[] PossibleChildren()
        {
            return new Type[] { typeof(C_AddMethodParameter) };
        }
        public Type[] ProbableChildren()
        {
            return new Type[] { typeof(C_AddMethodParameter) };
        }
        public string myID;
        public string ID
        {
            get
            {
                return _Variables.EvalVariableString(myID, true);
            }
        }

        public C_RunMethod(XmlNode n)
            : base(n)
        {
            myID = n.Attributes["ID"].Value;
        }

        public C_RunMethod()
        {
        }
        public override bool Run()
        {
            try
            {
                _Variables.CreateVariable(Current + "||LastMethod", typeof(string), ID);
                if (!EvalChildren())
                {
                    return false;
                }
                return true;
            }
            catch (MyException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new MyException(this, _Variables, e);
            }
        }
        public string Parse2()
        {
            return base.ToString(GetType().GetFields(), this.GetType().Name);
        }
    }

    public class C_GetUserFromGameUserID : myCardGameObject, ICardGameObject
    {
        public Type[] PossibleChildren()
        {
            return Type.EmptyTypes;
        }
        public Type[] ProbableChildren()
        {
            return Type.EmptyTypes;
        }
        public string myGameUserID;
        public string myVariable;
        public string GameUserID
        {
            get
            {
                return _Variables.EvalVariableString(myGameUserID, true);
            }
        }
        public string Variable
        {
            get
            {
                return _Variables.EvalVariableString(myVariable, true);
            }
        }

        public C_GetUserFromGameUserID(XmlNode n)
            : base(n)
        {
            myGameUserID = n.Attributes["GameUserID"].Value;
            myVariable = n.Attributes["Variable"].Value;
        }
        public C_GetUserFromGameUserID()
        {
        }

        public override bool Run()
        {
            try
            {
                if (EvalTop)
                {
                    foreach (_User myUser in ((List<_User>)_Variables["AllUsers"]))
                    {
                        if (myUser.GameUserID == ((int)EvaluateVarFromString(GameUserID, _Variables)))
                            _Variables.CreateVariable((Variable), typeof(_User), myUser);
                    }
                }
                return true;
            }
            catch (MyException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new MyException(this, _Variables, e);
            }
        }
        public string Parse2()
        {
            return base.ToString(GetType().GetFields(), this.GetType().Name);
        }
    }

    public class C_WinType : myCardGameObject, ICardGameObject
    {
        public Type[] PossibleChildren()
        {
            return AllObjects;
        }
        public Type[] ProbableChildren()
        {
            return AllObjects;
        }
        public string myID;
        public string ID
        {
            get
            {
                return _Variables.EvalVariableString(myID, true);
            }
        }

        public C_WinType(XmlNode n)
            : base(n)
        {
            myID = n.Attributes["ID"].Value;
        }
        public C_WinType()
        {
        }

        public override bool Run()
        {
            try
            {
                //   _Variables.SetVarFromString(_Variables[ID + "_Variable"].ToString(), _Variables[ID]);
                return true;
            }
            catch (MyException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new MyException(this, _Variables, e);
            }
        }
        public string Parse2()
        {
            return base.ToString(GetType().GetFields(), this.GetType().Name);
        }
    }

    public class C_DeclareWinner : myCardGameObject, ICardGameObject
    {
        public Type[] PossibleChildren()
        {
            return AllObjects;
        }
        public Type[] ProbableChildren()
        {
            return AllObjects;
        }
        public string myID;
        public string ID
        {
            get
            {
                return _Variables.EvalVariableString(myID, true);
            }
        }

        public C_DeclareWinner(XmlNode n)
            : base(n)
        {
            myID = n.Attributes["ID"].Value;
        }

        public C_DeclareWinner()
        {
        }
        public override bool Run()
        {
            try
            {
                SetVarFromString(_Variables[ID + "_Variable"].ToString(), _Variables[ID], _Variables);
                return true;
            }
            catch (MyException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new MyException(this, _Variables, e);
            }
        }
        public string Parse2()
        {
            return base.ToString(GetType().GetFields(), this.GetType().Name);
        }
    }

    public class C_DeleteVariable : myCardGameObject, ICardGameObject
    {
        public Type[] PossibleChildren()
        {
            return Type.EmptyTypes;
        }
        public Type[] ProbableChildren()
        {
            return Type.EmptyTypes;
        }
        public string myVariable;

        public string Variable
        {
            get
            {
                return _Variables.EvalVariableString(myVariable, true);
            }
        }
        public C_DeleteVariable()
        {
        }
        public C_DeleteVariable(XmlNode n)
            : base(n)
        {
            myVariable = n.Attributes["Variable"].Value;
        }

        public override bool Run()
        {
            try
            {
                _Variables.DestroyVariable(Variable);
                return true;
            }
            catch (MyException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new MyException(this, _Variables, e);
            }
        }
        public string Parse2()
        {
            return base.ToString(GetType().GetFields(), this.GetType().Name);
        }
    }



    public class C_LoopCards : myCardGameObject, ICardGameObject
    {
        public Type[] PossibleChildren()
        {
            return AllObjects;
        }
        public Type[] ProbableChildren()
        {
            return AllObjects;
        }
        public string myPile;
        public string Pile
        {
            get
            {
                return _Variables.EvalVariableString(myPile, true);
            }
        }
        public C_LoopCards(XmlNode n)
            : base(n)
        {
            myPile = n.Attributes["Pile"].Value;
        }

        public C_LoopCards()
        {
        }
        public override bool Run()
        {
            try
            {

                List<_Card> temp;

                string pile = (Pile);

                if (_Variables.EvalVariable(pile) is _Pile)
                    temp = new List<_Card>(_Variables.EvalVariable<_Pile>(pile).Cards);
                else
                    temp = new List<_Card>(myHelper.ArrayListToList<_Card>(_Variables.EvalVariable<ArrayList>(pile)));

                foreach (_Card _c in temp)
                {
                    if (EvalTop)
                    {
                        _Variables.CreateVariable("CurrentCard", typeof(_Card), _c);
                    }
                    if (!EvalChildren())
                    {
                        return false;
                    }
                    _Variables.DestroyVariable("CurrentCard");
                }
                return true;
            }
            catch (MyException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new MyException(this, _Variables, e);
            }
        }
        public string Parse2()
        {
            return base.ToString(GetType().GetFields(), this.GetType().Name);
        }
    }

    public class C_LoopUsers : myCardGameObject, ICardGameObject
    {
        public Type[] PossibleChildren()
        {
            return AllObjects;
        }
        public Type[] ProbableChildren()
        {
            return AllObjects;
        }
        public C_LoopUsers(XmlNode n)
            : base(n)
        {

        }
        public C_LoopUsers()
        {
        }

        public override bool Run()
        {
            try
            {

                List<_User> al = (List<_User>)_Variables["AllUsers"];
                bool next = false;
                for (int bb = 0; bb < al.Count; bb++)
                {
                    _User user = al[bb];
                    if (EvalTop)
                    {
                        _Variables.CreateVariable("User", typeof(_User), user);
                        next = true;
                    }
                    else
                    {
                        if (((_User)_Variables["User"]).GameUserID == user.GameUserID)
                            next = true;
                    }
                    if (next)
                    {
                        if (!EvalChildren())
                        {
                            return false;
                        }
                        al[bb] = ((_User)_Variables["User"]);
                        EvalTop = true;
                        _Variables["AllUsers"] = al;
                        _Variables.DestroyVariable("User");
                    }
                    next = false;
                }
                return true;
            }
            catch (MyException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new MyException(this, _Variables, e);
            }
        }
        public string Parse2()
        {
            return base.ToString(GetType().GetFields(), this.GetType().Name);
        }
    }




    public class C_CardGame : myCardGameObject, ICardGameObject
    {
        public Type[] PossibleChildren()
        {
            return new Type[] { typeof(C_Deal), typeof(C_Game) };
        }
        public Type[] ProbableChildren()
        {
            return new Type[] { typeof(C_Deal), typeof(C_Game) };
        }

        public string myMaxPlayers;
        public string myKingIsTen;
        public string myName;
        public string myMinPlayers;


        public string MaxPlayers
        {
            get
            {
                return _Variables.EvalVariableString(myMaxPlayers, true);
            }
        }
        public string KingIsTen
        {
            get
            {
                return _Variables.EvalVariableString(myKingIsTen, true);
            }
        }
        public string Name
        {
            get
            {
                return _Variables.EvalVariableString(myName, true);
            }
        }
        public string MinPlayers
        {
            get
            {
                return _Variables.EvalVariableString(myMinPlayers, true);
            }
        }


        private List<_User> myUsers;
        public List<_User> Users
        {
            get
            {
                return myUsers;
            }
            set
            {
                myUsers = value;
            }
        }

        public C_CardGame(XmlNode n)
            : base(n)
        {
            myMaxPlayers = n.Attributes["MaxPlayers"].Value;
            myMinPlayers = n.Attributes["MinPlayers"].Value;
            if (n.Attributes["KingIsTen"] != null)
                myKingIsTen = n.Attributes["KingIsTen"].Value;
            else
                myKingIsTen = "false";
            myName = n.Attributes["Name"].Value;
        }
        public C_CardGame()
        {
        }

        public override bool Run()
        {
            try
            {

                if (int.Parse(MaxPlayers) >= Users.Count && int.Parse(MinPlayers) <= Users.Count)
                {
                    if (EvalTop)
                    {
                        _Variables.CreateVariable("AllUsers", typeof(List<_User>), Users);
                        _Variables.CreateVariable("CardGameName", typeof(string), Name);
                        _Variables.CreateVariable("KingIsTen", typeof(bool), KingIsTen);
                    }

                    if (!EvalChildren())
                    {
                        return false;
                    }
                    _Variables.DestroyVariable("KingIsTen");
                    _Variables.DestroyVariable("LoggedInUser");
                    _Variables.DestroyVariable("AllUsers");
                    _Variables.DestroyVariable("LoggedInUserID");
                }
                return true;
            }
            catch (MyException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new MyException(this, _Variables, e);
            }
        }
        public string Parse2()
        {
            return base.ToString(GetType().GetFields(), this.GetType().Name);
        }
    }

    public class C_Deal : myCardGameObject, ICardGameObject
    {
        public Type[] PossibleChildren()
        {
            return AllObjects;
        }
        public Type[] ProbableChildren()
        {
            return new Type[] { typeof(C_DealCards) };
        }

        public string myCards;
        public string Cards
        {
            get
            {
                return _Variables.EvalVariableString(myCards, true);
            }
        }
        public C_Deal(XmlNode n)
            : base(n)
        {
            myCards = n.Attributes["Cards"].Value;
        }
        public C_Deal()
        {
        }

        public override bool Run()
        {
            try
            {
                if (EvalTop)
                {
                    _Pile p = new _Pile();
                    p.Deal(int.Parse(Cards), (bool)_Variables["KingIsTen"]);
                    p.Visible = false;
                    _Variables.Piles.Add("Deck", p);
                }
                if (!EvalChildren())
                {
                    return false;
                }
                return true;
            }
            catch (MyException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new MyException(this, _Variables, e);
            }
        }
        public string Parse2()
        {
            return base.ToString(GetType().GetFields(), this.GetType().Name);
        }
    }*/
}
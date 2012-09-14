using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using AnyCardGame2Classes;

namespace AnyCardGameClasses
{/*
    public class MyException : Exception
    {
        private string NodeName;
        private string Variables;
        private string Piles;
        private string Error;

        public override string ToString()
        {
            return Error + NodeName + Variables + Piles;
        }

        private string newLine = "<br />";

        public MyException(string current, Variables _Variable, Exception e)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(new myGameRoom((int)_Variable["GameID"]).GameXML);

            NodeName = buildNamesFromCurrent( current, doc) + newLine + newLine;

            string vars = "";

            foreach (KeyValuePair<string, object> var in _Variable.Vars)
            {
                vars += "Variable Name: " + var.Key + newLine;
                vars += "Variable Type: " + _Variable.VarTypes[var.Key] + newLine;
                vars += "Variable Value: " + var.Value + newLine + newLine;
            }
            Variables = vars;

            string piles = "";
            foreach (KeyValuePair<string, _Pile> var in _Variable.Piles)
            {
                piles += "Pile Name: " + var.Key + newLine;
                piles += "Pile Length: " + var.Value.Cards.Count + newLine + newLine;
            }
            Piles = piles;

            Error = e.Message + newLine;
        }

        public MyException(myCardGameObject that, Variables _Variable, Exception e)
        {

            NodeName = buildNamesFromThat(that) + newLine + newLine;


            string vars = "";

            foreach (KeyValuePair<string, object> var in _Variable.Vars)
            {
                vars += "Variable Name: " + var.Key + newLine;
                vars += "Variable Type: " + _Variable.VarTypes[var.Key] + newLine;
                vars += "Variable Value: " + var.Value + newLine + newLine;
            }
            Variables = vars;

            string piles = "";
            foreach (KeyValuePair<string, _Pile> var in _Variable.Piles)
            {
                piles += "Pile Name: " + var.Key + newLine;
                piles += "Pile Length: " + var.Value.Cards.Count + newLine + newLine;
            }
            Piles = piles;

            Error = e.Message + newLine;
        }

        private string buildNamesFromThat(myCardGameObject aThat)
        {
            string str = "";
            if (aThat.Parent != null)
            {
                str = aThat.NodeName+ " (";
                if (aThat.GetType().GetFields().Length != 0)
                    foreach (FieldInfo n in aThat.GetType().GetFields())
                    {
                        str += n.Name + " ";
                        str += n.GetValue(aThat) + " ,";
                    }
                str += ") " + newLine;

                str += buildNamesFromThat(aThat.Parent);
            }
            return str;
        }
        private string buildNamesFromCurrent(string aCurrent, XmlNode aDoc)
        {
            string str = "";
            if (aCurrent != "")
            {
                str = aDoc.Name + " (";
                if (aDoc.Attributes != null)
                    foreach (XmlNode n in aDoc.ChildNodes)
                    {
                        if (n.Name == "Field")
                        {
                            str += n.Attributes["name"].Value + " ";
                            str += n.Attributes["value"].Value + " ,";
                        }
                    }
                str += ") " + newLine;

                str += buildNamesFromCurrent(myHelper.RemoveFirst(aCurrent, 1), aDoc.ChildNodes[int.Parse(aCurrent[0].ToString())]);
            }
            return str;
        }
    }
    public class myCardGameLogic
    {
        public List<_User> Users = new List<_User>();
        private string goBack;

        public myCardGameLogic(string _goback)
        {
            goBack = _goback;
        }
        public bool EvaluateNodes(XmlNodeList list, Variables _vars, string cur)
        { 
                int i = 0;
                foreach (XmlNode node in list)
                {
                    if (goBack != "")
                    {
                        if (goBack == cur + i)
                        {
                            if (!EvaluateNode(node, _vars, cur + i))
                            {
                                goBack = "";
                                return false;
                            }
                            goBack = "";
                        }
                        if (goBack.StartsWith(cur + i))
                            if (!EvaluateNode(node, _vars, cur + i))
                            {
                                return false;
                            }
                    }
                    else
                    {
                        if (!EvaluateNode(node, _vars, cur + i))
                        {
                            return false;
                        }
                    }
                    i++;
                }
                return true;
           
        }

        private void SetVarFromString(string from, object o, Variables _Variables)
        {
            if (from.Contains("."))
                _Variables.SetEvalObjectVariable(from.Substring(0, from.IndexOf('.')), from.Substring(from.IndexOf('.') + 1, from.Length - from.IndexOf('.') - 1), o);
            else if (from.Contains("|"))
                _Variables.SetEvalObjectVariable(from, "", o);
            else
                _Variables.SetVariable(from, o);
        }
        private string NodeAttributeToEvalString(XmlNode node, string attributeName, Variables _v)
        {
            return _v.EvalVariableString(node.Attributes[attributeName].Value, true);
        }
        private object NodeAttributeToEval(XmlNode node, string attributeName, Variables _v)
        {
            return _v.EvalVariable(_v.EvalVariableString(node.Attributes[attributeName].Value, true));
        }
        private T NodeAttributeToEval<T>(XmlNode node, string attributeName, Variables _v)
        {
            return _v.EvalVariable<T>(_v.EvalVariableString(node.Attributes[attributeName].Value, true));
        }

        private DateTime TimeSpan = DateTime.Now;
        private bool EvaluateNode(XmlNode node, Variables _Variables, string cur)
        {


            try
            {
                switch (node.Name)
                {
                    case "LoopUsers":
                        if (!EvalLoopUsers(goBack == "", node, _Variables, cur))
                            return false;
                        break;
                    case "DeleteVariable":
                        _Variables.DestroyVariable(NodeAttributeToEvalString(node, "variable", _Variables));
                        break;
                    case "LoopCards":
                        if (!EvalLoopCards(goBack == "", node, _Variables, cur))
                            return false;
                        break;
                    case "LoopInputAnswer":
                        if (!EvalLoopInputAnswer(goBack == "", node, _Variables, cur))
                            return false;
                        break;
                    case "CreateMethod":
                        if (!EvalCreateMethod(goBack == "", node, _Variables, cur))
                            return false;
                        break; 
                    case "AddReturnParameter":
                        if (!EvalAddReturnParameter(goBack == "", node, _Variables, cur))
                            return false;
                        break;
                    case "ReturnParameter":
                        if (!EvalReturnParameter(goBack == "", node, _Variables, cur))
                            return false;
                        break;
                    case "AddMethodParameter":
                        if (!EvalAddMethodParameter(goBack == "", node, _Variables, cur))
                            return false;
                        break;
                    case "UpdateMethodParameter":
                        if (!EvalUpdateMethodParameter(goBack == "", node, _Variables, cur))
                            return false;
                        break;
                    case "RunMethod":
                        if (!EvalRunMethod(goBack == "", node, _Variables, cur))
                            return false;
                        break;
                    case "GetUserFromGameUserID":
                        if (!EvalGetUserFromGameUserID(goBack == "", node, _Variables, cur))
                            return false;
                        break;
                    case "Deal":
                        if (!EvalDeal(goBack == "", node, _Variables, cur))
                            return false;
                        break;
                    case "CardGame":
                        if (!EvalCardGame(goBack == "", node, _Variables, cur))
                            return false;
                        break;
                    case "WinType":
                        _Variables.CreateVariable("WinType" + node.Attributes["id"].Value, typeof (string), node.InnerXml);
                        break;
                    case "DeclareWinner":
                        if (!EvalDeclareWinner(goBack == "", node, _Variables, cur))
                            return false;
                        break;
                    case "CheckWinner":
                        if (!EvalCheckWinner(goBack == "", node, _Variables, cur))
                            return false;
                        break;
                    case "ShowInformation":
                        if (!EvalShowInformation(goBack == "", node, _Variables, cur))
                            return false;
                        break;
                    case "ShowAllInformation":
                        if (!EvalShowAllInformation(goBack == "", node, _Variables, cur))
                            return false;
                        break;
                    case "DealCards":
                        if (!EvalDealCards(goBack == "", node, _Variables, cur))
                            return false;
                        break;
                    case "PossibleAnswer":
                        if (!EvalPossibleAnswer(goBack == "", node, _Variables, cur))
                            return false;
                        break;
                    case "GetUserInput":
                        if (!EvalGetUserInput(goBack == "", node, _Variables, cur))
                            return false;
                        break;
                    case "GetAllUserInput":
                        if (!EvalGetAllUserInput(goBack == "", node, _Variables, cur))
                            return false;
                        break;
                    case "MoveCards":
                        if (!EvalMoveCards(goBack == "", node, _Variables, cur))
                            return false;
                        break;
                    case "CreatePile":
                        EvalCreatePile(goBack == "", node, _Variables, cur);
                        break;
                    case "MethodParameter":
                        EvalMethodParameter(goBack == "", node, _Variables, cur);
                        break;
                    case "CreateVariable":
                        //NodeAttributeToEvalString(node, "variable", _Variables)
                        _Variables.CreateVariable(NodeAttributeToEvalString(node, "variable", _Variables), getTypeFromString(node.Attributes["type"].Value),NodeAttributeToEval(node, "value", _Variables));
                        break;
                    case "CreateListVariable":
                        _Variables.CreateVariable(NodeAttributeToEvalString(node, "variable", _Variables), new ArrayList().GetType(), new ArrayList());
                        break;
                    case "AddToList":
                        ((ArrayList) _Variables[NodeAttributeToEvalString(node, "variable", _Variables)]).Add(NodeAttributeToEval(node, "value", _Variables));
                        break;
                    case "RemoveFromList":
                        ((ArrayList)_Variables[NodeAttributeToEvalString(node, "variable", _Variables)]).Remove(NodeAttributeToEval(node, "value", _Variables));
                        break;
                    case "SetVariable":
                        EvalSetVariable(goBack == "", node, _Variables, cur);
                        break;
                    case "Shuffle":
                        _Variables.Piles[NodeAttributeToEvalString(node, "Pile", _Variables)].shuffle();
                        break;
                    case "Card":

                        EvalCard(goBack == "", node, _Variables);

                        break;
                    case "Game":
                        if (!EvalGame(goBack == "", node, _Variables, cur))
                            return false;
                        break;
                    case "Round":
                        if (!EvalRound(goBack == "", node, _Variables, cur))
                            return false;

                        break;
                    case "Evaluate":
                        if (!EvalEvaluate(goBack == "", node, _Variables, cur))
                            return false;
                        break;
                    case "isTrue":
                        if (!EvalIsTrue(goBack == "", node, _Variables, cur))
                            return false;
                        break;
                    case "isFalse":
                        if (!EvalIsFalse(goBack == "", node, _Variables, cur))
                            return false;

                        break;
                    case "Bet":
                        //BET
                        break;
                }
                return true;
            }
            catch (MyException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new MyException(cur, _Variables, e);
            }
        }

        private bool EvalReturnParameter(bool EvalTop, XmlNode node, Variables _Variables, string cur)
        {
            if (EvalTop)
            {
                _Variables.CreateVariable( _Variables.EvalVariableString(node.Attributes["id"].Value, true), getTypeFromString(node.Attributes["type"].Value), getTypeFromString(node.Attributes["type"].Value).GetConstructor(Type.EmptyTypes).Invoke(null));
            }
            return true;
        }

        private bool EvalAddReturnParameter(bool EvalTop, XmlNode node, Variables _Variables, string cur)
        {
            if (EvalTop)
            {
                _Variables.CreateVariable(_Variables.EvalVariableString(node.Attributes["id"].Value ,true), getTypeFromString(node.Attributes["type"].Value), getTypeFromString(node.Attributes["type"].Value).GetConstructor(Type.EmptyTypes).Invoke(null));
            }
            return true;
        }

        private bool EvalUpdateMethodParameter(bool EvalTop, XmlNode node, Variables _Variables, string cur)
        {
            if (EvalTop)
            {
       //         string id = _Variables[myHelper.RemoveLast(cur, 1) + "||LastMethod"].ToString();

                string var =  node.Attributes["id"].Value + "_Variable";



                SetVarFromString(_Variables[var].ToString(),_Variables[  node.Attributes["id"].Value], _Variables);


                // _Variables.SetVariable("Method" + node.ParentNode.Attributes["id"].Value, node.InnerXml);

            }
            return true;
        }

        private bool EvalCreateMethod(bool EvalTop, XmlNode node, Variables _Variables, string cur)
        {
            if (EvalTop)
            {

                _Variables.CreateVariable("Method" + node.Attributes["id"].Value, typeof(string), node.InnerXml);

                
               // _Variables.SetVariable("Method" + node.ParentNode.Attributes["id"].Value, node.InnerXml);

            }
            return true;
        }

        private bool EvalAddMethodParameter(bool EvalTop, XmlNode node, Variables _Variables, string cur)
        {
            if (EvalTop)
            {
                
          //      _Variables.CreateVariable("Method" + node.ParentNode.Attributes["id"].Value + "Parameter" + node.Attributes["id"].Value+"Variable", typeof(string), node.Attributes["Variable"].Value);
                _Variables.CreateVariable(node.Attributes["id"].Value + "_Variable", typeof(string), node.Attributes["Variable"].Value);

                _Variables.CreateVariable("Method" + node.ParentNode.Attributes["id"].Value + "Parameter" + node.Attributes["id"].Value, _Variables.GetVariable(node.Attributes["Variable"].Value).GetType(), _Variables.GetVariable(node.Attributes["Variable"].Value));
            }
            return true;

            

        }
        private bool EvalMethodParameter(bool EvalTop, XmlNode node, Variables _Variables, string cur)
        {
            if (EvalTop)
            {
                string id = _Variables[myHelper.RemoveLast(cur, 1) + "||LastMethod"].ToString();



                _Variables.CreateVariable(node.Attributes["id"].Value, _Variables.GetVariable("Method" + id + "Parameter" + node.Attributes["id"].Value).GetType(), _Variables.GetVariable("Method" + id + "Parameter" + node.Attributes["id"].Value));
            }
            return true;

        }


        private bool EvalRunMethod(bool EvalTop, XmlNode node, Variables _Variables, string cur)
        {
            if (!EvaluateNodes(node.ChildNodes, _Variables, cur))
            {
                return false;
            }

            _Variables.CreateVariable(cur + "||LastMethod", typeof(string), node.Attributes["id"].Value);
            if (!EvaluateNodes(stringToXmlNode(_Variables.GetVariable("Method" + node.Attributes["id"].Value).ToString()).ChildNodes, _Variables, cur))
            {
                return false;
            }
            return true;
        }

        private XmlNode stringToXmlNode(string str)
        {
            XmlTextReader xmlReader = new XmlTextReader(new StringReader("<root>"+str+"</root>"));
 
            XmlDocument xmlDocument = new XmlDocument();
            XmlNode node = xmlDocument.ReadNode(xmlReader);
            return node;
        }

        private void EvalSetVariable(bool EvalTop, XmlNode node, Variables _Variables, string cur)
        {
            if (_Variables.GetVariable(NodeAttributeToEvalString(node, "variable", _Variables)) == null)
                _Variables.SetVariable(NodeAttributeToEvalString(node, "variable", _Variables), EvaluateVarFromString(NodeAttributeToEvalString(node, "value", _Variables), _Variables));
            else
            {
                if (_Variables.GetVariable(_Variables.EvalVariableString(node.Attributes["variable"].Value, true)).ToString() == node.Attributes["variable"].Value)
                    SetVarFromString(_Variables.EvalVariableString(node.Attributes["variable"].Value, true), EvaluateVarFromString(_Variables.EvalVariableString(node.Attributes["value"].Value, true), _Variables), _Variables);
                else
                    _Variables.SetVariable(_Variables.EvalVariableString(node.Attributes["variable"].Value, true), EvaluateVarFromString(_Variables.EvalVariableString(node.Attributes["value"].Value, true), _Variables));
            }
        }

        private void EvalCreatePile(bool EvalTop, XmlNode node, Variables _Variables, string cur)
        {
            _Pile p = new _Pile();
            if (node.Attributes["Stack"] != null)
            {
                p.Stack = bool.Parse(node.Attributes["Stack"].Value);
            }
            if (node.Attributes["Visible"] != null)
            {
                p.Visible = bool.Parse(node.Attributes["Visible"].Value);
            }
            _Variables.Piles.Add(_Variables.EvalVariableString(node.Attributes["id"].Value), p);
        }

        private bool EvalGetUserFromGameUserID(bool EvalTop, XmlNode node, Variables _Variables, string cur)
        {
            if (EvalTop)
            {
                foreach (_User myUser in ((List<_User>) _Variables["AllUsers"]))
                {
                    if (myUser.GameUserID == ((int)EvaluateVarFromString(node.Attributes["GameUserID"].Value.ToString(),_Variables)))
                        _Variables.CreateVariable(_Variables.EvalVariableString(node.Attributes["variable"].Value.ToString()), typeof (_User), myUser);
                }
            }
            return true;
        }

        private bool EvalShowInformation(bool EvalTop, XmlNode node, Variables _Variables, string cur)
        {
            if (EvalTop)
            {
                myChatLine line = new myChatLine();
                line.GameUserID = -1;
                line.GameRoomID = (int) _Variables["GameID"];
                line.TimePosted = DateTime.Now;
                line.LineContent = "/sys " + (new myGameUser((int) EvaluateVarFromString(node.Attributes["UserID"].Value.ToString(), _Variables))).GameUserName + " " +
                                   NodeAttributeToEvalString(node, "message", _Variables);
                line.InsertData();
            }
            return true;
        }

        private bool EvalShowAllInformation(bool EvalTop, XmlNode node, Variables _Variables, string cur)
        {
            if (EvalTop)
            {
                myChatLine line = new myChatLine();
                line.GameUserID = -1;
                line.GameRoomID = (int) _Variables["GameID"];
                line.TimePosted = DateTime.Now;
                line.LineContent = _Variables.EvalVariableString(node.Attributes["message"].Value.ToString());
                line.InsertData();
            }
            return true;
        }

        private bool EvalLoopInputAnswer(bool EvalTop, XmlNode node, Variables _Variables, string cur)
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
                    if (!EvaluateNodes(node.ChildNodes, _Variables, cur))
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

        private bool EvalDealCards(bool EvalTop, XmlNode node, Variables _Variables, string cur)
        {
            if (EvalTop)
            {
                List<_Card> temp = new List<_Card>(_Variables.Piles[node.Attributes["Pile"].Value].Cards);


                int cardCount = (int.Parse(_Variables.EvalVariableString(node.Attributes["Cards"].Value)))*((List<_User>) _Variables["AllUsers"]).Count;
                cardCount = cardCount > temp.Count ? temp.Count : cardCount;

                int user = 0;

                List<_User> al = (List<_User>) _Variables["AllUsers"];

                do
                {
                    cardCount--;
                    _Card c = temp[cardCount];
                    if (node.Attributes["Face"] != null)
                        c.Face = node.Attributes["Face"].Value == "DOWN" ? false : true;
                    if (node.Attributes["Peak"] != null)
                        c.Peak = bool.Parse(node.Attributes["Peak"].Value);
                    al[user].Cards.Cards.Add(c);
                    temp.Remove(temp[cardCount]);

                    user++;
                    if (user == Users.Count)
                        user = 0;
                } while (cardCount != 0);

                _Variables["AllUsers"] = al;
                _Variables.Piles[node.Attributes["Pile"].Value].Cards = temp;
            }
            return true;
        }

        private bool EvalIsFalse(bool EvalTop, XmlNode node, Variables _Variables, string cur)
        {
            if (EvalTop)
            {
                if (!((bool)_Variables.GetVariable("EvaluatedExpression" + cur.Substring(0, cur.Length - 1))))
                    if (!EvaluateNodes(node.ChildNodes, _Variables, cur))
                    {
                        return false;
                    }
            }
            else
            {
                if (!EvaluateNodes(node.ChildNodes, _Variables, cur))
                {
                    return false;
                }
            }
            return true;
        }

        private bool EvalIsTrue(bool EvalTop, XmlNode node, Variables _Variables, string cur)
        {
            if (EvalTop)
            {
                if (((bool)_Variables.GetVariable("EvaluatedExpression" + cur.Substring(0, cur.Length - 1))))
                    if (!EvaluateNodes(node.ChildNodes, _Variables, cur))
                    {
                        return false;
                    }
            }
            else
            {
                if (!EvaluateNodes(node.ChildNodes, _Variables, cur))
                {
                    return false;
                }
            }
            return true;
        }

        private bool EvalEvaluate(bool EvalTop, XmlNode node, Variables _Variables, string cur)
        {
            if (EvalTop)
            {
                _Variables.CreateVariable("EvaluatedExpression" + cur, typeof(bool), evaluateString(_Variables.EvalVariableString( node.Attributes["expression"].Value,true), _Variables));
            }
            if (!EvaluateNodes(node.ChildNodes, _Variables, cur))
            {
                return false;
            }
            return true;
        }



        private bool evaluateString(string str, Variables _Variables)
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
                        object peice = EvaluateVarFromString(s1, _Variables);
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
                        string peice = EvaluateVarFromString(s1, _Variables).ToString();

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
                        string peice = EvaluateVarFromString(s1, _Variables).ToString();

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
                        object peice = EvaluateVarFromString(s1, _Variables);
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
                        string peice = EvaluateVarFromString(s1, _Variables).ToString();

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
                        string peice = EvaluateVarFromString(s1, _Variables).ToString();

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

        private object EvaluateVarFromString(string str, Variables _Variables)
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

            if (isSingleVar )
                return _Variables.EvalVariable(str);

            if (str.Contains(donts[0].ToString()))
            {
                string[] s = str.Split(donts[0]);
                object r = null;
                foreach (string s1 in s)
                {
                    int val;
                    string peice = EvaluateVarFromString(s1, _Variables).ToString();
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
                    if (s1.Trim(' ') == "")
                    {
                        r = 0;
                    }
                    else
                    {
                        string peice = EvaluateVarFromString(s1, _Variables).ToString();
                        int val;
                        if (int.TryParse(peice, out val))
                        {
                            if (r == null)
                                r = 0;

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
                }
                return r;
            }
            if (str.Contains(donts[2].ToString()))
            {
                string[] s = str.Split(donts[2]);
                int r = 0;

                string peice = EvaluateVarFromString(s[0], _Variables).ToString();
                if (int.TryParse(peice, out r))
                {
                    
                }

                int val;
                peice = EvaluateVarFromString(s[1], _Variables).ToString();
                if (int.TryParse(peice, out val))
                {
                    r = r/val;
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
                    string peice = EvaluateVarFromString(s1, _Variables).ToString();
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
        private bool EvalRound(bool EvalTop, XmlNode node, Variables _Variables, string cur)
        {
            int roundNumber = int.Parse(node.Attributes["number"].Value);
            if (roundNumber == -1)
            {
                do
                {
                 //   Variables tempVars = (Variables) _Variables.Clone();
                    if (EvalTop)
                    {
                        roundNumber++;
                        _Variables.CreateVariable("RoundNumber",typeof(int),roundNumber);
                    }
                    if (!EvaluateNodes(node.ChildNodes, _Variables, cur))
                    {
                        return false;
                    }
                    _Variables.DestroyVariable("RoundNumber");
                    EvalTop = true;
                } while (true);
            }
            else
            {
                if (!EvaluateNodes(node.ChildNodes, _Variables, cur))
                {
                    return false;
                }
            }

            return true;
        }

        private bool EvalGame(bool EvalNode, XmlNode node, Variables _Variables, string cur)
        {
            if (EvalNode)
            {
                _Variables.CreateVariable("RoundNumber", typeof(int), 0);
            }

            if (!EvaluateNodes(node.ChildNodes, _Variables, cur))
            {
                return false;
            }
            _Variables.DestroyVariable("RoundNumber");

            return true;
        }

        private void EvalCard(bool EvalTop, XmlNode node, Variables _Variables)
        {
            _Card card = new _Card();
            string _spot =  _Variables.GetVariable("Spot").ToString();
            int outSpot;

            if (_spot == "Spot" || _spot == "TakeLast")
                card = ((_Pile) _Variables.GetVariable("FromLocation")).Cards[0];
            else if (_spot == "TakeFirst")
                card = ((_Pile) _Variables.GetVariable("FromLocation")).Cards[((_Pile) _Variables.GetVariable("FromLocation")).Cards.Count - 1];
            else if (int.TryParse(_spot, out outSpot))
                card = ((_Pile)_Variables.GetVariable("FromLocation")).Cards[outSpot];
            else
            {
                foreach (_Card myCard in ((_Pile) _Variables.GetVariable("FromLocation")).Cards)
                {
                    if (myCard.getCardName == _spot)
                    {
                        card = myCard;
                        break;
                    }
                }
            }


            card.Face = node.Attributes["Face"].Value == "DOWN" ? false : true;
            if (node.Attributes["Peak"] != null)
                card.Peak = bool.Parse(node.Attributes["Peak"].Value);

            _Variables["LastCard"] = card;

            ((_Pile) _Variables.GetVariable("ToLocation")).Cards.Add(card);

            if (_spot == "Spot" || _spot == "TakeLast")
                ((_Pile) _Variables.GetVariable("FromLocation")).Cards.RemoveAt(0);
            else if (_spot == "TakeFirst")
                ((_Pile) _Variables.GetVariable("FromLocation")).Cards.RemoveAt(((_Pile) _Variables.GetVariable("FromLocation")).Cards.Count - 1);
            else if (int.TryParse(_spot, out outSpot))
                ((_Pile)_Variables.GetVariable("FromLocation")).Cards.RemoveAt(outSpot);
            else
            {
                foreach (_Card myCard in ((_Pile)_Variables.GetVariable("FromLocation")).Cards)
                {
                    if (myCard.getCardName == _spot)
                    {
                        ((_Pile) _Variables.GetVariable("FromLocation")).Cards.Remove(myCard);
                        break;
                    }
                }
            }
        }

        private bool EvalMoveCards(bool EvalTop, XmlNode node, Variables _Variables, string cur)
        {
            string from = _Variables.EvalVariableString(node.Attributes["FromLocation"].Value,true);
            string to = _Variables.EvalVariableString(node.Attributes["ToLocation"].Value, true);
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

                
                if (node.Attributes["Spot"] != null)
                    _Variables.CreateVariable("Spot", typeof(string), EvaluateVarFromString(_Variables.EvalVariableString(node.Attributes["Spot"].Value,true), _Variables));

                _Variables.CreateVariable("LastCard", typeof(_Card), new _Card());

                if (!EvaluateNodes(node.ChildNodes, _Variables, cur))
                {
                    return false;
                }

                _Variables.DestroyVariable("LastCard");

                if (_Variables.Piles.ContainsKey(to))
                    _Variables.Piles[to] = (_Pile)_Variables.GetVariable("ToLocation");
                else if (_Variables.EvalVariable(to) is ArrayList)
                    SetVarFromString(to, myHelper.ListToArrayList(((_Pile)_Variables.GetVariable("ToLocation")).Cards), _Variables);
                else if (!_Variables.Piles.ContainsKey(to))
                    SetVarFromString(to, (_Pile)_Variables.GetVariable("ToLocation"), _Variables);



                if (_Variables.Piles.ContainsKey(from))
                    _Variables.Piles[from] = (_Pile)_Variables.GetVariable("FromLocation");
                else if (_Variables.EvalVariable(from) is ArrayList)
                    SetVarFromString(from, myHelper.ListToArrayList(((_Pile)_Variables.GetVariable("FromLocation")).Cards), _Variables);
                else if (!_Variables.Piles.ContainsKey(to))
                    SetVarFromString(from, (_Pile)_Variables.GetVariable("FromLocation"), _Variables);

                
                _Variables.DestroyVariable("Spot");
                _Variables.DestroyVariable("ToLocation");
                _Variables.DestroyVariable("FromLocation");
            }
            else
            {
                _Variables.DestroyVariable("LastCard");

                if (_Variables.EvalVariable(to) is _Pile)
                    _Variables.Piles[to] = (_Pile)_Variables.GetVariable("ToLocation");
                else if (_Variables.EvalVariable(to) is ArrayList)
                    SetVarFromString(to, myHelper.ListToArrayList(((_Pile)_Variables.GetVariable("ToLocation")).Cards), _Variables);
                else if (!_Variables.Piles.ContainsKey(to))
                    SetVarFromString(to, (_Pile)_Variables.GetVariable("ToLocation"), _Variables);



                if (_Variables.EvalVariable(from) is _Pile)
                    _Variables.Piles[from] = (_Pile)_Variables.GetVariable("FromLocation");
                else if (_Variables.EvalVariable(from) is ArrayList)
                    SetVarFromString(from, myHelper.ListToArrayList(((_Pile)_Variables.GetVariable("FromLocation")).Cards), _Variables);
                else if (!_Variables.Piles.ContainsKey(to))
                    SetVarFromString(from, (_Pile)_Variables.GetVariable("FromLocation"), _Variables);


                _Variables.DestroyVariable("Spot");
                _Variables.DestroyVariable("ToLocation");
                _Variables.DestroyVariable("FromLocation");
            }
            return true;
        }

        private bool EvalGetUserInput(bool EvalTop, XmlNode node, Variables _Variables, string cur)
        {
            if (EvalTop)
            {
                if (node.Attributes["default"] == null)

                    _Variables.CreateVariable(node.Attributes["id"].Value, getTypeFromString(node.Attributes["type"].Value), null);
                else
                    _Variables.CreateVariable(node.Attributes["id"].Value, getTypeFromString(node.Attributes["type"].Value), EvaluateVarFromString(_Variables.EvalVariableString(node.Attributes["default"].Value, true), _Variables));

                _Variables.CreateVariable("PossibleAnswers", typeof(List<_PossibleAnswer>), new List<_PossibleAnswer>());
                if (!EvaluateNodes(node.ChildNodes, _Variables, cur))
                {
                    return false;
                }
                GetUserInput((_User)_Variables.GetVariable("User"), node.Attributes["question"].Value,
                             (List<_PossibleAnswer>)_Variables.GetVariable("PossibleAnswers"), _Variables, cur);

                myQuestion q = new myQuestion((int)_Variables["QuestionID"]);
                q.VariableInfo = _Variables.ToString();
                
                q.UpdateData();


                return false;
            }
            else
            {
                myQuestion m = new myQuestion((int)_Variables.GetVariable("QuestionID"));
                if (m.QuestionName.Split('|')[0] == cur)
                {
                    m.DeleteQuestion();

                    foreach (_PossibleAnswer answer in (List<_PossibleAnswer>) _Variables.GetVariable("PossibleAnswers"))
                    {
                        if (answer.PossibleAnswer == int.Parse(m.AnswerChosen))
                            _Variables.SetVariable(node.Attributes["id"].Value, EvaluateVarFromString(answer.getPossibleAnswer().PossibleAnswerValue, _Variables));
                    }
                    _Variables.DestroyVariable("PossibleAnswers");
                    _Variables.DestroyVariable("QuestionID");
                }
            }
            return true;
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


        private bool EvalGetAllUserInput(bool EvalTop, XmlNode node, Variables _Variables, string cur)
        {
            if (EvalTop)
            {
                _Variables.CreateVariable(node.Attributes["id"].Value, getTypeFromString(node.Attributes["type"].Value), null);

                _Variables.CreateVariable("PossibleAnswers", typeof(List<_PossibleAnswer>), new List<_PossibleAnswer>());


                List<_User> al = (List<_User>)_Variables["AllUsers"];

                foreach (_User myUser in al)
                {
                    _Variables.CreateVariable("_UserID", typeof(int), myUser.GameUserID);
                    if (!EvaluateNodes(node.ChildNodes, _Variables, cur))
                    {
                        return false;
                    }
                }
                _Variables.DestroyVariable("_UserID");
                GetAllUserInput(node.Attributes["question"].Value, (List<_PossibleAnswer>)_Variables.GetVariable("PossibleAnswers"), _Variables, cur);

                myQuestion q = new myQuestion((int)_Variables["QuestionID"]);
                q.VariableInfo = _Variables.ToString();
                q.UpdateData();

                return false;
            }
            else
            {
                myQuestion m = new myQuestion((int) _Variables.GetVariable("QuestionID"));
                m.DeleteQuestion();

                foreach (string answer_ in m.AnswerChosen.Split('_'))
                {
                    if (answer_ != "")
                    {
                        _PossibleAnswer p = new _PossibleAnswer(int.Parse(answer_.Split('|')[0]));

                        if (p.PossibleAnswer == int.Parse(answer_.Split('|')[0]))
                        {
                            object str = EvaluateVarFromString(p.getPossibleAnswer().PossibleAnswerValue, _Variables);
                            _Variables.CreateVariable(node.Attributes["id"].Value + "|" + int.Parse(answer_.Split('|')[1]), _Variables.GetVariable(node.Attributes["id"].Value).GetType(), str.ToString().Split('|')[0]);
                        }
                    }
                    _Variables.DestroyVariable("PossibleAnswers");
                    _Variables.DestroyVariable("QuestionID");
                }
            }
            return true;
        }

        private void GetAllUserInput( string question, List<_PossibleAnswer> list, Variables _Variables, string cur)
        {
            myQuestion q = new myQuestion();
            q.QuestionName = cur + "|" + question;
            q.GameUserID =-1;
            q.AnswerChosen = ""; 
            q.GameRoomID = (int)_Variables["GameID"];
            list.ForEach(delegate (_PossibleAnswer p){q.PossibleAnswers.Add(p.getPossibleAnswer());});
            q.InsertData();

            list.ForEach(delegate(_PossibleAnswer p)
            {
                myPossibleAnswer a = p.getPossibleAnswer();
                a.QuestionID = q.QuestionID;
                a.UpdateData();
            });

            _Variables.CreateVariable("QuestionID", typeof(int), q.QuestionID);
        }


        private bool EvalPossibleAnswer(bool EvalTop, XmlNode node, Variables _Variables, string cur)
        {
            if (EvalTop)
            {
                myPossibleAnswer posAns = new myPossibleAnswer();
                posAns.PossibleAnswerContent = _Variables.EvalVariableString(node.Attributes["name"].Value);
                if (_Variables["_UserID"].ToString() != "_UserID")
                    posAns.PossibleAnswerValue = _Variables.EvalVariableString(node.Attributes["value"].Value) + "|" + _Variables["_UserID"];
                else
                    posAns.PossibleAnswerValue = _Variables.EvalVariableString(node.Attributes["value"].Value);
                posAns.InsertData();
                List<_PossibleAnswer> lst = ((List<_PossibleAnswer>)_Variables.GetVariable("PossibleAnswers"));
                lst.Add(new _PossibleAnswer(posAns.PossibleAnswerID));
                _Variables.SetVariable("PossibleAnswers", lst);
            }
            if (!EvaluateNodes(node.ChildNodes, _Variables, cur))
            {
                return false;
            }
            return true;
        }

        private bool EvalCheckWinner(bool EvalTop, XmlNode node, Variables _Variables, string cur)
        {
            if (EvalTop)
            {
            }
            if (!EvaluateNodes(((XmlNode)_Variables.GetVariable("WinType" + node.Attributes["id"].Value)).ChildNodes, _Variables, cur))
            {
                return false;
            }

            return true;
        }

        private bool EvalDeclareWinner(bool EvalTop, XmlNode node, Variables _Variables, string cur)
        {
            if (EvalTop)
            {
            }

            if (!EvaluateNodes(node.ChildNodes, _Variables, cur))
            {
                return false;
            }
            _Variables.CreateVariable("WinningUser", typeof(_User), _Variables.GetVariable(node.Attributes["User"].Value));

            return true;
        }

        private bool EvalCardGame(bool EvalTop, XmlNode node, Variables _Variables, string cur)
        {
            int max = 100;
            int min = 0;
            bool KingIsTen=false;
            if (node.Attributes["MaxPlayers"] != null)
                max = int.Parse(node.Attributes["MaxPlayers"].Value);
            if (node.Attributes["KingIsTen"] != null)
                KingIsTen = bool.Parse(node.Attributes["KingIsTen"].Value);
            if (node.Attributes["MinPlayers"] != null)
                min = int.Parse(node.Attributes["MinPlayers"].Value);

            if (max >= Users.Count && min <= Users.Count)
            {
                if (EvalTop)
                {
                    _Variables.CreateVariable("AllUsers", typeof (List<_User>), Users);
                    _Variables.CreateVariable("CardGameName", typeof (string), node.Attributes["name"].Value);
                    _Variables.CreateVariable("KingIsTen", typeof (bool), KingIsTen);
                }

                if (!EvaluateNodes(node.ChildNodes, _Variables, cur))
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

        private bool EvalDeal(bool EvalTop, XmlNode node, Variables _Variables, string cur)
        {
            if (EvalTop)
            {
                _Pile p = new _Pile();
                
                if (node.Attributes["Cards"] == null)
                    p.Deal(52,(bool) _Variables["KingIsTen"]);
                else
                    p.Deal(int.Parse(node.Attributes["Cards"].Value), (bool)_Variables["KingIsTen"]);
                p.Visible = false;
                _Variables.Piles.Add("Deck", p);
            }
            if (!EvaluateNodes(node.ChildNodes, _Variables, cur))
            {
                return false;
            }
            return true;
        }

        private bool EvalLoopCards(bool EvalTop, XmlNode node, Variables _Variables, string cur)
        {
            List<_Card> temp;

            string pile = _Variables.EvalVariableString(node.Attributes["Pile"].Value, true);

            if (_Variables.EvalVariable(pile) is _Pile)
                temp = new List<_Card>(_Variables.EvalVariable<_Pile>(pile).Cards);
            else
                temp = new List<_Card>(myHelper.ArrayListToList<_Card>(_Variables.EvalVariable<ArrayList>(pile)));

            foreach (_Card _c in temp)
            {
                if (EvalTop)
                {
                    _Variables.CreateVariable("CurrentCard", typeof (_Card), _c);
                }
                if (!EvaluateNodes(node.ChildNodes, _Variables, cur))
                {
                    return false;
                }
                _Variables.DestroyVariable("CurrentCard");
            }
            return true;
        }

        private bool EvalLoopUsers(bool EvalTop, XmlNode node, Variables _Variables, string cur)
        {
            List<_User> al = (List<_User>) _Variables["AllUsers"];
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
                    if (!EvaluateNodes(node.ChildNodes, _Variables, cur))
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
        private static Type getTypeFromString(string value)
        {
            switch (value)
            {
                case "bool":
                    return typeof(bool);
                case "int":
                    return typeof(int);
                case "string":
                    return typeof(string);
                case "Pile":
                    return typeof(_Pile);
                case "_Pile":
                    return typeof(_Pile);
                case "_User":
                    return typeof(_User);
                case "_Card":
                    return typeof(_Card);
                case "User":
                    return typeof(_User);
                case "Card":
                    return typeof(_Card);
            }
            return null;
        }
    }



    public class Variables : ICloneable, IComparable<Variables>
    {
        public Dictionary<string, Type> VarTypes = new Dictionary<string, Type>();
        public Dictionary<string, object> Vars = new Dictionary<string, object>();
        public Dictionary<string, _Pile> Piles = new Dictionary<string, _Pile>();

        public int CompareTo(Variables v)
        {
            bool pilesSame = true;
            bool varsSame = true;
            foreach (KeyValuePair<string, object> var in v.Vars)
            {
                if (!Vars[var.Key].Equals(var.Value))
                {
                    varsSame = false;
                }
            }

            foreach (KeyValuePair<string, _Pile> var in v.Piles)
            {
                if (!Piles[var.Key].Equals(var.Value))
                {
                    pilesSame = false;
                }
            }

            if (varsSame && pilesSame)
                return 1;

            return 0;
        }

        public Variables(string str)
        {
            XmlDocument d = new XmlDocument();
            d.InnerXml = "<?xml version=\"1.0\" encoding=\"utf-16\"?>" + str;
            XmlNode n = d.GetElementsByTagName("Vars")[0];

            foreach (XmlNode node in n.ChildNodes)
            {
                VarTypes.Add(node.Attributes["name"].Value, Type.GetType(node.Attributes["type"].Value));
                if (node.InnerXml.Contains("<"))
                {
                    if (node.Attributes["type"].Value.Contains("Xml"))
                        node.Attributes["type"].Value +=
                            ", System.Xml, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
                    XmlSerializer x = new XmlSerializer(Type.GetType(node.Attributes["type"].Value), new Type[] { typeof(_Card), typeof(_Pile), typeof(_PossibleAnswer), typeof(ArrayList), typeof(TimeSpan) });
                    try
                    {
                        TextReader t = new StringReader("<?xml version=\"1.0\" encoding=\"utf-16\"?>" + node.InnerXml);
                        if (VarTypes[node.Attributes["name"].Value] == typeof(ArrayList))
                            Vars.Add(node.Attributes["name"].Value, (ArrayList) x.Deserialize(t));
                        else
                            Vars.Add(node.Attributes["name"].Value, x.Deserialize(t));
                    }
                    catch (Exception e)
                    {
                    }
                }
                else
                {
                    Vars.Add(node.Attributes["name"].Value, node.InnerXml);
                }
            }

            n = d.GetElementsByTagName("Piles")[0];

            foreach (XmlNode node in n.ChildNodes)
            {
                XmlSerializer x = new XmlSerializer(Type.GetType(node.Attributes["type"].Value));

                try
                {
                    TextReader t =
                        new StringReader("<?xml version=\"1.0\" encoding=\"utf-16\"?>" + node.InnerXml);
                    Piles.Add(node.Attributes["name"].Value, (_Pile)x.Deserialize(t));
                }
                catch (Exception e)
                {
                }
            }
        }

        public override string ToString()
        {
            XmlDocument d = new XmlDocument();
            XmlNode parent = d.CreateElement("Variables");

            XmlNode n = d.CreateElement("Vars");
            foreach (KeyValuePair<string, object> var in Vars)
            {
                try
                {
                    XmlNode n1 = d.CreateNode(XmlNodeType.Element, "VariableEntry", "");
                    XmlAttribute a1 = d.CreateAttribute("name");

                    a1.Value = var.Key;
                    n1.Attributes.Append(a1);
                    a1 = d.CreateAttribute("type");
                    a1.Value = VarTypes[var.Key].ToString();
                    n1.Attributes.Append(a1);

                    XmlSerializer x = new XmlSerializer(VarTypes[var.Key]);

                    if(var.Value is ArrayList)
                    {
                        List<Type> listOfTypes =  buildListFromArrayList(var.Value);
                        x = new XmlSerializer(VarTypes[var.Key],listOfTypes.ToArray());
                    }
                    TextWriter t = new StringWriter();
                    try
                    {
                        x.Serialize(t, var.Value);
                        n1.InnerXml = t.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
                    }
                    catch (Exception e)
                    {
                        n1.InnerText = var.Value.ToString();
                    }
                    n.AppendChild(n1);
                }catch(Exception e)
                {
                    
                }
            }
            parent.AppendChild(n);
            n = d.CreateElement("Piles");
            foreach (KeyValuePair<string, _Pile> var in Piles)
            {
                XmlNode n1 = d.CreateNode(XmlNodeType.Element, "VariableEntry", "");
                XmlAttribute a1 = d.CreateAttribute("name");
                a1.Value = var.Key;
                n1.Attributes.Append(a1);
                a1 = d.CreateAttribute("type");
                a1.Value = typeof(_Pile).ToString();
                n1.Attributes.Append(a1);
                XmlSerializer x = new XmlSerializer(typeof(_Pile));
                TextWriter t = new StringWriter();
                try
                {
                    x.Serialize(t, var.Value);
                    n1.InnerXml = t.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
                }
                catch (Exception e)
                {
                    n1.InnerText = var.Value.ToString();
                }
                n.AppendChild(n1);
            }
            parent.AppendChild(n);
            d.AppendChild(parent);


            for (int i = d.GetElementsByTagName("VariableInfo").Count - 1; i >= 0;i-- )
            {
                XmlNode node = d.GetElementsByTagName("VariableInfo")[i];
                node.ParentNode.RemoveChild(node);
            }

            return d.InnerXml;
        }

        private List<Type> buildListFromArrayList(object value)
        {
            List<Type> listOfTypes = new List<Type>();
            listOfTypes.Add(typeof(ArrayList));
            foreach (object myObject in ((ArrayList)value))
            {
                if (!listOfTypes.Contains(myObject.GetType()))
                {
                    listOfTypes.Add(myObject.GetType());
                }
                if (myObject is ArrayList)
                    listOfTypes.AddRange(buildListFromArrayList(myObject));
            }
            return listOfTypes;
        }

        public Variables()
        {
        }

        public object GetVariable(string name)
        { 
            string split = "";
            if (name.Contains("|") && !name.Contains("||"))
            {
                split = name.Split('|')[1];
                name = name.Split('|')[0];
            }

            bool a;
            if (bool.TryParse(name, out a))
            {
                return a;
            }

            
            if (!Vars.ContainsKey(name))
                if (Piles.ContainsKey(name))
                    return Piles[name];
                else
                    return name;

            if (Vars[name].ToString() == "null")
                return null;


            if (Vars[name].ToString() == "new")
                return VarTypes[name].GetConstructor(Type.EmptyTypes).Invoke(null);
            

            if (bool.TryParse(Vars[name].ToString(), out a))
            {
                return a;
            }

            int i;
            if (int.TryParse(Vars[name].ToString(), out i))
            {
                return i;
            }


            if (split == "")
                return Vars[name];
            else
            {
                split = EvalVariableString(split, true);
                int r;
                if (int.TryParse(split, out r))
                    return Vars[name].GetType().GetProperty("Item").GetGetMethod().Invoke(Vars[name], new object[] {r});
                else
                   return Vars[name].GetType().GetProperty("Item").GetGetMethod().Invoke(Vars[name], new object[] {split});
            }
        }

        public object this[string name]
        {
            get
            {
                return GetVariable(name);
            }
            set
            {
                SetVariable(name, value);
            }
        }

        public void CreateVariable(string name, Type t, object value)
        {
            DestroyVariable(EvalVariableString(name, true));
            VarTypes.Add(EvalVariableString(name, true), t);

            if (value.ToString() == "new")
                Vars.Add(EvalVariableString(name, true), VarTypes[name].GetConstructor(Type.EmptyTypes).Invoke(null));
            else
                Vars.Add(EvalVariableString(name, true), value);
        }

        public void SetVariable(string name, object value)
        {
            if (Vars.ContainsKey(EvalVariableString(name,true)))
                Vars[EvalVariableString(name, true)] = value;
            else
                Piles[EvalVariableString(name, true)] = (_Pile)value;
        }

        public void DestroyVariable(string name)
        {
            if (Piles.ContainsKey(name))
            {
                Piles.Remove(name);
                return;
            }
            foreach (KeyValuePair<string, object> myPair in new Dictionary<string,object>(Vars))
            { 
                if (myPair.Key.Contains("|"))
                {
                    if (myPair.Key.Split('|')[0] == name)
                    {
                        Vars.Remove(myPair.Key);
                        VarTypes.Remove(myPair.Key);
                    }
                }
            }

            if (Vars.ContainsKey(name))
            {
                Vars.Remove(name);
                VarTypes.Remove(name);
            }
        }

        public object Clone()
        {
            Variables var = new Variables();
            var.Vars = new Dictionary<string, object>(Vars);
            var.VarTypes = new Dictionary<string, Type>(VarTypes);
            var.Piles = Piles;
            return var;
        }

        public T EvalVariable<T>(string str)
        {
            if (str.Contains(".") && !str.Contains(".."))

            {
                return (T)
                    EvalObjectVariable(str.Substring(0, str.IndexOf('.')), str.Substring(str.IndexOf('.') + 1, str.Length - str.IndexOf('.') - 1));
            }
            else
            {
                return (T)GetVariable(str);
            }
        }

        public object EvalVariable(string str)
        {
            if (str.Contains(".") && !str.Contains("..") )
            {
                return
                    EvalObjectVariable(str.Substring(0, str.IndexOf('.')), str.Substring(str.IndexOf('.') + 1, str.Length - str.IndexOf('.') - 1));
            }
            else
            {
                return GetVariable(str);
            }
        }

        public string EvalVariableString(string str)
        {
            if (str.Contains("["))
            {
                string var = str.Substring(str.IndexOf('[') + 1, FindLimitedNext(str, ']', '[') - str.IndexOf('[') - 1);
                str = str.Replace("[" + var + "]", (EvalVariableString(var)));
                str = EvalVariableString(str);
            }
            return EvalVariable(str).ToString();
        }
        public string EvalVariableString(string str, bool returnString)
        {
            if (str.Contains("["))
            {
                string var = str.Substring(str.IndexOf('[') + 1, FindLimitedNext(str, ']', '[') - str.IndexOf('[') - 1);
                str = str.Replace("[" + var + "]", (EvalVariableString(var)));
                str = EvalVariableString(str,returnString);
            }
            if (returnString)
                return str;
            else
                return EvalVariable(str).ToString();
        }

        private int FindLimitedNext(string str, char Find, char After)
        {
            int count = 0;
            int i;
            for (i = str.IndexOf(After); i < str.Length; i++)
            {
                if (str[i] == After)
                    count++;

                if (str[i] == Find)
                    count--;

                if (count == 0)
                {
                    count = i;
                    break;
                }
            }
            return count;
        }

        public object EvalObjectVariable(string str, string rest)
        {
            string[] s = rest.Split('.');

            //hack for user to gameuser
            object o;

            string split = "";
            if (str.Contains("|") && !str.Contains("||"))
            {
                split = str.Split('|')[1];
                str = str.Split('|')[0];
            }
 
            o = GetVariable(str);

            if (o.GetType().Name == "_User")
            {
                foreach (MethodInfo myInfo in o.GetType().GetMethods())
                {
                    if (myInfo.Name == "GetUser" && myInfo.GetParameters().Length == 0)
                    {
                        o = myInfo.Invoke(o, null);
                        for (int myInt = 0; myInt < s.Length; myInt++)
                        {
                            string s1 = s[myInt];
                         
                            
                            if (o.GetType().GetProperty(s1) != null)
                                o = o.GetType().GetProperty(s1).GetValue(o, null);
                            else if (o.GetType().GetField(s1) != null)
                                o = o.GetType().GetField(s1).GetValue(o);
                            else
                                o = GetVariable(str);

                            if (split != "")
                         
                            {
                                int r;
                                if (int.TryParse(split, out r))
                                    return o.GetType().GetProperty("Item").GetGetMethod().Invoke(o, new object[] { r });
                                else
                                    return o.GetType().GetProperty("Item").GetGetMethod().Invoke(o, new object[] { split });
                            }


            
                        }
                        break;
                    }
                }
                if (o!=GetVariable(str))
                {
                    return o;
                }
            }
            if (split != "")
            {
                split = EvalVariableString(split, true);
                int r;
                if (int.TryParse(split, out r))
                    o= Vars[str].GetType().GetProperty("Item").GetGetMethod().Invoke(Vars[str], new object[] { r });
                else
                    o=Vars[str].GetType().GetProperty("Item").GetGetMethod().Invoke(Vars[str], new object[] { split });
            }


            foreach (string s1 in s)
            {
                if (o.GetType().GetProperty(s1) != null)
                    o = o.GetType().GetProperty(s1).GetValue(o, null);
                else if (o.GetType().GetField(s1) != null)
                    o = o.GetType().GetField(s1).GetValue(o);
                else
                    return null;
            }


            return o;
        }

        public void SetEvalObjectVariable(string str, string rest, object obj)
        {
            string split = "";
            if (str.Contains("|") && !str.Contains("||"))
            {
                split = str.Split('|')[1];
                str = str.Split('|')[0];
            }

            if (split == "")
            {
                string[] s = rest.Split('.');
                object o = GetVariable(str);
                foreach (string s1 in s)
                {
                    if (s1 != s[s.Length - 1])
                    {
                        if (o.GetType().GetProperty(s1) != null)
                            o = o.GetType().GetProperty(s1).GetValue(o, null);
                        else if (o.GetType().GetField(s1) != null)
                            o = o.GetType().GetField(s1).GetValue(o);
                    }
                    else
                    {
                        if (o.GetType().GetProperty(s1) != null)
                            o.GetType().GetProperty(s1).SetValue(o, obj, null);
                        else if (o.GetType().GetField(s1) != null)
                            o.GetType().GetField(s1).SetValue(o, obj);
                    }
                }
            }

            else
            {
                int r;
                if (int.TryParse(split, out r))
                    Vars[str].GetType().GetProperty("Item").GetSetMethod().Invoke(Vars[str], new object[] {r, obj});
                else
                    Vars[str].GetType().GetProperty("Item").GetSetMethod().Invoke(Vars[str], new object[] {split, obj});
            }
        }

        private bool evaluateString(string str, Variables _Variables)
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
                    int val;
                    object peice = EvaluateVarFromString(s1, _Variables);
                    if (r == null)
                        r = peice;
                    else
                        return r == peice;
                }
                return false;
            }
            else
                if (str.Contains(donts[1].ToString()))
                {
                    string[] temp = { donts[1] };
                    string[] s = str.Split(temp, StringSplitOptions.RemoveEmptyEntries);
                    int r = 0;
                    foreach (string s1 in s)
                    {
                        int val;
                        string peice = EvaluateVarFromString(s1, _Variables).ToString();

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
                else

                    if (str.Contains(donts[2].ToString()))
                    {
                        string[] temp = { donts[2] };
                        string[] s = str.Split(temp, StringSplitOptions.RemoveEmptyEntries);
                        int r = 0;
                        foreach (string s1 in s)
                        {
                            int val;
                            string peice = EvaluateVarFromString(s1, _Variables).ToString();

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
                    else

                        if (str.Contains(donts[3].ToString()))
                        {
                            string[] temp = { donts[3] };
                            string[] s = str.Split(temp, StringSplitOptions.RemoveEmptyEntries);
                            object r = null;
                            foreach (string s1 in s)
                            {
                                object peice = EvaluateVarFromString(s1, _Variables);
                                if (r == null)
                                    r = peice;
                                else
                                    return r.ToString() != peice.ToString();
                            }
                            return false;
                        }
                        else

                            if (str.Contains(donts[4].ToString()))
                            {
                                string[] temp = { donts[4] };
                                string[] s = str.Split(temp, StringSplitOptions.RemoveEmptyEntries);
                                int r = 0;
                                foreach (string s1 in s)
                                {
                                    int val;
                                    string peice = EvaluateVarFromString(s1, _Variables).ToString();

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
                            else

                                if (str.Contains(donts[5].ToString()))
                                {
                                    string[] temp = { donts[5] };
                                    string[] s = str.Split(temp, StringSplitOptions.RemoveEmptyEntries);
                                    int r = 0;
                                    foreach (string s1 in s)
                                    {
                                        int val;
                                        string peice = EvaluateVarFromString(s1, _Variables).ToString();

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

        private object EvaluateVarFromString(string str, Variables _Variables)
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
                return _Variables.EvalVariable(str);

            if (str.Contains(donts[0].ToString()))
            {
                string[] s = str.Split(donts[0]);
                object r = null;
                foreach (string s1 in s)
                {
                    int val;
                    string peice = EvaluateVarFromString(s1, _Variables).ToString();
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
                    string peice = EvaluateVarFromString(s1, _Variables).ToString();
                    if (int.TryParse(peice, out val))
                    {
                        if (r == null)
                            r = 0;

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
                return r;
            }
            if (str.Contains(donts[2].ToString()))
            {
                string[] s = str.Split(donts[2]);
                int r = 0;
                foreach (string s1 in s)
                {
                    int val;
                    string peice = EvaluateVarFromString(s1, _Variables).ToString();
                    if (int.TryParse(peice, out val))
                    {
                        r = r * val;
                    }
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
                    string peice = EvaluateVarFromString(s1, _Variables).ToString();
                    if (int.TryParse(peice, out val))
                    {
                        r = r / val;
                    }
                }
                return r;
            }
            return new object();
            // _Variables.GetVariable() 
        }
    }

    [System.Serializable()]
    public class _User
    {
        private _Pile myCards;
        public _Pile Cards
        {
            get
            {
                return myCards;
            }
            set
            {
                myCards = value;
            }
        }

        private int myGameUserID;
        public int GameUserID
        {
            get
            {
                return myGameUserID;
            }
            set
            {
                myGameUserID = value;
            }
        }


        
        public myGameUser GetUser()
        {
            return new myGameUser(myGameUserID);
        }

        public myGameUser GetUser(string name)
        {
            myGameUser u = new myGameUser();
            u.GetGameUserByGameUserName(name);
            return u;
        }
        
        public _User()
        {
            myCards = new _Pile();
        }
        public _User(int id)
        {
            myGameUserID = id;
            myCards = new _Pile();
        }

        /*        public _User(string s, string s1, ref string result, ref int res)
                    : base(s, s1, ref result, ref res)
                {
                    myCards = new _Pile();
                }* /
    }
    [System.Serializable()]
    public class _PossibleAnswer
    { 

        private int myPossibleAnswer;
        public int PossibleAnswer
        {
            get
            {
                return myPossibleAnswer;
            }
            set
            {
                myPossibleAnswer= value;
            }
        }



        public myPossibleAnswer getPossibleAnswer()
        {
            return new myPossibleAnswer(myPossibleAnswer);
        }


        public _PossibleAnswer(int answerID)
        {
            myPossibleAnswer = answerID;
        }
        public _PossibleAnswer()
        {
        }  
    }
    public class _Pile
    {
        private bool myStack=false;
        private bool myVisible = true;
        private string mySort = "";
        public void Deal(int cardCount,bool kingIsTen)
        {
            int a = 1;
            for (int i = 1; i * a <= 52; )
            {
                _Card c = new _Card();
                c.Type = a;
                c.Value = i;
                c.KingIsTen = kingIsTen;
                Cards.Add(c);
                if (i == 13)
                {
                    i = 0;
                    a++;
                }
                if (a == 5)
                    i = 100;
                i++;
            }
            shuffle();

            for (int i = 51; i >= cardCount; i--)
            {
                Cards.RemoveAt(i);
            }

        }

        public _Pile shuffle()
        {
            Random r = new Random();
            for (int i = 0; i <= 20; i++)
            {
                myShuffle(r);
            }
            return myShuffle(r);
        }

        private _Pile myShuffle(Random r)
        {
            int count = 0;
            for (int i = 0; i < myCards.Count; i++)
            {

                    _Card card = myCards[i];

                    myCards.Remove(card);
                    myCards.Insert(((int)r.Next(myCards.Count)), card);
                
            }
            return this;
        }


        public List<_Card> Cards
        {
            get
            {
                if(mySort=="ASC")
                    myCards.Sort(delegate(_Card c1, _Card c2){return c2.Value > c1.Value?1:0;});


                return myCards;
            }
            set
            {
                myCards = value;
            }
        }

        public bool Stack
        {
            get
            {
                return myStack;
            }
            set
            {
                myStack = value;
            }
        }

        public bool Visible
        {
            get
            {
                return myVisible;
            }
            set
            {
                myVisible = value;
            }
        }

        public string Sort
        {
            get
            {
                return mySort;
            }
            set
            {
                mySort = value;
            }
        }

        private List<_Card> myCards;

        public _Pile()
        {
            Cards = new List<_Card>();
        }
    }

    [System.Serializable()]
    public class _Card : ICloneable
    {
        private int value_;

        public int Value
        {
            get
            {
                return (myKingIsTen ? (value_ > 10 ? 10 : value_) : value_);
            }
            set
            {
                value_ = value;
            }
        }

        public int Type
        {
            get
            {
                return ((type - 1) % 4 + 1);
            }
            set
            {
                type = value;
            }
        }

        private int type;
        public bool Face;
        public bool Peak = true;
        private bool myKingIsTen;


        public string getCardName
        {
            get
            { 

                string str = "";
                switch (value_)
                {
                    case 1:
                        str += "Ace";
                        break;
                    case 2:
                        str += "Deuce";
                        break;
                    case 3:
                        str += "Three";
                        break;
                    case 4:
                        str += "Four";
                        break;
                    case 5:
                        str += "Five";
                        break;
                    case 6:
                        str += "Six";
                        break;
                    case 7:
                        str += "Seven";
                        break;
                    case 8:
                        str += "Eight";
                        break;
                    case 9:
                        str += "Nine";
                        break;
                    case 10:
                        str += "Ten";
                        break;
                    case 11:
                        str += "Jack";
                        break;
                    case 12:
                        str += "Queen";
                        break;
                    case 13:
                        str += "King";
                        break;
                }
                str += " Of ";
                switch (Type)
                {
                    case 1:
                        str += "Hearts";
                        break;
                    case 2:
                        str += "Clubs";
                        break;
                    case 3:
                        str += "Diamonds";
                        break;
                    case 4:
                        str += "Spades";
                        break;
                }
                return str;
            }
        }


        public string ValueName
        {
            get
            { 

                string str = "";
                switch (value_)
                {
                    case 1:
                        str += "Ace";
                        break;
                    case 2:
                        str += "Two";
                        break;
                    case 3:
                        str += "Three";
                        break;
                    case 4:
                        str += "Four";
                        break;
                    case 5:
                        str += "Five";
                        break;
                    case 6:
                        str += "Six";
                        break;
                    case 7:
                        str += "Seven";
                        break;
                    case 8:
                        str += "Eight";
                        break;
                    case 9:
                        str += "Nine";
                        break;
                    case 10:
                        str += "Ten";
                        break;
                    case 11:
                        str += "Jack";
                        break;
                    case 12:
                        str += "Queen";
                        break;
                    case 13:
                        str += "King";
                        break;
                }

                return str;
            }
        }
        public string TypeName
        {
            get
            {
                int type = Type;

                string str = ""; 
                switch (type)
                {
                    case 1:
                        str += "Hearts";
                        break;
                    case 2:
                        str += "Clubs";
                        break;
                    case 3:
                        str += "Diamonds";
                        break;
                    case 4:
                        str += "Spades";
                        break;
                }
                return str;
            }
        }

        public bool KingIsTen
        {
            get { return myKingIsTen; }
            set { myKingIsTen = value; }
        }

        #region ICloneable Members

        public object Clone()
        {
            _Card c = new _Card();
            c.Value = value_;
            c.Type = Type;
            return c;
        }

        #endregion
    }*/
}

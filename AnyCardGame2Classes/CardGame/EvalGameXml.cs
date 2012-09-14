using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using AnyCardGame2Classes;
//using C_CardGameObjects;

namespace AnyCardGameClasses
{/*
    public class EvalGameXml
    {
        public static bool MoveCreateMethodInsideRunMethod=true;



     //   public myCardGameObject Objects=new myCardGameObject();
            
        private readonly static string[] ReplaceNew = { "ID", "Name", "Expression", "Variable", "Value", "Type", "Default", "Question", "Number" };
        private readonly static string[] ReplaceOld = { "id", "name", "expression", "variable", "value", "type", "default", "question", "number" };


        public static myCardGameObject EvalGameFromXml(XmlDocument dd)
        {
            string s = dd.OuterXml;
            s = myHelper.ReplaceAll(ReplaceOld,ReplaceNew,s);
            dd.LoadXml(s);

            myCardGameObject Objects = new myCardGameObject();
            Methods.Clear();
             Objects = EvaluateNode(dd.ChildNodes[1], "0", Objects);
             Objects.Current = "0";

//            ((C_CardGame) Objects).Users = new List<_User>(new _User[] {new _User(2), new _User(2)});

            //myCardGameObject.XmlToObject(Objects.Parse());


  //          Objects.EvalTop = true;
    //        Objects.Run();
            return Objects;
        }

        public static List<myCardGameObject> EvaluateNodes(XmlNodeList list, string cur, myCardGameObject child)
        {
            int i = 0;
            List<myCardGameObject> o = new List<myCardGameObject>();
            foreach (XmlNode node in list)
            {
                myCardGameObject obj = EvaluateNode(node, cur + i, child);
                if (obj != null)
                {
                    obj.Current = cur + i;
                    o.Add(obj);
                }
                i++;
            }
            return o;
        }

        private static Dictionary<string, XmlNode> Methods = new Dictionary<string, XmlNode>();

        private static myCardGameObject EvaluateNode(XmlNode node, string cur, myCardGameObject parent)
        { 

            //try{
                switch (node.Name)
                {
                    case "LoopUsers":
                        C_LoopUsers loopusers = new C_LoopUsers(node);
                        loopusers.Parent = parent;
                        loopusers.Children = EvaluateNodes(node.ChildNodes, cur, loopusers);
                        return loopusers;
                        break;

                    case "DeleteVariable":
                        C_DeleteVariable deletevariable = new C_DeleteVariable(node);
                        deletevariable.Parent = parent;
                        deletevariable.Children = EvaluateNodes(node.ChildNodes, cur, deletevariable);
                        return deletevariable;
                        break;
                    case "LoopCards":
                        C_LoopCards loopcards = new C_LoopCards(node);
                        loopcards.Parent = parent;
                        loopcards.Children = EvaluateNodes(node.ChildNodes, cur, loopcards);
                        return loopcards;
                        break;

                    case "LoopInputAnswer":
                        C_LoopInputAnswer loopinputanswer = new C_LoopInputAnswer(node);
                        loopinputanswer.Parent = parent;
                        loopinputanswer.Children = EvaluateNodes(node.ChildNodes, cur, loopinputanswer);
                        return loopinputanswer;
                        break;

                    case "CreateMethod":

                        if (!MoveCreateMethodInsideRunMethod)
                        {
                            C_CreateMethod createmethod = new C_CreateMethod(node);
                            createmethod.Parent = parent;
                            createmethod.Children = EvaluateNodes(node.ChildNodes, cur, createmethod);
                            return createmethod;
                        }
                        else
                        {
                            Methods.Add(node.Attributes["ID"].Value, node);
                            return null;
                        }
                        break;

                    case "AddReturnParameter":
                     /*   C_AddReturnParameter addreturnparameter = new C_AddReturnParameter(node);
                        addreturnparameter.Parent = parent;
                        addreturnparameter.Children = EvaluateNodes(node.ChildNodes, cur, addreturnparameter);
                        return addreturnparameter;* /
                        break;
                    case "ReturnParameter":
                      /*  C_ReturnParameter returnparameter = new C_ReturnParameter(node);
                        returnparameter.Parent = parent;
                        returnparameter.Children = EvaluateNodes(node.ChildNodes, cur, returnparameter);
                        return returnparameter;* /
                        break;
                    case "AddMethodParameter":
                        C_AddMethodParameter addmethodparameter = new C_AddMethodParameter(node);
                        addmethodparameter.Parent = parent;
                        addmethodparameter.Children = EvaluateNodes(node.ChildNodes, cur, addmethodparameter);
                        return addmethodparameter;
                        break;
                    case "UpdateMethodParameter":
                        C_UpdateMethodParameter updatemethodparameter = new C_UpdateMethodParameter(node);
                        updatemethodparameter.Parent = parent;
                        updatemethodparameter.Children = EvaluateNodes(node.ChildNodes, cur, updatemethodparameter);
                        return updatemethodparameter;
                        break;

                    case "RunMethod":
                        C_RunMethod runmethod = new C_RunMethod(node);
                        runmethod.Parent = parent;
                        runmethod.Children = EvaluateNodes(node.ChildNodes, cur, runmethod);
                        if (MoveCreateMethodInsideRunMethod)
                            runmethod.Children.AddRange(EvaluateNodes(Methods[runmethod.myID].ChildNodes, cur, runmethod));
           
                        return runmethod;
                        break;

                    case "GetUserFromGameUserID":
                        C_GetUserFromGameUserID getuserfromgameuserid = new C_GetUserFromGameUserID(node);
                        getuserfromgameuserid.Parent = parent;
                        getuserfromgameuserid.Children = EvaluateNodes(node.ChildNodes, cur, getuserfromgameuserid);
                        return getuserfromgameuserid;
                        break;
                
                    case "Deal":
                        C_Deal deal = new C_Deal(node);
                        deal.Parent = parent;
                        deal.Children = EvaluateNodes(node.ChildNodes, cur, deal);
                        return deal;
                    case "CardGame":
                        C_CardGame cardgame = new C_CardGame(node);
                        cardgame.Parent = parent;
                        cardgame.Children = EvaluateNodes(node.ChildNodes, cur, cardgame);
                        return cardgame;
                        break;
                    case "WinType":
                        C_WinType wintype = new C_WinType(node);
                        wintype.Parent = parent;
                        wintype.Children = EvaluateNodes(node.ChildNodes, cur, wintype);
                        return wintype;
                        break;

                    case "DeclareWinner":
                        C_DeclareWinner declarewinner = new C_DeclareWinner(node);
                        declarewinner.Parent = parent;
                        declarewinner.Children = EvaluateNodes(node.ChildNodes, cur, declarewinner);
                        return declarewinner;
                        break;

                    case "CheckWinner":
                        C_CheckWinner checkwinner = new C_CheckWinner(node);
                        checkwinner.Parent = parent;
                        checkwinner.Children = EvaluateNodes(node.ChildNodes, cur, checkwinner);
                        return checkwinner;
                        break;
                    case "ShowInformation":
                        C_ShowInformation showinformation = new C_ShowInformation(node);
                        showinformation.Parent = parent;
                        showinformation.Children = EvaluateNodes(node.ChildNodes, cur, showinformation);
                        return showinformation;
                        break;
                    case "ShowAllInformation":
                        C_ShowAllInformation showallinformation= new C_ShowAllInformation(node);
                        showallinformation.Parent = parent;
                        showallinformation.Children = EvaluateNodes(node.ChildNodes, cur, showallinformation);
                        return showallinformation;
                        break;
                    case "DealCards":
                        C_DealCards dealcards = new C_DealCards(node);
                        dealcards.Parent = parent;
                        dealcards.Children = EvaluateNodes(node.ChildNodes, cur, dealcards);
                        return dealcards;
                        break;
                    case "PossibleAnswer":
                        C_PossibleAnswer possibleanswer = new C_PossibleAnswer(node);
                        possibleanswer.Parent = parent;
                        possibleanswer.Children = EvaluateNodes(node.ChildNodes, cur, possibleanswer);
                        return possibleanswer;
                        break;
                    case "GetUserInput":
                        C_GetUserInput getuserinput = new C_GetUserInput(node);
                        getuserinput.Parent = parent;
                        getuserinput.Children = EvaluateNodes(node.ChildNodes, cur, getuserinput);
                        return getuserinput;
                        break;
                    case "GetAllUserInput":
                        C_GetAllUserInput getalluserinput = new C_GetAllUserInput(node);
                        getalluserinput.Parent = parent;
                        getalluserinput.Children = EvaluateNodes(node.ChildNodes, cur, getalluserinput);
                        return getalluserinput;
                        break;
                    case "MoveCards":
                        C_MoveCards movecards = new C_MoveCards(node);
                        movecards.Parent = parent;
                        movecards.Children = EvaluateNodes(node.ChildNodes, cur, movecards);
                        return movecards;
                        break;
                    case "CreatePile":
                        C_CreatePile createpile = new C_CreatePile(node);
                        createpile.Parent = parent;
                        createpile.Children = EvaluateNodes(node.ChildNodes, cur, createpile);
                        return createpile;
                        break; 
                    case "MethodParameter":
                        C_MethodParameter methodparameter= new C_MethodParameter(node);
                        methodparameter.Parent = parent;
                        methodparameter.Children = EvaluateNodes(node.ChildNodes, cur, methodparameter);
                        return methodparameter;
                        break;
                    case "CreateVariable":
                        C_CreateVariable createvariable = new C_CreateVariable(node);
                        createvariable.Parent = parent;
                        createvariable.Children = EvaluateNodes(node.ChildNodes, cur, createvariable);
                        return createvariable;
                    case "InPile":
                        C_InPile inpile = new C_InPile(node);
                        inpile.Parent = parent;
                        inpile.Children = EvaluateNodes(node.ChildNodes, cur, inpile);
                        return inpile;
                        break; 
                    case "CreateListVariable":
                        C_CreateListVariable createlistvariable = new C_CreateListVariable(node);
                        createlistvariable.Parent = parent;
                        createlistvariable.Children = EvaluateNodes(node.ChildNodes, cur, createlistvariable);
                        return createlistvariable;
                        break;
                    case "AddToList":
                        C_AddToList addtolist = new C_AddToList(node);
                        addtolist.Parent = parent;
                        addtolist.Children = EvaluateNodes(node.ChildNodes, cur, addtolist);
                        return addtolist;
                        break;
                    case "ClearList":
                        C_ClearList clearList= new C_ClearList(node);
                        clearList.Parent = parent;
                        clearList.Children = EvaluateNodes(node.ChildNodes, cur, clearList);
                        return clearList;
                        break; 
                    case "RemoveFromList":
                        C_RemoveFromList removefromlist = new C_RemoveFromList(node);
                        removefromlist.Parent = parent;
                        removefromlist.Children = EvaluateNodes(node.ChildNodes, cur, removefromlist);
                        return removefromlist;
                        break; 
                    case "SetVariable":
                        C_SetVariable setvariable = new C_SetVariable(node);
                        setvariable.Parent = parent;
                        setvariable.Children = EvaluateNodes(node.ChildNodes, cur, setvariable);
                        return setvariable;
                        break; 
                    case "Shuffle":
                        C_Shuffle shuffle = new C_Shuffle(node);
                        shuffle.Parent = parent;
                        shuffle.Children = EvaluateNodes(node.ChildNodes, cur, shuffle);
                        return shuffle;
                        break;
                    case "Card": 
                        C_Card card = new C_Card(node);
                        card.Parent = parent;
                        card.Children = EvaluateNodes(node.ChildNodes, cur, card);
                        return card;
                        break; 
                    case "Game":
                        C_Game game = new C_Game(node);
                        game.Parent = parent;
                        game.Children = EvaluateNodes(node.ChildNodes, cur, game);
                        return game;
                        break; 
                    case "Round":
                        C_Round round = new C_Round(node);
                        round.Parent = parent;
                        round.Children = EvaluateNodes(node.ChildNodes, cur, round);
                        return round;
                        break; 
                    case "Evaluate":
                        C_Evaluate evaluate = new C_Evaluate(node);
                        evaluate.Parent = parent;
                        evaluate.Children = EvaluateNodes(node.ChildNodes, cur, evaluate);
                        return evaluate;
                        break; 
                    case "isTrue":
                        C_isTrue istrue = new C_isTrue(node);
                        istrue.Parent = parent;
                        istrue.Children = EvaluateNodes(node.ChildNodes, cur, istrue);
                        return istrue;
                        break;
                    case "isFalse":
                        C_isFalse isfalse = new C_isFalse(node);
                        isfalse.Parent = parent;
                        isfalse.Children = EvaluateNodes(node.ChildNodes, cur, isfalse);
                        return isfalse;
                        break;
                    case "Loop":
                        C_Loop loop = new C_Loop(node);
                        loop.Parent = parent;
                        loop.Children = EvaluateNodes(node.ChildNodes, cur, loop);
                        return loop;
                        break; 
                    case "Bet":
                        C_Bet bet = new C_Bet(node);
                        bet.Parent = parent;
                        bet.Children = EvaluateNodes(node.ChildNodes, cur, bet);
                        return bet;
                        break;
                        //BET
                        break;
                }
                return null;
            /*}
            catch (MyException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new MyException(cur, v, e);
            }* /
        }
    }
 */
}


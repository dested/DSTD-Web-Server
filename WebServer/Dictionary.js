/*
File: Dictionary.js
Version: 1.0
Last modified: 09.17.2002
Author: Alexandar Minkovsky (a_minkovsky@hotmail.com ; URL: http://www24.brinkster.com/alekz)
Copyright: Left
Type: Class
Exports: Dictionary class.
Dependencies: None.
Description:
    Similar to ASP "Scripting.Dictionary" Object, or an associative array
    with somewhat limited functionality. There's a few differences:
        - "item" property is replaced by two methods getVal and setVal.
        - "key" property is replaced by the setKey method.
    As the value of an item can be virtually anything, even another Dictionary object,
    the Dictionary Class might be usefull.
    If someone have a suggestion or wants the Dictionary class to be extended some way,
    feel free to drop me an e-mail.
Tested with: IE4; IE5; IE5.5; IE6; NS4.78; NS6.0; NS6.1; NS6.2; NS7.0; Mozilla 1.0; Mozilla 1.1; Opera 6.0
*/
/*
================
Dictionary Class
================
    - Instanciating
        oDict = new Dictionary();

    - Properties
       ~ Public
         (int) count - Number of keys in the Dictionary. Default: 0. Read only, do never manually set this property!
       ~ Private
         (Object) Obj - the object actually containing the data. Do never use it directly.

    - Methods - look at the function descriptions for detailed explanation.
      ~ Public
        (BOOL)  exists(sKey)
        (BOOL)  add (sKey,aVal)
        (BOOL)  remove(sKey)
        (void)  removeAll()
        (Array) values()
        (Array) keys()
        (Array) items()
        (Any)   getVal(sKey)
        (void)  setVal(sKey,aVal)
        (void)  setKey(sKey,sNewKey)
*/
//****************************************
//Dictionary Object
//****************************************
/*
function: Dictionary
Parameters: None
Returns: A new Dictionary object.
Actions: Constructor.
*/
function Dictionary(){
//Properties
  //~Public
  this.count = 0;
  //~Private
  this.Obj = new Object();
//Methods
  //~Public
  this.exists = Dictionary_exists;
  this.add = Dictionary_add;
  this.remove = Dictionary_remove;
  this.removeAll = Dictionary_removeAll;
  this.values = Dictionary_values;
  this.keys = Dictionary_keys;
  this.items = Dictionary_items;
  this.getVal = Dictionary_getVal;
  this.setVal = Dictionary_setVal;
  this.setKey = Dictionary_setKey;
}
//****************************************
//Method implementations
//****************************************
/*
function: Dictionary_exists
implements: Dictionary.exists
Parameters:
    (String) sKey - Key name being looked for.
Returns: (BOOL) - true if sKey is found ; false if it is not.
Actions: Iterates through all Dictionary keys and checks for sKey.
*/
function Dictionary_exists(sKey){
  return (this.Obj[sKey])?true:false;
}
//****************************************
/*
function: Dictionary_add
implements: Dictionary.add
Parameters:
    (String) sKey - Key name to be added.
    (Any)    aVal - Value to be associated with sKey.
Returns: (BOOL) - true if sKey is created ; false if it is not (because of a duplicate Key name).
Actions: Adds a new Key=Value pair to the Dictionary.
*/
function Dictionary_add(sKey,aVal){
  var K = String(sKey);
  if(this.exists(K)) return false;
  this.Obj[K] = aVal;
  this.count++;
  return true;
}
//****************************************
/*
function Dictionary_remove
implements: Dictionary.remove
Parameters:
    (String) sKey - Key to be removed.
Returns: (BOOL) - true if sKey has been removed ; false if it has not (did not exist).
Actions: Removes a specified key from the Dictionary.
*/
function Dictionary_remove(sKey){
  var K = String(sKey);
  if(!this.exists(K)) return false;
  delete this.Obj[K];
  this.count--;
  return true;
}
//****************************************
/*
function: Dictionary_removeAll
implements: Dictionary.removeAll
Parameters: None
Returns: Nothing
Actions: Removes all key=value pairs from a Dictionary object.
*/
function Dictionary_removeAll(){
  for(var key in this.Obj) delete this.Obj[key];
  this.count = 0;
}
//****************************************
/*
function: Dictionary_values
implements: Dictionary.values
Parameters: None
Returns: Returns an Array containing all the item values in a Dictionary object.
Actions: Iterates through the Dictionary name=value pairs and builds an Array of all values.
*/
function Dictionary_values(){
  var Arr = new Array();
  for(var key in this.Obj) Arr[Arr.length] = this.Obj[key];
  return Arr;
}
//****************************************
/*
function: Dictionary_keys
implements: Dictionary.keys
Parameters: None
Returns: Returns an Array containing all existing keys in a Dictionary object.
Actions: Iterates through the Dictionary name=value pairs and builds an Array of all keys.
*/
function Dictionary_keys(){
  var Arr = new Array();
  for(var key in this.Obj) Arr[Arr.length] = key;
  return Arr;
}
//****************************************
/*
function: Dictionary_items
implements: Dictionary.items
Parameters: None
Returns: Returns a bidimensional Array containing all existing keys=value pairs in a Dictionary object.
Actions:
    - Iterates through the Dictionary key=value pairs and builds a bidimensional Array.
    - First index contains the key name ; second index contains the value:
        ex. Arr[0][0] is the key name of the first Dictionary item
            Arr[0][1] is the value of the first Dictionary item
*/
function Dictionary_items(){
  var Arr = new Array();
  for(var key in this.Obj){
    var A = new Array(key,this.Obj[key]);
    Arr[Arr.length] = A;
  }
  return Arr;
}
//****************************************
/*
function: Dictionary_getVal
implements: Dictionary.getVal
Parameters:
    (String) sKey
Returns: Item value for the passed sKey.
Actions: Retrieves the Dictionary item value corresponding to sKey.
*/
function Dictionary_getVal(sKey){
  var K = String(sKey);
  return this.Obj[K];
}
//****************************************
/*
function: Dictionary_setVal
implements: Dictionary.setVal
Parameters:
    (String) sKey
    (Any)    aVal
Returns: Nothing.
Actions:
    - Sets the Dictionary item value corresponding to sKey to aVal.
    - If The key is not found in the dictionary it is created.
*/
function Dictionary_setVal(sKey,aVal){
  var K = String(sKey);
  if(this.exists(K))
    this.Obj[K] = aVal;
  else
    this.add(K,aVal);
}
//****************************************
/*
function: Dictionary_setKey
implements: Dictionary.setKey
Parameters:
    (String) sKey
    (String) sNewKey
Returns: Nothing.
Actions:
    - Changes sKey to sNewKey
    - if sKey is not found, creates a new item with key=sNewKey and value=null
    - if sKey is not found, but sNewKey is found - does nothing.
    - if sKey and sNewKey both exist - does nothing.
*/
function Dictionary_setKey(sKey,sNewKey){
  var K = String(sKey);
  var Nk = String(sNewKey);
  if(this.exists(K)){
    if(!this.exists(Nk)){
      this.add(Nk,this.getVal(K));
      this.remove(K);
    }
  }
  else if(!this.exists(Nk)) this.add(Nk,null);
}
//****************************************
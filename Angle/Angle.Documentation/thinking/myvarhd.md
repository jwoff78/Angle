#Ideas past bin
i use this just to lay down some info and get my head in order, and my ideas my thouts etc. please dont try and edit this even if there is spelling mestakes.

## To do's
- [ ] Create lexer
- [ ] Cretae parser
- [ ] Decide on a main token list

## Stuff Finshed
- [X] Create project sceliton

## ideas list(cheacked mean good idea, uncheacked means thinking about it.)
- [X] Use Eclang engine 
- [ ] build some speech reonizion abilitys


## some notes:
* the user will never work with variables directly

##Hello world
Basic :

Computer create an program named "HelloWorld" that`(begin(1))`, prints`(action(2))` "Hello World"`(Value(3))` to the console`(refine(4))`.

basic codes token list would be :

Token|Value
-----|-----
Begin|"HelloWorld"
action|prints
value|"Hello world"
refine|console

logic to run this:

```csharp
foreach var i in tokens
{
//we have an begin token first so stor the ditails
app.name = "HelloWorld";

//we have an action
tmpaction = convertNameToMethod("prints");//returns print withc is method in low level api
have action = true;//used to build ast later

//now we have the value token add it to var array
invokeperamsArray.add(ConvertTextToType("Hello Wrold")); // add the string "hello world"

//now we get the refine token
app.ast.add(resolveRefine(tmpaction, ConvertRefineString("console")) /* returns ConsoleWrite method to invoke in api */);


}

/* 
now we send the app to the engine
the engine will execute the ast
*/
```
##posible tokens
Token name|Poible data in token|description
----------|--------------------|-----------
< Begin >|Prgramm name, Programm type|This token is used to give some info about the programm
< Action >|Action type|This token is used to specify an action
< value >|a value (hard value)|Used to give manual in put
< refine >|a refineing object|This is an od one but it is used to refine the action tak in curent statment even more

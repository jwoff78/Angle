
/* init stuff */
InvokeStatement = "Console.WriteLine({Params});";

AddActions("display");
AddActions("displays");
AddActions("print");
AddActions("prints");

AddRefine("console");
AddRefine("terminal");
AddRefine("terminal");

AddImport("System"); 

/* buildStuff */

InvokeStatmentBuilded = "Console.WriteLine(" + Params + ");"
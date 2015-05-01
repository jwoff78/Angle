
/* init stuff */
InvokeStatement = "def frm = new Form()";

AddActions("create");
AddActions("make");
AddActions("display");

AddRefine("form");
AddRefine("windows form");

AddImport("System.Windows.Forms"); 

/* buildStuff */

InvokeStatmentBuilded = "dec frm = new Form();"

InvokeStatmentBuildedEnd = "frm" + ".Show()\n";
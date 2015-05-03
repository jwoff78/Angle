
/* init stuff */
InvokeStatement = "var frm = new Form()";

AddActions("create");
AddActions("make");
AddActions("display");

AddRefine("form");
AddRefine("windows form");

AddImport("System.Windows.Forms"); 

/* buildStuff */

InvokeStatmentBuilded = "var frm = new Form();"

InvokeStatmentBuildedEnd = "frm" + ".Show();while(true){Application.DoEvents();}\n";
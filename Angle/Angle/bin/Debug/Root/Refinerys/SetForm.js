
/* init stuff */
InvokeStatement = "def frm = new Form()";

AddActions("set");

AddRefine("form");
AddRefine("forms");
AddRefine("windows form");

AddImport("System.Windows.Forms"); 

/* buildStuff */

InvokeStatmentBuilded = "frm." + capitalizeFirstLetter(ParamsArrayTrimed[0]) + " = " + ParamsArray[1] + ";";


function capitalizeFirstLetter(string) {
    return string.charAt(0).toUpperCase() + string.slice(1);
}
# Angle.Core.Engine

## Getting started
You only need to use this class to use the entire implmentaion.
here is an example:
```csharp
var en = new Engine();
en.Run("your code here");
```

## Internals
this section is for coders who would like to know more about this class.

### Methods
MethodName|Agrs|Description|More detail
----------|----|-----------|-----------
Void Run|(string English)|This method runs the code|The method start by converting the code to and token list, it the converts the token list to and ast witch is then ran using the resolve method

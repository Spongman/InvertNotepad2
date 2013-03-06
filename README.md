InvertNotepad2
==============


Inverts the brightness of colors in a notepad2 theme .ini file
 
### Requirements
  .NET framework v2.0
  
  find your C# compiler:
  
    where /r %windir%\microsoft.net csc.exe
 
### Build

    csc.exe InvertNodepad2.cs
		
### Usage
1. in notepad2, View->Customize schemes->Export
2. InvertNodepad2.exe "\<path to .ini\>"
3. in notepad2, View->Customize schemes->Import
4. manually switch Fore/Back of Default Text/Default Style


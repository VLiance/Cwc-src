using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cwc {
    class Msg {
       
         public static  void fShowIntroMessage() {
//Debug.fTrace("Number Of Logical Processors: {0}", Environment.ProcessorCount);

            //Output.Trace("\f0FCwC\fs was a intelligent \f0FCwim\fs & \f0FC++\fs compiler/manager, it can be used as a \f0Bdirect remplacement\fs of \f0FGCC\fs or \f0FClang\fs");
            Output.Trace("\f0FCwc\fs is an intelligent frontend Multi-languages compiler \f0B(C, C++, C~, Python, ...)\fs, it can be used as a \f0Bdirect remplacement\fs of \f0FGCC\fs or \f0FClang\fs");
            Output.Trace("");
            //  Output.TraceColored("\f08---- CwC Inteligent compiler main feature ---");
           // Debug.fTrace();
            Output.Trace("Now you can build files simultaneously, with multiple input and even directly by folder");

               Output.Trace("");
            Output.Trace("The \f0FC++\fs compiler, by default, use the backend toolchain \f0ALibRT\fs base on \f0AClang && MinGW\fs, to build \f0Aany C++ code\fs and \f0Across-compile\fs to others platforms.");
          //  Output.Trace("If you want someting more complete & standard, you can configure another C++ compiler");
                   Output.Trace("");
      //      Output.Trace(" \f17-Use the \f1DSetting.ini\f17 to change configuration-");
       
            Output.Trace("\f08---- Main Cwc usage ---");
            Output.Trace("Cwc can be used as any stantdard compiler by command-line");
            Output.Trace("He accept all arguments of standard compilers: GCC && Clang");
    Output.Trace("");
            Output.Trace("\f0FThe main addition is the separators \f0D|\fs && \f0D>\fs");
           Output.Trace("");
            Output.Trace(" \f0D|\fs Build multiples commands simultaneously (Multithread & Output is always in same order)");
            Output.Trace(" \f0D>\fs Sequences your build -> Wait for previous commands");
       Output.Trace("");
            Output.Trace("\f08Exemple:\fs -c Src1.cpp -o Src1.o \f0D|\fs -c Src2.cpp -o Src2.o \f0D>\fs -o App.exe Src1.o Src2.o");

                  Output.Trace(""); 
                  Output.Trace(""); 
              Output.Trace("--\f37 How to use:\fs ");
              Output.Trace("");
                Output.Trace("\f0B- The Wiki:    \fs[\f09 https://github.com/VLiance/Cwc/wiki/Wiki \fs] ");
                Output.Trace("\f0B- Directives:  \fs[\f09 https://github.com/VLiance/Cwc/wiki/Directives \fs]  ");
                Output.Trace("\f0B- Variables:   \fs[\f09 https://github.com/VLiance/Cwc/wiki/Variables \fs] " );
                Output.Trace(""); 
            Output.Trace(""); 
              Output.Trace("--\f37 Try somes Cwc Examples:\fs ");
                 Output.Trace("");
              Output.Trace("\f0F- Main Cwc examples:   \f1E L:[VLiance/Demos/]\fs");
             Output.Trace("\f0B- Some libs using Cwc:  \fs[\f09 https://github.com/Cwc-Lib/ \fs] ");


                 
            /*
         //   Output.Trace(" \f0E-Compiler=LibRT\fs   : \f0B(Default)\fs");

            //Output.TraceColored(" \f0E-Compiler=<name>\fs  ");
              Output.Trace("");
            Output.Trace("\f08---- Choose your backend Toolchain --- \f0D(Host by default on Github)");
            Output.Trace(" \f0E-_wToolchain \f0B[Server]Autor/Name[Type]/(Version)\f08 : (Default:VLiance/LibRT)\fs");
       Output.Trace("");
             Output.Trace("\f08   ---- Examples ----");
             
            Output.Trace(" \f06  -{_wToolchain}= VLianceTool/LibRT         \f03(Build for Windows with Clang)\fs");
            Output.Trace(" \f06  -{_wToolchain}= VLianceTool/LibRT[Mingw]  \f03(Build for Windows with Mingw)\fs");
            Output.Trace(" \f06  -{_wToolchain}= VLianceTool/WebRT         \f03(Build for Web with Emsc)\fs");


            //Output.TraceColored("Designed to be lite & minimal, it doesn't contain every possible libs. If you really want it, it still compatible with all MinGw libs, but you may loose your portability.");
           Output.Trace("");*/
/*
            Output.Trace("\f08----- Build to Platform Flags ---");
            Debug.fTrace();
            Output.Trace(" \f0E-Windows\fs     : \f0B(Default)\fs");
            Output.Trace(" \f0E-Web_Emsc\fs    : Export to Web with Emscriptem");
            Output.Trace(" \f0E-CpcDos\fs      : Export to CpcDos (Windows Compatible)");
            Debug.fTrace();
*/
/*
            Output.Trace("\f08----- Directives --- [\f09 https://github.com/VLiance/Cwc/wiki/Directives \fs]  ");

             Output.Trace(" \f0E-#To\fs    (output)                     : \f0BTake all preceding object files in the same sequence to make an Output (.exe/.a/.dll) \fs");
             Output.Trace(" \f0E-#Copy\fs  (input)  (output)            : \f0BCopy all files if newer (accept folder) \fs");
             Output.Trace(" \f0E-#If_NotExist\fs  (inputs) :: (PassCmd) : \f0BTest of non-existance of inputs files, if yes do the PassCmd \fs");
           
            Output.Trace("");


            Output.Trace("\f08----- Variables  --- [\f09 https://github.com/VLiance/Cwc/wiki/Variables \fs] " );
            //Output.Trace("\f0F Variables are between bracket \f0D'{}'\fs, begin with \f0D'_'\f0F (cwc var)\fs or \f0D'v'\f0F(custom var) \fs");
            Output.Trace("\f0F Variables are between bracket \f0D'{}'\fs");
             Output.Trace("\f08   ---- Example ----");
            Output.Trace(" \f0E{vHelloVar}=\f0BCustomPath/\fs  Example: \f0B-c src/ -o bin/\f0D{vHelloVar}\f0Boutput/  \fsResult: \f0B-c src/ -o bin/CustomPath/output/");   
            Output.Trace("");
*/
        }



    }
}
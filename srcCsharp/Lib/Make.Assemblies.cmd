rem @echo off

setlocal

set CD=%~dp0

set ANT_HOME=%CD%Etc\ant
set ANT=%ANT_HOME%\bin\ant.bat

set IKVMC=%CD%ikvmc.exe

set HSQLDB_HOME=%CD%..\..\..\..\base-one\trunk
set HSQLDB_GLUE_HOME=%CD%Etc\hsqldb-glue
set HSQLDB_VERSION=1.8.1.3

call %ANT% -f "%HSQLDB_HOME%\build\build.xml" hsqldb
call %ANT% -f "%CD%Etc\hsqldb-glue\build.xml" jar
call %ANT% -f "%CD%Etc\restest\resdb\build.xml" jar

rename "%CD%Org.Hsqldb.dll" Org.Hsqldb.dll~

"%IKVMC%" -assembly:Org.Hsqldb "-keyfile:%CD%../Org.Hsqldb.snk" -target:library -version:%HSQLDB_VERSION% "%HSQLDB_HOME%\lib\hsqldb.jar" "%HSQLDB_HOME%\lib\servlet.jar" "%HSQLDB_GLUE_HOME%\dist\hsqldb-glue.jar" 

"%IKVMC%" -assembly:resdb "-keyfile:%CD%../Org.Hsqldb.snk" -target:library -version:%HSQLDB_VERSION% "%CD%Etc\restest\resdb\dist\resdb.jar"

endloal

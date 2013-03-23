@echo off
..\HashGetter\bin\Release\HashGetter.exe -format """bin\\curl http://dev.goclassing.com/Dev/Checkout?hash={0}""" > tmp.cmd
Call tmp.cmd
del tmp.cmd
pause
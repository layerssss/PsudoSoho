@echo off
"C:\Users\LayersSss\Documents\Visual Studio 2010\Projects\PsudoSoho\PublishingOptimization\bin\Release\PublishingOptimization.exe" -conf GCPublishing\PublishingOptimization.conf
"C:\Users\LayersSss\Documents\Visual Studio 2010\Projects\PsudoSoho\EncodeName\bin\Release\EncodeName.exe" -conf GCPublishing\EncodeName.conf
"C:\Users\LayersSss\Documents\Visual Studio 2010\Projects\LLife\LZip\bin\Debug\LZip.exe" -conf GCPublishing\LZip.conf
move C:\Users\LayersSss\Desktop\gcpublishing.zip C:\Users\LayersSss\Desktop\goclassing\gcpublishing.zip
"D:\Program Files\curl\curl.exe" -T C:\Users\LayersSss\Desktop\goclassing\gcpublishing.zip -u goclassing:Sh@ngke8 ftp://www.goclassing.com/gcpublishing.zip
pause
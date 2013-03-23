@echo off
@echo Masonry Gallery发布优化脚本
"C:\Users\LayersSss\Documents\Visual Studio 2010\Projects\LLife\LZip\bin\Debug\LZip.exe" -conf LZipCopy1.conf
"C:\Users\LayersSss\Documents\Visual Studio 2010\Projects\LLife\LZip\bin\Debug\LZip.exe" -conf LZipCopy2.conf
del tmp.zip /Q
"C:\Users\LayersSss\Documents\Visual Studio 2010\Projects\PsudoSoho\PublishingOptimization\bin\Release\PublishingOptimization.exe" -conf PublishingOptimization.conf
del d:\mg_opt\mg.zip /Q
"C:\Users\LayersSss\Documents\Visual Studio 2010\Projects\LLife\LZip\bin\Debug\LZip.exe" -conf LZip.conf
move mg.zip d:\mg_opt\mg.zip
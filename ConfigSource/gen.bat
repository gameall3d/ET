set WORKSPACE=..
set CONFIG_DIR_NAME=GameConfigs
set GEN_CLIENT=dotnet  %WORKSPACE%\Tools\Luban\Luban.ClientServer\Luban.ClientServer.dll
set TEMPLATE_SERVER=%WORKSPACE%\Tools\Luban\Template
set CONF_ROOT=%WORKSPACE%\ConfigSource
set OUTPUT_SERVER_DATA_DIR=%WORKSPACE%\Config\%CONFIG_DIR_NAME%
set OUTPUT_CODE_DIR=%WORKSPACE%\Unity\Assets\Scripts\Codes\Model\Generate
set OUTPUT_CLIENT_DATA_DIR=%WORKSPACE%\Unity\Assets\Bundles\%CONFIG_DIR_NAME%

echo ======================= Server GameConfig ==========================
%GEN_CLIENT% -t CustomTemplates -j cfg --^
 -d %CONF_ROOT%\Defines\__root__.xml ^
 --input_data_dir %CONF_ROOT%\Datas ^
 --output_data_dir %OUTPUT_SERVER_DATA_DIR% ^
 --output:exclude_tags c ^
 --gen_types data_bin ^
 -s all

echo ======================= Server Code ==========================
%GEN_CLIENT% -t CustomTemplates -j cfg --^
 -d Defines\__root__.xml ^
 --input_data_dir Datas ^
 --output_data_dir output_json ^
 --output_code_dir %OUTPUT_CODE_DIR%\Server\%CONFIG_DIR_NAME% ^
 --output:exclude_tags c ^
 --gen_types code_cs_bin,data_json ^
 -s all


echo ======================= Client GameConfig ==========================
%GEN_CLIENT% -t CustomTemplates -j cfg --^
 -d Defines\__root__.xml ^
 --input_data_dir Datas ^
 --output_data_dir %OUTPUT_CLIENT_DATA_DIR% ^
 --output:exclude_tags s ^
 --gen_types data_bin ^
 -s all

echo ======================= Client Server Code ==========================
%GEN_CLIENT% -t CustomTemplates -j cfg --^
 -d Defines\__root__.xml ^
 --input_data_dir Datas ^
 --output_code_dir %OUTPUT_CODE_DIR%\ClientServer\%CONFIG_DIR_NAME% ^
 --gen_types code_cs_bin ^
 -s all
 
echo ======================= Client Code ==========================
 %GEN_CLIENT% -t CustomTemplates -j cfg --^
  -d Defines\__root__.xml ^
  --input_data_dir Datas ^
  --output_code_dir %OUTPUT_CODE_DIR%\Client\%CONFIG_DIR_NAME% ^
  --gen_types code_cs_bin ^
  -s all

pause
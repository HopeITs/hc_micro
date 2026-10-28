param([Parameter(Mandatory=$true)][string]$AssemblyPath,[int]$Visits=1000,[switch]$WithGrid)
$ErrorActionPreference='Stop'
$path=(Resolve-Path -LiteralPath $AssemblyPath).Path
$dir=Split-Path -Parent $path
[void][Reflection.Assembly]::LoadFrom((Join-Path $dir 'MySql.Data.dll'))
[void][Reflection.Assembly]::LoadFrom((Join-Path $dir 'Newtonsoft.Json.dll'))
$app=[Reflection.Assembly]::LoadFrom($path)
$type=$app.GetType('MyWorkApplication.Classes.KoboVisitResults',$true)
$filter=[Activator]::CreateInstance($app.GetType('MyWorkApplication.Classes.KoboResultsFilter',$true))
$filter.From=[datetime]'2026-01-01';$filter.To=[datetime]'2026-12-31'
$catalog=Get-Content (Join-Path $PSScriptRoot '../MyWorkApplication/KoboQuestionCatalog.json') -Raw -Encoding UTF8 | ConvertFrom-Json
$forms=@($catalog.PSObject.Properties | Where-Object { $_.Value.region -eq 'aleppo' })
$payloads=@{}
foreach($form in $forms) {
    $json=@{}
    foreach($field in $form.Value.fields) {
        $value=if($field.choices.Count){$field.choices[0].code}elseif($field.type -match 'integer|decimal'){'123'}elseif($field.type -eq 'date'){'2026-10-20'}else{'قيمة تجريبية لمقارنة الأداء'}
        $json[$field.path]=$value
    }
    $payloads[$form.Name]=ConvertTo-Json $json -Compress
}
$data=New-Object Data.DataTable
foreach($name in @('ID','Kobo_Submission_ID','Kobo_Form_ID','Kobo_Form_Name','Kobo_Form_Version','Raw_JSON','Sync_Status','Project_ID','Person_ID','Visit_Date','Stage','Visit_Kind','Beneficiary','MP_Type_ID','Category_ID','SubCategory_ID','Career_ID','SubCategory_Name','Career_Name','Kobo_Is_Deleted')) { [void]$data.Columns.Add($name) }
$data.BeginLoadData()
for($i=0;$i -lt $Visits;$i++) {
    $form=$forms[$i % $forms.Count]
    [void]$data.Rows.Add(@([string]($i+1),[string](600000+$i),$form.Name,$form.Name,'v1',$payloads[$form.Name],'local_inserted',[string]($i+1),[string]($i+1),'2026-10-20','M1','o','مستفيد تجريبي','1','2','3','4','حرف','خياطة','0'))
}
$data.EndLoadData()
$timer=[Diagnostics.Stopwatch]::StartNew()
foreach($mode in @($true,$true,$false)) {
    $timer.Restart()
    $report=$type.GetMethod('BuildCombined').Invoke($null,@($data.PSObject.BaseObject,$filter,$mode))
    $timer.Stop()
    if($report.Visits -ne $Visits){throw 'Benchmark visit count changed'}
    Write-Output ("Build counts={0}, visits={1}, rows={2}, columns={3}: {4} ms" -f $mode,$report.Visits,$report.Table.Rows.Count,$report.Table.Columns.Count,$timer.ElapsedMilliseconds)
}
if(!$WithGrid){exit 0}
Add-Type -AssemblyName System.Windows.Forms
Add-Type -AssemblyName System.Drawing
$formType=$app.GetType('MyWorkApplication.Visit_Forms.Visit_Results_Form',$true)
$form=$formType.GetConstructors()[0].Invoke(@($null))
$flags=[Reflection.BindingFlags]'Instance,NonPublic'
function Control($name){$formType.GetField($name,$flags).GetValue($form)}
function Set-Field($name,$value){$formType.GetField($name,$flags).SetValue($form,$value)}
$load=[Delegate]::CreateDelegate([EventHandler],$form,$formType.GetMethod('Monitoring_Visit_Results_Form_Load',$flags))
$form.remove_Load($load)
$form.ShowInTaskbar=$false;$form.StartPosition='Manual';$form.Location=New-Object Drawing.Point(-20000,-20000)
try {
    $modes=New-Object 'Collections.Generic.List[string]'
    foreach($mode in @('عدد إجابات كل سؤال','إجابات المستفيدين','عدد إجابات كل سؤال (كوبو)','إجابات المستفيدين (كوبو)')){[void](Control 'SearchBy_comboBox').Items.Add($mode);$modes.Add($mode)}
    Set-Field 'searchBy_list' $modes;Set-Field 'koboModeVisible' $true;Set-Field 'koboSources' $data.PSObject.BaseObject;Set-Field 'koboSourceUid' '';Set-Field 'Hided' $true
    $choices=New-Object Data.DataTable
    [void]$choices.Columns.Add('UID');[void]$choices.Columns.Add('Name');[void]$choices.Rows.Add('','جميع نماذج كوبو')
    (Control 'KoboForm_comboBox').DisplayMember='Name';(Control 'KoboForm_comboBox').ValueMember='UID';(Control 'KoboForm_comboBox').DataSource=$choices
    $versionArgs=New-Object object[] 1;$versionArgs[0]=$data.PSObject.BaseObject
    (Control 'KoboVersion_comboBox').DisplayMember='Name';(Control 'KoboVersion_comboBox').ValueMember='Key';(Control 'KoboVersion_comboBox').DataSource=$type.GetMethod('GetVersions').Invoke($null,$versionArgs)
    (Control 'DateFrom_dateTimePicker').Value=$filter.From;(Control 'DateTo_dateTimePicker').Value=$filter.To
    (Control 'Search_panel').Height=0
    $form.ClientSize=New-Object Drawing.Size(1500,780)
    $form.Show();$form.Check_Theme();[Windows.Forms.Application]::DoEvents()
    foreach($mode in @(2,3,2)) {
        (Control 'SearchBy_comboBox').SelectedIndex=$mode
        $timer.Restart()
        $formType.GetMethod('LoadKoboResults',$flags).Invoke($form,@())|Out-Null
        [Windows.Forms.Application]::DoEvents();$timer.Stop()
        Write-Output ("Grid mode={0}, visits={1}: {2} ms" -f $mode,$Visits,$timer.ElapsedMilliseconds)
    }
} finally {$form.Close();$form.Dispose()}

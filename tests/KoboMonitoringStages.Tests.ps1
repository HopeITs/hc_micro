param([Parameter(Mandatory=$true)][string]$AssemblyPath)
$ErrorActionPreference = 'Stop'
$path = (Resolve-Path -LiteralPath $AssemblyPath).Path
$dir = Split-Path -Parent $path
[void][Reflection.Assembly]::LoadFrom((Join-Path $dir 'MySql.Data.dll'))
$json = [Reflection.Assembly]::LoadFrom((Join-Path $dir 'Newtonsoft.Json.dll'))
$app = [Reflection.Assembly]::LoadFrom($path)
$type = $app.GetType('MyWorkApplication.Classes.KoboMonitoringStageMapping', $true)
$parse = $json.GetType('Newtonsoft.Json.Linq.JObject').GetMethod('Parse', [type[]]@([string]))
$method = $type.GetMethod('ResolveStage')
$script:checks = 0
function Check($condition, $message) {
    if (!$condition) { throw $message }
    $script:checks++
}
function Resolve($data, $field, $fixed) {
    $text = ConvertTo-Json -InputObject $data -Compress
    $submission = $parse.Invoke($null, @($text.PSObject.BaseObject))
    $method.Invoke($null, @($submission, [string]$field, $fixed))
}
function Reject($data, $field, $fixed) {
    $rejected = $false
    try { [void](Resolve $data $field $fixed) } catch {
        $cause = $_.Exception
        while ($cause.InnerException) { $cause = $cause.InnerException }
        if ($cause -isnot [InvalidOperationException] -and $cause.GetType() -ne [Exception]) { throw }
        $rejected = $true
    }
    Check $rejected 'Invalid stage was accepted'
}
$forms = Get-Content (Join-Path $PSScriptRoot 'fixtures/kobo-monitoring-stages.json') -Raw | ConvertFrom-Json
foreach ($form in $forms) {
    if ($null -ne $form.fixedStage) {
        Check ((Resolve @{} '' ([int]$form.fixedStage)) -eq 4) 'Final must be M4'
    } elseif (!$form.field) {
        Reject @{} '' $null
    } else {
        $stages = if ($form.field -eq 'vist_') { 1..2 } else { 1..4 }
        foreach ($stage in $stages) {
            $code = if ($form.field -eq '__010') { "M&E_$stage" } else { "m$stage" }
            $data = @{}; $data[$form.field] = $code
            Check ((Resolve $data $form.field $null) -eq $stage) "Wrong stage for $($form.uid)"
            $data = @{}; $data['old_group/' + $form.field] = $code
            Check ((Resolve $data $form.field $null) -eq $stage) 'Grouped stage field should resolve'
        }
        Reject @{} $form.field $null
        $data = @{}; $data[$form.field] = 'M5'
        Reject $data $form.field $null
    }
}
foreach ($bad in @('5','M0','M5','Closing','Final','M&E_9','M1 M2','')) { Reject @{stage=$bad} 'stage' $null }
Reject @{} '' 5
Reject @{stage='m1'} 'stage' 4
Reject @{'a/stage'='m1';'b/stage'='m2'} 'stage' $null
Write-Output "Passed $script:checks monitoring stage checks across $($forms.Count) supplied forms. No database connection used."

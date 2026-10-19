param([Parameter(Mandatory=$true)][string]$AssemblyPath)
$ErrorActionPreference = 'Stop'
# Reuse the verified visitor mapping test context and regression suite.
. (Join-Path $PSScriptRoot 'KoboOpeningVisitors.Tests.ps1') -AssemblyPath $AssemblyPath
$importerType = $appAssembly.GetType('MyWorkApplication.Classes.KoboMonitoringImporter', $true)
$resolveDate = $importerType.GetMethod('ResolveDate')
function Date-Of($data) {
    $text = ConvertTo-Json -InputObject $data -Compress
    $submission = $importerType.GetMethod('ParseSubmission').Invoke($null, @($text.PSObject.BaseObject))
    $resolveDate.Invoke($null, [object[]](,$submission))
}
Assert-True ((Date-Of @{ today='2026-10-16'; _submission_time='2026-10-19T12:00:00Z' }).ToString('yyyy-MM-dd') -eq '2026-10-16') 'Visit date must precede upload date'
Assert-True ((Date-Of @{ _submission_time='2026-10-16T23:30:00-04:00' }).ToString('yyyy-MM-dd') -eq '2026-10-16') 'Source calendar date must not shift with local timezone'
foreach ($data in @(@{}, @{ today='invalid'; _submission_time='2026-10-16' })) {
    $rejected = $false
    try { [void](Date-Of $data) } catch { $rejected = $true }
    Assert-True $rejected 'Invalid or missing date must not silently become today'
}
$definitions = Get-Content (Join-Path $PSScriptRoot 'fixtures/kobo-monitoring-fields.json') -Raw -Encoding UTF8 | ConvertFrom-Json
foreach ($definition in $definitions) {
    $context = New-Context $definition.database
    $mapper = $context.Mapper
    $fieldsTable = $mapperType.GetField('fields', $flags).GetValue($mapper)
    $project = $definition.fields | Where-Object { $_.label.Contains('رقم المشروع') } | Select-Object -First 1
    $companion = $definition.fields | Where-Object { $_.label.Contains('مرافق') -and $_.type.StartsWith('select_') } | Select-Object -First 1
    $textField = $definition.fields | Where-Object { $_.label.Contains('مرافق') -and $_.type -eq 'text' } | Select-Object -First 1
    Assert-True ([bool]$project -and [bool]$companion -and [bool]$textField) 'Every supplied M&E form needs verified project and companion fields'
    [void]$fieldsTable.Rows.Add($definition.uid, $project.path, $companion.path, $textField.path, '')
    foreach ($choice in $companion.choices) {
        $source = $fixture.aliases | Where-Object { $_.database -eq $definition.database -and $_.displayName.Trim() -eq $choice.label.Trim() } | Select-Object -First 1
        $ignored = if ($choice.label.Trim() -eq 'لا يوجد مرافق') {1} elseif ($source) {$source.ignored} else {0}
        $id = if ($source -and $source.localUserName) { $context.Ids[$source.localUserName] } else { [DBNull]::Value }
        [void]$context.Mappings.Rows.Add($definition.uid, $choice.code, $choice.label, $ignored, $id)
        $data = @{username='Primary Visitor'}
        $data[$companion.path] = $choice.code
        $data[$textField.path] = 'Unknown Historical Visitor'
        $form = [pscustomobject]@{UID=$definition.uid}
        $people = Get-Visitors $context $form $data
        $expected = if ($ignored) {2} else {3}
        Assert-True ($people.Count -eq $expected) 'M&E primary, companion and unlinked name must be preserved'
        Assert-True ($people[0].UserId -eq 999) 'Primary visitor must be linked'
        Assert-True ($people[$people.Count-1].DisplayName -eq 'Unknown Historical Visitor') 'Unknown person must stay visible'
    }
}
Write-Output "Passed $script:checks cumulative opening and M&E import checks across $($definitions.Count) M&E forms. No database was changed."

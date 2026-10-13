param([Parameter(Mandatory = $true)][string]$AssemblyPath)

$ErrorActionPreference = 'Stop'
$assemblyFile = (Resolve-Path -LiteralPath $AssemblyPath).Path
$assemblyDirectory = Split-Path -Parent $assemblyFile
[void][Reflection.Assembly]::LoadFrom((Join-Path $assemblyDirectory 'MySql.Data.dll'))
$jsonAssembly = [Reflection.Assembly]::LoadFrom((Join-Path $assemblyDirectory 'Newtonsoft.Json.dll'))
$appAssembly = [Reflection.Assembly]::LoadFrom($assemblyFile)
$importerType = $appAssembly.GetType('MyWorkApplication.Classes.KoboLocalVisitImporter', $true)
$importer = [Activator]::CreateInstance($importerType)
$flags = [Reflection.BindingFlags]'Instance, NonPublic'
$candidatesMethod = $importerType.GetMethod('GetVisitorNameCandidates', $flags)
$matchMethod = $importerType.GetMethod('FindVisitorUserId', $flags)
$projectMethod = $importerType.GetMethod('GetProjectNumber', $flags)
$parseMethod = $jsonAssembly.GetType('Newtonsoft.Json.Linq.JObject').GetMethod('Parse', [type[]]@([string]))
$script:checks = 0

function Assert-Candidates([string]$formName, [string]$json, [string[]]$expected) {
    $table = New-Object System.Data.DataTable
    [void]$table.Columns.Add('Kobo_Form_Name', [string])
    [void]$table.Columns.Add('Visit_Category', [string])
    $row = $table.NewRow()
    $row['Kobo_Form_Name'] = $formName
    $row['Visit_Category'] = ''
    $submission = $parseMethod.Invoke($null, @($json))
    $actual = $candidatesMethod.Invoke($importer, @($submission, $row))
    if (($actual -join '|') -cne ($expected -join '|')) {
        throw "$formName candidates: expected '$($expected -join '|')', got '$($actual -join '|')'"
    }
    $script:checks++
}

Assert-Candidates 'Opening Visit-2025-Aleppo' '{"username":"Visitor","_submitted_by":"Uploader","group/select_one_vt8tw79":"kabi","text_xt6wd52":"First Person, Second Person","__006":"debt","__007":"reason","__005":"other job"}' @('Visitor', 'kabi', 'First Person', 'Second Person')
Assert-Candidates 'Opening Viste - Agre - 2026 - Aleppo' '{"_submitted_by":"Visitor","group/__006":"Simon","__007":"Guest","__005":"address"}' @('Visitor', 'Simon', 'Guest')
Assert-Candidates 'Opining visits - livestock- 2026 - Aleppo' '{"username":"Visitor","select_one_vt8tw79":"Fadi","__005":"Guest","__007":"not a visitor"}' @('Visitor', 'Fadi', 'Guest')
Assert-Candidates 'Vehicles Projects - Opening Visit - Aleppo' '{"username":"Visitor","group/__006":"Cloud","group/__007":"Guest","__005":"address"}' @('Visitor', 'Cloud', 'Guest')
Assert-Candidates 'Opening Visit-2025-Aleppo' '{"username":"First_Person","select_one_vt8tw79":"No_compa","text_xt6wd52":"first person"}' @('First_Person')
Assert-Candidates 'Opening Visit-2025-Aleppo' '{}' @()

$users = New-Object System.Data.DataTable
[void]$users.Columns.Add('UserID', [int])
[void]$users.Columns.Add('UserName', [string])
[void]$users.Rows.Add(1, '  First Person  ')
# Labels are escaped so this test script works in Windows PowerShell without a BOM.
$kabiLabel = -join ([char[]]@(0x643,0x627,0x628,0x64A,0x20,0x62F,0x627,0x64A,0x62E))
[void]$users.Rows.Add(2, $kabiLabel)
foreach ($case in @(@('FIRST_PERSON', 1), @('kabi', 2), @('Unknown', -1), @('First', -1))) {
    $actual = $matchMethod.Invoke($importer, @($users.PSObject.BaseObject, $case[0]))
    if ($actual -ne $case[1]) { throw "Visitor '$($case[0])': expected $($case[1]), got $actual" }
    $script:checks++
}
Write-Output "Passed $script:checks Opening visitor regression checks. No database connection was used."

foreach ($case in @(
    @('Opening Visit-2025-Aleppo', '{"text_fw08u96":"123","__001":"__1"}', 123),
    @('Vehicles Projects - Opening Visit - Aleppo', '{"group/__003":"124","__001":"_"}', 124),
    @('Opening Viste - Agre - 2026 - Aleppo', '{"group/__001":"125","__003":"Farmer"}', 125),
    @('Opining visits - livestock- 2026 - Aleppo', '{"__001":"126","__003":"999"}', 126),
    @('Opening Visit-2025-Aleppo', '{"__001":"127"}', -1)
)) {
    $table = New-Object System.Data.DataTable
    [void]$table.Columns.Add('Kobo_Form_Name', [string])
    [void]$table.Columns.Add('Visit_Category', [string])
    $row = $table.NewRow()
    $row['Kobo_Form_Name'] = $case[0]
    $row['Visit_Category'] = ''
    $submission = $parseMethod.Invoke($null, @($case[1]))
    $actual = $projectMethod.Invoke($importer, @($submission, $row))
    if ($actual -ne $case[2]) { throw "$($case[0]) project: expected $($case[2]), got $actual" }
}
Write-Output 'Passed 5 Opening project-number regression checks.'

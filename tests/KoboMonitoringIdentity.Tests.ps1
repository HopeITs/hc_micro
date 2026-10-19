param([Parameter(Mandatory=$true)][string]$AssemblyPath)
$ErrorActionPreference='Stop'
$app=[Reflection.Assembly]::LoadFrom((Resolve-Path -LiteralPath $AssemblyPath).Path)
$type=$app.GetType('MyWorkApplication.Classes.KoboMonitoringIdentity', $true)
$script:count=0
function Check($condition,$message) { if(!$condition){throw $message}; $script:count++ }
function Relationships($rows) {
    $table=New-Object Data.DataTable
    [void]$table.Columns.Add('Person_ID',[int])
    [void]$table.Columns.Add('PersonType',[string])
    foreach($row in $rows){[void]$table.Rows.Add($row[0],$row[1])}
    return ,$table
}
function Select-Person($table) {
    $type.GetMethod('ResolveParticipant').Invoke($null,[object[]](,$table.PSObject.BaseObject))
}
function Must-Reject($table) {
    $failed=$false
    try{[void](Select-Person $table)}catch{
        $e=$_.Exception;while($e.InnerException){$e=$e.InnerException}
        if($e -isnot [InvalidOperationException]){throw}
        $failed=$true
    }
    Check $failed 'Ambiguous or missing identity must not be guessed'
}
$beneficiary='مستفيد';$partner='شريك'
foreach($id in @(9179,13681)) {
    $table=Relationships (,@($id,$partner))
    $chosen=Select-Person $table
    Check ($chosen.Person_ID -eq $id) 'A sole partner must be preserved as the actual project participant'
    Check ($chosen.PersonType -eq $partner) 'Partner role must not be rewritten as beneficiary'
}
$table=Relationships @(@(100,$partner),@(200,$beneficiary))
Check ((Select-Person $table).Person_ID -eq 200) 'Explicit beneficiary takes precedence over partner'
$table=Relationships @(@(200,$beneficiary),@(200,$beneficiary))
Check ((Select-Person $table).Person_ID -eq 200) 'Repeated relationships for the same person must deduplicate'
Must-Reject (Relationships @())
Must-Reject (Relationships @(@(100,$partner),@(200,$partner)))
Must-Reject (Relationships @(@(100,$beneficiary),@(200,$beneficiary)))
Must-Reject (Relationships (,@(100,'Other role')))
$projects=New-Object Data.DataTable
[void]$projects.Columns.Add('MicroProject_ID',[int])
[void]$projects.Rows.Add(828)
[void]$projects.Rows.Add(1149)
$isolate=$type.GetMethod('RequiresProjectIsolation')
Check ($isolate.Invoke($null,@($projects.PSObject.BaseObject,828))) 'Person 3542 source project 828 must remain outside person-only legacy storage'
[void]$projects.Rows.RemoveAt(1)
Check (!$isolate.Invoke($null,@($projects.PSObject.BaseObject,828))) 'Single-project participant may use legacy storage'
$failed=$false
try{[void]$isolate.Invoke($null,@($projects.PSObject.BaseObject,1149))}catch{$failed=$true}
Check $failed 'Cannot assign a person to a project absent from their relationships'
Write-Output "Passed $script:count project identity checks. No database changes."

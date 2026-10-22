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
$named=$type.GetMethod('ResolveNamedParticipant')
function Named-Table($rows) {
    $t=New-Object Data.DataTable
    [void]$t.Columns.Add('Person_ID',[int])
    foreach($c in @('PersonType','P_FirstName','P_LastName','P_FatherName')){[void]$t.Columns.Add($c)}
    foreach($r in $rows){[void]$t.Rows.Add($r)}
    return ,$t
}
function Match-Name($table,$name){$named.Invoke($null,@($table.PSObject.BaseObject,[string]$name))}
function Reject-Name($table,$name){
    $rejected=$false
    try{[void](Match-Name $table $name)}catch{
        $cause=$_.Exception;while($cause.InnerException){$cause=$cause.InnerException}
        if($cause -isnot [InvalidOperationException]){throw};$rejected=$true
    }
    Check $rejected ("Uncertain name matching must not assign a person: " + $name)
}
$t=Named-Table @(@(101,$partner,'ديما','بيطار','سمير'),@(102,$partner,'سارة','حنا','جورج'))
Check ((Match-Name $t 'ديما بيطار').Person_ID -eq 101) 'Full Kobo name must select one of two project partners'
Check ((Match-Name $t "  ديما`t  بيطار  ").Person_ID -eq 101) 'Whitespace formatting must not prevent exact full-name comparison'
Check ((Match-Name $t 'دِيما بيطار').Person_ID -eq 101) 'Arabic vowel marks are formatting, not a separate identity'
Check ((Match-Name $t 'ديـما بيطار').Person_ID -eq 101) 'Tatweel formatting must be ignored'
Check ((Match-Name $t 'ديما سمير بيطار').Person_ID -eq 101) 'First father last format must compare against separated database fields'
Check ((Match-Name $t 'ديما بيطار ابن/ة سمير').Person_ID -eq 101) 'Existing app full-name format must compare correctly'
Check ((Match-Name $t 'ديما بيطار').PersonType -eq $partner) 'Named matching must not change partner roles'
Reject-Name $t 'ديمة بيطار'
Reject-Name $t 'ديما'
Reject-Name $t 'ديما حنا'
Reject-Name $t 'شخص من مشروع آخر'
[void]$t.Rows.Add(103,$partner,'ديما','بيطار','جورج')
Reject-Name $t 'ديما بيطار'
Check ((Match-Name $t 'ديما سمير بيطار').Person_ID -eq 101) 'Father name may disambiguate two equal first/last names'
$t=Named-Table @(@(101,$beneficiary,'سارة','حنا','جورج'),@(102,$partner,'ديما','بيطار','سمير'))
Check ((Match-Name $t 'ديما بيطار').Person_ID -eq 102) 'An explicit partner name must not be overridden by another beneficiary role'
$t=Named-Table @(@(6326,$beneficiary,'كارولين','عزيز','وليد'),@(7502,$partner,'فادي','عزيز','وليد'))
Check ((Match-Name $t 'كارولين عزيز وفادي عزيز').Person_ID -eq 6326) 'When Kobo includes the beneficiary and partner names, choose the sole matching beneficiary'
Check ((Match-Name $t 'فادي عزيز وكارولين عزيز').Person_ID -eq 6326) 'Combined names must match in either order and with an attached Arabic conjunction'
$multipleBeneficiaries=Named-Table @(@(6326,$beneficiary,'كارولين','عزيز','وليد'),@(7502,$beneficiary,'فادي','عزيز','وليد'))
Reject-Name $multipleBeneficiaries 'كارولين عزيز وفادي عزيز'
$single=Named-Table (,@(101,$partner,'ديما','بيطار','سمير'))
Check ((Match-Name $single 'اسم مختلف').Person_ID -eq 101) 'Name comparison must not reject an already unambiguous single-person project'
Check ((Match-Name $single 'ديمة بيطار').Person_ID -eq 101) 'Spelling variation must not invalidate a sole project participant'
Check ((Match-Name $single 'ديما').Person_ID -eq 101) 'Incomplete Kobo name must not invalidate an unambiguous project link'
$missing=Named-Table (,@(101,$partner,'','',''))
Check ((Match-Name $missing 'اسم المستفيد').Person_ID -eq 101) 'Missing database name fields must not invalidate the sole project link'
Check ((Match-Name $single '').Person_ID -eq 101) 'Missing Kobo name must preserve existing unambiguous project-only behavior'
$uniqueBeneficiary=Named-Table @(@(101,$beneficiary,'سارة','حنا','جورج'),@(102,$partner,'ديما','بيطار','سمير'))
Check ((Match-Name $uniqueBeneficiary '').Person_ID -eq 101) 'A missing name preserves the existing unique-beneficiary fallback'
Check ($type.GetMethod('NormalizePersonName').Invoke($null,@('ديما')) -ne $type.GetMethod('NormalizePersonName').Invoke($null,@('ديمة'))) 'No automatic spelling aliases may be introduced'
Write-Output "Passed $script:count project identity checks. No database changes."

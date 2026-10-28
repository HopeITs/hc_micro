param([Parameter(Mandatory=$true)][string]$AssemblyPath, [string]$PreviewDirectory)
$ErrorActionPreference = 'Stop'
$path = (Resolve-Path -LiteralPath $AssemblyPath).Path
$dir = Split-Path -Parent $path
[void][Reflection.Assembly]::LoadFrom((Join-Path $dir 'MySql.Data.dll'))
[void][Reflection.Assembly]::LoadFrom((Join-Path $dir 'Newtonsoft.Json.dll'))
$app = [Reflection.Assembly]::LoadFrom($path)
$type = $app.GetType('MyWorkApplication.Classes.KoboVisitResults', $true)
$filterType = $app.GetType('MyWorkApplication.Classes.KoboResultsFilter', $true)
$script:checks = 0
function Check($condition, $message) { if (!$condition) { throw $message }; $script:checks++ }
function Check-SummaryOrder($table) {
    $answers=@($table.Columns | Where-Object ColumnName -match '^Answer\d+$')
    for($i=1;$i -le $answers.Count;$i++) {
        Check ($table.Columns[$i].ColumnName -eq "Answer$i") 'All answer labels must appear together before any count columns'
        Check ($table.Columns[$answers.Count+$i].ColumnName -eq "Count$i") 'All count columns must follow all answers in the same numeric order'
    }
    Check ($table.Columns[2*$answers.Count+1].ColumnName -eq 'Answered') 'The total must follow all answer and count columns'
}
$schemaGuard=$type.GetMethod('ValidateMonitoringSchema',[Reflection.BindingFlags]'Static,NonPublic')
$schemaGuard.Invoke($null,@($true,'ready_database')) | Out-Null
Check ($true) 'An installed monitoring registry must allow the query to proceed'
foreach ($database in @('hcsyria_micro','another_governorate')) {
    $schemaError=$null
    try { $schemaGuard.Invoke($null,@($false,$database)) | Out-Null }
    catch { $schemaError=$_.Exception.GetBaseException() }
    Check ($schemaError -is [InvalidOperationException]) 'A missing registry must stop the report, not return misleading empty results'
    Check ($schemaError.Message.Contains($database) -and $schemaError.Message.Contains('kobo_monitoring_record')) 'The setup message must identify the selected database and missing table without a hardcoded governorate'
    Check ($schemaError.Message.Contains('تعذر عرض نتائج زيارات كوبو')) 'Missing schema must produce an understandable Arabic message'
    $visitor=$schemaError.Message.IndexOf('KoboVisitorMapping_Upgrade.sql')
    $stage=$schemaError.Message.IndexOf('KoboMonitoringStage_Upgrade.sql')
    $import=$schemaError.Message.IndexOf('KoboMonitoringImport_Upgrade.sql')
    Check ($visitor -ge 0 -and $stage -gt $visitor -and $import -gt $stage) 'The message must list the existing migrations in prerequisite order'
}
$catalog = Get-Content (Join-Path $PSScriptRoot '../MyWorkApplication/KoboQuestionCatalog.json') -Raw -Encoding UTF8 | ConvertFrom-Json
$assessmentQuestion=$type.GetMethod('IsAssessmentQuestion',[Reflection.BindingFlags]'Static,NonPublic')
Check (@($catalog.PSObject.Properties).Count -eq 41) 'All 41 supplied forms must be bundled'
foreach ($entry in $catalog.PSObject.Properties) {
    Check ($entry.Value.fields.Count -gt 0) 'A language-column mismatch must never produce an empty form definition'
    $paths = @{}
    foreach ($field in $entry.Value.fields) {
        Check (!$paths.ContainsKey($field.path)) "Duplicate field path in $($entry.Name): $($field.path)"
        $paths[$field.path] = $true
        Check (![string]::IsNullOrWhiteSpace($field.label)) 'Every defined question must retain its source label'
        if($field.path -match '^(group_mo5jr41|group_vv6uu79|group_vz9zl11)/' -or $field.label -eq 'رقم الزيارة') {
            Check (!$assessmentQuestion.Invoke($null,@([string]$field.path,[string]$field.label))) 'Administrative fields must be excluded consistently across every supplied form'
        }
    }
}
$recovered=@($catalog.aL3TGhp5wAhuPseTMhEksd.fields | Where-Object path -eq 'input_verification/inputs_used')[0]
Check ($recovered.label -match '[\u0600-\u06ff]' -and $recovered.choices.Count -gt 0) 'Arabic labels stored under the English language column must be recovered for questions and choices'
foreach($case in @(
    @('assessment/location_effect','ما مدى تأثير المنطقة على المشروع ؟'),
    @('assessment/services','ما هو وضع الخدمات في المنطقة بالنسبة لمشروعك (كهرباء، مياه) ؟'),
    @('assessment/debts','هل يترتب عليك ديون متعلقة بالمشروع ؟'),
    @('general_info/total_area','مساحة الأرض الكلية (بالدونم)')
)) {
    Check ($assessmentQuestion.Invoke($null,@($case[0],$case[1]))) 'Assessment content must not be excluded merely because it mentions location, services or general project information'
}
$uid = 'aDGPm94uwS7HYk4LcG46oh'
$multi = @($catalog.$uid.fields | Where-Object type -like 'select_multiple*')[0]
$codes = @($multi.choices | ForEach-Object code)
Check ($codes.Count -gt 3) 'Fixture must exercise more than the three legacy answer slots'
$data = New-Object Data.DataTable
foreach ($name in @('ID','Kobo_Submission_ID','Kobo_Form_ID','Kobo_Form_Name','Kobo_Form_Version','Raw_JSON','Sync_Status','Project_ID','Person_ID','Visit_Date','Stage','Visit_Kind','Beneficiary','MP_Type_ID','Category_ID','SubCategory_ID','Career_ID','SubCategory_Name','Career_Name')) {
    [void]$data.Columns.Add($name)
}
[void]$data.Columns.Add('Kobo_Is_Deleted',[object])
function Source($id,$json,$status='local_inserted',$deleted=0,$project='100',$person='6326',$stage='M1',$date='2026-10-20',$version='v1') {
    $row=$data.NewRow()
    $values=@{ID=$id;Kobo_Submission_ID="600$id";Kobo_Form_ID=$uid;Kobo_Form_Name='Final Visit - Aleppo';Kobo_Form_Version=$version;Raw_JSON=$json;Sync_Status=$status;Kobo_Is_Deleted=$deleted;Project_ID=$project;Person_ID=$person;Visit_Date=$date;Stage=$stage;Visit_Kind='o';Beneficiary='كارولين عزيز';MP_Type_ID='1';Category_ID='2';SubCategory_ID='3';Career_ID='4';SubCategory_Name='حرف';Career_Name='خياطة'}
    foreach($key in $values.Keys){$row[$key]=$values[$key]}
    [void]$data.Rows.Add($row)
}
$freeText="نص حر O'Neil [A]%*"
$first=@{}; $first[$multi.path]="$($codes[0]) $($codes[1]) $($codes[0])"; $first['future_field']=$freeText;$first['_id']=123
$second=@{}; $second[$multi.path]=$codes[1]
$fourth=@{today='2026-10-20'}; $fourth[$multi.path]='new_code_from_another_version'
Source 1 (ConvertTo-Json $first -Compress) 'local_inserted' $false
Source 2 (ConvertTo-Json $second -Compress) 'local_updated' 0 '100' '6326' 'M1' '2026-10-20' 'v2'
Source 3 '{}' 'local_inserted' 0 '100' '7502' 'M4' '2026-10-21'
Source 4 (ConvertTo-Json $fourth -Compress) 'local_inserted' 0 '200' '6326' 'Opening'
Source 5 '{}' 'local_error'
Source 6 '{}' 'local_inserted' $true
Source 7 '{}' 'review_replaced'
Source 8 '{}' 'review_ignored'
Source 9 '{}' 'review_deferred'
Source 10 '{}' 'raw_updated'
Source 11 '{}' 'local_inserted' 0 ''
Source 12 '{broken json'
$data.ImportRow($data.Rows[0])
function New-ResultFilter {
    $filter=[Activator]::CreateInstance($filterType)
    $filter.From=[datetime]'2026-01-01';$filter.To=[datetime]'2026-12-31'
    return $filter
}
function Report($filter,$counts=$true,$formUid=$uid) {
    return $type.GetMethod('Build').Invoke($null,@($data.PSObject.BaseObject,$formUid,$filter,$counts))
}
$filter=New-ResultFilter
$report=Report $filter
Check-SummaryOrder $report.Table
Check ($report.Visits -eq 4) 'Each confirmed source must count once, including different UUIDs with the same project/stage/date'
Check ($report.Excluded -eq 7) 'Errors, pending imports, ignored, deferred, replaced, deleted and unlinked sources must be excluded'
Check ($report.InvalidJson -eq 1) 'Unreadable JSON must be reported separately'
$row=@($report.Table.Rows | Where-Object Question -eq $multi.label)[0]
Check ($row.Answer1 -eq $multi.choices[0].label) 'Arabic choices must retain XLSForm order'
Check ($row.Count1 -eq 1 -and $row.Count2 -eq 2) 'Multiple selections must count each code once per submission'
Check ($row.Count3 -eq 0) 'Unselected defined choices must remain visible with zero counts'
Check ($row.Answer10 -eq 'new_code_from_another_version' -and $row.Count10 -eq 1) 'Unknown historical choice codes must remain distinguishable'
Check ($row.Answered -eq 3 -and $row.Unanswered -eq 1 -and $row.Visits -eq 4) 'Answered plus blank must reconcile to confirmed visits'
Check (@($report.Table.Rows | Where-Object Question -eq '_id').Count -eq 0) 'Kobo transport metadata must not become a questionnaire row'
$detail=Report $filter $false
Check ($detail.Table.Rows.Count -eq $report.Visits) 'Beneficiary row count must equal the summary source count'
Check ($detail.Table.Rows[0].Project_ID -eq 100 -and $detail.Table.Rows[3].Project_ID -eq 200) 'The same person in different projects must retain the explicit project link'
$multiColumn=@($detail.Table.Columns | Where-Object { $_.ExtendedProperties['Path'] -eq $multi.path })[0]
Check ($detail.Table.Rows[0][$multiColumn] -like "*$($multi.choices[0].label)*") 'Beneficiary cells must display the original choice label'
$futureColumn=@($detail.Table.Columns | Where-Object { $_.ExtendedProperties['Path'] -eq 'future_field' })[0]
Check ($detail.Table.Rows[0][$futureColumn] -eq $freeText) 'Free text must not be split into choice codes'
$filter.Version='v2';Check ((Report $filter).Visits -eq 1) 'Version filter must isolate the selected version'
$filter=New-ResultFilter;$filter.Stage='M1';Check ((Report $filter).Visits -eq 2) 'Exact stage filter must work'
$filter.Stage='Openings';Check ((Report $filter).Visits -eq 1) 'Opening visits must have their own stage'
$filter.Stage='Monitorings';Check ((Report $filter).Visits -eq 3) 'All monitorings must exclude opening visits'
$filter=New-ResultFilter;$filter.From=[datetime]'2026-10-20';$filter.To=$filter.From
Check ((Report $filter).Visits -eq 3) 'Visit date range must include both endpoints'
$filter=New-ResultFilter;$filter.LastVisitOnly=$true
$last=Report $filter $false
Check ($last.Visits -eq 3 -and @($last.Table.Rows | Where-Object { $_.Project_ID -eq 100 -and $_[$multiColumn.ColumnName] -eq $multi.choices[1].label }).Count -eq 1) 'Latest visit must be deterministic per project/person, including same-day ties'
$filter=New-ResultFilter;$filter.ProjectType='9';Check ((Report $filter).Visits -eq 0) 'Project filters must not silently ignore a mismatch'
$versionArgs=New-Object object[] 1
$versionArgs[0]=$data.PSObject.BaseObject
$versions=$type.GetMethod('GetVersions').Invoke($null,$versionArgs)
Check ($versions.Rows.Count -eq 3) 'Version list must contain all versions and the all-versions entry'

# Exercise nested and repeated JSON without relying on guessed field names.
$repeatData=$data.Clone()
$repeatData.ImportRow($data.Rows[0]);$repeatData.Rows[0].Raw_JSON='{"group":{"note":"nested"},"repeat":[{"repeat/value":"one"},{"repeat/value":"two"}]}'
$repeat=$type.GetMethod('Build').Invoke($null,@($repeatData.PSObject.BaseObject,'unknown-form',(New-ResultFilter),$false))
$repeated=@($repeat.Table.Columns | Where-Object { $_.ExtendedProperties['Path'] -eq 'repeat/value' })[0]
Check ($repeat.Table.Rows[0][$repeated] -eq 'one | two') 'Repeat fields must be preserved within a single visit row'
Check ($repeat.Table.Rows.Count -eq 1) 'Repeated groups must not inflate visit counts'

$openingData=$data.Clone();$openingData.ImportRow($data.Rows[3])
$openingData.Rows[0].Visit_Date='2026-11-30';$openingData.Rows[0].Visit_Kind='O-op'
$openingFilter=New-ResultFilter;$openingFilter.From=[datetime]'2026-10-20';$openingFilter.To=$openingFilter.From;$openingFilter.Kind='o'
$opening=$type.GetMethod('Build').Invoke($null,@($openingData.PSObject.BaseObject,$uid,$openingFilter,$false))
Check ($opening.Visits -eq 1 -and $opening.Table.Rows[0].Visit_Date -eq [datetime]'2026-10-20') 'Opening filters must use the source visit date and normalize legacy opening category codes'
Check ($openingData.Rows[0].Visit_Date -eq '2026-11-30') 'Building a report must not alter source data'
$openingData.Rows[0].Raw_JSON='{}'
$badOpening=$type.GetMethod('Build').Invoke($null,@($openingData.PSObject.BaseObject,$uid,$openingFilter,$false))
Check ($badOpening.Visits -eq 0 -and $badOpening.InvalidDate -eq 1) 'Missing opening source dates must be identified, not replaced with the current date'

# Combine real questions whose field paths, choice lists and codes differ across forms.
$allData=$data.Clone()
foreach ($i in @(0,1,2)) { $allData.ImportRow($data.Rows[$i]) }
$uids=@('a3BwzyULUpD3P8jh9B8ADn','a6yNApUu9Jqph4RumUzxAw','aDGPm94uwS7HYk4LcG46oh')
$question='هل يترتب عليك ديون؟'
for ($i=0;$i -lt 3;$i++) {
    $formUid=$uids[$i]
    $debt=@($catalog.$formUid.fields | Where-Object { $_.label -eq $question })
    if($i -eq 2){$debt=@($catalog.$formUid.fields | Where-Object { $_.label -eq 'هل كان لديك ديون متعلقة بانطلاق المشروع؟' })}
    $yes=@($debt[0].choices | Where-Object label -eq 'نعم')[0]
    $answer=@{};$answer[$debt[0].path]=$yes.code
    $allData.Rows[$i].Kobo_Form_ID=$formUid;$allData.Rows[$i].Kobo_Form_Name="Form $i"
    $allData.Rows[$i].Raw_JSON=ConvertTo-Json $answer -Compress
}
$allData.ImportRow($allData.Rows[0])
$allData.ImportRow($data.Rows[5])
$firstAll=$allData.Rows[0].Raw_JSON | ConvertFrom-Json
$searchUid=$uids[0]
$searchField=@($catalog.$searchUid.fields | Where-Object { $_.type -eq 'text' -and $_.path -notmatch '^(group_mo5jr41|group_vv6uu79|group_vz9zl11)/' })[0]
$firstAll | Add-Member -NotePropertyName $searchField.path -NotePropertyValue $freeText
$firstAll | Add-Member -NotePropertyName future_field -NotePropertyValue $freeText
$allData.Rows[0].Raw_JSON=ConvertTo-Json $firstAll -Compress
function Combined($table,$filter=(New-ResultFilter),$counts=$true) {
    return $type.GetMethod('BuildCombined').Invoke($null,@($table.PSObject.BaseObject,$filter,$counts))
}
$combined=Combined $allData
Check ($combined.Visits -eq 3 -and $combined.Excluded -eq 1) 'One combined report must count submissions across forms without multiplying duplicate IDs or including deleted sources'
$debtRows=@($combined.Table.Rows | Where-Object Question -eq $question)
Check ($debtRows.Count -eq 1) 'The same question in different form paths must produce one shared row'
$debtRow=$debtRows[0]
Check ($debtRow.Answer1 -eq 'نعم' -and $debtRow.Count1 -eq 2) 'Different choice codes (Yes/yes) with the same label must contribute to the same answer count'
Check ($debtRow.Visits -eq 2 -and $debtRow.Answered -eq 2 -and $debtRow.Unanswered -eq 0) 'Forms that do not ask a question must not inflate its blank-answer denominator'
Check (@($combined.Table.Rows | Where-Object Question -eq 'هل كان لديك ديون متعلقة بانطلاق المشروع؟').Count -eq 1) 'Differently worded debt questions must remain separate, without guessed aliases'
Check-SummaryOrder $combined.Table
$combinedDetails=Combined $allData (New-ResultFilter) $false
Check ($combinedDetails.Table.Rows.Count -eq 3) 'All-form beneficiary mode must have one row per confirmed source'
Check ($combinedDetails.UnmappedFields -eq 1 -and @($combinedDetails.Table.Columns | Where-Object Caption -like '*future_field*').Count -eq 0) 'Undefined fields must be counted for source review, not exposed as technical report headers'
foreach($column in @($combinedDetails.Table.Columns | Where-Object ColumnName -match '^q_')) {
    Check (@($combinedDetails.Table.Rows | Where-Object { ![string]::IsNullOrWhiteSpace([string]$_[$column]) }).Count -gt 0) 'The beneficiary report must not contain entirely empty question columns'
    Check ($column.Caption -notmatch 'group_|field_challenges/|input_verification/|Form \d|question_') 'Question headers must contain readable labels rather than source paths or form names'
}
$debtColumns=@($combinedDetails.Table.Columns | Where-Object Caption -eq $question)
Check ($debtColumns.Count -eq 1 -and $combinedDetails.Table.Rows[0][$debtColumns[0]] -eq 'نعم' -and $combinedDetails.Table.Rows[1][$debtColumns[0]] -eq 'نعم') 'Shared beneficiary answer columns must translate each form code independently'
Check ($combinedDetails.Table.Rows[2][$debtColumns[0]] -eq '') 'A question absent from a form must remain blank in the shared beneficiary table'
Check (@($combined.Table.Rows | Where-Object { $_.Question -match '^(المتبرع|العنوان|المرافقين|رقم الزيارة)$' -or $_.Question -like 'هل يرافقك قسم*' }).Count -eq 0) 'Combined frequency results must contain assessment questions, not basic visit information'
Check (@($combinedDetails.Table.Columns | Where-Object { [string]$_.ExtendedProperties['Path'] -match '(group_mo5jr41|group_vv6uu79|group_vz9zl11)/' }).Count -eq 0) 'Beneficiary answer columns must also exclude the basic-information and vehicle-identification sections'
Check ($combinedDetails.Table.Columns.Contains('Project_ID') -and $combinedDetails.Table.Columns.Contains('Beneficiary') -and $combinedDetails.Table.Columns.Contains('Stage') -and $combinedDetails.Table.Columns.Contains('Visit_Date')) 'Existing project/person/date/stage context columns must remain available'
foreach($column in @('Kobo_Form_Name','Kobo_Form_Version','Kobo_Submission_ID')) {
    Check (!$combinedDetails.Table.Columns.Contains($column)) 'Technical Kobo metadata must not become beneficiary report or export columns'
}
$lastCombinedFilter=New-ResultFilter;$lastCombinedFilter.LastVisitOnly=$true
Check ((Combined $allData $lastCombinedFilter).Visits -eq 2) 'Last visit must be chosen across forms, not once per form'
$combinedVersion=New-ResultFilter;$combinedVersion.Version='v2'
Check ((Combined $allData $combinedVersion).Visits -eq 1) 'Version selection must continue to filter combined results'
$unknownCodeData=$allData.Copy()
for($i=0;$i -lt 2;$i++) {
    $formUid=$uids[$i];$debt=@($catalog.$formUid.fields | Where-Object label -eq $question)[0]
    $answer=@{};$answer[$debt.path]='unmapped';$unknownCodeData.Rows[$i].Raw_JSON=ConvertTo-Json $answer -Compress
}
$unknownCode=Combined $unknownCodeData
$unknownRow=@($unknownCode.Table.Rows | Where-Object Question -eq $question)[0]
$unknownOptions=@($unknownCode.Table.Columns | Where-Object { $_.ColumnName -match '^Answer\d+$' -and [string]$unknownRow[$_] -like 'unmapped*' })
Check ($unknownOptions.Count -eq 2) 'The same unmapped code in two forms must not be guessed to mean the same answer'
$combinedMulti=Combined $data
Check-SummaryOrder $combinedMulti.Table
$combinedMultiRow=@($combinedMulti.Table.Rows | Where-Object Question -eq $multi.label)[0]
Check ($combinedMultiRow.Count1 -eq 1 -and $combinedMultiRow.Count2 -eq 2 -and $combinedMultiRow.Answer10 -like 'new_code*') 'Combined multiple-choice results must retain every option and count each selection once per source'

$collisionData=$allData.Clone()
$runtimeCatalog=$type.GetField('catalog',[Reflection.BindingFlags]'Static,NonPublic').GetValue($null).Value
function AssessmentDefinition($uid,$source,$label,$path,$context='',$section='') {
    $definition=@{fields=@(@{path=$path;label=$label;type=$source.type;repeat=$false;context=$context;section=$section;choices=@($source.choices)})}
    $runtimeCatalog.Add($uid,[Newtonsoft.Json.Linq.JObject]::Parse((ConvertTo-Json $definition -Depth 8 -Compress)))
}
$collisionSources=@('a3BwzyULUpD3P8jh9B8ADn','ay2c9afff5StTkQdneHHUS')
$collisionUids=@('test-assessment-code-a','test-assessment-code-b')
for($i=0;$i -lt 2;$i++) {
    $collisionData.ImportRow($allData.Rows[$i]);$formUid=$collisionUids[$i]
    $sourceUid=$collisionSources[$i]
    $address=@($catalog.$sourceUid.fields | Where-Object label -eq 'العنوان - حلب')[0]
    AssessmentDefinition $formUid $address 'سؤال تقييم تجريبي' 'assessment/code_collision'
    $answer=@{'assessment/code_collision'='region_53'}
    $collisionData.Rows[$i].Kobo_Form_ID=$formUid;$collisionData.Rows[$i].Raw_JSON=ConvertTo-Json $answer -Compress
}
$collision=Combined $collisionData
$addressRow=@($collision.Table.Rows | Where-Object Question -eq 'سؤال تقييم تجريبي')[0]
foreach($label in @('متنقل','أدلب - جديدة')) {
    $answerColumn=@($collision.Table.Columns | Where-Object { $_.ColumnName -match '^Answer\d+$' -and [string]$addressRow[$_] -eq $label })[0]
    Check ($null -ne $answerColumn -and $addressRow[($answerColumn.ColumnName -replace '^Answer','Count')] -eq 1) 'A real reused code (region_53) with different labels in different forms must not combine different answers'
}
$blankLabelData=$allData.Clone();$blankLabelData.ImportRow($allData.Rows[0])
$blankSourceUid='aPLm9ggZu2a6qryEZ2HPoY'
$blankUid='test-assessment-unlabelled-option'
$blankAddress=@($catalog.$blankSourceUid.fields | Where-Object label -eq 'العنوان')[0]
$blankAddress=$blankAddress | ConvertTo-Json -Depth 8 | ConvertFrom-Json
@($blankAddress.choices | Where-Object code -eq 'region_54')[0].label=''
AssessmentDefinition $blankUid $blankAddress 'سؤال تقييم بخيارات غير مترجمة' 'assessment/unlabelled'
$answer=@{'assessment/unlabelled'='region_54'}
$blankLabelData.Rows[0].Kobo_Form_ID=$blankUid;$blankLabelData.Rows[0].Raw_JSON=ConvertTo-Json $answer -Compress
$blankLabelReport=Combined $blankLabelData
$blankRow=@($blankLabelReport.Table.Rows | Where-Object Question -eq 'سؤال تقييم بخيارات غير مترجمة')[0]
$blankOption=@($blankLabelReport.Table.Columns | Where-Object { $_.ColumnName -match '^Answer\d+$' -and [string]$blankRow[$_] -like 'region_54 *' })[0]
Check ($null -ne $blankOption -and $blankRow[($blankOption.ColumnName -replace '^Answer','Count')] -eq 1) 'A choice missing its source label must retain its own code instead of merging all blank labels'

$futureAdminData=$allData.Copy()
$payload=$futureAdminData.Rows[0].Raw_JSON | ConvertFrom-Json
$payload | Add-Member -NotePropertyName 'group_mo5jr41/future_basic_field' -NotePropertyValue 'metadata'
$payload | Add-Member -NotePropertyName 'group_na4gu14/vist_no' -NotePropertyValue 'M2'
$futureAdminData.Rows[0].Raw_JSON=ConvertTo-Json $payload -Compress
$futureAdmin=Combined $futureAdminData (New-ResultFilter) $false
Check (@($futureAdmin.Table.Columns | Where-Object { [string]$_.ExtendedProperties['Path'] -match 'future_basic_field|group_na4gu14/vist_no' }).Count -eq 0) 'Unknown fields inside basic-information sections must not leak back in as raw question paths'
Check ($futureAdminData.Rows[0].Raw_JSON.Contains('future_basic_field')) 'Report filtering must not remove original source data'

# Formatting-only differences merge, but generic follow-up questions retain meaningful context.
$formattedData=$allData.Clone()
for($i=0;$i -lt 2;$i++) {
    $formattedData.ImportRow($allData.Rows[$i]);$formUid="test-formatted-$i"
    $sourceUid=$uids[$i];$sourceQuestion=@($catalog.$sourceUid.fields | Where-Object label -eq $question)[0]
    $label=if($i -eq 0){'هَل يترتب عليك ديون؟'}else{'هل يترتب عليك ديون ?'}
    AssessmentDefinition $formUid $sourceQuestion $label 'assessment/debt'
    $yes=@($sourceQuestion.choices | Where-Object label -eq 'نعم')[0]
    $formattedData.Rows[$i].Kobo_Form_ID=$formUid;$formattedData.Rows[$i].Raw_JSON=ConvertTo-Json @{'assessment/debt'=$yes.code} -Compress
}
$formatted=Combined $formattedData (New-ResultFilter) $false
Check ($formatted.Table.Columns.Count -eq 7 -and $formatted.Table.Rows[0]['q_0'] -eq 'نعم' -and $formatted.Table.Rows[1]['q_0'] -eq 'نعم') 'Vowel marks and trailing question-mark formatting must not create duplicate columns'
$whyData=$allData.Clone()
$whySource=[pscustomobject]@{type='text';choices=@()}
for($i=0;$i -lt 3;$i++) {
    $whyData.ImportRow($allData.Rows[$i]);$formUid="test-followup-$i"
    $context=if($i -lt 2){'هل حقق المشروع الهدف؟'}else{'هل لديك ديون؟'}
    AssessmentDefinition $formUid $whySource 'لماذا ؟' 'assessment/why' $context 'الاستمارة'
    $whyData.Rows[$i].Kobo_Form_ID=$formUid;$whyData.Rows[$i].Raw_JSON=ConvertTo-Json @{'assessment/why'="سبب $i"} -Compress
}
$why=Combined $whyData (New-ResultFilter) $false
$whyColumns=@($why.Table.Columns | Where-Object ColumnName -match '^q_')
Check ($whyColumns.Count -eq 2 -and $why.Table.Rows.Count -eq 3) 'Equivalent follow-ups must share a column; follow-ups about different parent questions must remain separate without losing visits'
Check ($whyColumns[0].Caption -like '*هل حقق المشروع الهدف*' -and $whyColumns[1].Caption -like '*هل لديك ديون*') 'Distinct follow-up headers must use Arabic question context, never technical field paths'
$missingFormData=$allData.Clone();$missingFormData.ImportRow($allData.Rows[0])
$missingFormData.Rows[0].Kobo_Form_ID='aL3TGhp5wAhuPseTMhEksd'
$missingFormData.Rows[0].Raw_JSON=ConvertTo-Json @{$recovered.path=$recovered.choices[0].code} -Compress
$recoveredReport=Combined $missingFormData (New-ResultFilter) $false
Check ($recoveredReport.UnmappedFields -eq 0 -and $recoveredReport.Table.Columns[6].Caption -eq $recovered.label -and $recoveredReport.Table.Rows[0][6] -eq $recovered.choices[0].label) 'Previously empty agriculture definitions must now produce readable question and answer labels'

Add-Type -AssemblyName System.Windows.Forms
Add-Type -AssemblyName System.Drawing
$formType=$app.GetType('MyWorkApplication.Visit_Forms.Visit_Results_Form',$true)
$form=$formType.GetConstructors()[0].Invoke(@($null))
$flags=[Reflection.BindingFlags]'Instance,NonPublic'
function Control($name){return $formType.GetField($name,$flags).GetValue($form)}
function Set-Field($name,$value){$formType.GetField($name,$flags).SetValue($form,$value)}
$load=[Delegate]::CreateDelegate([EventHandler],$form,$formType.GetMethod('Monitoring_Visit_Results_Form_Load',$flags))
$form.remove_Load($load)
$form.ShowInTaskbar=$false;$form.StartPosition='Manual';$form.Location=New-Object Drawing.Point(-20000,-20000)
try {
    $modes=New-Object 'Collections.Generic.List[string]'
    foreach($mode in @('عدد إجابات كل سؤال','إجابات المستفيدين','عدد إجابات كل سؤال (كوبو)','إجابات المستفيدين (كوبو)')){[void](Control 'SearchBy_comboBox').Items.Add($mode);$modes.Add($mode)}
    Set-Field 'searchBy_list' $modes
    Set-Field 'koboModeVisible' $true
    Set-Field 'koboSources' $allData.PSObject.BaseObject
    Set-Field 'koboSourceUid' ''
    Set-Field 'Hided' $true
    $formChoices=New-Object Data.DataTable
    [void]$formChoices.Columns.Add('UID');[void]$formChoices.Columns.Add('Name');[void]$formChoices.Rows.Add('','جميع نماذج كوبو');[void]$formChoices.Rows.Add($uid,'Final Visit - Aleppo')
    (Control 'KoboForm_comboBox').DisplayMember='Name';(Control 'KoboForm_comboBox').ValueMember='UID';(Control 'KoboForm_comboBox').DataSource=$formChoices
    (Control 'KoboVersion_comboBox').DisplayMember='Name';(Control 'KoboVersion_comboBox').ValueMember='Key';(Control 'KoboVersion_comboBox').DataSource=$versions
    (Control 'DateFrom_dateTimePicker').Value=[datetime]'2026-01-01';(Control 'DateTo_dateTimePicker').Value=[datetime]'2026-12-31'
    (Control 'KoboFilters_panel').Visible=$false
    (Control 'Search_panel').Height=0
    $form.Show();[Windows.Forms.Application]::DoEvents()
    $form.Check_Theme()
    foreach($width in @(1000,1500)) {
        $form.ClientSize=New-Object Drawing.Size($width,780)
        foreach($mode in @(2,3)) {
            (Control 'SearchBy_comboBox').SelectedIndex=$mode
            $formType.GetMethod('LoadKoboResults',$flags).Invoke($form,@())|Out-Null
            [Windows.Forms.Application]::DoEvents()
            $grid=Control 'Answers_dataGridView'
            Check ($grid.Rows.Count -gt 0) 'Both modes must bind data in the existing grid'
            Check ($grid.FilterAndSortEnabled -and $grid.DataSource.RaiseListChangedEvents -and !(Control 'koboBinding')) 'Bulk binding must restore filtering, change events and selection updates'
            foreach($name in @('KoboForm_comboBox','KoboVersion_comboBox','KoboSummary_label')) {
                $control=Control $name
                Check ($control.Right -le $control.Parent.ClientSize.Width -and $control.Bottom -le $control.Parent.ClientSize.Height) 'Kobo filter controls must fit at each tested width'
            }
            Check ((Control 'KoboForm_comboBox').Right -le (Control 'KoboForm_label').Left) 'RTL form label must not overlap its selector'
            Check ((Control 'KoboVersion_comboBox').Right -le (Control 'KoboVersion_label').Left) 'RTL version label must not overlap its selector'
            if($mode -eq 3) {
                Check ($grid.Rows.Count -eq 3 -and $grid.Columns['Project_ID'].Frozen -and $grid.Columns['Beneficiary'].Frozen) 'All-form beneficiary mode must retain fixed project/person columns'
                $export=([Data.DataTable](Control 'Answers_dataGridView').DataSource.DataSource).Copy()
                $exportArgs=New-Object object[] 1
                $exportArgs[0]=$export.PSObject.BaseObject
                $formType.GetMethod('PrepareKoboExport',[Reflection.BindingFlags]'Static,NonPublic').Invoke($null,$exportArgs)|Out-Null
                Check ($export.Columns[0].ColumnName -eq 'رقم المشروع') 'Excel export must use readable Arabic headers, not internal keys'
                foreach($column in @('Kobo_Form_Name','Kobo_Form_Version','Kobo_Submission_ID')) {
                    Check (!$grid.Columns.Contains($column)) 'The beneficiary grid must not display technical source metadata'
                }
                Check ($export.Columns.Contains('إصدار النموذج') -eq $false -and $export.Columns.Contains('نموذج كوبو') -eq $false -and $export.Columns.Contains('رقم سجل كوبو') -eq $false) 'Excel export must also omit the removed technical columns'
                $uiFutureColumn=@(([Data.DataTable]$grid.DataSource.DataSource).Columns | Where-Object { [string]$_.ExtendedProperties['Path'] -like "*$($searchField.path)" })[0]
                Check ($grid.ColumnHeadersHeight -eq 126 -and $grid.Columns['q_0'].Width -eq 145) 'Beneficiary mode must use compact wrapped headers like the legacy grid'
                Check ($grid.Columns['SubCategory_Name'].DisplayIndex -eq 2 -and $grid.Columns['Stage'].DisplayIndex -eq 4) 'Beneficiary context columns must precede the questionnaire columns in legacy-style order'
                $formType.GetMethod('SearchKoboResults',$flags).Invoke($form,@($freeText,$uiFutureColumn.ColumnName,$true))|Out-Null
                Check ((Control 'Answers_dataGridView').Rows.Count -eq 1) 'Exact search must support apostrophes and brackets in Kobo answers'
                $formType.GetMethod('SearchKoboResults',$flags).Invoke($form,@("O'Neil [A]%*",$uiFutureColumn.ColumnName,$false))|Out-Null
                Check ((Control 'Answers_dataGridView').Rows.Count -eq 1) 'Partial search must treat wildcard symbols as literal user text'
                $formType.GetMethod('SearchKoboResults',$flags).Invoke($form,@('',$null,$false))|Out-Null
            }
            if($mode -eq 2) {
                Check ($grid.RowTemplate.Height -eq 27 -and $grid.Columns['Question'].Frozen) 'Summary must retain the original compact row height and fixed question column'
                $answerColumns=@($grid.Columns | Where-Object Name -match '^Answer\d+$')
                for($i=1;$i -le $answerColumns.Count;$i++) {
                    Check ($grid.Columns["Answer$i"].DisplayIndex -eq $i -and $grid.Columns["Count$i"].DisplayIndex -eq $answerColumns.Count+$i) 'Rendered summary must group every answer before every count, including options beyond three'
                }
                Check ($grid.Columns['Answered'].DisplayIndex -eq 2*$answerColumns.Count+1) 'The rendered total must follow all count columns'
                Check (!(Control 'KoboFilters_panel').Visible) 'The main report must not require a visible per-form selector'
                $sharedRows=@($grid.Rows | Where-Object { $_.Cells['Question'].Value -eq $question })
                Check ($sharedRows.Count -eq 1 -and $sharedRows[0].Cells['Count1'].Value -eq 2) 'The rendered default report must combine the shared question across different forms'
            }
            if($PreviewDirectory) {
                [void][IO.Directory]::CreateDirectory($PreviewDirectory)
                $bitmap=New-Object Drawing.Bitmap($form.Width,$form.Height)
                try { $form.DrawToBitmap($bitmap,(New-Object Drawing.Rectangle(0,0,$form.Width,$form.Height)));$bitmap.Save((Join-Path $PreviewDirectory "kobo-results-$mode-$width.png")) } finally { $bitmap.Dispose() }
            }
        }
    }
    $formType.GetMethod('ShowHide_button_Click',$flags).Invoke($form,@($null,[EventArgs]::Empty))|Out-Null
    Check ((Control 'KoboFilters_panel').Visible -and (Control 'KoboForm_comboBox').SelectedIndex -eq 0) 'Show must expose optional filters with all forms selected by default'
    Set-Field 'legacyFilterSelections' ([int[]]@(1,0,-1,-1,-1,-1))
    (Control 'SearchBy_comboBox').SelectedIndex=0
    $formType.GetMethod('SetKoboMode',$flags).Invoke($form,@($false))|Out-Null
    Check (!(Control 'KoboFilters_panel').Visible -and (Control 'VisitNumber_comboBox').SelectedIndex -eq 1 -and (Control 'VisitKind_comboBox').SelectedIndex -eq 0) 'Returning to a legacy mode must hide Kobo selectors and restore the old filter choices'
    Check ((Control 'Answers_dataGridView').RowTemplate.Height -eq 27) 'Returning to legacy results must restore the original row size'
} finally { $form.Close();$form.Dispose() }
Write-Output "Kobo visit results: $script:checks checks passed, including two-mode Designer rendering. No database connection used."

param([Parameter(Mandatory=$true)][string]$AssemblyPath,[string]$PreviewDirectory)
$ErrorActionPreference='Stop'
$path=(Resolve-Path -LiteralPath $AssemblyPath).Path
$dir=Split-Path -Parent $path
[void][Reflection.Assembly]::LoadFrom((Join-Path $dir 'MySql.Data.dll'))
[void][Reflection.Assembly]::LoadFrom((Join-Path $dir 'Newtonsoft.Json.dll'))
$app=[Reflection.Assembly]::LoadFrom($path)
$type=$app.GetType('MyWorkApplication.Classes.KoboPersonReview',$true)
$script:checks=0
function Check($condition,$message){if(!$condition){throw $message};$script:checks++}
$candidates=New-Object Data.DataTable
[void]$candidates.Columns.Add('Person_ID',[int])
foreach($column in @('PersonType','P_FirstName','P_FatherName','P_LastName')){[void]$candidates.Columns.Add($column)}
[void]$candidates.Rows.Add(101,'شريك','ديما','سمير','بيطار')
[void]$candidates.Rows.Add(102,'شريك','سارة','جورج','حنا')
$review=New-Object Data.DataTable
[void]$review.Columns.Add('Project_ID',[int]);[void]$review.Columns.Add('Person_ID',[int])
foreach($column in @('Source_Hash','Relationship_Hash','State')){[void]$review.Columns.Add($column)}
$hash=$type.GetMethod('RelationshipHash').Invoke($null,@($candidates.Rows[0]))
[void]$review.Rows.Add(20,101,'source-fingerprint',$hash,'confirmed')
function Current { $type.GetMethod('ChoiceIsCurrent').Invoke($null,@($review.Rows[0],20,'source-fingerprint',$candidates.PSObject.BaseObject)) }
Check (Current) 'An explicit confirmed choice is reusable only for the same source and relationship'
$review.Rows[0].State='auto_resolved'
Check (Current) 'A safely auto-matched beneficiary can be reused while its source and relationship remain unchanged'
foreach($state in @('pending','deferred','stale')){
    $review.Rows[0].State=$state; Check (!(Current)) 'Unconfirmed choices must never be imported'
}
$review.Rows[0].State='confirmed'
$review.Rows[0].Project_ID=21; Check (!(Current)) 'Project changes invalidate approval';$review.Rows[0].Project_ID=20
$review.Rows[0].Source_Hash='changed'; Check (!(Current)) 'Source changes invalidate approval';$review.Rows[0].Source_Hash='source-fingerprint'
$review.Rows[0].Person_ID=999; Check (!(Current)) 'Non-project people cannot be selected';$review.Rows[0].Person_ID=101
$review.Rows[0].Person_ID=[DBNull]::Value; Check (!(Current)) 'No selection is not approval';$review.Rows[0].Person_ID=101
foreach($column in @('PersonType','P_FirstName','P_FatherName','P_LastName')){
    $old=$candidates.Rows[0][$column];$candidates.Rows[0][$column]='changed'
    Check (!(Current)) 'Changed relationship or name requires fresh review'
    $candidates.Rows[0][$column]=$old
}
$candidates.Rows.RemoveAt(0);Check (!(Current)) 'Removed relationship cannot be reused'
[void]$candidates.Rows.Add(101,'شريك','ديما','سمير','بيطار')

Add-Type -AssemblyName System.Windows.Forms
Add-Type -AssemblyName System.Drawing
$formType=$app.GetType('MyWorkApplication.KoboPersonReview_Form',$true)
$form=[Activator]::CreateInstance($formType)
$flags=[Reflection.BindingFlags]'NonPublic,Instance'
function Control($name){$formType.GetField($name,$flags).GetValue($form)}
Check (!(Control 'confirm').Enabled) 'No person may be preselected'
Check ($null -eq $formType.GetField('selectedPerson',$flags).GetValue($form)) 'Selected ID starts empty'
$eventsProperty=[System.ComponentModel.Component].GetProperty('Events',$flags)
$events=$eventsProperty.GetValue($form,$null)
$loadKey=[Windows.Forms.Form].GetField('EVENT_LOAD',[Reflection.BindingFlags]'NonPublic,Static').GetValue($null)
$events.RemoveHandler($loadKey,$events[$loadKey])
$form.StartPosition='Manual';$form.Location=New-Object Drawing.Point(-20000,-20000);$form.ShowInTaskbar=$false
try{
    $form.Show();[Windows.Forms.Application]::DoEvents()
    foreach($width in @(950,1184)){
        $form.ClientSize=New-Object Drawing.Size($width,740)
        $formType.GetMethod('Check_Theme').Invoke($form,@())|Out-Null
        (Control 'candidates').DataSource=$candidates
        $labels=@('رقم الشخص','العلاقة بالمشروع','الاسم الأول','اسم الأب','الكنية')
        for($i=0;$i -lt $labels.Count;$i++){(Control 'candidates').Columns[$i].HeaderText=$labels[$i]}
        $queueData=New-Object Data.DataTable
        foreach($column in @('رقم السجل','المشروع','الاسم في كوبو','الحالة')){[void]$queueData.Columns.Add($column)}
        [void]$queueData.Rows.Add('501','20','ديمة سمير بيطار','بانتظار المطابقة')
        $formType.GetField('loading',$flags).SetValue($form,$true)
        (Control 'queue').DataSource=$queueData
        $formType.GetField('loading',$flags).SetValue($form,$false)
        (Control 'sourceLabel').Text='المشروع: 20 | الاسم في كوبو: ديمة سمير بيطار'
        (Control 'reason').Text='تمت مراجعة بيانات المشروع والاسم'
        (Control 'status').Text='الشخص المحدد: ديما سمير بيطار (101)'
        (Control 'candidates').ClearSelection();(Control 'candidates').CurrentCell=$null
        [Windows.Forms.Application]::DoEvents()
        $actions=Control 'actions'
        foreach($name in @('reason','confirm','defer')){
            $control=Control $name
            Check ($control.Right -le $actions.ClientSize.Width -and $control.Bottom -le $actions.ClientSize.Height) 'Footer controls must fit'
        }
        Check ((Control 'confirm').Left -ge (Control 'defer').Right) 'RTL action buttons must not overlap'
        Check ((Control 'reason').Left -ge (Control 'confirm').Right) 'Reason must not overlap action buttons'
        Check ((Control 'candidates').DefaultCellStyle.SelectionBackColor -eq [Drawing.Color]::FromArgb(189,189,189)) 'Selection must use app gray, not blue'
        $formType.GetField('submission',$flags).SetValue($form,501)
        $click=New-Object Windows.Forms.DataGridViewCellEventArgs(0,0)
        $formType.GetMethod('Candidate_Click',$flags).Invoke($form,@((Control 'candidates'),$click.PSObject.BaseObject))|Out-Null
        Check ((Control 'confirm').Enabled) 'Only explicit candidate action enables confirmation'
        Check ($formType.GetField('selectedPerson',$flags).GetValue($form) -eq 102) 'Explicit choice must use clicked row, not a default person'
        $formType.GetField('selectedPerson',$flags).SetValue($form,$null)
        (Control 'confirm').Enabled=$false
        if($PreviewDirectory){
            [void][IO.Directory]::CreateDirectory($PreviewDirectory)
            $bitmap=New-Object Drawing.Bitmap($form.Width,$form.Height)
            try{$form.DrawToBitmap($bitmap,(New-Object Drawing.Rectangle(0,0,$form.Width,$form.Height)));$bitmap.Save((Join-Path $PreviewDirectory "person-review-$width.png"))}finally{$bitmap.Dispose()}
        }
    }
}finally{$form.Close();$form.Dispose()}
Write-Output "Kobo person review: $script:checks checks passed (offline; no database writes)."

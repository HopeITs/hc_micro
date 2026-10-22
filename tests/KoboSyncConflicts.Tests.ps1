param([Parameter(Mandatory=$true)][string]$AssemblyPath,[string]$PreviewDirectory)
$ErrorActionPreference='Stop'
$path=(Resolve-Path -LiteralPath $AssemblyPath).Path
$dir=Split-Path -Parent $path
[void][Reflection.Assembly]::LoadFrom((Join-Path $dir 'MySql.Data.dll'))
[void][Reflection.Assembly]::LoadFrom((Join-Path $dir 'Newtonsoft.Json.dll'))
$app=[Reflection.Assembly]::LoadFrom($path)
$policy=$app.GetType('MyWorkApplication.Classes.KoboConflictPolicy',$true)
$script:checks=0
function Check($condition,$message){if(!$condition){throw $message};$script:checks++}
function Compare-Responses($left,$right){return ,$policy.GetMethod('Compare').Invoke($null,@($left,$right))}
$a='{"project":"1270","income":"130","extras":"yes","empty":"","nullable":null}'
$b='{"project":"1270","income":"140","extras":"no","empty":"","new":"value"}'
$table=Compare-Responses $a $b
Check ($table.Rows.Count -eq 6) 'All union fields must be represented'
Check ($table.Select('Changed=true').Count -eq 4) 'Changed, missing, and null values must be distinguished'
Check ($table.Select("Field='/income'")[0].Existing -eq '"130"') 'Current financial answer must remain intact'
Check ($table.Select("Field='/income'")[0].Incoming -eq '"140"') 'Incoming financial answer must remain intact'
$same=Compare-Responses $a $a
Check ($same.Select('Changed=true').Count -eq 0) 'Identical responses must have no differences'
$nested=Compare-Responses '{"a/b":"one","a":{"b":"two"}}' '{"a/b":"one","a":{"b":"three"}}'
Check ($nested.Rows.Count -eq 2 -and $nested.Select('Changed=true').Count -eq 1) 'Nested objects must not collide with Kobo slash field names'
$hash=$policy.GetMethod('Hash')
Check ($hash.Invoke($null,@($a)) -eq $hash.Invoke($null,@($a))) 'Hashes must be deterministic'
Check ($hash.Invoke($null,@($a)) -ne $hash.Invoke($null,@($b))) 'Changed answers must invalidate a reviewed snapshot'
foreach($action in @('replace','keep_both','ignore','defer')){
    [void]$policy.GetMethod('ValidateDecision').Invoke($null,@($action,'reviewed by test'))
    Check $true 'Supported decision must validate'
}
foreach($action in @('replace','keep_both','ignore','unknown')){
    $rejected=$false
    try{[void]$policy.GetMethod('ValidateDecision').Invoke($null,@($action,''))}catch{$rejected=$true}
    Check $rejected 'Unknown or unreasoned decisions must be rejected'
}
$labels=$app.GetType('MyWorkApplication.Classes.KoboReviewLabels',$true)
$pointer='/group_tl5ny44~1income';$uid='aDGPm94uwS7HYk4LcG46oh'
Check ($labels.GetMethod('Label').Invoke($null,@($uid,$pointer)) -ne $pointer) 'Supplied Arabic question labels must be bundled in the executable'
Check ($labels.GetMethod('Value').Invoke($null,@($uid,'/group_tl5ny44~1Xtra_fees','"Xtra_fees_yes"')).Contains('Xtra_fees_yes')) 'Choice translation must preserve the original code'
Check ($labels.GetMethod('Label').Invoke($null,@('missing-form','/unknown')) -eq '/unknown') 'Unknown fields must not receive guessed labels'
Add-Type -AssemblyName System.Windows.Forms
Add-Type -AssemblyName System.Drawing
$messages=$app.GetType('MyWorkApplication.Classes.KoboSyncUserMessages',$true)
$explanation=$messages.GetMethod('Conflict').Invoke($null,@('3395','598587424','598593564','open'))
Check ($explanation -match '[\u0600-\u06ff]' -and !$explanation.Contains('Possible duplicate')) 'Conflict explanation must be Arabic, not the database diagnostic'
Check ($explanation.Contains('3395') -and $explanation.Contains('598593564')) 'Arabic explanation must identify the reviewed visit and incoming source'
$summary=$messages.GetMethod('Summary').Invoke($null,@(2,2))
Check ($summary -match '[\u0600-\u06ff]') 'Sync warning must be in Arabic'
$mixed=$messages.GetMethod('Summary').Invoke($null,@(3,2))
Check ($mixed.Length -gt $summary.Length) 'Mixed errors must include separate guidance for non-conflict failures'
$errorsOnly=$messages.GetMethod('Summary').Invoke($null,@(3,0))
Check ($errorsOnly -ne $summary) 'Unregistered errors must not be described as reviewable duplicate conflicts'
$pair=New-Object Data.DataTable
[void]$pair.Columns.Add('ID',[int]);[void]$pair.Columns.Add('Raw_JSON');[void]$pair.Columns.Add('Kobo_Is_Deleted',[int])
[void]$pair.Rows.Add(1,$a,0);[void]$pair.Rows.Add(2,$b,0)
$conflict=New-Object Data.DataTable
[void]$conflict.Columns.Add('Incoming_ID',[int]);[void]$conflict.Columns.Add('Existing_ID',[int])
[void]$conflict.Columns.Add('Incoming_Hash');[void]$conflict.Columns.Add('Existing_Hash')
[void]$conflict.Rows.Add(2,1,$hash.Invoke($null,@($b)),$hash.Invoke($null,@($a)))
$current=$app.GetType('MyWorkApplication.Classes.KoboConflictReview').GetMethod('Current',[Reflection.BindingFlags]'Static,NonPublic')
Check ($current.Invoke($null,@($conflict.Rows[0],$pair.PSObject.BaseObject))) 'Exact reviewed snapshots must remain valid'
$pair.Rows[1]['Raw_JSON']=$a
Check (!$current.Invoke($null,@($conflict.Rows[0],$pair.PSObject.BaseObject))) 'A changed incoming source must invalidate the decision'
$pair.Rows[1]['Raw_JSON']=$b;$pair.Rows[0]['Kobo_Is_Deleted']=1
Check (!$current.Invoke($null,@($conflict.Rows[0],$pair.PSObject.BaseObject))) 'Deleted existing source must invalidate the decision'
$pair.Rows[0]['Kobo_Is_Deleted']=0;$pair.Rows[1]['Kobo_Is_Deleted']=1
Check (!$current.Invoke($null,@($conflict.Rows[0],$pair.PSObject.BaseObject))) 'Deleted incoming source must invalidate the decision'
$form=[Activator]::CreateInstance($app.GetType('MyWorkApplication.KoboSyncConflicts_Form',$true))
$flags=[Reflection.BindingFlags]'Instance,NonPublic'
try {
    foreach($name in @('conflicts','comparison','history','decision','reason','apply','filter','refresh')) {
        $control=$form.GetType().GetField($name,$flags).GetValue($form)
        Check ($control -is [Windows.Forms.Control] -and $control.Parent) "Designer control $name must exist"
    }
    $form.GetType().GetField('loading',$flags).SetValue($form,$true)
    $form.Check_Theme()
    $loadHandler=[Delegate]::CreateDelegate([EventHandler],$form,$form.GetType().GetMethod('Form_Load',$flags))
    $form.remove_Load($loadHandler)
    $form.ShowInTaskbar=$false
    $form.StartPosition=[Windows.Forms.FormStartPosition]::Manual
    $form.Location=New-Object Drawing.Point(-20000,-20000)
    $comparison=$form.GetType().GetField('comparison',$flags).GetValue($form)
    $table.DefaultView.RowFilter='Changed=true'
    $comparison.DataSource=$table.DefaultView
    $form.GetType().GetField('identity',$flags).GetValue($form).Text='Project 1270 | Current visit 3395 | M4 | 2025-11-05'
    $form.GetType().GetField('status',$flags).GetValue($form).Text=$explanation
    $queue=New-Object Data.DataTable
    foreach($column in @('Project','Beneficiary','Stage','Visit_Date','Incoming','Existing','State')){[void]$queue.Columns.Add($column)}
    [void]$queue.Rows.Add('1270','Test participant','4','2025-11-05','598593564','598587424','open')
    $form.GetType().GetField('conflicts',$flags).GetValue($form).DataSource=$queue
    foreach($size in @(@(1184,785),@(950,700))){
        $form.ClientSize=New-Object Drawing.Size($size[0],$size[1])
        $form.Show();$form.PerformLayout();[Windows.Forms.Application]::DoEvents()
        $layout=$form.GetType().GetField('layout',$flags).GetValue($form)
        Check ($layout.Bounds.Width -le $form.ClientSize.Width) 'Root layout must fit viewport'
        $buttons=$form.GetType().GetField('actions',$flags).GetValue($form)
        Check ($buttons.Bottom -le $layout.ClientSize.Height) 'Decision controls must remain inside layout'
        $decisionControl=$form.GetType().GetField('decision',$flags).GetValue($form)
        $reasonControl=$form.GetType().GetField('reason',$flags).GetValue($form)
        $applyControl=$form.GetType().GetField('apply',$flags).GetValue($form)
        Check ($decisionControl.Top -eq $reasonControl.Top -and $applyControl.Top -eq $reasonControl.Top) 'Footer controls must share their top edge'
        foreach($name in @('conflicts','comparison','history')){
            $grid=$form.GetType().GetField($name,$flags).GetValue($form)
            Check ($grid.ColumnHeadersDefaultCellStyle.SelectionBackColor.ToArgb() -eq $grid.ColumnHeadersDefaultCellStyle.BackColor.ToArgb()) 'Header selection must use the app palette, not system blue'
        }
        if($PreviewDirectory){
            [void][IO.Directory]::CreateDirectory($PreviewDirectory)
            $bitmap=New-Object Drawing.Bitmap($form.Width,$form.Height)
            try{$form.DrawToBitmap($bitmap,(New-Object Drawing.Rectangle(0,0,$bitmap.Width,$bitmap.Height)))
                $bitmap.Save((Join-Path $PreviewDirectory ("conflicts-{0}.png" -f $size[0])),[Drawing.Imaging.ImageFormat]::Png)
            }finally{$bitmap.Dispose()}
        }
    }
}finally{$form.Dispose()}
Write-Output "Passed $script:checks conflict policy and Designer rendering checks. No database changes."

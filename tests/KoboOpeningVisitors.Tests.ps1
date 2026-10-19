param([Parameter(Mandatory = $true)][string]$AssemblyPath)

$ErrorActionPreference = 'Stop'
$assemblyFile = (Resolve-Path -LiteralPath $AssemblyPath).Path
$assemblyDirectory = Split-Path -Parent $assemblyFile
[void][Reflection.Assembly]::LoadFrom((Join-Path $assemblyDirectory 'MySql.Data.dll'))
$jsonAssembly = [Reflection.Assembly]::LoadFrom((Join-Path $assemblyDirectory 'Newtonsoft.Json.dll'))
$appAssembly = [Reflection.Assembly]::LoadFrom($assemblyFile)
$mapperType = $appAssembly.GetType('MyWorkApplication.Classes.KoboVisitorMapping', $true)
$displayType = $appAssembly.GetType('MyWorkApplication.Classes.KoboVisitVisitorDisplay', $true)
$parseMethod = $jsonAssembly.GetType('Newtonsoft.Json.Linq.JObject').GetMethod('Parse', [type[]]@([string]))
$fixture = Get-Content -LiteralPath (Join-Path $PSScriptRoot 'fixtures/kobo-opening-visitors.json') -Raw -Encoding UTF8 | ConvertFrom-Json
$flags = [Reflection.BindingFlags]'Instance, NonPublic'
$script:checks = 0

function Assert-True([bool]$condition, [string]$message) {
    if (!$condition) { throw $message }
    $script:checks++
}

function New-Context([string]$database) {
    $mapper = [Activator]::CreateInstance($mapperType)
    $fields = New-Object System.Data.DataTable
    foreach ($column in @('Kobo_Form_UID','Project_Number_Field','Visitor_Choice_Field','Visitor_Text_Field','Vehicle_Number_Field')) {
        [void]$fields.Columns.Add($column, [string])
    }
    foreach ($form in $fixture.forms | Where-Object { $_.Database -eq $database }) {
        [void]$fields.Rows.Add($form.UID, $form.Fields.Project, $form.Fields.Choice, $form.Fields.Text, [string]$form.Fields.Vehicle)
    }
    $users = New-Object System.Data.DataTable
    [void]$users.Columns.Add('UserID', [int])
    [void]$users.Columns.Add('UserName', [string])
    $ids = @{}
    $nextId = 100
    foreach ($alias in $fixture.aliases | Where-Object { $_.database -eq $database -and $_.localUserName }) {
        if (!$ids.ContainsKey($alias.localUserName)) {
            $ids[$alias.localUserName] = $nextId
            [void]$users.Rows.Add($nextId, $alias.localUserName)
            $nextId++
        }
    }
    [void]$users.Rows.Add(999, 'Primary Visitor')
    if ($database -eq 'hcsyria_micro_d') { [void]$users.Rows.Add(89, 'Rawia') }
    if ($database -eq 'hcsyria_micro') { [void]$users.Rows.Add(65, 'Fadi') }
    $mappings = New-Object System.Data.DataTable
    foreach ($column in @('Kobo_Form_UID','Kobo_Name','Display_Name')) { [void]$mappings.Columns.Add($column, [string]) }
    [void]$mappings.Columns.Add('Is_Ignored', [int])
    [void]$mappings.Columns.Add('User_ID', [int])
    foreach ($alias in $fixture.aliases | Where-Object { $_.database -eq $database }) {
        $id = [DBNull]::Value
        if ($alias.localUserName) { $id = $ids[$alias.localUserName] }
        [void]$mappings.Rows.Add($alias.formUid, $alias.koboName, $alias.displayName, $alias.ignored, $id)
    }
    $mapperType.GetField('fields', $flags).SetValue($mapper, $fields.PSObject.BaseObject)
    $mapperType.GetField('mappings', $flags).SetValue($mapper, $mappings.PSObject.BaseObject)
    $mapperType.GetField('users', $flags).SetValue($mapper, $users.PSObject.BaseObject)
    return @{ Mapper=$mapper; Ids=$ids; Mappings=$mappings }
}

function Get-Visitors($context, $form, $data) {
    $json = ConvertTo-Json -InputObject $data -Depth 8 -Compress
    $submission = $parseMethod.Invoke($null, @($json.PSObject.BaseObject))
    return ,$mapperType.GetMethod('GetVisitors').Invoke($context.Mapper, @($submission, [string]$form.UID))
}

foreach ($form in $fixture.forms) {
    $context = New-Context $form.Database
    $data = @{ username='Primary Visitor'; _submitted_by='Uploader'; '__005'='address'; '__008'='34.8036238'; '__006'='debt'; '__007'='reason' }
    $data[$form.Fields.Text] = 'Former Employee'
    foreach ($choice in $form.Choices) {
        $data[$form.Fields.Choice] = $choice.name
        $visitors = Get-Visitors $context $form $data
        $alias = $fixture.aliases | Where-Object { $_.database -eq $form.Database -and $_.formUid -eq $form.UID -and $_.koboName -eq $choice.name }
        $expectedCount = if ($alias.ignored) { 2 } else { 3 }
        Assert-True ($visitors.Count -eq $expectedCount) "$($form.UID) / $($choice.name): wrong visitors or unrelated fields included"
        Assert-True ($visitors[0].UserId -eq 999) 'Primary visitor must use username before uploader'
        Assert-True ($visitors[$visitors.Count-1].DisplayName -eq 'Former Employee') 'Unlinked name was lost'
        Assert-True ($null -eq $visitors[$visitors.Count-1].UserId) 'Former employee must not get a fake account'
        if (!$alias.ignored) {
            Assert-True ($visitors[1].DisplayName -eq $alias.displayName) 'Choice code should display its source label'
            $expectedId = if ($alias.localUserName) { $context.Ids[$alias.localUserName] } else { $null }
            Assert-True ($visitors[1].UserId -eq $expectedId) 'Choice mapped to wrong local account'
        }
    }
    $data = @{}
    $data[$form.Fields.Project] = '123'
    if ($form.Fields.Vehicle) { $data[$form.Fields.Vehicle] = '456' }
    $json = ConvertTo-Json -InputObject $data -Compress
    $submission = $parseMethod.Invoke($null, @($json.PSObject.BaseObject))
    $valueMethod = $mapperType.GetMethod('GetValue')
    Assert-True ($valueMethod.Invoke($null, @($submission, [string]$form.Fields.Project)) -eq '123') 'Project field path did not resolve'
    if ($form.Fields.Vehicle) {
        Assert-True ($valueMethod.Invoke($null, @($submission, [string]$form.Fields.Vehicle)) -eq '456') 'Vehicle field path did not resolve'
    }
}

foreach ($database in @('hcsyria_micro','hcsyria_micro_h','hcsyria_micro_w','hcsyria_micro_d','hcsyria_micro_l')) {
    $context = New-Context $database
    $form = $fixture.forms | Where-Object { $_.Database -eq $database } | Select-Object -First 1
    foreach ($alias in $fixture.aliases | Where-Object { $_.database -eq $database -and $_.formUid -eq '' -and !$_.ignored }) {
        $visitors = Get-Visitors $context $form @{ username=$alias.koboName }
        Assert-True ($visitors.Count -eq 1) 'Account alias did not produce a visitor'
        Assert-True ($visitors[0].DisplayName -eq $alias.displayName) 'Alias lost its display name'
        $expectedId = if ($alias.localUserName) { $context.Ids[$alias.localUserName] } else { $null }
        Assert-True ($visitors[0].UserId -eq $expectedId) 'Account alias mapped to the wrong local user'
    }
    $visitors = Get-Visitors $context $form @{ _submitted_by='Primary Visitor' }
    Assert-True ($visitors[0].UserId -eq 999) 'Missing username should fall back to submitted_by'
    $visitors = Get-Visitors $context $form @{ username='Brand_New_User' }
    Assert-True ($null -eq $visitors[0].UserId -and $visitors[0].DisplayName -eq 'Brand_New_User') 'Unknown user must remain visible and unlinked'
    $data = @{ username='Primary Visitor' }
    $data[$form.Fields.Text] = 'Primary Visitor'
    $visitors = Get-Visitors $context $form $data
    Assert-True ($visitors.Count -eq 1) 'Duplicate visitor links should be removed'
    $data[$form.Fields.Text] = 'First Person, Second Person'
    $visitors = Get-Visitors $context $form $data
    Assert-True ($visitors.Count -eq 3) 'Separate names should not become a fourth combined name'
}

$context = New-Context 'hcsyria_micro_d'
$form = $fixture.forms | Where-Object { $_.Database -eq 'hcsyria_micro_d' } | Select-Object -First 1
$visitors = Get-Visitors $context $form @{ username='rawan_maksoud' }
Assert-True ($visitors[0].UserId -eq $context.Ids['Rawan'] -and $visitors[0].UserId -ne 89) 'Rawan must link to her historical account, never Rawia'
$visitors = Get-Visitors $context $form @{ username='sandra_jreige' }
Assert-True ($visitors[0].UserId -eq $context.Ids['Sandra']) 'Out of Service Sandra must retain historical credit'
foreach ($form in $fixture.forms | Where-Object { $_.Database -eq 'hcsyria_micro_d' }) {
    $data = @{ username='sandra_jreige' }
    $data[$form.Fields.Text] = 'داني حنا وراما ميخائيل'
    $visitors = Get-Visitors $context $form $data
    Assert-True ($visitors.Count -eq 3) 'Primary and both accompanying visitors must be retained'
    Assert-True (@($visitors | Where-Object { $null -ne $_.UserId }).Count -eq 3) 'All three participants must be linked for statistics'
}
# Simulate a deleted local user: LEFT JOIN in Load returns NULL while keeping the name.
$mapping = $context.Mappings.Rows | Where-Object { $_.Kobo_Name -eq 'omran_alnasser' }
$mapping['User_ID'] = [DBNull]::Value
$visitors = Get-Visitors $context $form @{ username='omran_alnasser' }
Assert-True ($null -eq $visitors[0].UserId -and $visitors[0].DisplayName) 'Deleted account must retain display name'

foreach ($case in @(
    @('hcsyria_micro', 'ماريو ارسلان جورج يشوع', 2, 2),
    @('hcsyria_micro', 'جورج تيلو وطوني عربجي', 2, 2),
    @('hcsyria_micro', 'ايفانتيا سلوم و جورج تيللو', 2, 1),
    @('hcsyria_micro_d', 'داني حنا وراما ميخائيل', 2, 2),
    @('hcsyria_micro_d', 'داني و راما', 2, 2),
    @('hcsyria_micro_d', 'راما . داني', 2, 2),
    @('hcsyria_micro_d', 'دانيال', 1, 0),
    @('hcsyria_micro_h', 'ميشيل سكروج - فارس مرضعة', 2, 0),
    @('hcsyria_micro_w', 'ميرفت دهام وغيث اسعد', 2, 1),
    @('hcsyria_micro_l', 'بشار صائغ وادونيس مواس.', 2, 2),
    @('hcsyria_micro_l', 'عبدالله جبور وبشار صايغ', 2, 2),
    @('hcsyria_micro_l', 'بشار صائغ واديب خال', 2, 1)
)) {
    $context = New-Context $case[0]
    $form = $fixture.forms | Where-Object { $_.Database -eq $case[0] } | Select-Object -First 1
    $data = @{}; $data[$form.Fields.Text] = $case[1]
    $visitors = Get-Visitors $context $form $data
    Assert-True ($visitors.Count -eq $case[2]) "Combined companions count: $($case[1])"
    Assert-True (@($visitors | Where-Object { $null -ne $_.UserId }).Count -eq $case[3]) "Combined companions links: $($case[1])"
}

$names = New-Object 'System.Collections.Generic.List[string]'
foreach ($name in @('One','Two','Three','Four','Five')) { $names.Add($name) }
$slots = $displayType.GetMethod('GetDisplaySlots').Invoke($null, @($names.PSObject.BaseObject, 4))
Assert-True ($slots.Count -eq 4 -and $slots[3].Contains('Four') -and $slots[3].Contains('Five')) 'Display must preserve names beyond four slots'

Add-Type -AssemblyName System.Windows.Forms
$controls = New-Object 'System.Windows.Forms.ComboBox[]' 4
$lookup = New-Object System.Data.DataTable
[void]$lookup.Columns.Add('U_ID', [int])
[void]$lookup.Columns.Add('Visitors', [string])
[void]$lookup.Rows.Add(999, 'Local Visitor')
try {
    for ($i=0; $i -lt 4; $i++) {
        $controls[$i] = New-Object System.Windows.Forms.ComboBox
        $controls[$i].DisplayMember = 'Visitors'
        $controls[$i].ValueMember = 'U_ID'
        $controls[$i].DataSource = $lookup
    }
    $bindMethod = $displayType.GetMethod('BindNames', [Reflection.BindingFlags]'Static, NonPublic')
    [void]$bindMethod.Invoke($null, @($names.PSObject.BaseObject, $controls.PSObject.BaseObject))
    for ($i=0; $i -lt 4; $i++) {
        Assert-True (!$controls[$i].Enabled) 'Kobo visitor controls must be read-only'
        Assert-True ($controls[$i].Text -eq $slots[$i]) 'Original combo bindings must not hide Kobo names'
        Assert-True ($null -eq $controls[$i].DataSource) 'Display-only names must not create fake user IDs'
    }
} finally {
    foreach ($control in $controls) { if ($control) { $control.Dispose() } }
}
Write-Output "Passed $script:checks checks across all 16 Opening forms and five databases. No database connection was used."

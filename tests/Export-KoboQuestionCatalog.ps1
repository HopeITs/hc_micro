param([string]$Downloads='C:/Users/asus/Downloads', [string]$OutputPath=(Join-Path $PSScriptRoot '../MyWorkApplication/KoboQuestionCatalog.json'))
$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.IO.Compression.FileSystem
function Read-Sheet($zip, $path, $strings) {
    $entry = $zip.GetEntry($path)
    if (!$entry) { return }
    $reader = New-Object IO.StreamReader($entry.Open())
    try { [xml]$xml = $reader.ReadToEnd() } finally { $reader.Dispose() }
    foreach ($row in $xml.worksheet.sheetData.row) {
        $cells = @{}
        foreach ($cell in $row.c) {
            $cells[($cell.r -replace '\d','')] = if ($cell.t -eq 's') { $strings[[int]$cell.v] } elseif ($cell.t -eq 'inlineStr') { $cell.is.InnerText } else { [string]$cell.v }
        }
        $cells
    }
}
function Label-Columns($header) {
    $keys = @($header.Keys | Sort-Object)
    $arabic = @($keys | Where-Object { $header[$_] -match '^label::.*(Arabic|arab|العربية)' })
    $plain = @($keys | Where-Object { $header[$_] -eq 'label' })
    return @($arabic) + @($plain) + @($keys | Where-Object { $header[$_] -match '^label' -and $_ -notin $arabic -and $_ -notin $plain })
}
function Read-Label($row,$columns) {
    $values=@($columns | ForEach-Object { [string]$row[$_] } | Where-Object { ![string]::IsNullOrWhiteSpace($_) })
    $arabic=@($values | Where-Object { $_ -match '[\u0600-\u06ff]' })
    if($arabic.Count){ return $arabic[0].Trim() }
    if($values.Count){ return $values[0].Trim() }
    return ''
}
function Column($header, $name) { return @($header.Keys | Where-Object { $header[$_] -eq $name })[0] }
$result = [ordered]@{}
foreach ($folder in @('aleppo','damascus','homs','latakia','wadi')) {
    foreach ($file in Get-ChildItem (Join-Path $Downloads $folder) -Filter *.xlsx | Sort-Object Name) {
        $uid = $file.BaseName -replace ' \(\d+\)$',''
        if ($result.Contains($uid)) { throw "Duplicate form definition: $uid" }
        $zip = [IO.Compression.ZipFile]::OpenRead($file.FullName)
        try {
            $strings = @()
            $entry = $zip.GetEntry('xl/sharedStrings.xml')
            if ($entry) {
                $reader = New-Object IO.StreamReader($entry.Open())
                try { [xml]$xml = $reader.ReadToEnd() } finally { $reader.Dispose() }
                $strings = @($xml.sst.si | ForEach-Object { $_.InnerText })
            }
            $survey = @(Read-Sheet $zip 'xl/worksheets/sheet1.xml' $strings)
            $choices = @(Read-Sheet $zip 'xl/worksheets/sheet2.xml' $strings)
            $typeColumn = Column $survey[0] 'type'
            $nameColumn = Column $survey[0] 'name'
            $labelColumns = @(Label-Columns $survey[0])
            $choiceList = Column $choices[0] 'list_name'
            $choiceName = Column $choices[0] 'name'
            $choiceLabels = @(Label-Columns $choices[0])
            if (!$typeColumn -or !$nameColumn -or !$labelColumns.Count) { throw "Invalid survey headers: $uid" }
            $stack = New-Object 'Collections.Generic.List[string]'
            $repeatStack = New-Object 'Collections.Generic.List[bool]'
            $sectionStack = New-Object 'Collections.Generic.List[string]'
            $previousQuestion = @{}
            $fields = New-Object 'Collections.Generic.List[object]'
            foreach ($row in $survey | Select-Object -Skip 1) {
                $type = [string]$row[$typeColumn]; $name = [string]$row[$nameColumn]
                if ($type -match '^begin[_ ](group|repeat)$') { $isRepeat=$Matches[1] -eq 'repeat'; $stack.Add($name); $repeatStack.Add($isRepeat); $sectionStack.Add((Read-Label $row $labelColumns)); continue }
                if ($type -match '^end[_ ](group|repeat)$') { if ($stack.Count) { $stack.RemoveAt($stack.Count-1); $repeatStack.RemoveAt($repeatStack.Count-1); $sectionStack.RemoveAt($sectionStack.Count-1) }; continue }
                $label = Read-Label $row $labelColumns
                if (!$name -or !$label -or $type -match '^(note|calculate|hidden|start|end|today|username|deviceid|audit)$') { continue }
                $options = New-Object 'Collections.Generic.List[object]'
                if ($type -match '^select_(one|multiple)\s+(.+)$') {
                    $list = $Matches[2]
                    foreach ($choice in $choices | Select-Object -Skip 1 | Where-Object { $_[$choiceList] -eq $list }) {
                        if ($choice[$choiceName]) { $options.Add([ordered]@{code=[string]$choice[$choiceName]; label=(Read-Label $choice $choiceLabels)}) }
                    }
                }
                $section=(@($sectionStack | Where-Object { $_ }) -join ' / ')
                $groupPath=@($stack)-join '/'
                $context=if($previousQuestion.ContainsKey($groupPath)){$previousQuestion[$groupPath]}else{$section}
                $fields.Add([ordered]@{path=(@($stack)+$name)-join '/'; label=$label; type=$type; repeat=$repeatStack.Contains($true); section=$section; context=$context; choices=@($options.ToArray())})
                $previousQuestion[$groupPath]=$label
            }
            if(!$fields.Count){throw "No labelled questions extracted from $uid"}
            $result[$uid] = [ordered]@{region=$folder; fields=@($fields.ToArray())}
        } finally { $zip.Dispose() }
    }
}
$json = ConvertTo-Json -InputObject $result -Depth 9 -Compress
[IO.File]::WriteAllText([IO.Path]::GetFullPath($OutputPath), $json, (New-Object Text.UTF8Encoding($false)))
"Exported full questionnaire definitions for $($result.Count) forms."

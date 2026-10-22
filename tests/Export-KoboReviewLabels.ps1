param([string]$Downloads='C:/Users/asus/Downloads',[string]$OutputPath=(Join-Path $PSScriptRoot '../MyWorkApplication/KoboMonitoringFieldLabels.json'))
$ErrorActionPreference='Stop'
Add-Type -AssemblyName System.IO.Compression.FileSystem
function Read-Rows($zip,$path,$strings){
    $entry=$zip.GetEntry($path);if(!$entry){return}
    $reader=New-Object IO.StreamReader($entry.Open())
    try{[xml]$xml=$reader.ReadToEnd()}finally{$reader.Dispose()}
    foreach($row in $xml.worksheet.sheetData.row){
        $result=@{}
        foreach($cell in $row.c){$result[($cell.r -replace '\d','')]=if($cell.t -eq 's'){$strings[[int]$cell.v]}elseif($cell.t -eq 'inlineStr'){$cell.is.InnerText}else{[string]$cell.v}}
        [pscustomobject]$result
    }
}
$forms=Get-Content (Join-Path $PSScriptRoot 'fixtures/kobo-monitoring-stages.json') -Raw|ConvertFrom-Json
$result=@{}
foreach($folder in @('aleppo','damascus','homs','latakia','wadi')){
    foreach($file in Get-ChildItem (Join-Path $Downloads $folder) -Filter *.xlsx){
        $uid=$file.BaseName -replace ' \(\d+\)$',''
        if(!($forms|Where-Object{$_.uid -eq $uid})){continue}
        $zip=[IO.Compression.ZipFile]::OpenRead($file.FullName)
        try{
            $reader=New-Object IO.StreamReader($zip.GetEntry('xl/sharedStrings.xml').Open())
            try{[xml]$xml=$reader.ReadToEnd()}finally{$reader.Dispose()}
            $strings=@($xml.sst.si|ForEach-Object{$_.InnerText})
            $survey=@(Read-Rows $zip 'xl/worksheets/sheet1.xml' $strings)
            $choices=@(Read-Rows $zip 'xl/worksheets/sheet2.xml' $strings)
            $stack=New-Object 'Collections.Generic.List[string]'
            $fields=@{}
            foreach($row in $survey){
                if($row.A -eq 'begin_group' -or $row.A -eq 'begin_repeat'){$stack.Add($row.B);continue}
                if($row.A -eq 'end_group' -or $row.A -eq 'end_repeat'){if($stack.Count){$stack.RemoveAt($stack.Count-1)};continue}
                if(!$row.B -or !$row.C -or $row.A -match '^(type|note|begin|end)'){continue}
                $path=(@($stack)+$row.B)-join '/'
                $labels=@{}
                if($row.A -match '^select_(one|multiple)\s+(.+)$'){
                    $list=$Matches[2]
                    foreach($choice in $choices|Where-Object{$_.A -eq $list}){if($choice.B -and $choice.C){$labels[$choice.B]=$choice.C.Trim()}}
                }
                $fields[$path]=@{label=$row.C.Trim();choices=$labels}
            }
            $result[$uid]=$fields
        }finally{$zip.Dispose()}
    }
}
$json=ConvertTo-Json -InputObject $result -Depth 7 -Compress
[IO.File]::WriteAllText([IO.Path]::GetFullPath($OutputPath),$json,(New-Object Text.UTF8Encoding($false)))
"Generated labels for $($result.Count) supplied Kobo forms."

param([Parameter(Mandatory=$true)][string]$AssemblyPath)
$ErrorActionPreference = 'Stop'
$assembly = [Reflection.Assembly]::LoadFrom((Resolve-Path -LiteralPath $AssemblyPath).Path)
$type = $assembly.GetType('MyWorkApplication.Classes.KoboSubmissionViewer', $true)
$script:checks = 0
function Check($condition, $message) { if (!$condition) { throw $message }; $script:checks++ }

$raw = '{"username":"Judy","group":{"field":"answer"},"repeat":[{"item":"first"},{"item":"second"}],"blank":"","_id":123,"meta":{"instanceID":"ignored"}}'
$table = $type.GetMethod('ParseAnswers').Invoke($null, @('unknown-form', $raw))
Check ($table.Rows.Count -eq 5) 'User answers, nested fields, repeats and blank answers should be listed; Kobo metadata should be hidden'
Check ($table.Select("Question='group/field'").Count -eq 1) 'Nested group field paths should remain inspectable'
Check ($table.Select("Question='repeat [1]/item'").Count -eq 1) 'First repeat index should be visible'
Check ($table.Select("Question='repeat [2]/item'").Count -eq 1) 'Second repeat index should be visible'
Check ($table.Select("Question='blank'").Count -eq 1) 'Blank answers should remain represented'
Check ($table.Select("Question='_id'").Count -eq 0) 'Kobo transport metadata should not appear as a form question'

$known = $type.GetMethod('ParseAnswers').Invoke($null, @('aRwMM45vMjfL7Y5jSYnVsD', '{"production_data/production_started":"yes"}'))
Check ($known.Rows.Count -eq 1) 'Known form response should be listed'
Check ($known.Rows[0].Question -like '*بدأ الإنتاج*') 'Known question should use its saved Kobo Arabic label'
Check ($known.Rows[0].Response -like '*نعم*' -and $known.Rows[0].Response -like '*yes*') 'Choice should show the Kobo label and stored code together'

Write-Output "KoboSubmissionViewer.Tests.ps1: $script:checks checks passed"

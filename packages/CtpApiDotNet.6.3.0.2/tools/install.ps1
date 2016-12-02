param($installPath, $toolsPath, $package, $project)

$depDlls = "thostmduserapi.dll", "thosttraderapi.dll"
$propertyName = "CopyToOutputDirectory"

foreach($dll in $depDlls) {
  $item = $project.ProjectItems.Item($dll)
  if ($item -eq $null) {
    continue
  }

  $property = $item.Properties.Item($propertyName)
  if ($property -eq $null) {
    continue
  }

  $property.Value = 1
}
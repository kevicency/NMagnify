$packages = ls -Filter packages.config -recurse
foreach($package in $packages) { ./Tools/nuget.exe i $package.FullName -o packages }
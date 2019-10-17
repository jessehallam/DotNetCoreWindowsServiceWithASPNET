New-Service -Name WorkerService1 `
  -Description ".NET Core Worker Service" `
  -DisplayName WorkerService1 `
  -StartupType Manual `
  -BinaryPathName (Resolve-Path (Join-Path "." "bin/Debug/netcoreapp3.0/WorkerService1.exe"))
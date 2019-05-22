# AspNetCoreCracker
A multi thread tool to attempt to crack ASP.NET Core Identity hashes

## Usage
```
  --hashes or -h       Required. The path to a file with a list of hashes to crack.

  --passwords or -p    Required. The path to a file with a list of passwords to hash.

  --thread or -t       Optional. The count of thread used to work
```

## Prerequisite
.NET Core 2.2.0 (Cross-platform)

Tested on Windows 10 x64 and Ubuntu 18.04.2 x64.

## Run
`dotnet run -h hashes.txt -p passwords.txt -t 10`

```
[+] Cracked Hash: 15
[+] Checked Hash: 1344
[+] Current Hash: AKF4m8tri6iAad73szDdoBU3fXDALi3h6pTpf2o9WYo50scTPo/3CclPsBDsrLPrOQ==
[+] Hash/Min: 542
[+] Check/Sec: 8752
[+] Progress: 1344/20026 7%
[+] Time: 3m 3s

[P] Pause - [R] Resume - [S] Save
```

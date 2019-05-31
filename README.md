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
[+] Cracked Hash: 86
[+] Checked Hash: 1189
[+] Current Hash: ACMT/36DJzuUByPbe8hhT0pbvb8NlwgO7Me1NiQlHURjba06L5Lki/n3SYdYflY0ww==
[+] Hash/Min: 922
[+] Check/Sec: 4496
[+] Progress: 310211/710424 44%
[+] Time: 1m 18s
[+] Remaining time: 1m 30s

[P] Pause - [R] Resume - [S] Save
```

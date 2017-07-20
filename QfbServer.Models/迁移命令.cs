/*

Enable-Migrations -ContextTypeName "QfbServer.Models.QfbServerContext" -ProjectName "QfbServer.Models" -StartUpProjectName "QfbServer" -ConnectionStringName "QfbServerContext" -Verbose

Add-Migration -Name "20170707"  -ProjectName "QfbServer.Models" -StartUpProjectName "QfbServer" -ConnectionStringName "QfbServerContext" -Verbose

Update-Database -Script -ProjectName "QfbServer.Models" -StartUpProjectName "QfbServer" -ConnectionStringName "QfbServerContext"  -Verbose

*/
# INFINITE.CORE
Your Project Description Here..

## Migration Data
Run "Database.sql" di sql server untuk membuat database dan generate table

## Use Code Generator
### 1.Install tool
klik kanan di project INFINITE.CORE.Data kemudian pilih open in terminal kemudian ketik :
```powershell
dotnet tool install --global dotnet-ef 
atau
dotnet tool update --global dotnet-ef
```
### 2.Scaffolding 
setelah mengeksekusi install tool kemudian ketik/copy code berikut :
```scaffold
dotnet ef dbcontext scaffold "Server=localhost;Database=INFINITE.CORE;user id =sa;password=Banda@40;MultipleActiveResultSets=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer --output-dir "..\INFINITE.CORE.Data\Model" -c ApplicationDBContext --context-dir "..\INFINITE.CORE.Data" --namespace "INFINITE.CORE.Data.Model" --context-namespace "INFINITE.CORE.Data" --no-pluralize -f --no-onconfiguring --schema "dbo"
```
ganti localhost username dan password apabila ingin merubah koneksi ke server yang dituju.

### 3.Generated file
show all files di project INFINITE.CORE.Data maka akan terbentuk file *generated" dan didalamnya terdapat backend dan frontend disana tinggal copy paste saja kedalam core untuk kebutuhan table.



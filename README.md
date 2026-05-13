# 📋 Pré-requisitos

Antes de começar, certifique-se de ter instalado:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 18+](https://nodejs.org/)
- [Angular CLI 17](https://angular.io/cli)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (opcional)
- [SQL Server](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads) ou SQL Server LocalDB (opcional)

## 🚀 Como Executar o Projeto

### Opção 1: Execução Local (Recomendado para desenvolvimento)

#### Backend

# Clone o repositório
git clone https://github.com/seu-usuario/debt-management-system.git
cd debt-management-system/backend

# Restaure os pacotes NuGet
dotnet restore

# Execute as migrações (se usar SQL Server)
cd DebtManagement.API
dotnet ef database update

# Execute a API
dotnet run

# A API estará disponível em:
# https://localhost:7000/swagger (Swagger UI)
# http://localhost:5000 (HTTP)


#### Frontend

# Em outro terminal, navegue até a pasta do frontend
cd frontend/debt-management-app

# Instale as dependências
npm install

# Execute o servidor de desenvolvimento
ng serve

# O frontend estará disponível em:
# http://localhost:4200

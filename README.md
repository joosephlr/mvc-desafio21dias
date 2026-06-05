# 📚 MVC Desafio 21 Dias API

Um projeto ASP.NET Core MVC para gerenciamento de alunos e suas notas. Desenvolvido como parte de um desafio de 21 dias de aprendizado em desenvolvimento web com C# e ASP.NET Core.

## 🎯 Objetivo

Criar um sistema web de controle de alunos que permite:
- Visualizar lista de alunos com suas notas e situação (Aprovado/Reprovado)
- Criar novos alunos com notas
- Calcular automaticamente a média das notas
- Determinar se o aluno foi aprovado ou reprovado

## 🛠️ Tecnologias

- **Framework**: ASP.NET Core 5.0 MVC
- **Linguagem**: C#
- **Banco de Dados**: SQL Server
- **ORM**: ADO.NET (SQL direto)
- **Frontend**: Razor Views + Bootstrap
- **Porta**: 5001

## 📋 Requisitos

- [.NET SDK 5.0](https://dotnet.microsoft.com/en-us/download/dotnet/5.0) ou superior
- SQL Server (LocalDB ou servidor local)
- Visual Studio Code ou Visual Studio
- Git

## 🚀 Como Configurar

### 1. Clonar o repositório

```bash
git clone <seu-repositorio>
cd mvc-desafio-21-dias-api
```

### 2. Configurar o banco de dados

#### Criar o banco de dados:

```sql
CREATE DATABASE VendasDB;
```

#### Criar a tabela de alunos:

```sql
USE VendasDB;

CREATE TABLE Alunos (
    id int IDENTITY(1,1) PRIMARY KEY,
    nome varchar(150) NOT NULL,
    matricula varchar(15) NOT NULL,
    notas varchar(255)
);
```

### 3. Verificar a string de conexão

Abra o arquivo [Models/AlunoDTO.cs](Models/AlunoDTO.cs) e verifique/atualize a string de conexão:

```csharp
return "Server=localhost;database=VendasDB;user=sa;password=P@ssw0rd";
```

Ajuste conforme suas credenciais do SQL Server.

### 4. Restaurar dependências

```bash
dotnet restore
```

### 5. Compilar o projeto

```bash
dotnet build
```

## ▶️ Como Executar

### Modo desenvolvimento com hot-reload:

```bash
dotnet watch run
```

### Modo normal:

```bash
dotnet run
```

### Publicar para produção:

```bash
dotnet publish
```

Após iniciar, acesse a aplicação em: **http://localhost:5001**

## 📁 Estrutura do Projeto

```
mvc-desafio-21-dias-api/
├── Controllers/
│   ├── AlunosController.cs       # Controller para gerenciar alunos
│   └── HomeController.cs         # Controller para home
├── Models/
│   ├── Aluno.cs                  # Modelo de dados do Aluno (métodos de instância)
│   ├── AlunoDTO.cs               # Classe com métodos estáticos de acesso ao banco
│   └── ErrorViewModel.cs         # Modelo para página de erro
├── Views/
│   ├── Alunos/
│   │   ├── Index.cshtml          # Lista de alunos
│   │   └── Create.cshtml         # Formulário para criar aluno
│   ├── Home/
│   │   ├── Index.cshtml
│   │   └── Privacy.cshtml
│   └── Shared/
│       ├── _Layout.cshtml        # Layout principal
│       ├── _ValidationScriptsPartial.cshtml
│       └── Error.cshtml
├── wwwroot/                       # Arquivos estáticos
│   ├── css/
│   ├── js/
│   └── lib/
├── Properties/
│   └── launchSettings.json        # Configurações de launch
├── appsettings.json              # Configurações da aplicação
├── appsettings.Development.json  # Configurações de desenvolvimento
├── Program.cs                     # Ponto de entrada
├── Startup.cs                     # Configuração de startup
└── mvc.csproj                     # Arquivo de projeto
```

## 🔌 Rotas/Endpoints

| Rota | Método | Descrição |
|------|--------|-----------|
| `/alunos` | GET | Lista todos os alunos |
| `/alunos/create` | GET | Exibe formulário para criar novo aluno |
| `/alunos/create` | POST | Salva um novo aluno no banco |
| `/` | GET | Página inicial |
| `/home/privacy` | GET | Página de privacidade |

## 📊 Modelo de Dados

### Aluno

**Propriedades:**
- `Id` (int): Identificador único
- `Nome` (string): Nome do aluno
- `Matricula` (string): Número de matrícula
- `Notas` (List<double>): Lista de notas do aluno

**Métodos de Instância:**
- `CalcularMedia()`: Calcula a média das notas
- `Situacao()`: Retorna "Aprovado" se média >= 7, senão "Reprovado"
- `StrNotas()`: Retorna as notas em formato string separado por " | "
- `Salvar()`: Insere ou atualiza o aluno no banco
- `Apagar()`: Remove o aluno do banco

**Métodos Estáticos:**
- `Todos()`: Retorna lista de todos os alunos
- `Incluir(Aluno)`: Insere um novo aluno
- `Atualizar(Aluno)`: Atualiza dados de um aluno existente
- `ApagarPorId(int)`: Remove aluno pelo ID

## 💾 Banco de Dados

### String de Conexão

A aplicação usa SQL Server com as seguintes credenciais padrão:
- **Server**: localhost
- **Database**: VendasDB
- **User**: sa
- **Password**: P@ssw0rd

⚠️ **Altere a senha padrão em produção!**

### Schema da Tabela Alunos

```sql
CREATE TABLE Alunos (
    id int IDENTITY(1,1) PRIMARY KEY,
    nome varchar(150) NOT NULL,
    matricula varchar(15) NOT NULL,
    notas varchar(255)
);
```

As notas são armazenadas como string separadas por vírgula: `"7.5,8.0,9.5"`

## 🎨 Interface

### Página de Alunos
- Lista todos os alunos em uma tabela
- Exibe: ID, Nome, Matrícula, Notas e Situação
- Botão para criar novo aluno
- Situação colorida: Verde para Aprovado, Vermelho para Reprovado

### Criar Aluno
- Formulário simples com campos:
  - Nome (obrigatório)
  - Matrícula (obrigatório)
  - Notas (opcional, separadas por vírgula)
- Botões Salvar e Cancelar

## 🔧 Compilar e Testar

### Build Debug:
```bash
dotnet build
```

### Build Release:
```bash
dotnet build -c Release
```

### Executar testes:
```bash
dotnet test
```

### Limpar build anterior:
```bash
dotnet clean
```

## 📝 Exemplo de Uso

1. **Acessar a página de alunos**: http://localhost:5001/alunos

2. **Criar um novo aluno**:
   - Clique em "+ Criar Novo Aluno"
   - Preencha:
     - Nome: João Silva
     - Matrícula: 2024001
     - Notas: 7.5,8.0,9.5
   - Clique em "Salvar"

3. **Visualizar alunos**: A lista será atualizada com o novo aluno

## ⚙️ Configurações

### appsettings.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "Kestrel": {
    "Endpoints": {
      "Http": {
        "Url": "http://localhost:5001"
      }
    }
  }
}
```

## 🐛 Troubleshooting

### Erro de conexão com banco de dados
- Verifique se SQL Server está rodando
- Confirme a string de conexão em `Models/AlunoDTO.cs`
- Verifique credenciais (usuário e senha)

### Porta 5001 já em uso
- Altere a porta em `appsettings.json` ou `Properties/launchSettings.json`

### Projeto não compila
- Limpe o projeto: `dotnet clean`
- Restaure dependências: `dotnet restore`
- Reconstrua: `dotnet build`

## 📚 Recursos Adicionais

- [Documentação ASP.NET Core](https://docs.microsoft.com/aspnet/core)
- [C# Documentation](https://docs.microsoft.com/dotnet/csharp)
- [SQL Server Documentation](https://docs.microsoft.com/sql/sql-server)

## 🤝 Contribuindo

Para contribuir com este projeto:

1. Faça um Fork
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📄 Licença

Este projeto é de código aberto e está disponível sob a licença MIT.

## ✅ Checklist de Funcionalidades

- ✅ Listar todos os alunos
- ✅ Visualizar notas e situação de cada aluno
- ✅ Criar novo aluno com notas
- ✅ Calcular média de notas
- ✅ Determinar aprovação/reprovação
- ✅ Integração com SQL Server
- ✅ Interface responsiva com Bootstrap
- ✅ Validação de dados

## 🚧 Melhorias Futuras

- [ ] Editar dados de aluno existente
- [ ] Deletar aluno
- [ ] Adicionar notas para aluno existente
- [ ] Autenticação de usuários
- [ ] Relatórios de desempenho
- [ ] API REST com Entity Framework
- [ ] Testes unitários
- [ ] Validações mais robustas

---

**Última atualização**: Junho de 2026


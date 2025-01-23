# ArtigosCientíficos

ArtigosCientíficos é um sistema ASP.NET MVC para a gestão de artigos científicos e respetivas estatísticas. Este guia irá ajudá-lo a configurar o projeto para a sua primeira execução.

## 🚀 Configuração do Projeto

Antes de executar o projeto pela primeira vez, siga os passos abaixo para configurar a base de dados e garantir que tudo funcione corretamente.

### Pré-requisitos

Certifique-se de que tem o seguinte instalado no seu computador:

- [.NET 6 ou superior](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) ou um sistema de base de dados compatível
- [Entity Framework Core CLI](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)

### 📂 Passos para configurar

1. **Clonar o repositório:**

   Clone o repositório para o seu computador:
   ```bash
   git clone https://github.com/Capa03/ArtigosCientificos.git
   cd ArtigosCientificos
   ```

2. **Configurar a base de dados:**

   O projeto utiliza o Entity Framework Core para gerir a base de dados. Siga os passos abaixo para criar a base de dados:

   - Abra o **Package Manager Console** no Visual Studio ou utilize o terminal integrado.
   - Navegue até ao diretório do projeto principal:
     ```bash
     cd ArtigosCientificos
     ```
   - Execute o comando para aplicar as migrações e criar a base de dados:
     ```bash
     dotnet ef database update
     ```

   Este comando irá criar a base de dados de acordo com as migrações configuradas no projeto.

3. **Configurar a string de ligação:**

   - Certifique-se de que a string de ligação à base de dados está corretamente configurada no ficheiro `appsettings.json`:
     ```json
     "ConnectionStrings": {
       "DefaultConnection": "Server=SEU_SERVIDOR;Database=ArtigosCientificos;Trusted_Connection=True;MultipleActiveResultSets=true"
     }
     ```
   - Substitua `SEU_SERVIDOR` pelo endereço do seu servidor SQL Server.

4. **Executar o projeto:**

   O projeto está agora pronto para ser executado. Pode iniciar o servidor executando o seguinte comando no terminal:
   ```bash
   dotnet run
   ```

   Alternativamente, pode utilizar o botão de execução no Visual Studio.

5. **Aceder à aplicação:**

   Abra o seu navegador e aceda ao endereço:
   ```
   http://localhost:5000
   ```

---

## 🛠 Tecnologias Utilizadas

- **ASP.NET Core MVC**: Framework principal do projeto.
- **Entity Framework Core**: Gestão de base de dados.
- **Bootstrap**: Para estilização do front-end.
- **Chart.js**: Geração de gráficos dinâmicos.

---

## 🧑‍💻 Contribuir

Contribuições são bem-vindas! Siga os passos abaixo para contribuir para o projeto:

1. Faça um fork do repositório.
2. Crie uma nova branch com a sua funcionalidade ou correção:
   ```bash
   git checkout -b minha-funcionalidade
   ```
3. Realize as suas alterações e faça o commit:
   ```bash
   git commit -m "Adiciona a minha nova funcionalidade"
   ```
4. Envie para o repositório remoto:
   ```bash
   git push origin minha-funcionalidade
   ```
5. Abra um Pull Request.

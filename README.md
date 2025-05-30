# SeuManoel.Empacotamento - Sistema de Empacotamento de Pedidos

Um microsservico de API para otimização de empacotamento de produtos em caixas, desenvolvido para a loja online de Seu Manoel. Este sistema processa pedidos com produtos de diferentes dimensões e determina a melhor forma de embalá-los, usando o algoritmo First Fit Decreasing.

## Índice

- Pré-requisitos
- Instalação e Execução
- Autenticação
- Endpoints da API
- Algoritmo de Empacotamento
- Estrutura do Projeto
- Testes

## Pré-requisitos

Para executar este projeto, você precisa ter instalado:
- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/install/)

Nenhuma instalação adicional é necessária, pois todo o ambiente é configurado em containers.

## Instalação e Execução

1. Clone o repositório:
   ```bash
   git clone https://github.com/seu-usuario/SeuManoel.Empacotamento.git
   cd SeuManoel.Empacotamento
   ```

2. Execute o projeto usando Docker Compose:
   ```bash
   docker-compose up -d
   ```

   Este comando:
   - Constrói a imagem da aplicação
   - Inicia o SQL Server em um container
   - Inicia a API em outro container
   - Configura a rede entre os serviços
   - Executa migrações do banco de dados

3. A API estará disponível em:
   ```
   http://localhost:5274/swagger/index.html
   ```

4. Para encerrar a aplicação:
   ```bash
   docker-compose down
   ```

## Autenticação

A API usa autenticação JWT para proteger os endpoints. Para utilizar os endpoints protegidos:

1. Primeiro, crie um usuário:
   - Acesse `POST /User` no Swagger
   - Insira username e senha

2. Faça login para obter um token:
   - Acesse `POST /Login` no Swagger
   - Use as mesmas credenciais
   - Copie o token JWT retornado

3. Autorize-se no Swagger:
   - Clique no botão "Authorize" no topo da página (Icone com um Cadeado)
   - Insira o token no formato: `Bearer {seu-token}`

O token tem validade de 1 hora. 

## Endpoints da API

### Autenticação e Usuários
- `POST /User` - Cria um novo usuário
- `POST /Login` - Autentica usuário e retorna token JWT

### Empacotamento
- `POST /Packing/packing-input` - Processa pedidos para empacotamento

#### Exemplo de Payload:
```json
{
  "pedidos": [
    {
      "pedido_id": 1,
      "produtos": [
        {"produto_id": "PS5", "dimensoes": {"altura": 40, "largura": 10, "comprimento": 25}},
        {"produto_id": "Volante", "dimensoes": {"altura": 40, "largura": 30, "comprimento": 30}}
      ]
    }
  ]
}
```

#### Exemplo de Resposta:
```json
[
  {
    "pedido_id": 1,
    "caixas": [
      {
        "caixa_id": "Caixa 2",
        "produtos": ["PS5", "Volante"],
        "observacao": null
      }
    ]
  }
]
```

## Algoritmo de Empacotamento

O sistema implementa o algoritmo First Fit Decreasing para otimizar a alocação de produtos em caixas:

1. Ordena os produtos por volume (do maior para o menor)
2. Tenta encaixar os produtos na menor caixa possível
3. Se um produto não couber em nenhuma caixa, retorna uma observação adequada

Caixas disponíveis:
- Caixa 1: 30 x 40 x 80
- Caixa 2: 80 x 50 x 40
- Caixa 3: 50 x 80 x 60

## Estrutura do Projeto

- br.seumanoel.empacotamento.api - Projeto principal da API
  - `Controllers` - Controladores da API
  - `Models` - Entidades e DTOs
  - `Services` - Lógica de negócio, incluindo o algoritmo de empacotamento
  - `Data` - Contexto e configurações do banco de dados
  - `Factory` - Fábricas de objetos
  - `Interfaces` - Interfaces para injeção de dependência

- br.seumanoel.empacotamento.tests - Projeto de testes unitários

## Testes

Para executar os testes unitários:

```bash
# Dentro do container
dotnet test

# Ou localmente, se tiver o .NET SDK instalado
dotnet test br.seumanoel.empacotamento.tests/br.seumanoel.empacotamento.tests.csproj
```

---

Desenvolvido como parte do teste técnico para L2 Dev Jr.
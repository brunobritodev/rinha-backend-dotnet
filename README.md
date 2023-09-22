# Rinha de Backend - versão .NET 7

Esta API foi criada com o objetivo de realizar testes de stress para gravação massiva de dados em um PostgreSQL. A aplicação é construída usando ASP.NET Core e está otimizada visando atender exclusivamente o teste de stress, sem compromisso com cenários do mundo real.

O maior problema nas discussões estava na rede do docker, portanto essa versão utiliza o modo `network_mode: host`. Dessa forma o maior impacto da carga vai recair sobre o banco de dados. Tanto a API quanto o nginx trabalham com folga, por isso as otimizações visam o BD.

Para cargas maiores, poderia seguir por outras estratégias como outros membros fizeram, adicionando uma fila, cache e etc, mas para as restrições e volume atual só o básico dá conta. KISS manda um abraço.

# Resultados

Foram feitos dois testes. O primeiro com a carga inicial e o segundo dobrando a carga.

## Carga proposta pelo teste inicial: 

![resultado](/misc/carga-inicial/resultado.png)

Todos os componentes trabalham com folga de Memória e CPU:

Postgres:

![postgres](/misc/carga-inicial/postgres.png)

.NET:

![dotnet](/misc/carga-inicial/dotnet.png)

nginx:

![dotnet](/misc/carga-inicial/nginx.png)

# Carga Dobrada

Com essa configuração e dobrando a carga, vemos um aumento de memória e CPU, bem como chamadas mais lentas, seria necessário ajustar a CPU e memória da aplicação e talvez mexer no pool de conexões e conseguir aumentar ainda mais a carga.

![resultado](/misc/carga-dobrada/resultado.png)

Postgres:

![postgres](/misc/carga-dobrada/postgres.png)

.NET:

![dotnet](/misc/carga-dobrada/dotnet.png)

nginx:

![dotnet](/misc/carga-dobrada/nginx.png)

## Requisitos
* Docker e Docker Compose
* .NET 7 (para desenvolvimento)

## Configuração Inicial

1. Clone o repositório.
2. Navegue até o diretório do projeto.

## Executando com Docker Compose

Para iniciar todos os serviços, incluindo a API e o banco de dados PostgreSQL:

```sh
docker-compose up
```

Isso iniciará:

* Duas instâncias da API rodando nas portas 8080 e 8081.
* Um servidor PostgreSQL.
* Um servidor nginx atuando como LB, escutando na porta 9999.

# Endpoints da API

* POST /pessoas: Para inserir uma nova pessoa.
* GET /pessoas/{id}: Para obter uma pessoa por ID.
* GET /pessoas: Para listar pessoas com base em um termo de pesquisa.
* GET /contagem-pessoas: Para obter a contagem total de pessoas.

# Configuração do Banco de Dados
O schema e os índices do banco de dados são definidos no arquivo ddl.sql. Consulte este arquivo para mais detalhes sobre a estrutura do banco de dados.

# Configurações de Otimização

Diversas otimizações foram feitas para garantir que a aplicação possa lidar com um grande número de inserções em um curto período de tempo. Isso inclui configurações no postgres, otimizações no nginx e otimizações na aplicações.

# Contribuição
Sinta-se à vontade para contribuir com melhorias e otimizações.

# Licença
MIT License


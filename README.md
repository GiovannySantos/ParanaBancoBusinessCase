# 💼 Paraná Banco Business Case

## 📘 Visão Geral

Este sistema é composto por uma arquitetura distribuída baseada em microsserviços para cadastro de clientes, geração de propostas de crédito e emissão de cartões. Utiliza comunicação assíncrona baseada em eventos, com foco em **resiliência**, **escalabilidade** e **desacoplamento**. Desenvolvido como um **desafio técnico backend** utilizando princípios de design modernos.

---

## 🎯 Objetivo

Cadastrar um novo cliente por meio de uma API REST e orquestrar, de forma desacoplada e resiliente, os processos de geração de proposta de crédito e emissão de cartões, garantindo tolerância a falhas e consistência eventual.

---

## 🔍 Motivação da Solução

- Desacoplamento entre serviços com troca de mensagens via eventos
- Tolerância a falhas sem interromper o cadastro de clientes
- Estratégia de **consistência eventual** para ambientes distribuídos
- Observabilidade por meio de logs e eventos de erro
- Escalabilidade horizontal com serviços independentes

---

## 📦 Tecnologias Utilizadas

- [.NET 8.0](https://learn.microsoft.com/dotnet/)
- [RabbitMQ](https://www.rabbitmq.com/)
- [PostgreSQL](https://www.postgresql.org/)
- [Polly](https://github.com/App-vNext/Polly) para Retry
- Docker + Docker Compose
- DDD (Domain-Driven Design)
- Mensageria assíncrona (Pub/Sub)
- Logging e rastreamento de falhas

---

## 🏗️ Arquitetura

### 🧱 Princípios Arquiteturais

- Microsserviços independentes, cada um com seu próprio contexto de domínio
- Separação em camadas: API / Application / Domain / Infrastructure
- Comunicação entre serviços por meio de eventos (RabbitMQ)
- Resiliência implementada com Retry, Fallback e DLQ

### 🧭 Bounded Contexts

- **Cadastro de Clientes**
- **Proposta de Crédito**
- **Cartão de Crédito**

Cada contexto é modelado de forma isolada, com seus próprios contratos de eventos.

---

## 🔁 Fluxo de Comunicação entre Microsserviços

1. `CadastroClientes` publica `ClienteCriadoEvent`
2. `PropostaCredito` consome, gera proposta e publica:
   - ✅ `PropostaGeradaEvent`
   - ❌ `PropostaCreditoFalhouEvent`
3. `CartaoCredito` consome e tenta emitir cartão:
   - ✅ `CartaoEmitidoEvent`
   - ❌ `CartaoEmissaoFalhouEvent`
4. `CadastroClientes` escuta falhas e executa ações compensatórias

---

## 📊 Diagrama de Arquitetura

O diagrama abaixo representa a orquestração entre os microsserviços utilizando **RabbitMQ**, com eventos, consumidores, DLQs e mecanismos de resiliência.

<img src="./docs/DiagramaParanaBanco.svg" alt="Fluxo de Microsserviços" width="100%" />

📄 [Clique aqui para baixar o PDF do diagrama](./docs/DiagramaParanaBanco.drawio.pdf)

---

## 🧩 Microsserviços e Responsabilidades

| Serviço                  | Responsabilidade                               |
|--------------------------|------------------------------------------------|
| **CadastroClientes.API** | Cadastro de cliente e orquestração via eventos |
| **PropostaCredito.API**  | Geração da proposta de crédito                 |
| **CartaoCredito.API**    | Emissão de um ou mais cartões de crédito       |

---

## 🐳 Executando com Docker Compose

### ✅ Pré-requisitos

- [Docker](https://www.docker.com/)
- [Docker Compose](https://docs.docker.com/compose/)

### ▶️ Subindo o ambiente

```bash
docker-compose up --build
````

### 🌐 Serviços Disponíveis

| Serviço              | Porta                                            | Descrição                            |
| -------------------- | ------------------------------------------------ | ------------------------------------ |
| CadastroClientes.API | [http://localhost:5001](http://localhost:5001)   | POST `/api/clientes`                 |
| PropostaCredito.API  | [http://localhost:5002](http://localhost:5002)   | Escuta eventos e processa propostas  |
| CartaoCredito.API    | [http://localhost:5003](http://localhost:5003)   | Emite cartões de crédito             |
| RabbitMQ UI          | [http://localhost:15672](http://localhost:15672) | Interface de administração do broker |
| RabbitMQ Broker      | `5672` (AMQP)                                    | Comunicação interna entre serviços   |
| PostgreSQL           | `5432`                                           | Banco de dados dos serviços          |

---

## 📡 Testando a API

Requisição de exemplo para cadastrar cliente:

```http
POST http://localhost:5001/api/clientes
Content-Type: application/json

{
  "nome": "Maria Oliveira",
  "cpf": "12345678901",
  "email": "maria@exemplo.com",
  "telefone": "41999999999",
  "dataNascimento": "1990-01-01",
  "rendaMensal": 5000,
  "valorCreditoDesejado": 10000
}
```

---

## 🛡️ Resiliência

* **Polly (Retry):** tratamento de falhas transitórias com política de backoff
* **DLQ (Dead Letter Queue):** eventos com falha redirecionados para análise posterior
* **Fallbacks:** serviços reagem a falhas publicadas por outros contextos

---

## 📈 Observabilidade

* **RabbitMQ Management UI:** monitoramento em [http://localhost:15672](http://localhost:15672)
* **Logs estruturados:** cada serviço registra eventos e falhas
* **Eventos de falha específicos:** utilizados para rastreamento e compensação

---

## ✅ Considerações Finais

Esta aplicação demonstra uma arquitetura moderna, resiliente e escalável para processos críticos de negócio que exigem coordenação entre múltiplos microsserviços desacoplados. O uso de padrões como retry, fallback e publish/subscribe permite uma comunicação robusta e flexível entre domínios distintos.

> 🚀 *Pronto para ser expandido com monitoramento, métricas e deploy em cloud!*
